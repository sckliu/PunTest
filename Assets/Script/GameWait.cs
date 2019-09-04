using UnityEngine;
using UnityEngine.UI;

public class GameWait : MonoBehaviour
{
    [Header("部隊")]
    public GameObject[] Soldier = new GameObject[6];
    [Header("呼叫物件按鈕")]
    public Button[] btnTmp;
    [Header("生成位置-主玩家")]
    public Transform CreateRolePoint2;
    public float height { get; set; }

    #region 小兵設定區
    [Header("小兵按鈕-限制花費分數")]
    public float[] buttonCost = new float[6];
    [Header("小兵按鈕-召喚所需費用")]
    public float[] cost = new float[6];
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StaticVar.btnArr = btnTmp;
        for (int i = 0; i < 6; i++)
        {

            string btn_img = StaticVar.Roles[i].Replace("Role", "btn_img");
            //Debug.Log("User btnTmp:" + i + "..." + btn_img);
            //Debug.Log("User btnTmp:" + btnTmp[i].GetComponent<Image>().sprite);
            btnTmp[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(btn_img);    //按鈕圖
            btnTmp[i].transform.Find("Filled").GetComponent<Image>().sprite = Resources.Load<Sprite>(btn_img); //按鈕-開場先抓CD冷卻黑圖
            Soldier[i] = Resources.Load<GameObject>(StaticVar.Roles[i]);
        }
    }
    public void CharacterSoldier(int index)
    {

        //GameObject Role = Instantiate(Soldier[index], CreateRolePoint2.position, new Quaternion(0, 90, 0, 0));
        //Role.transform.position = CreateRolePoint2.position;
        //Role.transform.rotation = CreateRolePoint2.rotation;
        //GameObject temp = Instantiate(Soldier[index], new Vector3(-10, height, 0), Quaternion.identity);
        GameObject temp = Instantiate(Soldier[index], CreateRolePoint2.position, Quaternion.identity);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
