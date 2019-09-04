using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Threading;

public class Character : MonoBehaviourPun, IPunObservable
{
    public Data data;

    [Header("Photon 元件")]
    public PhotonView pv;
    [Header("Rigidbody2D 元件")]
    public Rigidbody2D ri;
    [Header("玩家法術物件")]
    public GameObject FireObject;
    [Header("hpBar")]
    public Image hpBar;

    public float hp;
    float maxHp;
    public float speed;
    //public float hitSpeed;
    float RPCspeed;
    float RPChp;
    GameManager GMScript;

    void Start()
    {

        //Debug.Log("ssss");
        speed = data.speed;
        //一開始的血量,要給值
        hp = data.hp;
        maxHp = data.hp;
        
        //因為選角色時會引用到這支程式,但遊戲管理器物件在Game場景下,所以加入判斷
        if (GameObject.Find("遊戲管理器") != null) GMScript = GameObject.Find("遊戲管理器").GetComponent<GameManager>();
    }
    
    private void DelayDestroy()
    {
        // 伺服器.刪除(物件); - 用伺服器生成的物件必須透過伺服器刪除。
        PhotonNetwork.Destroy(gameObject);
    }

    private float delay = 0;
    private float delayEnd = 5;
    /// <summary>
    /// 固定頻率執行事件
    /// </summary>
    private void FixedUpdate()
    {
        Move(speed);
        ChkHurtAll();
        //Invoke("ChkHurtAll", 1f);//設定延遲時間
        //ChkSildHurt();
        //testTower();
        delay += 5 * Time.deltaTime;
        if (delay >= delayEnd)
        {
            ChkTowerHurt();
        }
        
    }

    float CountTime = 0;

