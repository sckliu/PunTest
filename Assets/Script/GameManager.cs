using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class GameManager : MonoBehaviourPun, IPunObservable
{
    [Header("部隊")]
    public GameObject[] Soldier = new GameObject[6];
    [Header("呼叫物件按鈕")]
    public Button[] btnTmp;
    [Header("主塔血量")]
    public float MainBlood;
    [Header("主塔總血量")]
    public float MainMaxBlood;
    [Header("主塔hpBar")]
    public GameObject MainHpBar;
    [Header("對手主塔血量")]
    public float SideBlood;
    [Header("對手主塔總血量")]
    public float SideMaxBlood;
    [Header("對手主塔hpBar")]
    public GameObject SideHpBar;

    #region 小兵設定區
    [Header("小兵按鈕-限制花費分數")]
    public float[] buttonCost = new float[6];
    [Header("小兵按鈕-召喚所需費用")]
    public float[] cost = new float[6];

    #endregion

    #region 同步設定區
    [Header("Photon 元件")]
    public PhotonView pv;
    [Header("生成位置-主玩家")]
    public Transform CreateRolePoint1;
    [Header("生成位置-玩家")]
    public Transform CreateRolePoint2;
    [Header("主塔Photon 元件")]
    public PhotonView MainPv;
    private string SendRoles;

    private List<string> nRoles;
    #endregion

    public float height { get; set; }

    private void Start()
    {
        StaticVar.MainBlood = MainBlood;
        StaticVar.btnArr = btnTmp;
        
        for (int i = 0; i < StaticVar.Roles.Count; i++)
        {
            string btn_img = StaticVar.Roles[i].Replace("Role", "btn_img");
            btnTmp[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(btn_img);    //按鈕圖
            btnTmp[i].transform.Find("Filled").GetComponent<Image>().sprite = Resources.Load<Sprite>(btn_img); //按鈕-開場先抓CD冷卻黑圖
            Soldier[i] = Resources.Load<GameObject>(StaticVar.Roles[i]);
        }
    }
    public bool test = false;
    public bool IsFightTower = false;
    public void CharacterSoldier(int index)
    {

        GameObject Role = PhotonNetwork.Instantiate(StaticVar.Roles[index], CreateRolePoint2.position, new Quaternion(0, 90, 0, 0), 0);

        //給標籤,用來分辨是否是自己的
        Role.tag = (pv.IsMine) ? "P1" : "P2";
        pv.RPC("AddTag", RpcTarget.All, Role.tag, Role.GetComponent<PhotonView>().ViewID);

        GameObject.Find("MainTower").tag = Role.tag;
        GameObject.Find("SideTower").tag = (Role.tag == "P2") ? "P1" : "P2";

        ////剛生成的時候,先忽略所遇到的塔
        Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>()); //自己畫面
        Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>()); //自己畫面
        pv.RPC("RPCIgnoreCollider", RpcTarget.All, true, Role.GetComponent<PhotonView>().ViewID);   //對方畫面,RPC的自己
        
        Role.transform.position = CreateRolePoint1.position;
        Role.transform.rotation = CreateRolePoint1.rotation;

        ////移動位置後,將對方塔打開,忽略自己塔(不要忽略對方塔)-自己的畫面
        Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>(), false);
        //對方畫面,RPC的自己,不要忽略對方主塔
        pv.RPC("RPCIgnoreCollider", RpcTarget.All, false, Role.GetComponent<PhotonView>().ViewID);
        
    }

    [PunRPC]
    private void RPCIgnoreCollider(bool IsIgnore, int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].name.Substring(0, 4) == "Role")
            {
                if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                {

                    if (IsIgnore)
                    {
                        //Debug.Log("PUN生成的物件--000--" + IsIgnore);
                        Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>());
                        Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>());
                        //Physics2D.IgnoreLayerCollision(8, 9);
                    }
                    else
                    {
                        //Debug.Log("PUN生成的物件--000--" + IsIgnore);
                        //Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>(), false);
                        Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>(), false);
                        //Physics2D.IgnoreLayerCollision(8, 9, false);
                    }
                }
            }
        }
    }

    [PunRPC]
    public virtual void AddTag(string tag,int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].name.Substring(0, 4) == "Role")
            {
                if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                {
                    PlayerTarget[i].tag = tag;
                }
            }
        }
    }
    
    /// <summary>
    /// 固定頻率執行事件
    /// </summary>
    private void FixedUpdate()
    {
        // 如果 是自己的物件
        if (pv.IsMine)
        {
        }
        else
        {
        }
    }

    //1-2主塔扣血
    public virtual void SideTowerHurt(float damage)
    {
        if (SideBlood >= 0) {

            SideBlood -= damage;
            SideHpBar.GetComponent<Image>().fillAmount = SideBlood / SideMaxBlood;

            //2-1
            pv.RPC("RPCMainTowerHurt", RpcTarget.All, SideBlood);
        }
    }


    //1-1主塔扣血
    public virtual void MainTowerHurt(float damage)
    {
        if (MainBlood >= 0)
        {

            MainBlood -= damage;
            MainHpBar.GetComponent<Image>().fillAmount = MainBlood / MainMaxBlood;

            //2-2
            pv.RPC("RPCSideTowerHurt", RpcTarget.All, MainBlood);
        }
    }

    //RPC 2-2 對方畫面,我們的主塔扣血
    [PunRPC]
    void RPCSideTowerHurt(float BloodRate, PhotonMessageInfo info)
    {
        if (pv.IsMine)
        {
            SideHpBar.GetComponent<Image>().fillAmount = BloodRate;
        }
    }

    //對方主塔扣血
    [PunRPC]
     void RPCMainTowerHurt(float BloodRate, PhotonMessageInfo info)
    {
        if (pv.IsMine)
        {
            MainHpBar.GetComponent<Image>().fillAmount = BloodRate;
        }
    }


    // 同步資料方法
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 如果 正在寫入資料
        if (stream.IsWriting)
        {
            //傳遞(寫入)資料
            //stream.SendNext(String.Join(",", StaticVar.Roles.ToArray()));
        }
        // 如果 正在讀取資料
        else if (stream.IsReading)
        {
            
            //if (nRoles != null) {

            //    string[] tmpRoles = ((string)stream.ReceiveNext()).Split(',');

            //    for (int i = 0; i < tmpRoles.Length; i++)
            //    {
            //        nRoles.Add(tmpRoles[i]);
            //    }
            //}
            
            //nRoles = (List<string>)stream.ReceiveNext();
            
            //Debug.Log("讀取資料");
        }
    }

}
