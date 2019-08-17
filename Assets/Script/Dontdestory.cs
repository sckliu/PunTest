using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dontdestory : MonoBehaviour
{
    private void Awake()
    {
        //GameObject 變數型態
        //gameObject 物件本身，誰有此腳本就代表此物件
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
