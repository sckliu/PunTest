using UnityEngine;

public class Fireball : MonoBehaviour
{
    Animator AnimatorFIRE;
    [Header("移動速度")]
    public float Speed;
    [Header("傷害值")]
    public float damage;
    
    // Start is called before the first frame update

    private void Awake()
    {
        AnimatorFIRE = GetComponent<Animator>();
        //switch (gameObject.name) {
        //    case "fox_Fireball":
        //        damage = 3;
        //        break;
        //    case "Arrow":
        //        damage = 4;
        //        break;
        //    case "冰爆_0":
        //        damage = 5;
        //        break;
        //}
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(gameObject.transform.root.tag + "-------------before-----<<<<<<<<<<<<<<<<" + collider.gameObject.tag);
        if (gameObject.transform.root.tag != collider.gameObject.tag)
        {
            //Debug.Log(collider.gameObject.GetComponent<Character>().hp + "-------------before-----<<<<<<<<<<<<<<<<"+ collider.name);

            collider.gameObject.GetComponent<Character>().hurt(damage, 1, false);

             //Debug.Log(collider.name + "-------------next-----<<<<<<<<<<<<<<<<"+ gameObject.transform.root.name+"...."+ damage);
            //if (collider.name == "fox_Fireball")
            if (gameObject.transform.root.name.Substring(0,3) == "fox") 
            {
                //Debug.Log(collider.name + "-------------next-----<<<<<<<<<<<<<<<<" + gameObject.transform.root.name);
                AnimatorFIRE.SetTrigger("explosion");
            }
            //Debug.Log(transform.name + "------------------<<<<<<<<<<<<<<<<");
            
            CancelInvoke();                 // 取消所有 Invoke
            Invoke("Destroy", .2f);//設定延遲時間
            // Invoke("Destroy", .2f);//設定延遲時間
        }
        //if (collider.gameObject.tag == "玩家2")
        //{
        //    AnimatorFIRE.SetTrigger("explosion");
        //    Invoke("Destroy", .2f);//設定延遲時間

        //}
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    
}
