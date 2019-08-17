using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Character : MonoBehaviourPun, IPunObservable
{
    public Data data;

    [Header("Photon 元件")]
    public PhotonView pv;
    [Header("玩家法術物件")]
    public GameObject FireObject;
    [Header("hpBar")]
    public Image hpBar;

    float hp;
    float maxHp;
    float speed;
    //public float hitSpeed;
    string chkTag;
    string chkTowerTag;
    float RPCspeed;
    bool IsStop = false;
    bool RPCGetIsStop = false;
    float RPChp;
    GameManager GMScript;

    public float NoMeHp
    {
        get => hp;
        //set => hp = value;
    }

    void Start()
    {
        //Debug.Log("ssss");
        speed = data.speed;
        hp = data.hp;
        maxHp = data.hp;
        //NoMeHp = hp;    //一開始的血量,要給值
        if (pv.IsMine)
        {
            chkTag = "P2";
            chkTowerTag = "MainTower";
        }
        else
        {
            chkTag = "P1";
            chkTowerTag = "SildTower";
        }

        //因為選角色時會引用到這支程式,但遊戲管理器物件在Game場景下,所以加入判斷
        if (GameObject.Find("遊戲管理器") != null) GMScript = GameObject.Find("遊戲管理器").GetComponent<GameManager>();
    }

    private void DelayDestroy()
    {
        // 伺服器.刪除(物件); - 用伺服器生成的物件必須透過伺服器刪除。
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// 固定頻率執行事件
    /// </summary>
    private void FixedUpdate()
    {
        Move(speed);
        ChkSildHurt();
        ChkTowerHurt();
    }

    private void OnCollisionEnter2D(Collision2D hit)
    {
        Debug.Log("進入碰撞!");


    }
   
    private void ChkSildHurt()
    {
        int layerMask = ~(LayerMask.GetMask("玩家1"));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, data.hitArea, layerMask);
        
        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.red, data.hitArea, true);
            
            string tmpChkTag = (transform.tag == "P2") ? "P1" : "P2";
            //先抓到對方的角色
            if (hit.collider.gameObject.tag != transform.tag && hit.collider.name.Substring(0, 4) == "Role")
            {
                    //自己的角色停住
                    speed = 0;
                    //自己的PUN角色停住
                    pv.RPC("StopLeftRole", RpcTarget.All, true, transform.GetComponent<PhotonView>().ViewID);

                //sck start 應該往前跑到自己角色前才停住,目前暫時不停住,之後再修改
                ////自己畫面的對方角色
                //hit.collider.gameObject.GetComponent<Character>().speed = 0;
                ////自己畫面的對方PUN角色
                //pv.RPC("StopLeftRole", RpcTarget.All, true, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
               

                if (data.name == "Role1")
                {
                    transform.GetComponent<Animator>().SetBool("atk", true);
                    pv.RPC("MyFire", RpcTarget.All, true, transform.GetComponent<PhotonView>().ViewID);
                }

                //扣自己血,遠端攻擊的角色,在遠方就會停住,發射攻擊,所以不會被對方近端攻擊
                if (data.hitArea == 1)// && hit.collider.gameObject.GetComponent<Character>().data.hitArea == 1) //當自己的攻擊距離>1(遠端),且對方攻擊距離=1(近端),不會受傷
                {
                    float MyHurt = hit.collider.gameObject.GetComponent<Character>().data.damage;
                    hurt(MyHurt);
                    pv.RPC("SideHurt", RpcTarget.All, MyHurt, transform.GetComponent<PhotonView>().ViewID);
                    if (hp <= 0)
                    {
                        Destroy(transform.gameObject);
                        //殺死PUN中的自己
                        pv.RPC("DelMyRole", RpcTarget.All, transform.GetComponent<PhotonView>().ViewID);
                    }
                }
                else {

                }

                //扣對方血
                //.Log(chkTag + ".打到Role--->." + hit.collider.gameObject.tag + "...." + transform.name);
                if (hit.collider.gameObject.GetComponent<Character>().hp <= 0)
                {
                    //如果玩家角色被打死,自己的角色要往前走
                    PhotonNetwork.Destroy(hit.collider.gameObject);
                    speed = data.speed;
                    //自己的PUN角色往前走
                    pv.RPC("StopLeftRole", RpcTarget.All, false, transform.GetComponent<PhotonView>().ViewID);
                    if (data.name == "Role1")
                    {
                        transform.GetComponent<Animator>().SetBool("atk", false);
                        pv.RPC("MyFire", RpcTarget.All, false, transform.GetComponent<PhotonView>().ViewID);
                    }
                }
                else
                {
                    //扣自己畫面中,對方角色的血
                    //hit.collider.gameObject.GetComponent<Character>().hurt(data.damage);
                    //扣對方畫面中,角色的血
                    pv.RPC("SideHurt", RpcTarget.All, data.damage, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                }
            }
        }
    }

    [PunRPC]
    public virtual void StopLeftRole(bool IsStop, int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].name.Substring(0, 4) == "Role")
            {
                if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                {
                    float tmpSpeed = PlayerTarget[i].GetComponent<Character>().data.speed;
                    if (IsStop)
                    {
                        PlayerTarget[i].GetComponent<Character>().speed = 0;
                    }
                    else
                    {
                        PlayerTarget[i].GetComponent<Character>().speed = tmpSpeed;
                    }
                }
            }
        }
    }

    private void ChkTowerHurt()
    {
        int layerMask = ~(LayerMask.GetMask("玩家1"));
        string tmpChkTag = (pv.IsMine) ? "P1" : "P2"; ;
        
        if (pv.IsMine)
        {
            //己方畫面,自己角色射線
            RaycastHit2D[] MyHitSideTower = Physics2D.RaycastAll(transform.position, Vector2.right, 1, layerMask);
            for (int i = 0; i < MyHitSideTower.Length; i++)
            {
                //1-1 Role to 1-2 Towe,同時,2-2 Role to (會先碰到2-2 Tower 要略過)
                if (MyHitSideTower[i].collider.gameObject.name == "SideTower" && transform.tag != tmpChkTag)
                {

                    Debug.Log("1-1:自己的角色是 " + transform.name + ", 自己的標籤是 " + transform.tag + ", 反面標籤是 " + tmpChkTag + ",SideTower的標籤是 " + GameObject.Find("SideTower").tag + ", 打到 Tower的標籤是 " + MyHitSideTower[i].collider.gameObject.name);
                    Debug.DrawLine(transform.position, MyHitSideTower[i].collider.transform.position, Color.blue, 1, true);

                    //抓到要攻擊的塔的血量
                    if (GMScript.SideBlood > 0)
                    {
                        //自己的角色停住
                        speed = 0;
                        //自己的PUN角色停住
                        //pv.RPC("StopLeftRole", RpcTarget.All, true, transform.GetComponent<PhotonView>().ViewID);

                        //pv.RPC("RPCMove", RpcTarget.All, 0.0f); //  PUN方角色停住
                        //transform.GetComponent<Animator>().SetBool("att", false);
                        GMScript.SideTowerHurt(data.damage);
                    }
                }
            }
        }
        else {
            //己方畫面,1-2對方角色射線
            RaycastHit2D[] MyHitSideTower = Physics2D.RaycastAll(transform.position, Vector2.left, 1, layerMask);
            for (int i = 0; i < MyHitSideTower.Length; i++)
            {
                //1-2 Role to 1-1 Towe,同時,2-1 Role to (會先碰到2-1 Tower 要略過)
                if (MyHitSideTower[i].collider.gameObject.name == "MainTower" && transform.tag != tmpChkTag)
                {

                    Debug.Log("1-2:自己的角色是 " + transform.name + ", 自己的標籤是 " + transform.tag + ", 反面標籤是 " + tmpChkTag + ",SideTower的標籤是 " + GameObject.Find("SideTower").tag + ", 打到 Tower的標籤是 " + MyHitSideTower[i].collider.gameObject.name);
                    Debug.DrawLine(transform.position, MyHitSideTower[i].collider.transform.position, Color.green, 1, true);

                    //抓到要攻擊的塔的血量
                    if (GMScript.MainBlood > 0)
                    {
                        //自己的角色停住
                        speed = 0;
                        GMScript.MainTowerHurt(data.damage);
                    }
                }
            }
        }
    }

    public virtual void Move(float SetSpeed)
    {
        transform.Translate(transform.right * Time.deltaTime * SetSpeed, Space.World);

    }

    [PunRPC]
    public virtual void DelMyRole(int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].name.Substring(0, 4) == "Role")
            {
                if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                {
                    Destroy(PlayerTarget[i]);
                }
            }
        }
    }

    [PunRPC]
    public virtual void MyFire(bool IsFire, int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].name.Substring(0, 4) == "Role")
            {
                if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                {
                    PlayerTarget[i].GetComponent<Animator>().SetBool("atk", IsFire);
                }
            }
        }
    }

    [PunRPC]
    public virtual void SideHurt(float damage,int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].name.Substring(0, 4) == "Role") {
                if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                {
                    PlayerTarget[i].gameObject.GetComponent<Character>().hurt(data.damage);
                }
            }
        }
    }

    public virtual void hurt(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            hpBar.fillAmount = hp / maxHp;
        }
    }

    public void CreatFireObjaect()
    {
        Instantiate(FireObject, new Vector3(transform.position.x + 1, transform.position.y + .5f, 0), transform.rotation);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //傳遞資料
            //stream.SendNext(String.Join(",", StaticVar.Roles.ToArray()));
        }
        // 如果 正在讀取資料
        else if (stream.IsReading)
        {
        }
    }
}
