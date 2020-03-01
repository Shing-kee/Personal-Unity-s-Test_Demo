using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private DataBase weaponDB;
    private WeaponFactory weaponFactory;

    public WeaponMgr testMgr;

    void Awake()
    {
        CheckGameObject();
        CheckSingle();
    }

    void Start(){
        InitWeaponDB();
        InitWeaponFactory();
        testMgr.UpdateWeaponCollider("Right",weaponFactory.CreateWeapon("Mace","R",testMgr));
        testMgr.ChangeDualHands(false);
    }

    void OnGUI(){
        if(GUI.Button(new Rect(10,10,150,30),"R:Sword")){
            testMgr.UnloadWeapon("Right");
            testMgr.UpdateWeaponCollider("Right",weaponFactory.CreateWeapon("Sword","R",testMgr));
            testMgr.ChangeDualHands(true);
        }
        if(GUI.Button(new Rect(10,60,150,30),"R:Falchion")){
            testMgr.UnloadWeapon("Right");
            testMgr.UpdateWeaponCollider("Right",weaponFactory.CreateWeapon("Falchion","R",testMgr));
            testMgr.ChangeDualHands(true);
        }
        if(GUI.Button(new Rect(10,110,150,30),"R:Mace")){
            testMgr.UnloadWeapon("Right");
            testMgr.UpdateWeaponCollider("Right",weaponFactory.CreateWeapon("Mace","R",testMgr));
            testMgr.ChangeDualHands(false);
        }
        if(GUI.Button(new Rect(10,140,150,30),"R:Unload")){
            testMgr.UnloadWeapon("Right");
        }     
    }

    public void InitWeaponDB(){
        weaponDB = new DataBase();
    }

    public void InitWeaponFactory(){
        weaponFactory = new WeaponFactory(weaponDB);
    }

    private void CheckSingle(){
        if(_instance == null){
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(this);
    }
    private void CheckGameObject(){
        if(tag == "GM"){
            return;
        }
        Destroy(this);
    }


}
