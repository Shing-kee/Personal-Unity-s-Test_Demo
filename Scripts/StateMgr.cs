using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态管理
/// </summary>
public class StateMgr : IActorMgrInterface
{
    public float HPMax = 20.0f;
    public float HP = 20.0f;
    public float ATK = 10.0f;

    [Header("1st order state flags")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isDefense;
    public bool isHit;
    public bool isBlock;
    public bool isDie;
    public bool isCounterBack;
    public bool isCounterBackEnable;

    [Header("2nd order state flags")]
    public bool isAllowDefense;
    public bool isImmortal;
    public bool isCounterBackSuccess;
    public bool isCounterBackFaillure;

    private void Start()
    {
        HP = HPMax;
    }
    private void Update()
    {
        #region 状态旗
        isGround = actMgr.actCtrl.CheckState("ground");
        isJump = actMgr.actCtrl.CheckState("Jump");
        isFall = actMgr.actCtrl.CheckState("fall");
        isRoll = actMgr.actCtrl.CheckState("roll");
        isJab = actMgr.actCtrl.CheckState("jab");
        isAttack = actMgr.actCtrl.CheckStateTag("AttackR") || actMgr.actCtrl.CheckStateTag("AttackL");
        isHit = actMgr.actCtrl.CheckState("hit3");
        isBlock = actMgr.actCtrl.CheckState("blocked");
        isDie = actMgr.actCtrl.CheckState("die");
        isCounterBack = actMgr.actCtrl.CheckState("counterBack");
        isCounterBackSuccess = isCounterBackEnable;
        isCounterBackFaillure = isCounterBack && !isCounterBackEnable;

        isAllowDefense = isDefense || isGround;
        isDefense = isAllowDefense && actMgr.actCtrl.CheckState("Ybot|shieldUp", "Defense");
        isImmortal = isRoll || isJab;
        #endregion
    }

    public void Add(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);       
    }
}
