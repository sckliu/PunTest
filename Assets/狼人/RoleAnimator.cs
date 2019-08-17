using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAnimator : MonoBehaviour
{
    [Header("角色預置物件")]
    public GameObject objectWolf;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            objectWolf.GetComponent<Animator>().SetBool("isWalk", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            objectWolf.GetComponent<Animator>().SetBool("isWalk", false);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            objectWolf.GetComponent<Animator>().SetBool("isWin", true);
        }
        else if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            objectWolf.GetComponent<Animator>().SetBool("isWin", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            objectWolf.GetComponent<Animator>().SetBool("isAttack", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            objectWolf.GetComponent<Animator>().SetBool("isAttack", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            objectWolf.GetComponent<Animator>().SetTrigger("isHurt");
        }
    }
}
