using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sound : MonoBehaviour
{
    [Header("放入控制聲音的Slider物件")]
    public Slider ControlMusicSlider;
    //public Text soundtex;
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        AudioListener.volume = ControlMusicSlider.value;
        //顯示聲音的文字
        //soundtex.text = ControlMusicSlider.value.ToString();
    }
    public void Sound()
    {
        //儲存數值在靜態變數中->靜態變數的腳本名稱.變數名稱=要儲存的數值;
        //VolumeNumber = ControlMusicSlider.value;
    }
}