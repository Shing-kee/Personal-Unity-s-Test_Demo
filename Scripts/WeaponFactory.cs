using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory {
    private DataBase weaponDB;
    public WeaponFactory(DataBase _weaponDB){
        this.weaponDB = _weaponDB;
    }

    public GameObject CreateWeapon(string weaponName,Vector3 pos,Quaternion rot){
        GameObject prefab = Resources.Load(weaponName) as  GameObject;
        GameObject obj = GameObject.Instantiate(prefab,pos,rot);

        WeaponData wpnDat = obj.AddComponent<WeaponData>();
        wpnDat.ATK = weaponDB.WeaponDatabase[weaponName]["ATK"].f;

        return obj;
    }

    public Collider CreateWeapon(string weaponName,string side,WeaponMgr wpnMgr){
        WeaponCtrl weaponCtrl;

        if(side == "L"){
            weaponCtrl = wpnMgr.wpnCtrlL; 
        }else if(side == "R"){
            weaponCtrl = wpnMgr.wpnCtrlR;
            Debug.Log("weaponCtrl:"+weaponCtrl + ",WeaponMgr:" +wpnMgr + ",weaponCtrlR:"+wpnMgr.wpnCtrlR);
        }
        else{
            return null;
        }

        GameObject prefab = Resources.Load(weaponName) as  GameObject;
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.parent = weaponCtrl.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        WeaponData wpnDat = obj.AddComponent<WeaponData>();
        wpnDat.ATK = weaponDB.WeaponDatabase[weaponName]["ATK"].f;
        weaponCtrl.wData = wpnDat;

        Collider result = obj.GetComponent<Collider>();

        return result;
    }
}
