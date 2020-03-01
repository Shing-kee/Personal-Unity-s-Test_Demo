using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行为管理
/// </summary>
public class ActorMgr : MonoBehaviour
{
    public Actor_Controller actCtrl;//动画控制器

    [Header("===Auto Generate if Null===")]
    public BattleMgr btlMgr;//战斗管理
    public WeaponMgr wpnMrg;//武器管理
    public StateMgr staMgr; //状态管理
    public DirectorMgr dirMgr;//动画管理
    public InterationMgr intMgr;//互动管理
    
    [Header("OverrideController Animators")]
    public AnimatorOverrideController SglHandAnim;
    public AnimatorOverrideController DblHandAnim;

    private void Awake()
    {
        actCtrl = GetComponent<Actor_Controller>();
        GameObject model = actCtrl.model;
        GameObject sensor = null;
        try
        {
            sensor = transform.Find("sensor").gameObject;
        }
        catch (System.Exception)
        {
            //
            // if there is no sensor object
            //
        }
        

        btlMgr = Bind<BattleMgr>(sensor);
        wpnMrg = Bind<WeaponMgr>(model);
        staMgr = Bind<StateMgr>(gameObject);
        dirMgr = Bind<DirectorMgr>(gameObject);
        intMgr = Bind<InterationMgr>(sensor);

        actCtrl.OnAction += DoAction;
    }

    public void DoAction()
    {
        if(intMgr.lst_ovrlapEvnCstr.Count != 0)
        {
            if(intMgr.lst_ovrlapEvnCstr[0].active == true)
            {
                dirMgr.victim = intMgr.lst_ovrlapEvnCstr[0].actMgr;
                if (intMgr.lst_ovrlapEvnCstr[0].eventName == "frontStab")
                {
                    dirMgr.PlayFrontStab("frontStab", this, intMgr.lst_ovrlapEvnCstr[0].actMgr);
                }
                else if (intMgr.lst_ovrlapEvnCstr[0].eventName == "openBox")
                {
                    if (BattleMgr.CheckAnglePlayer(actCtrl.model, intMgr.lst_ovrlapEvnCstr[0].actMgr.gameObject, 30))
                    {
                        intMgr.lst_ovrlapEvnCstr[0].active = false;
                        transform.position = intMgr.lst_ovrlapEvnCstr[0].actMgr.transform.position + intMgr.lst_ovrlapEvnCstr[0].actMgr.transform.TransformVector(intMgr.lst_ovrlapEvnCstr[0].offset);
                        actCtrl.model.transform.LookAt(intMgr.lst_ovrlapEvnCstr[0].actMgr.transform, Vector3.up);
                        dirMgr.PlayFrontStab("openBox", this, intMgr.lst_ovrlapEvnCstr[0].actMgr);
                    }    
                }
                else if (intMgr.lst_ovrlapEvnCstr[0].eventName == "leverUp")
                {
                    if (BattleMgr.CheckAnglePlayer(actCtrl.model, intMgr.lst_ovrlapEvnCstr[0].actMgr.gameObject, 180))
                    {
                        //intMgr.lst_ovrlapEvnCstr[0].active = false;
                        transform.position = intMgr.lst_ovrlapEvnCstr[0].actMgr.transform.position + intMgr.lst_ovrlapEvnCstr[0].actMgr.transform.TransformVector(intMgr.lst_ovrlapEvnCstr[0].offset);
                        actCtrl.model.transform.LookAt(intMgr.lst_ovrlapEvnCstr[0].actMgr.transform, Vector3.up);
                        dirMgr.PlayFrontStab("leverUp", this, intMgr.lst_ovrlapEvnCstr[0].actMgr);
                    }
                }
            }

        }
    }

    public T Bind<T>(GameObject go) where T : IActorMgrInterface {
        T tempInstance;
        if(go == null)
        {
            return null; 
        }
        tempInstance = go.GetComponent<T>();
        if(tempInstance == null){
            tempInstance = go.AddComponent<T>();
        }
        tempInstance.actMgr = this;
        return tempInstance;
    }

    /// <summary>
    /// 遭遇后行为判定方法
    /// </summary>
    /// <param name="targetWpnCtrl"></param>
    public void TryDoDamage(WeaponCtrl targetWpnCtrl,bool attackerValid,bool counterValid)//, bool defenseValid
    {
        if (staMgr.isCounterBackSuccess)//盾反成功
        {
            if (counterValid)
            {
                targetWpnCtrl.wpnMgr.actMgr.Stunned();
            }
        }
        else if (staMgr.isCounterBackFaillure)//盾反失败
        {
            if (attackerValid)
            {
                HitOrDie(targetWpnCtrl,false);
            }
        }
        else if (staMgr.isImmortal)//无敌状态
        {
            //do nothing!
        }
        else if(staMgr.isDefense)//防御状态
        {
            //if (defenseValid){ Block(); }
            Block();
        }
        else//被打或者死亡
        {
            if (attackerValid)
            {
                HitOrDie(targetWpnCtrl,true);
            }
        }
    }

    public void Block()
    {
        actCtrl.IssueTrigger("Block");
    }

    public void Hit()
    {
        actCtrl.IssueTrigger("Hit");
    }

    public void HitOrDie(WeaponCtrl target , bool doHitAnimation)
    {
        if (staMgr.HP <= 0)
        {
            //already dead;
        }
        else
        {
            staMgr.Add(-1 * target.GetAtk());
            if (staMgr.HP > 0)
            {
                if (doHitAnimation)
                {
                    Hit();
                }
                //do some vfx, like splatter blood...
            }
            else
            {
                Die();
            }
        }
    }

    public void Die()
    {
        actCtrl.IssueTrigger("Die");
        actCtrl.IUI.inputEnabled = false;
        if (actCtrl.cameraCtrl.lockState)
        {
            actCtrl.cameraCtrl.LockUnLock();
        }
        actCtrl.cameraCtrl.enabled = false;
    }

    public void Stunned()
    {
        actCtrl.IssueTrigger("Stunned");
    }

    public void LockUnlockActCtrl(bool value)
    {
        actCtrl.SetBoolean("Lock", value);
    }

    /// <summary>
    /// 盾反有效帧状态
    /// </summary>
    /// <param name="value">flag</param>
    public void SetIsCounterBack(bool value)
    {
        staMgr.isCounterBackEnable = value;
    }

    public void ChangeDualHands(bool dualOn){
        if(dualOn){
        actCtrl.anim.runtimeAnimatorController = DblHandAnim;
        }else{
            actCtrl.anim.runtimeAnimatorController = SglHandAnim;
        }
        
    }
}
