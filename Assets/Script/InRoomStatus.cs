using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class InRoomStatus : MonoBehaviourPunCallbacks
{
    public static InRoomStatus Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Private Methods


    void LoadArena()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("GameWait");
        }
        else {
            PhotonNetwork.LoadLevel("Game");
        }
        
    }


    #endregion
    #region Public Methods


    public void LeaveRoom(string RoomName)
    {
        //if (GameObject.Find("遊戲管理器") != null)
        //{
        //    GameObject[] PlayerTarget = GameObject.Find("遊戲管理器").gameObject.scene.GetRootGameObjects();
        //    foreach (GameObject monster in PlayerTarget)
        //    {
                
        //        if (monster.GetPhotonView() != null) {
        //            //if (monster.GetComponent<PhotonView>().IsMine)
        //            //{
        //                PhotonNetwork.Destroy(monster);
        //            //}
        //            //PhotonNetwork.Destroy(monster);
        //        }
        //    }
        //}
        PhotonNetwork.DestroyAll(true);
        PlayerPrefs.SetString("RoomName", RoomName);
        PhotonNetwork.LeaveRoom();
        
    }

    #endregion
    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        
        SceneManager.LoadScene(PlayerPrefs.GetString("RoomName"));
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    #endregion
}
