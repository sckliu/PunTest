using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [Header("血條")]
    public Image hpBar;

    public float hp = 100;
    private float maxhp;

    GameManager GMScript;
    private void Start()
    {
        maxhp = hp;
        if (GameObject.Find("遊戲管理器") != null) GMScript = GameObject.Find("遊戲管理器").GetComponent<GameManager>();
    }

    /// <summary>
    /// 受傷功能
    /// </summary>
    /// <param name="damage"></param>接收傷害值
    public void Damage(float damage)
    {
        hp -= damage;
        hpBar.fillAmount = hp / maxhp;

    }


    private void OnTriggerEnter2D(Collider2D hit)
    {

        float MyHurt = hit.gameObject.GetComponent<Character>().data.damage;
        hit.gameObject.GetComponent<Character>().speed = 0;
        GMScript.SideTowerHurt(MyHurt);
        //if (collider.gameObject.tag == "Bullet1")
        //{
        //    Debug.Log("87");
        //    hp -= 10;
        //    hpBar.fillAmount = hp / maxhp;

        //}
    }


}