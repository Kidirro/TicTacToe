using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Managers;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static int GetCurrentPlayerSide()
    {
        Debug.Log("Current Player side" + PhotonNetwork.LocalPlayer.ActorNumber);
        return PhotonNetwork.LocalPlayer.ActorNumber;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (GameplayManager.CurrentGameplayState == GameplayManager.GameplayState.GameOver) return;
        ScoreManager.Instance.RemovePlayer(otherPlayer.ActorNumber);
        GameplayManager.Instance.SetGameplayState(GameplayManager.GameplayState.GameOver);
        Debug.Log("Player Leave:" +otherPlayer.ActorNumber);
    }

    public static void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public static string GetPlayerInfo()
    {

           return string.Format("ActorNumber:{0}, PlayerSide:{1}", Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber, PlayerManager.Instance.Players[Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber - 1].SideId);
    }
}
