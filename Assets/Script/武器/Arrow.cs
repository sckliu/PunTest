using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Animator AnimatorArrow;
    [Header("移動速度")]
    public float Speed;
    // Start is called before the first frame update

    private void Awake()
    {
        AnimatorArrow = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "玩家1")
        {
            AnimatorArrow.SetTrigger("explosion");
            Invoke("Destroy", .1f);//設定延遲時間

        }
    }




    private void Destroy()
    {
        Destroy(gameObject);
    }
}
