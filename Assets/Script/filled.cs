using UnityEngine;
using UnityEngine.UI;

public class filled : MonoBehaviour
{
 
    public float ColorTime = 0;//冷卻時間
    private float Timer = 0;   //計時器的初始直
    private Image filledImage;//讀取條裡的圖
    private bool IsStartTime;//是否開始計算時間
    //public GameObject Soldier;//小兵物件
    //public GameObject Main;//主保物件
    public Button call_btu;//召喚按鈕

    // Start is called before the first frame update
    void Start()
    {
        filledImage = transform.Find("Filled").GetComponent<Image>();//取得Filled內的圖
    }

    // Update is called once per frame
    void Update()
    {
       
        filiedCD();

    }
    public void OnClick()
    {
        IsStartTime = true;
    }

    
    public void filiedCD()
    {
        
        if (IsStartTime)
        {
            Timer += Time.deltaTime;
            filledImage.fillAmount = (ColorTime - Timer) / ColorTime;
            call_btu.interactable = false;
        }
        if (Timer >= ColorTime)
        {
            filledImage.fillAmount = 0;
            Timer = 0;
            IsStartTime = false;
            call_btu.interactable = true;
        }
    }
    
}