    private void ChkHurtAll() {
        int layerMask = ~(LayerMask.GetMask("玩家1"));
        Vector2 way = (!pv.IsMine) ? Vector2.left : Vector2.right;
        string tmpChkTag = (transform.tag == "P2") ? "P1" : "P2";

        //RaycastHit2D[] hit = Physics2D.RaycastAll(new Vector2(ri.position.x + 1.8f, ri.position.y), way, data.hitArea, layerMask);
        RaycastHit2D[] hit = Physics2D.RaycastAll(ri.position, way, data.hitArea, layerMask | 256);

        bool flog = false;
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.tag != transform.tag && hit[i].collider.name.Substring(0, 4) == "Role")
            {
                flog = true;
            }
         }

        if (flog)
        {
            //扣血
            //Debug.Log(chkTag + ".扣血----------------->.");
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.tag != transform.tag && hit[i].collider.name.Substring(0, 4) == "Role")
                {
                    Debug.DrawLine(ri.position, hit[i].point, Color.grey, data.hitArea, true);

                    if (hp <= 0)    //1-1自己的血沒了
                    {
                        //PhotonNetwork.Destroy(transform.gameObject);
                        Destroy(transform.gameObject);
                    }
                    else
                    {
                        //還有血,要停下並攻擊
                        //Debug.Log(transform.name+"---------------------------------------------------");
                        //1-1自己的角色停下
                        speed = 0;
                        //pv.RPC("StopLeftRole", RpcTarget.All, true, transform.GetComponent<PhotonView>().ViewID);

                        //攻擊->受傷->扣血->後退
                        //開啟攻擊動畫
                        transform.GetComponent<Animator>().SetBool("isAttack", true);
                        //Debug.Log(CountTime + "......before--->.");
                        CountTime += Time.deltaTime;    //因為太快,要延遲動作才會完整
                        //Debug.Log(CountTime + "......next--->.");

                        if (CountTime > 0.5) {

                            //遠端攻擊,很早就停下了,不會扣血,要再偵測一次,如果對方衝過來才扣血(現在暫時直接扣血)
                            if (transform.name.Substring(0, 5) == "Role1" || transform.name.Substring(0, 5) == "Role4" || transform.name.Substring(0, 5) == "Role6" || transform.name.Substring(0, 5) == "Role7")
                            {
                                //自己受傷
                                float MyHurt = hit[i].collider.gameObject.GetComponent<Character>().data.damage;
                                hurt(MyHurt, 0, false);

                            }
                            else
                            {
                                //如果自己是近戰,若對方是近戰,對方後退
                                //如果自己是近戰,若對方是遠戰,對方不後退
                                //如果自己是遠戰,若對方是近戰,對方不後退
                                //如果自己是遠戰,若對方是近戰,對方不後退
                                bool isback = false;
                                if (hit[i].collider.name.Substring(0, 5) == "Role2" || hit[i].collider.name.Substring(0, 5) == "Role3" || hit[i].collider.name.Substring(0, 5) == "Role5" || hit[i].collider.name.Substring(0, 5) == "Role8")
                                {
                                    isback = true;
                                }

                                //讓對方受傷,扣血及退後
                                pv.RPC("SideHurt", RpcTarget.All, data.damage, isback, hit[i].collider.gameObject.GetComponent<PhotonView>().ViewID);
                                //Debug.Log(chkTag + ".sssssss--->." + hit[i].collider.gameObject.GetComponent<Character>().hp + "...." + transform.name);

                                if (isback)
                                {
                                    speed = data.speed;
                                }
                            }
                            CountTime = 0;
                        }
                    }
                }
                
            }
            //end
        }
        else {
            //往前
            speed = data.speed;
            transform.GetComponent<Animator>().SetBool("isAttack", false);
            //Debug.Log(chkTag + ".往前----------------->.");
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
                        //speed = 0;
                        //chgSpeed(0);
                        PlayerTarget[i].GetComponent<Character>().speed = 0;
                    }
                    else
                    {
                        //speed = tmpSpeed;
                        //chgSpeed(tmpSpeed);
                        PlayerTarget[i].GetComponent<Character>().speed = tmpSpeed;
                    }
                    //sck
                    PlayerTarget[i].GetComponent<Animator>().SetBool("isAttack", IsStop);
                }
            }
        }
    }

    private void ChkTowerHurt()
    {
        int layerMask =  ~(LayerMask.GetMask("玩家1"));
        int layerMask2 = ~(LayerMask.GetMask("Tower"));
        //string tmpChkTag = (pv.IsMine) ? "P1" : "P2";
        string tmpChkTag = (transform.tag == "P2") ? "P1" : "P2"; ;

        //Debug.DrawLine(transform.position, transform.position + new Vector3(data.hitArea, 0, 0), Color.blue, 2);
        if (pv.IsMine)
        {
            //Debug.Log("1-1:------ "+ pv.IsMine);
            //己方畫面,自己角色射線
            //Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), GameObject.Find("SideTower").GetComponent<Collider2D>()); //自己畫面
            RaycastHit2D[] MyHitSideTower = Physics2D.RaycastAll(ri.position, Vector2.right, 1, layerMask | 256);
            for (int i = 0; i < MyHitSideTower.Length; i++)
            {
                //Debug.Log("1-1////////////// " + pv.IsMine);
                //1-1 Role to 1-2 Towe,同時,2-2 Role to (會先碰到2-2 Tower 要略過)
                if (MyHitSideTower[i].collider.gameObject.name == "SideTower" && transform.tag != tmpChkTag)
                {

                    //Debug.Log("1-1:自己的角色是 " + transform.name + ", 自己的標籤是 " + transform.tag + ", 反面標籤是 " + tmpChkTag + ",SideTower的標籤是 " + GameObject.Find("SideTower").tag + ", 打到 Tower的標籤是 " + MyHitSideTower[i].collider.gameObject.name);
                    Debug.DrawLine(ri.position, MyHitSideTower[i].collider.transform.position, Color.blue, 5, true);

                    //抓到要攻擊的塔的血量
                    if (GMScript.SideBlood > 0)
                    {
                        //自己的角色停住
                        speed = 0;
                        transform.GetComponent<Animator>().SetBool("isAttack", true);
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
            //Debug.Log("1-2:------ " + pv.IsMine);
            RaycastHit2D[] MyHitSideTower = Physics2D.RaycastAll(ri.position, Vector2.left, data.hitArea, layerMask | 256);
            for (int i = 0; i < MyHitSideTower.Length; i++)
            {
                if (GameObject.Find("SideTower") != null) //因為角色選擇要用,所以加上判斷以防error
                {
                    //Debug.Log("1-2:自己的角色是 " + transform.name + ", 自己的標籤是 " + transform.tag + ", 反面標籤是 " + tmpChkTag + ",SideTower的標籤是 " + GameObject.Find("SideTower").tag + ", 打到 Tower的標籤是 " + MyHitSideTower[i].collider.gameObject.name);
                    //1-2 Role to 1-1 Towe,同時,2-1 Role to (會先碰到2-1 Tower 要略過)
                    if (MyHitSideTower[i].collider.gameObject.name == "MainTower" && transform.tag != tmpChkTag)
                    {

                        //Debug.Log("1-2:自己的角色是 " + transform.name + ", 自己的標籤是 " + transform.tag + ", 反面標籤是 " + tmpChkTag + ",SideTower的標籤是 " + GameObject.Find("SideTower").tag + ", 打到 Tower的標籤是 " + MyHitSideTower[i].collider.gameObject.name);
                        Debug.DrawLine(ri.position, MyHitSideTower[i].collider.transform.position, Color.green, 1, true);

                        //抓到要攻擊的塔的血量

                        //Debug.Log("1-2:自己的主塔血量是 " + GMScript.MainBlood);
                        //GMScript.test111(data.damage);
                        if (GMScript.MainBlood > 0)
                        //if (GMScript.SideBlood > 0)
                        {
                            //Debug.Log("1-2:自己的主塔血量是 " + GMScript.MainBlood);
                            //自己的角色停住
                            speed = 0;
                            transform.GetComponent<Animator>().SetBool("isAttack", true);
                            //GMScript.SideTowerHurt(data.damage);
                            GMScript.MainTowerHurt(data.damage);
                        }
                    }
                }
            }
        }
    }

    [PunRPC]
    public virtual void SideHurt(float damage, bool back, int pvID)
    {
        GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            if (PlayerTarget[i].GetPhotonView() != null) {

                if (PlayerTarget[i].name.Substring(0, 4) == "Role")
                {
                    if (PlayerTarget[i].GetPhotonView().ViewID == pvID)
                    {
                        //PlayerTarget[i].gameObject.GetComponent<Character>().hurt(damage);
                        PlayerTarget[i].gameObject.GetComponent<Character>().hurt(damage, 0, back);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 減少傷害值
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="HurtWay">傷害方式,0近戰攻擊,1遠端攻擊,近戰才退後</param>
    /// <param name="back">退後</param>
    public virtual void hurt(float damage,int HurtWay, bool back)
    {
        //對方死了或後退,自己要前進??
        //自己被打到要後退或死掉,這時對方應該要前進....
        if (hp > 0)
        {
            transform.GetComponent<Animator>().SetTrigger("isHurt");

            hp -= damage;
            hpBar.fillAmount = hp / maxHp;
            
            //GetInstanceID
            //ri.MovePosition(new Vector2(ri.position.x - 10f, ri.position.y));
            ////Move(-data.dia);
            if (HurtWay == 0 && back) {
                //Invoke("MoveBack", .2f);
                float tmp_dia = (pv.IsMine) ? -data.dia : data.dia;
                transform.position = new Vector3(transform.position.x + tmp_dia, transform.position.y, transform.position.z);
                //speed = data.speed;
            }
        }
        else {
            //PhotonNetwork.Destroy(transform.gameObject);
            //Destroy(transform.gameObject);
        }
    }

    public void MoveBack() {

        float tmp_dia = (pv.IsMine) ? -data.dia : data.dia;
        transform.position = new Vector3(transform.position.x + tmp_dia, transform.position.y, transform.position.z);
        //transform.GetComponent<Animator>().SetBool("isAttack", false);

    }

    public virtual void Move(float SetSpeed)
    {
        if (GameObject.Find("SideTower") == null)
        {
            SetSpeed = 0;
        }

        //transform.Translate(transform.right * Time.deltaTime * SetSpeed, Space.World);
        if (pv.IsMine)
        {
            ri.MovePosition(ri.position + Vector2.right * SetSpeed * Time.fixedDeltaTime);
        }
        else
        {
            ri.MovePosition(ri.position + Vector2.left * SetSpeed * Time.fixedDeltaTime);
        }
    }

    #region 產生技能
    public void CreatFireObjaect()
    {
        float tmp = (pv.IsMine) ? 1 : -1;
        GameObject obj = Instantiate(FireObject, new Vector3(gameObject.transform.Find("FirePoint").position.x + tmp, transform.position.y + .5f, 0), transform.rotation);
        obj.tag = transform.tag;
        //transform.GetComponent<Animator>().SetBool("isAttack", false);
        //transform.GetComponent<Animator>().SetBool("isWalk", true);
        //speed = data.speed;
    }


    /// <summary>
    /// 產生技能
    /// </summary>
    public void CreatIceObjaect()
    {
        //float tmp = (pv.IsMine) ? 0.5f : -0.5f;
        float tmp = 0;
        GameObject obj = Instantiate(FireObject, new Vector3(gameObject.transform.Find("FirePoint").position.x + tmp, transform.position.y + 2f, 0), transform.rotation);
        obj.tag = transform.tag;
    }

    /// <summary>
    /// 產生技能
    /// </summary>
    public void CreatArrowObjaect()
    {
        float tmp = (pv.IsMine) ? .1f : -.1f;
        GameObject obj = Instantiate(FireObject, new Vector3(gameObject.transform.Find("FirePoint").position.x + tmp, transform.position.y + 0.9f, 0), transform.rotation);
        obj.tag = transform.tag;
    }

    /// <summary>
    /// 產生技能
    /// </summary>
    public void CreatGreenObjaect()
    {
        //float tmp = (pv.IsMine) ? 0.1f : -0.1f;
        float tmp = 0;
        GameObject obj = Instantiate(FireObject, new Vector3(gameObject.transform.Find("FirePoint").position.x + tmp, transform.position.y, 0), transform.rotation);
        obj.tag = transform.tag;
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //傳遞資料
            stream.SendNext(speed);
        }
        // 如果 正在讀取資料
        else if (stream.IsReading)
        {
            speed = ((float)stream.ReceiveNext());
        }
    }


}
