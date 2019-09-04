using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ice : MonoBehaviour
{
    Animator AnimatorFIRE;

    private void Awake()
    {
        AnimatorFIRE = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "玩家1")
        {
            
            Invoke("Destroy", 1.5f);//設定延遲時間

        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
