using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase 
{
    public string weaponDatabaseFileName = "json";
    public readonly JSONObject jsonDataBase;
    private JSONObject weaponDatabase;

    public JSONObject WeaponDatabase{
        get{
            return weaponDatabase;
        }
    }

    public DataBase()
    {
        TextAsset myText = Resources.Load("abc") as TextAsset;
        weaponDatabase = new JSONObject(myText.text); 
        // Debug.Log(
    }            
}
