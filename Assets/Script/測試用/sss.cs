using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sss : MonoBehaviour
{
    [Header("玩家法術物件")]
    public GameObject a123;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(a123, new Vector3(transform.position.x + 1, transform.position.y + .5f, 0), transform.rotation);
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bullet1")
        {
            Destroy(gameObject);

        }
    }*/
    

    public void CreatFireObjaect()
    {
        Instantiate(a123, new Vector3(transform.position.x + 1, transform.position.y + .5f, 0), transform.rotation);
    }

    public void a1234()
    {
        Invoke("CreatFireObjaect", .1f);
    }

}
