using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器管理
/// </summary>
public class WeaponMgr : IActorMgrInterface
{
    private Collider weaponColL;
    [SerializeField]
    private Collider weaponColR;
    public GameObject wpnHdlLeft;  
    public GameObject wpnHdlRight;
    public WeaponCtrl wpnCtrlL;
    public WeaponCtrl wpnCtrlR;

    private void Start()
    {
        try
        {
            wpnHdlLeft = transform.DeepFind("weaponHandleL").gameObject;
            wpnCtrlL = BindWeaponController(wpnHdlLeft);
            weaponColL = wpnHdlLeft.GetComponentInChildren<Collider>();
            weaponColL.enabled = false;
        }
        catch (System.Exception)
        {
            //
            // if there is no "wenponHandle" or related object
            //
        }
        try
        {
            wpnHdlRight = transform.DeepFind("weaponHandleR").gameObject;
            wpnCtrlR = BindWeaponController(wpnHdlRight);
            weaponColR = wpnCtrlR.GetComponentInChildren<Collider>();
            weaponColR.enabled = false;
        }
        catch (System.Exception)
        {
            //
            // if there is no "wenponHandle" or related object
            //
        }
    }

    /// <summary>
    /// 绑定武器控制器
    /// </summary>
    /// <param name="targetObject">目标物体</param>
    /// <returns></returns>
    public WeaponCtrl BindWeaponController(GameObject targetObject)
    {
        WeaponCtrl tempWpnCtrl;
        tempWpnCtrl = targetObject.GetComponent<WeaponCtrl>();
        if(tempWpnCtrl == null)
        {
            tempWpnCtrl = targetObject.AddComponent<WeaponCtrl>();
        }
        tempWpnCtrl.wpnMgr = this;
        return tempWpnCtrl;
    }

    public void UpdateWeaponCollider(string side,Collider col){
        col.enabled = false;
        if(side == "Left"){
            weaponColL = col;
        }
        if(side == "Right"){
            weaponColR = col;
        }
    }

    public void UnloadWeapon(string side){
        if(side == "Left"){
             if(wpnCtrlR.transform.childCount <= 0){}
             else{
                foreach(Transform trans in wpnHdlLeft.transform){
                    weaponColL = null;
                    wpnCtrlL = null;
                    Destroy(trans.gameObject);
                }
            }
        }else if(side == "Right"){
            if(wpnCtrlR.transform.childCount <= 0){}
            else{
                foreach (Transform trans in wpnHdlRight.transform)
                {
                    weaponColR = null;
                    wpnCtrlR.wData = null;
                    Destroy(trans.gameObject);
                }
            }      
        }
    }

    /// <summary>
    /// 关闭武器碰撞机
    /// </summary>
    public void WeaponDisable()
    {
        if(actMgr.actCtrl.CheckStateTag("AttackL")){
            weaponColL.enabled = false;
        }else if(actMgr.actCtrl.CheckStateTag("AttackR")){
            weaponColR.enabled = false;
        }
    }
    
    /// <summary>
    /// 打开武器碰撞机
    /// </summary>
    public void WeaponEnable()
    {
        if (actMgr.actCtrl.CheckStateTag("AttackL"))
        {
            weaponColL.enabled = true;
        }
        else if(actMgr.actCtrl.CheckStateTag("AttackR"))
        {
            weaponColR.enabled = true;
        }
    }

    public void CounterBackEnable()
    {
        actMgr.SetIsCounterBack(true);
    }

    public void CounterBackDisable()
    {
        actMgr.SetIsCounterBack(false);
    }

    public void ChangeDualHands(bool dualOn){
        actMgr.ChangeDualHands(dualOn);
    }
}
