using UnityEngine;

public class Fireball : MonoBehaviour
{
    
    Animator AnimatorFIRE;
    [Header("移動速度")]
    public float Speed;
    // Start is called before the first frame update

    private void Awake()
    {
        AnimatorFIRE = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "玩家2")
        {
            AnimatorFIRE.SetTrigger("explosion");
            Invoke("Destroy", .2f);//設定延遲時間

        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    
}
