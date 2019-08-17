using System;
using UnityEngine;
using Photon.Pun;

public class playerManage : MonoBehaviourPun,IPunObservable
{
    [Header("鋼體")]
    public Rigidbody2D rig;
    [Header("速度")]
    public float speed = 10;
    [Header("photo元件")]
    public PhotonView pv;
    [Header("play腳本")]
    public playerManage player;
    [Header("攝影機")]
    public GameObject obj;
    [Header("同步placeData")]
    public Vector3 positionNext;
    [Header("同步speedData"),Range(0.1f,0.9f)]
    public float smoothSpeed;
    [Header("pic渲染器")]
    public SpriteRenderer sr;

    private void Start()
    {
        if (!pv.IsMine) {
            //player.enabled = false;
            obj.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            Move();
            Filp();
        }
        else {
            SmoothMove();
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position,positionNext, smoothSpeed*Time.deltaTime);
    }

    private void Move()
    {
        rig.AddForce((transform.right * Input.GetAxisRaw("Horizontal")+ transform.up * Input.GetAxisRaw("Vertical"))*speed);
    }
    [PunRPC]
    private void RPC_Filp(bool filp)
    {
        sr.flipX = filp;
    }
    private void Filp()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            sr.flipX = false;
            pv.RPC("RPC_Filp", RpcTarget.All,false);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            pv.RPC("RPC_Filp", RpcTarget.All, true);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading) {
            positionNext = (Vector3)stream.ReceiveNext();
        }
    }
}
