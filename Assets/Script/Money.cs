using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;



public class Money : MonoBehaviourPun, IPunObservable
{
    #region 宣告金錢欄位
    [Header("初始金錢")]
    private float money_end = 0;
    [Header("初始跳動金錢物件")]
    public Text money_mid;//抓text物件
    [Header("金錢最大值物件")]
    public Text Money_Max;
    [Header("每秒加錢數")]
    private float TimeSpeed = 5;//時間速度
    private float MoneyMax = 5;//金庫最大值
    [Header("升級金錢最大值物件")]
    public Text level_money;
    #endregion
     
    GameManager GMScript;

    void Start()
    {
        GMScript = GameObject.Find("遊戲管理器").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        money_end += TimeSpeed * Time.deltaTime;

        //錢足夠時激活button
        for (int i = 0; i < 6; i++)
        {
            StaticVar.btnArr[i].interactable = (money_end >= GMScript.buttonCost[i]);
        }
        
        Addmoney();

        if (money_end >= MoneyMax)
        {
            money_end = MoneyMax;
        }
    }
    #region 轉換字串
    public void Addmoney()//轉換字串
    {
        //顯示金錢物件=時間，轉換成字串
        money_mid.text = money_end.ToString("0");
        Money_Max.text = MoneyMax.ToString("0");
        level_money.text = MoneyMax.ToString("0");

    }
    #endregion
    #region 升級金礦最大容量
    public void level()
    {
        if (MoneyMax == money_end)
        {
            money_end = 0;
            MoneyMax += 10;
            Addmoney();
        }
    }
    #endregion
    #region 造兵條件
    //召喚小兵1
    public void moneycost(int index)
    {
        if (money_end >= GMScript.cost[index])
        {
            money_end -= GMScript.cost[index];
            GMScript.CharacterSoldier(index);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 如果 正在寫入資料
        if (stream.IsWriting)
        {
            //傳遞(寫入)資料
            stream.SendNext(MoneyMax);
        }
        // 如果 正在讀取資料
        else if (stream.IsReading)
        {
            MoneyMax = (float)stream.ReceiveNext();
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
    #endregion


}
