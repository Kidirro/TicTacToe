using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        private static GameplayState _gameplayState = GameplayState.NewGame;

        public static GameplayState CurrentGameplayState
        {
            get => _gameplayState;
        }

        public static GameType TypeGame = 0;

        private static bool _isNewStateInQueue = false;

        public static bool IsOnline;

        private int _figureCount = 0;

        public int _maxFigureCount = 3;

        private void Start()
        {
            SetGameplayState(GameplayState.NewGame);
            GameSceneManager.Instance.SetGameScene(GameSceneManager.GameScene.Game);
        }

        public void AddScore(int value, int sideId)
        {
            ScoreManager.Instance.AddScore(sideId, value);
            if (ScoreManager.Instance.IsExistRoundWinner() &&
                GameplayManager.CurrentGameplayState != GameplayState.GameOver)
            {
                SetGamePlayStateQueue(GameplayState.RoundOver);
            }

            InGameUI.Instance.UpdateScore();
        }

        public void CheckGameplayState()
        {
            switch (_gameplayState)
            {
                case GameplayState.None:
                    break;

                case GameplayState.NewGame:
                    Card.ChosedCell = new Vector2Int(-1, -1);
                    switch (TypeGame)
                    {
                        case GameType.SingleAI:
                            IsOnline = false;
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.AI);
                            ScoreManager.Instance.AddPlayer(2);
                            CoroutineManager.Instance.AddCoroutine(
                                InGameUI.Instance.IShowNewTurnAnimation((CellFigure) PlayerManager.Instance.GetCurrentSideOnDevice()));
                            break;
                        case GameType.SingleHuman:
                            IsOnline = false;
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(2);
                            CoroutineManager.Instance.AddCoroutine(
                                InGameUI.Instance.IShowNewTurnAnimation((CellFigure) PlayerManager.Instance.GetCurrentSideOnDevice()));
                            break;
                        case GameType.MultiplayerHuman:
                            IsOnline = true;
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(2);
                            CoroutineManager.Instance.AddCoroutine(
                                InGameUI.Instance.IShowNewTurnAnimation((CellFigure) PlayerManager.Instance.GetCurrentSideOnDevice()));
                            break;
                    }

                    ThemeManager.Instance.Initialization();
                    SetGameplayState(GameplayState.NewRound);
                    break;

                case GameplayState.NewTurn:
                    _isNewStateInQueue = false;
                    HistoryManager.Instance.AddHistoryNewTurn(PlayerManager.Instance.GetCurrentPlayer());
                    foreach (Card card in PlayerManager.Instance.GetCurrentPlayer().FullDeckPool)
                    {
                        card.Info.CardBonusManacost = 0;
                    }

                    PlayerManager.Instance.NextPlayer();
                    ManaManager.Instance.SetBonusMana(0);

                    CoroutineManager.Instance.AddCoroutine(
                        InGameUI.Instance.IShowNewTurnAnimation((CellFigure) PlayerManager.Instance.GetCurrentPlayer()
                            .SideId));


                    if (!IsOnline || PlayerManager.Instance.GetCurrentPlayer().SideId ==
                        RoomManager.GetCurrentPlayerSide())
                    {
                        SlotManager.Instance.NewTurn(PlayerManager.Instance.GetCurrentPlayer());
                        CoroutineManager.Instance.AddCoroutine(EffectManager.Instance.UpdateEffectTurn());
                    }

                    TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType,
                        !IsOnline || PlayerManager.Instance.GetCurrentPlayer().SideId ==
                        Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber);

                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();

                    SlotManager.Instance.UpdateCardPosition(false);
                    SlotManager.Instance.UpdateCardUI();

                    InGameUI.Instance.NewTurn();

                    if (PlayerManager.Instance.GetCurrentPlayer().EntityType.Equals(PlayerType.AI))
                    {
                        Debug.Log("AI TURN START");
                        _figureCount = Mathf.Min(_figureCount + 1, _maxFigureCount);
                        AIManager.Instance.StartBotTurn(_figureCount);
                    }
                    else
                    {
                        SetGameplayState(GameplayState.None);
                    }

                    break;
                case GameplayState.GameOver:
                    if (InGameUI.Instance.IsGameOverShowed) return;
                    if (IsOnline) RoomManager.LeaveRoom(false);
                    int valueMoney = 0;
                    if (GameplayManager.TypeGame != GameType.SingleHuman &&
                        ScoreManager.Instance.GetRoundWinner() != -1 &&
                        PlayerManager.Instance.GetCurrentSideOnDevice()==ScoreManager.Instance.GetRoundWinner()) valueMoney = CoinManager.CoinPerWin;
                    else if (GameplayManager.TypeGame != GameType.SingleHuman &&
                             ScoreManager.Instance.GetRoundWinner() == -1) valueMoney = CoinManager.CoinPerWin / 2;
                    CoinManager.AllCoins += valueMoney;
                    CoinManager.AllCoins += valueMoney;
                    InGameUI.Instance.StateGameOverPanel(true, valueMoney);
                    CoroutineManager.Instance.ClearQueue();
                    break;
                case GameplayState.RestartGame:
                    ScoreManager.Instance.ClearRoundWinners();
                    SetGameplayState(GameplayState.NewRound);

                    break;
                case GameplayState.RoundOver:
                    TurnTimerManager.Instance.StopTimer();
                    InGameUI.Instance.StopTimer();
                    CoroutineManager.Instance.ClearQueue();
                    ScoreManager.Instance.AddRoundWinner(ScoreManager.Instance.GetRoundWinner());
                    if (ScoreManager.Instance.IsExistGameWinner())
                    {
                        SetGameplayState(GameplayState.GameOver);
                    }
                    else
                    {
                        CoroutineManager.Instance.AddCoroutine(InGameUI.Instance.ShowRoundOverAnimation());
                        CoroutineManager.Instance.AddCoroutine(ISetStateQueueProcess(GameplayState.NewRound));
                    }

                    break;
                case GameplayState.NewRound:
                    _figureCount = 0;
                    AIManager.Instance.StopBotTurnForce();
                    PlayerManager.Instance.ResetCurrentPlayer();
                    Field.Instance.Initialization(ScoreManager.Instance.GetRoundCount());
                    CoroutineManager.Instance.ClearQueue();
                    HistoryManager.Instance.RestartGame();
                    EffectManager.Instance.ClearEffect();
                    for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
                    {
                        SlotManager.Instance.ResetHandPool(PlayerManager.Instance.Players[i]);
                        foreach (Card card in PlayerManager.Instance.Players[i].DeckPool)
                        {
                            card.Info.CardBonusManacost = 0;
                        }
                    }

                    ManaManager.Instance.SetBonusMana(0);
                    ManaManager.Instance.ResetMana(ScoreManager.Instance.GetRoundCount());
                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();

                    SlotManager.Instance.NewTurn(PlayerManager.Instance.GetCurrentPlayerOnDevice());
                    ScoreManager.Instance.ClearAllScore();
                    SlotManager.Instance.UpdateCardPosition(false);

                    TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType,
                        !IsOnline || PlayerManager.Instance.GetCurrentPlayer().SideId ==
                        Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber);


                    InGameUI.Instance.NewTurn();
                    InGameUI.Instance.SetSideBannerTurn(1);
                    InGameUI.Instance.UpdateScore();
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) Debug.Log(RoomManager.GetPlayerInfo());
            if (Input.GetKeyDown(KeyCode.K)) NetworkEventManager.RaiseEventEndTurn();
        }

        public void SetGameplayState(GameplayState state)
        {
            _gameplayState = state;
            CheckGameplayState();
            Debug.Log($"Current state :{state}");
        }

        public void SetGamePlayStateQueue(GameplayState state)
        {
            if (!_isNewStateInQueue)
            {
                _isNewStateInQueue = true;
                CoroutineManager.Instance.AddCoroutine(ISetStateQueueProcess(state));
            }
        }

        private IEnumerator ISetStateQueueProcess(GameplayState state)
        {
            SetGameplayState(state);
            _isNewStateInQueue = false;
            yield return null;
        }


        public enum GameplayState
        {
            None,

            NewGame,

            NewTurn,

            GameOver,

            RestartGame,

            RoundOver,

            NewRound
        }

        public enum GameType
        {
            SingleAI,

            SingleHuman,

            MultiplayerHuman
        }
    }
}