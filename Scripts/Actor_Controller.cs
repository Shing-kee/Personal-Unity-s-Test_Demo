using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物全局控制器
/// </summary>
public class Actor_Controller : MonoBehaviour
{
    public GameObject model;

    public IUserInput IUI;
    public CameraController cameraCtrl;
    public float walkSpeed = 1.4f;
    public float runMultiplier = 2.65f;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 1.0f;
    public float jabMultiplier = 3.0f;

    [Header("--------摩擦系数--------")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;
    public Animator anim;

    private Rigidbody rigid;
    private Vector3 planarVec;//人物值
    private Vector3 thurstVec;//跳跃值
    private bool canAttack;
    private CapsuleCollider col;
   // private float lerpTarget;//
    private Vector3 deltaPos;//
    private float tempVel = 1.0f;

    [Space(10)]
    public bool leftIsShield = true;
    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;

    private bool lockPlanar = false;
    private bool trackDirection = false;

    void Awake()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if(input.enabled == true)
            {
                IUI = input;
                break;
            }
        }

        anim = model.GetComponent<Animator>();
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("You have to Add the Rigidbody for this GameObject!");
        }
        col = transform.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        #region 动画处理
        //控制Animator里的slider数值，以执行动画的过滤切换
        //run
        if (IUI.run)
        {
            if (tempVel <= 2.0f)
            {
                tempVel += Time.deltaTime * 2.5f;
            }
        }
        else
        {
            tempVel = 1.0f;
        }

        //roll
        if (IUI.roll || rigid.velocity.magnitude > 7f)
        {
            anim.SetTrigger("Roll");
            canAttack = false;
        }

        //jump
        if (IUI.jump)
        {
            anim.SetTrigger("Jump");
            canAttack = false;
        }

        //attack
        if ((IUI.lAttack || IUI.rAttack || IUI.lb || IUI.rb) && (CheckState("ground") || CheckStateTag("AttackL") || CheckStateTag("AttackR")) && canAttack)
        {
            if ((IUI.lAttack || IUI.lb) && !leftIsShield)
            {
                anim.SetBool("Mirror", true);
                anim.SetTrigger("Attack");
            }
            else if (IUI.rAttack || IUI.rb)
            {
                anim.SetBool("Mirror", false);
                anim.SetTrigger("Attack");
            }
        }

        //counterBack
        if ((IUI.lPressA || IUI.rPressA || IUI.lt || IUI.rt) && (CheckState("ground") || CheckStateTag("AttackL") || CheckStateTag("AttackR")) && canAttack)
        {
            if (IUI.rt || IUI.rPressA) { }
            else
            {
                if (!leftIsShield) { }
                else { anim.SetTrigger("CounterBack"); }
            }
        }

        if (leftIsShield)
        {
            if (CheckState("ground") || CheckState("isGround"))
            {
                anim.SetBool("Defense", IUI.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("Defense"), 1);
            }
            else
            {
                anim.SetBool("Defense", false);//如果行为不在ground位，则不能进行防御
                anim.SetLayerWeight(anim.GetLayerIndex("Defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Defense"), 0);
        }

        //战斗视角
        if (cameraCtrl.lockState == false)
        {
            anim.SetFloat("Forward", IUI.Dmag * Mathf.Lerp(anim.GetFloat("Forward"), tempVel, 0.4f));//Mathf.Lerp线性插值法，从a到b需要用到多少单位:使forward的值变
            anim.SetFloat("Right", 0);
        }
        else
        {
            Vector3 localDVec = transform.InverseTransformVector(IUI.Dvec);
            anim.SetFloat("Forward", localDVec.z * tempVel);
            anim.SetFloat("Right", localDVec.x * tempVel);
        }

        #endregion
 
        if (IUI.Action)
        {
            OnAction.Invoke();
        }

        if (IUI.lockon)
        {
            cameraCtrl.LockUnLock();
        }

        if(cameraCtrl.lockState == false)
        {
            if(IUI.inputEnabled == true)
            {
                if (IUI.Dmag > 0.1f)//如果Animator滑动数值存在变化,则人物方向将转向指定方向
                {
                    //以模型的正前方方向的transform，通过向量进行人物的旋转
                    model.transform.forward = Vector3.Slerp(model.transform.forward, IUI.Dvec, 0.3f);//Vector3.Slerp()从第一个值到第二个值经过N秒实现
                }
                if (lockPlanar == false)
                {
                    planarVec = IUI.Dmag * model.transform.forward * walkSpeed * (IUI.run ? tempVel + 0.4f : 1.0f);//存储并运用方向向量值
                }
            }
        }
        else
        {
            if(trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }
            //model.transform.forward = transform.forward;
            if(lockPlanar == false)
            {
                planarVec = IUI.Dvec * walkSpeed * (IUI.run ? tempVel + 0.4f : 1.0f);
            } 
        }
    }
       

    void FixedUpdate()
    {
        rigid.position += deltaPos;//只有动画红条累积完成不再执行时才传参到FixedUpdate
        //rigid.position += planarVec * Time.fixedDeltaTime;//S = V * T
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thurstVec;//rigid.velocity.y为rigidbody的y值，用于确定重力值的固定
        thurstVec = Vector3.zero;//归0，避免不断累加
        //rigid.velocity = planarVec;//错误示范，因为刚体的Y速度发生了变化，非重力数值
        deltaPos = Vector3.zero;
    }

    //接受子物体的动画的传送信息，执行方法
    public void OnJumpEnter()
    {
        lockPlanar = true;//锁方向值
        IUI.inputEnabled = false;//锁移动向量值
        thurstVec = new Vector3(0, jumpVelocity, 0);
        trackDirection = true;
    }

    public void IsGround()
    {
        anim.SetBool("IsGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("IsGround", false);
    }

    public void OnEnterGround()
    {
        IUI.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        trackDirection = false;
        col.material = frictionOne;
    }

    public void OnExitGround()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        IUI.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        thurstVec = new Vector3(0, rollVelocity, 0);
        IUI.inputEnabled = false;
        lockPlanar = true;
        trackDirection = true;
    }

    public void OnJabEnter()
    {
        IUI.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnJabUpdateEnter()
    {
        thurstVec = model.transform.forward * anim.GetFloat("JabVelocity") * jabMultiplier;
    }

    public void OnBlockedEnter()
    {
        IUI.inputEnabled = false;
    }

    public void OnDieEnter()
    {
        IUI.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }

    public void OnStunnedEnter()
    {
        IUI.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBackEnter()
    {
        IUI.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBackExit()
    {
        model.SendMessage("CounterBackDisable");
    }

    public void OnLockEnter()
    {
        IUI.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }

    #region 攻击动画消息处理
    
    public void OnAttack1hAEnter()
    {
        IUI.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnAttackIdleEnter()
    {
        IUI.inputEnabled = true; 
    }

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnAttack1hAUpdate()
    {
        thurstVec = model.transform.forward * anim.GetFloat("Attack1hAVelocity");
    }

    //美术调整
    public void OnUpdateRootMotion(object _deltaPos)
    {
        if (CheckState("attack1hC"))
        {
            deltaPos += (0.4f * deltaPos + 0.6f * (Vector3)_deltaPos);//累积动画红条的数值
        }
        
    }
    #endregion

    #region 遭受打击
    public void OnHitEnter()
    {
        IUI.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }
    #endregion

    #region 触发器指针（动画）

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    public void SetBoolean(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }

    #endregion

    #region 检查状态包
    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName); //返回目前动画状态中layerName的索引值的该名字是否为动画里的某一个名称
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }
    #endregion
}
