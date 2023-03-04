using GameState;
using Managers;
using Photon.Pun;
using Photon.Realtime;
using Players.Interfaces;
using Score;
using UnityEngine;
using Zenject;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Network
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {

        #region Interfaces

        private IPlayerService _playerService;

        #endregion
    
        [Inject]
        private void Construct(IPlayerService playerService)
        {
            _playerService = playerService;
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
            GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.GameOver);
        }

        public void LeaveRoom(bool isPreExit)
        {
            if (!PhotonNetwork.InRoom) return;
            Hashtable hash = new Hashtable();
            hash.Add("isPreExit", isPreExit);
            PhotonNetwork.LocalPlayer.CustomProperties = hash;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["isPreExit"]);
            PhotonNetwork.LeaveRoom();
        }

        public string GetPlayerInfo()
        {
            return
                $"ActorNumber:{Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber}, PlayerSide:{_playerService.GetPlayers()[Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber - 1].SideId}";
        }
    }
}
