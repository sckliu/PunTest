using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Clock : MonoBehaviour
{
    [Header("初始時間")]
    public float StartTime;
    [Header("時間物件")]
    public Image Clock_Time;
    private float TimeSpeed =1;
    private float MaxTime;
    GameManager GMScript;

    // Start is called before the first frame update
    void Start()
    {
        MaxTime = StartTime;
        GMScript = GameObject.Find("遊戲管理器").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //MaxTime -= TimeSpeed * Time.deltaTime;
        //Clock_Time.fillAmount = MaxTime / StartTime;
        Clock_walking();
    }

    public void Clock_walking()
    {
        MaxTime -= TimeSpeed * Time.deltaTime;
        Clock_Time.fillAmount = MaxTime / StartTime;

        if (MaxTime <= 0) {
            GMScript.ClockGameOver();
        }
    }
}
