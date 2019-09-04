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
    [Header("遊戲結束 輸贏")]
    public Text OverText;
    [Header("遊戲結束UI")]
    public GameObject GameOverUI;

    //[Header("初始時間")]
    //public float StartTime;
    //[Header("時間物件")]
    //public Image Clock_Time;
    //private float TimeSpeed = 1;
    //private float MaxTime;

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

    public int GetRole = 0;
    public int GetRoleCount = 6;
    public bool test = false;
    #endregion

    public float height { get; set; }

    private void Start()
    {
        //MaxTime = StartTime;
        StaticVar.MainBlood = MainBlood;

        StaticVar.btnArr = btnTmp;
        if (!test) {

            if (pv.IsMine)
            {
                GetRole = 6;
                GetRoleCount = StaticVar.Roles.Count;
            }
        }

        for (int i = GetRole; i < GetRole + 6; i++)
        {

            //Debug.Log("User btnTmp:" + i +"---"+ StaticVar.Roles[i] + "..." + StaticVar.Roles[i].Replace("Role", "btn_img"));
            string btn_img = StaticVar.Roles[i].Replace("Role", "btn_img");
            //Debug.Log("User btnTmp:" + btnTmp[i].GetComponent<Image>().sprite);
            int tmp = i;
            if (!test)
            {
                if (pv.IsMine)
                {
                    tmp = i - 6;
                    GetRoleCount = StaticVar.Roles.Count;
                }
            }

            btnTmp[tmp].GetComponent<Image>().sprite = Resources.Load<Sprite>(btn_img);    //按鈕圖
            btnTmp[tmp].transform.Find("Filled").GetComponent<Image>().sprite = Resources.Load<Sprite>(btn_img); //按鈕-開場先抓CD冷卻黑圖
            Soldier[tmp] = Resources.Load<GameObject>(StaticVar.Roles[i]);
        }
    }
    public void CharacterSoldier(int index)
    {

        GameObject Role = PhotonNetwork.Instantiate(StaticVar.Roles[GetRole + index], CreateRolePoint2.position, new Quaternion(0, 90, 0, 0), 0);
        //GameObject Role = PhotonNetwork.Instantiate(StaticVar.Roles[index], CreateRolePoint2.position, new Quaternion(0, 90, 0, 0), 0);

        //給標籤,用來分辨是否是自己的
        Role.tag = (pv.IsMine) ? "P1" : "P2";
        pv.RPC("AddTag", RpcTarget.All, Role.tag, Role.GetComponent<PhotonView>().ViewID);

        GameObject.Find("MainTower").tag = Role.tag;
        GameObject.Find("SideTower").tag = (Role.tag == "P2") ? "P1" : "P2";

        //忽略自己
        Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), Role.GetComponent<Collider2D>());
        Physics2D.showColliderContacts = true;
        //忽略己方(P1或P2)
        GameObject[] P = GameObject.FindGameObjectsWithTag(Role.tag);
        foreach (GameObject g in P)
        {
            Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), g.GetComponent<Collider2D>());
        }

        pv.RPC("RPCIgnoreMySildCollider", RpcTarget.All, false, Role.GetComponent<PhotonView>().ViewID);

        ////剛生成的時候,先忽略所遇到的塔
        Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>()); //自己畫面
        Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>()); //自己畫面
                                                                                                                             //pv.RPC("RPCIgnoreCollider", RpcTarget.All, true, Role.GetComponent<PhotonView>().ViewID,"");   //對方畫面,RPC的自己

        Role.transform.position = CreateRolePoint1.position;
        Role.transform.rotation = CreateRolePoint1.rotation;

        ////移動位置後,將對方塔打開,忽略自己塔(不要忽略對方塔)-自己的畫面
        //Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>(), false);
        //對方畫面,RPC的自己,不要忽略對方主塔

        //Debug.Log(Role.tag + "--------------------------------------------->" + pv.IsMine);
        //if (Role.tag == "P1")
        //{
        //    Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>());
        //}
        if (pv.IsMine)
        {
            pv.RPC("RPCIgnoreCollider", RpcTarget.All, false, Role.GetComponent<PhotonView>().ViewID, "MainTower");
            if (Role.tag == "P1") {

                Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>(), false);
            }
            //Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>(), false);
        }
        else
        {
            //pv.RPC("RPCIgnoreCollider", RpcTarget.All, false, Role.GetComponent<PhotonView>().ViewID, "SideTower");
            if (Role.tag == "P2")
            {

                Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>(), false);
            }
            //Physics2D.IgnoreCollision(Role.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>(), false);
        }


    }
    [PunRPC]
    private void RPCIgnoreMySildCollider(bool IsIgnore, int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].name.Substring(0, 4) == "Role")
            {
                if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                {
                    Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), PlayerTarget[i].GetComponent<Collider2D>());
                    GameObject[] P = GameObject.FindGameObjectsWithTag(PlayerTarget[i].tag);
                    foreach (GameObject g in P)
                    {
                        Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), g.GetComponent<Collider2D>());
                    }
                }
            }
        }
    }

    [PunRPC]
    private void RPCIgnoreCollider(bool IsIgnore, int pvID,string Tower)
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
                        //GameObject[] P = GameObject.FindGameObjectsWithTag(PlayerTarget[i].tag);
                        //foreach (GameObject g in P)
                        //{
                        //    Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), g.GetComponent<Collider2D>());
                        //}

                        //Debug.Log("PUN生成的物件--000--" + IsIgnore);
                        Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>());
                        Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>());
                        //Physics2D.IgnoreLayerCollision(8, 9);
                    }
                    else
                    {
                        //Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find(Tower).GetComponent<Collider2D>(), false);
                        
                        //Physics2D.IgnoreCollision(PlayerTarget[i].GetComponent<Collider2D>(), GameObject.Find("MainTower").GetComponent<Collider2D>(), false);
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
        //Clock_walking();
        // 如果 是自己的物件
        if (pv.IsMine)
        {
        }
        else
        {
        }

        if (MainBlood <= 0 || SideBlood <= 0) {
            ClockGameOver();
        }
        
    }

    //public void Clock_walking()
    //{
    //    MaxTime -= TimeSpeed * Time.deltaTime;
    //    Clock_Time.fillAmount = MaxTime / StartTime;
    //}

    //1-2主塔扣血
    public virtual void SideTowerHurt(float damage)
    {
        if (SideBlood >= 0) {

            SideBlood -= damage;
            SideHpBar.GetComponent<Image>().fillAmount = SideBlood / SideMaxBlood;
            //Debug.Log("1-2:主塔血量 " + SideBlood);
            //2-1
            if (!pv.IsMine)
            {
                pv.RPC("RPCMainTowerHurt", RpcTarget.All, SideBlood);
            }
            else {
                //MainTowerHurt(damage);
                //pv.RPC("RPCSideTowerHurt", RpcTarget.All, SideBlood);
                //pv.RPC("RPCMainTowerHurt", RpcTarget.All, SideBlood);
                //pv.RPC("RPCSideTowerHurt", RpcTarget.All, 0);
            }
        }
    }


    //1-1主塔扣血
    public virtual void MainTowerHurt(float damage)
    {
        if (MainBlood >= 0)
        {

            MainBlood -= damage;
            MainHpBar.GetComponent<Image>().fillAmount = MainBlood / MainMaxBlood;
            if (!pv.IsMine)
            {
                //2-2
                pv.RPC("RPCSideTowerHurt", RpcTarget.All, MainBlood);
            }
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

    
    public void ClockGameOver()
    {
        if (MainBlood > SideBlood)
        {
            GameOver(true);
        }
        else {
            GameOver(false);
        }
    }

    public void GameOver(bool over)
    {
        GameOverUI.SetActive(true);
        if (over)
        {
            OverText.text = "你贏了";
        }
        else
        {
            OverText.text = "你輸了";
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
