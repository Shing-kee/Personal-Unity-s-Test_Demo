using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    public WeaponMgr wpnMgr;
    public WeaponData wData; 

    private void Awake()
    {
        wData = GetComponentInChildren<WeaponData>();
    }

    public float GetAtk()
    {
        return wData.ATK + wpnMgr.actMgr.staMgr.ATK;
    }
}
