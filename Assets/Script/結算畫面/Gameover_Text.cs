using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameover_Text : MonoBehaviour
{
    public Text OverText;
    public void GameOver(bool over)
    {
        if (over)
        {
            OverText.text = "你贏了";
        }
        else
        {
            OverText.text = "你輸了";
        }
    }
}
