using System;
using System.Threading.Tasks;
using Analytic.Interfaces;
using Cards.Interfaces;
using GameScene;
using GameScene.Interfaces;
using GameTypeService.Enums;
using GameTypeService.Interfaces;
using Network.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using UIPages.Interfaces;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Vibration.Interfaces;
using Zenject;
using Random = UnityEngine.Random;

namespace Network
{
    public class MasterConnectorManager : MonoBehaviourPunCallbacks, IMasterConnectorService
    {
        private bool isConnected;

        #region Dependency

        private ICardList _cardList;
        private IVibrationService _vibrationService;
        private IMatchEventsAnalyticService _matchEventsAnalyticService;
        private IGameSceneService _gameSceneService;
        private IGameTypeService _gameTypeService;
        private IMainMenuState _mainMenuUIService;

        [Inject]
        private void Construct(ICardList cardList, IVibrationService vibrationService,
            IMatchEventsAnalyticService matchEventsAnalyticService, IGameSceneService gameSceneService,
            IGameTypeService gameTypeService, IMainMenuState mainMenuUIService)
        {
            _cardList = cardList;
            _vibrationService = vibrationService;
            _matchEventsAnalyticService = matchEventsAnalyticService;
            _gameSceneService = gameSceneService;
            _gameTypeService = gameTypeService;
            _mainMenuUIService = mainMenuUIService;
        }

        #endregion

        void Start()
        {
            ConnectToMaster();
        }

        private void ConnectToMaster()
        {
            isConnected = PhotonNetwork.IsConnectedAndReady;
            _mainMenuUIService.SetIsConnectedToMaster(isConnected);
            if (!isConnected && PhotonNetwork.NetworkClientState != ClientState.Leaving)
            {
                PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
                PhotonNetwork.GameVersion = Application.version;
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.ConnectToRegion("us");
                //isConnected = PhotonNetwork.IsConnectedAndReady;
            }
        }
        
        public void StartSearchRoom()
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomName: Random.Range(1000, 9999).ToString(),
                roomOptions: new RoomOptions {MaxPlayers = 2});
            Debug.Log("Start search!");
        }

        public void StopSearch()
        {
            if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
            else CancelJoinRoom();
            isConnected = PhotonNetwork.IsConnectedAndReady;
            _mainMenuUIService.SetIsConnectedToMaster(isConnected);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("Async enter room");
            Debug.Log("Connect player!");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if (PhotonNetwork.IsMasterClient) PhotonNetwork.CurrentRoom.IsOpen = false;
                _gameTypeService.SetGameType(GameType.MultiplayerHuman);

                _vibrationService.Vibrate(500);
                _matchEventsAnalyticService.Player_Found_Match(SearchingEnemyWindow.TimePass);
                _matchEventsAnalyticService.Player_Start_Match(GameType.MultiplayerHuman, _cardList.GetCardList());
                _gameSceneService.BeginLoadGameScene(GameSceneManager.GameScene.Game);
                _gameSceneService.BeginTransaction();
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Async enter room");
            Debug.Log("Connect player!");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if (PhotonNetwork.IsMasterClient) PhotonNetwork.CurrentRoom.IsOpen = false;
                _gameTypeService.SetGameType(GameType.MultiplayerHuman);

                _vibrationService.Vibrate(500);
                _matchEventsAnalyticService.Player_Found_Match(SearchingEnemyWindow.TimePass);
                _matchEventsAnalyticService.Player_Start_Match(GameType.MultiplayerHuman, _cardList.GetCardList());
                _gameSceneService.BeginLoadGameScene(GameSceneManager.GameScene.Game);
                _gameSceneService.BeginTransaction();
            }
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connected" + PhotonNetwork.NickName);
            isConnected = PhotonNetwork.IsConnectedAndReady;
            _mainMenuUIService.SetIsConnectedToMaster(isConnected);
        }

        private async void CancelJoinRoom()
        {
            Debug.Log("Async enter");
            while (!PhotonNetwork.InRoom)
            {
                Debug.Log("Async wait");
                await Task.Yield();
            }

            PhotonNetwork.LeaveRoom();
            Debug.Log("Async end");
            isConnected = PhotonNetwork.IsConnectedAndReady;
            _mainMenuUIService.SetIsConnectedToMaster(isConnected);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log(
                    $"Is in master :{PhotonNetwork.IsConnectedAndReady}. Is local :{PhotonNetwork.IsConnectedAndReady}. Player count on master :{PhotonNetwork.CountOfPlayersOnMaster}");
                try
                {
                    Debug.Log(
                        $"Is in master :{PhotonNetwork.CurrentRoom} Player count :{PhotonNetwork.CurrentRoom.PlayerCount}");
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Debug.Log($"Pause {pauseStatus}");
            if (!pauseStatus) ConnectToMaster();
        }

        public bool GetIsConnectedToMaster()
        {
            return isConnected;
        }
    }
}