using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticVar : MonoBehaviour
{
    [Header("角色預製件")]
    static public GameObject[] SelectedCharacters;

    //靜態變數添加選擇角色到清單裡
    static public List<string> Roles = new List<string>();

    [Header("呼叫物件按鈕")]
    static public Button[] btnArr;
    [Header("主塔血量")]
    static public float MainBlood;

    //sck test end
    static public void AddTestVar() {
        StaticVar.AddVar("Role1");
        StaticVar.AddVar("Role2");
        StaticVar.AddVar("Role3");
        StaticVar.AddVar("Role4");
        StaticVar.AddVar("Role5");
        StaticVar.AddVar("Role6");
    }

    static public void AddTestVar2()
    {
        StaticVar.AddVar("Role6");
        StaticVar.AddVar("Role5");
        StaticVar.AddVar("Role4");
        StaticVar.AddVar("Role3");
        StaticVar.AddVar("Role2");
        StaticVar.AddVar("Role1");
    }

    static public void AddVar(string RoleName)
    {
        Roles.Add(RoleName);
    }
}
