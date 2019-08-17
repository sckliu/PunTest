﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    #region 欄位與屬性
    [Header("輸出文字")]
    public Text textPrint;
    [Header("輸入欄位")]
    public InputField playerIF;            // 輸入欄位：玩家名稱
    public InputField roomCreateIF;        // 輸入欄位：玩家建立房間
    public InputField roomJoinIF;          // 輸入欄位：玩家加入房間
    public Button BtnCreate, BtnJoin;      // 按鈕：建立與加入

    public string namePlayer, nameCreateRoom, nameJoinRoom;                                   // 字串： 玩家名稱、建房名稱、加入房間名稱

    public string NamePlayer
    {
        get => NamePlayer;
        set
        {
            namePlayer = value;
            PhotonNetwork.NickName = namePlayer;  // 伺服器.暱稱 = 玩家輸入的名稱
        }
    }
    public string NameCreateRoom { get => nameCreateRoom; set => nameCreateRoom = value; }
    public string NameJoinRoom { get => nameJoinRoom; set => nameJoinRoom = value; }
    #endregion
    private void Start()
    {
        Connect();
    }
    #region 自訂方法- 連線、按鈕
    /// <summary>
    /// 連線到伺服器
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(); // 連線伺服器
    }

    public void BtnCreatRoom()
    {
        PhotonNetwork.CreateRoom(NameCreateRoom, new RoomOptions { MaxPlayers = 2 }); //建立房間(房間名稱，房間選項 {最多人數 = 20});
    }

    public void BtnJoinRoom()
    {
        PhotonNetwork.JoinRoom(NameJoinRoom);
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        textPrint.text = "連線成功!";
        PhotonNetwork.JoinLobby();            // 加入大廳
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("連接失敗原因 reason {0}", cause);
    }

    public override void OnJoinedLobby()
    {
        //base.OnJoinedLobby();
        textPrint.text = "進入大廳";
        playerIF.interactable = true;
        roomCreateIF.interactable = true;
        roomJoinIF.interactable = true;
        BtnCreate.interactable = true;
        BtnJoin.interactable = true;
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        textPrint.text = "建立房間成功，房間名稱為：" + NameCreateRoom;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        textPrint.text = "建立房間失敗，Code：" + returnCode + "訊息：" + message;
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("User 進入了一個房間");
        StaticVar.Roles.Clear();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("主人 建立房間 ");

            StaticVar.AddTestVar();

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("GameWait");
        }
        else
        {
            Debug.Log("客人 進入房間 ");
            StaticVar.AddTestVar2();
            PhotonNetwork.LoadLevel("Game");
        }
        textPrint.text = "加入房間成功，房間名稱為：" + NameJoinRoom;
        //PhotonNetwork.LoadLevel("遊戲場景");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        textPrint.text = "加入房間失敗，Code：" + returnCode + "訊息：" + message;
    }
    #endregion
}
