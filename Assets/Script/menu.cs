﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Setting()
    {
        Application.LoadLevel("setting");
        print("4");
    }
    public void NextLevelScene()
    {
        Application.LoadLevel("ChooseCharacter");
        print("5");
    }
    public void Backtomenu()
    {
        Application.LoadLevel("menu");
        print("6");
    }
    // Update is called once per frame
   

    public void Quit()
    {
        Application.Quit();
        print("123");
    }

}
