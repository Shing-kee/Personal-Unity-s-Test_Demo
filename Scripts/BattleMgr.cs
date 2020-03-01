using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗管理
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class BattleMgr : IActorMgrInterface
{
    private CapsuleCollider defCol;
    private void Start()
    {
        //定义好指定的collider大小
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up * 1.0f;
        defCol.height = 2.0f;
        defCol.radius = 0.25f;
        defCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponCtrl targetWpnCtrl = other.GetComponentInParent<WeaponCtrl>();

        if(targetWpnCtrl == null)
        {
            return;
        }
        GameObject target = targetWpnCtrl.wpnMgr.gameObject;
        GameObject player = actMgr.wpnMrg.gameObject;
       
        if (other.tag =="Weapon")
        {
            actMgr.TryDoDamage(targetWpnCtrl,CheckAngleTarget(player,target,60),CheckAnglePlayer(player,target,30)); 
        }
    }

    public static bool CheckAnglePlayer(GameObject player,GameObject target, float playerAngleLimit)
    {
        Vector3 counterDir = target.transform.position - player.transform.position;

        float counterAngle1 = Vector3.Angle(player.transform.forward, counterDir);
        float counterAngle2 = Vector3.Angle(target.transform.forward, player.transform.forward);

        bool counterValid = (counterAngle1 < playerAngleLimit && Mathf.Abs(counterAngle2 - 180) < playerAngleLimit);

        return counterValid;
        //Vector3 defensingDir = target.transform.position - player.transform.position;
        //float defensingAngle1 = Vector3.Angle(target.transform.forward, player.transform.forward);
        //bool defenceValid = (Mathf.Abs(defensingAngle1 - 180) < 36);
    }

    public static bool CheckAngleTarget(GameObject player,GameObject target,float playerAngleLimit)
    {
        Vector3 attackingDir = player.transform.position - target.transform.position;

        float attackingAngle1 = Vector3.Angle(target.transform.forward, attackingDir);

        bool attackValid = (attackingAngle1 < playerAngleLimit);
        return attackValid;
    }
}
