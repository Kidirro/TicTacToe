using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LobbyConnection : MonoBehaviourPunCallbacks
{
    private bool _isCanConnect = false;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void StartSearchRoom()
    {
        if (_isCanConnect)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(null, 0, 0, null, null, Random.Range(1000, 9999).ToString(), new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
            Debug.Log("Start search!");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Finded!");
        SceneManager.LoadScene(1);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected" + PhotonNetwork.NickName);
        _isCanConnect = true;
    }
}
