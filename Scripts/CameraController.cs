using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 视角控制器（针对性父体控制）
/// </summary>
public class CameraController : MonoBehaviour
{  
    public float horizontalSpeed = 10.0f;//为镜头水平移动处理
    public float verticalSpeed = 80.0f;//为镜头垂直移动处理
    public float cameraDampValue = 0.5f;//镜头滑动速度
    public Image lockDot;//标点
    public bool lockState;//锁定敌人状态
    public bool isAI = false;

    //private PlayerInput playerInput;//角色控制器实例 <---- private JoystickInput joystickInput; ---->
    public IUserInput IUI;
    private GameObject cameraHandle;//相机处理手柄
    private GameObject playerHandle;//角色处理手柄
    private float tempEulerX;//储存欧拉角X

    [HideInInspector]
    private GameObject model;//模型
    private GameObject camera;//相机
    private Vector3 cameraVelocity;//为输出目前相机速度参数值
    private GameObject lastEnemyObj;
    [SerializeField]
    private LockTarget lockTarget;//锁定目标

    void Start()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20.0f;
        model = playerHandle.GetComponent<Actor_Controller>().model;
        IUI = playerHandle.GetComponent<Actor_Controller>().IUI;

        /*
         *摇杆 
         * ActorController ac = playerHandle.GetComponent<ActorController>();
         * model = ac.model;
         * joystickInput = ac.joystickInput;
         */
        if (!isAI)
        {
            camera = Camera.main.gameObject;
            lockDot.enabled = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
        
        lockState = false;
    }

    private void Update()
    {
        if(lockTarget != null)//锁定人物后，获取相机方法，将标点对位在敌人的正中心
        {
            if (!isAI)
            {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            }
            
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f || (lockTarget.actMgr != null && lockTarget.actMgr.staMgr.isDie))
            {
                LockProccessA(null, false, false, isAI);
            }
        }
    }

    void FixedUpdate()
    {
        if(lockTarget == null)//没有锁定人的情况下，画面、人物不受控制
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, IUI.JRight * Time.fixedDeltaTime * horizontalSpeed);//父物体的旋转
            tempEulerX -= IUI.JUp * verticalSpeed * Time.fixedDeltaTime;//存储控制器在垂直（上下）转动的欧拉X值
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);//将欧拉角限定在特定的值内

            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);//将相机控制器按转存欧拉角的多少移动

            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;//目标位置
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;//控制器朝向
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        if (!isAI)
        {
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraVelocity, cameraDampValue);//镜头跟随，形成延迟处理
            //camera.transform.eulerAngles = transform.eulerAngles;//将镜头位置固定在以世界单位的本体欧拉角位置
            camera.transform.LookAt(cameraHandle.transform);//（会随着控制器朝向而指定转向）
        }
    }

    public void LockUnLock()
    {
        Vector3 modelOrigin1 = playerHandle.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + playerHandle.transform.forward * 4f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), playerHandle.transform.rotation, LayerMask.GetMask(isAI?"Player":"Enemy"));

        if (IUI.lockon & lockTarget != null)/* 自己加的 */
        {
            cols = Physics.OverlapSphere(modelOrigin2, 7f, LayerMask.GetMask(isAI ? "Player" : "Enemy"));
        }

        if (cols.Length == 0)
        {
            LockProccessA(null, false, false, isAI);
        }
        else
        {
            foreach (var col in cols)
            {
                if(lockTarget != null && cols.Length < 2 && col.gameObject == lastEnemyObj)/* 自己加的 */
                {
                    LockProccessA(null, false, false, isAI);
                    break;
                }
                //print(cols.Length);
                //当锁定视角与视觉为同一目标
                //当锁定不为空，敌人数量超过1时，锁定目标将换为视野内另外一个
                else if (lockTarget != null && lockTarget.obj == lastEnemyObj && col.gameObject == lastEnemyObj)
                {
                    continue;/* 自己加的 */
                }
                lockTarget = new LockTarget(col.gameObject, col.bounds.extents.y);//col.bounds.extent计算collider的xyz半值
                    LockProccessA(lockTarget, true, true, isAI);
                    lastEnemyObj = lockTarget.obj;
                    break;
            }
        }
    }

    /// <summary>
    /// 锁定敌人相关运算开关及赋值
    /// </summary>
    /// <param name="_lockTarget">锁定敌人目标</param>
    /// <param name="_lockDotEnabled">标点</param>
    /// <param name="_lockState">锁定状态</param>
    /// <param name="_isAI">布林AI</param>
    private void LockProccessA(LockTarget _lockTarget, bool _lockDotEnabled, bool _lockState, bool _isAI)
    {
        if (!_isAI)
        {
            lockTarget = _lockTarget;
            lockState = _lockState;
            lockDot.enabled = _lockDotEnabled;
        }
    }

    /// <summary>
    /// 锁定敌人
    /// </summary>
    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;
        public ActorMgr actMgr;

        public LockTarget(GameObject _obj, float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
            actMgr = _obj.GetComponent<ActorMgr>();
        }
    }
}
