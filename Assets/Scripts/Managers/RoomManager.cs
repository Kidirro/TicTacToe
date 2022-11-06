using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Managers;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static bool IsOwnRoom
    {
        get => PhotonNetwork.IsMasterClient;
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
        if (otherPlayer.CustomProperties["isPreExit"] == null || (bool)otherPlayer.CustomProperties["isPreExit"]) ScoreManager.Instance.RemovePlayer(otherPlayer.ActorNumber);
        Debug.Log($"Player Leave:  {otherPlayer.ActorNumber}. Custom properties: {otherPlayer.CustomProperties["isPreExit"]}");
        GameplayManager.Instance.SetGameplayState(GameplayManager.GameplayState.GameOver);
    }

    public static void LeaveRoom(bool isPreExit)
    {
        if (!PhotonNetwork.InRoom) return;
        Hashtable hash = new Hashtable();
        hash.Add("isPreExit", isPreExit);
        PhotonNetwork.LocalPlayer.CustomProperties = hash;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["isPreExit"]);
        PhotonNetwork.LeaveRoom();
    }

    public static string GetPlayerInfo()
    {

        return string.Format("ActorNumber:{0}, PlayerSide:{1}", Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber, PlayerManager.Instance.Players[Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber - 1].SideId);
    }
}
