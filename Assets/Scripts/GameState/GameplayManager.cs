using System.Collections;
using AI.Interfaces;
using Analytic;
using Cards;
using Cards.CustomType;
using Cards.Interfaces;
using Coin;
using Coroutine;
using Effects;
using GameScene;
using GameState.Interfaces;
using History;
using Mana;
using Managers;
using Network;
using Players;
using Score;
using Theme;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class GameplayManager : IGameStateService
    {
        private static GameplayState _gameplayState = GameplayState.NewGame;
        
        public GameplayState GetCurrentGameplayState()
        {
            throw new System.NotImplementedException();
        }

        public static GameType TypeGame = 0;

        private static bool _isNewStateInQueue = false;

        public static bool IsOnline;

        private int _figureCount = 0;

        public int _maxFigureCount = 3;

        #region Interfaces

        private ICardList _cardList;
        private IAIService _aiService;

        #endregion

        [Inject]
        private void Construct(ICardList cardList,IAIService aiService)
        {
            _cardList = cardList;
            _aiService = aiService;
        }
        
        private void Start()
        {
            SetGameplayState(GameplayState.NewGame);
            GameSceneManager.Instance.BeginLoadGameScene(GameSceneManager.GameScene.MainMenu);
        }
        
        private void CheckGameplayState()
        {
            switch (_gameplayState)
            {
                case GameplayState.None:
                    break;

                case GameplayState.NewGame:
                    CardPoolController.chosedCell = new Vector2Int(-1, -1);
                    switch (TypeGame)
                    {
                        case GameType.SingleAI:
                            IsOnline = false;
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.AI);
                            ScoreManager.Instance.AddPlayer(2);
                            break;
                        case GameType.SingleHuman:
                            IsOnline = false;
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(2);
                            break;
                        case GameType.MultiplayerHuman:
                            IsOnline = true;
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(2);
                            break;
                    }

                    ThemeManager.Instance.Initialization();
                    SetGameplayState(GameplayState.NewRound);
                    CoroutineQueueController.Instance.AddCoroutine(
                        InGameUI.Instance.IShowNewTurnAnimation((CellFigure) PlayerManager.Instance.GetCurrentSideOnDevice()));
                    break;

                case GameplayState.NewTurn:
                    _isNewStateInQueue = false;
                    HistoryFactory.Instance.AddHistoryNewTurn(PlayerManager.Instance.GetCurrentPlayer());
                    foreach (CardModel card in PlayerManager.Instance.GetCurrentPlayer().FullDeckPool)
                    {
                        card.Info.CardBonusManacost = 0;
                    }

                    PlayerManager.Instance.NextPlayer();
                    ManaManager.Instance.SetBonusMana(0);

                    CoroutineQueueController.Instance.AddCoroutine(
                        InGameUI.Instance.IShowNewTurnAnimation((CellFigure) PlayerManager.Instance.GetCurrentPlayer()
                            .SideId));


                    if (!IsOnline || PlayerManager.Instance.GetCurrentPlayer().SideId ==
                        RoomManager.GetCurrentPlayerSide())
                    {
                        CardPoolController.Instance.ChangeCurrentPlayerView(PlayerManager.Instance.GetCurrentPlayer());
                        CoroutineQueueController.Instance.AddCoroutine(EffectManager.Instance.UpdateEffectTurn());
                    }

                    TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType,
                        !IsOnline || PlayerManager.Instance.GetCurrentPlayer().SideId ==
                        Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber);

                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();

                    CardPoolController.Instance.UpdateCardPosition(false);
                    CardPoolController.Instance.UpdateCardUI();

                    InGameUI.Instance.NewTurn();

                    if (PlayerManager.Instance.GetCurrentPlayer().EntityType.Equals(PlayerType.AI))
                    {
                        Debug.Log("AI TURN START");
                        _figureCount = Mathf.Min(_figureCount + 1, _maxFigureCount);
                        _aiService.StartBotTurn(_figureCount);
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
                    if (TypeGame != GameType.SingleHuman &&
                        ScoreManager.Instance.GetRoundWinner() != -1 &&
                        PlayerManager.Instance.GetCurrentSideOnDevice()==ScoreManager.Instance.GetRoundWinner()) valueMoney = CoinController.coinPerWin;
                    else if (TypeGame != GameType.SingleHuman &&
                             ScoreManager.Instance.GetRoundWinner() == -1) valueMoney = CoinController.coinPerWin / 2;
                    CoinController.AllCoins += valueMoney;
                    CoinController.AllCoins += valueMoney;
                    InGameUI.Instance.StateGameOverPanel(true, valueMoney);
                    CoroutineQueueController.Instance.ClearQueue();
                    
                    int currentWinner = ScoreManager.Instance.GetGameWinner();
                    if (currentWinner == -1)
                    {
                        AnalyticController.Player_Draw_Match(TypeGame,_cardList.GetCardList());
                    }
                    else
                    {
                        bool isWin = false;
                        switch (TypeGame)
                        {
                            case GameType.MultiplayerHuman:
                                isWin = currentWinner == RoomManager.GetCurrentPlayerSide();
                                break;
                            case GameType.SingleAI:
                                isWin = PlayerManager.Instance.Players[currentWinner - 1].EntityType == PlayerType.Human;
                                break;
                        }
                        if (isWin) AnalyticController.Player_Win_Match(TypeGame,_cardList.GetCardList());
                        else AnalyticController.Player_Lose_Match(TypeGame,_cardList.GetCardList());
                    }

                    break;
                case GameplayState.RestartGame:
                    ScoreManager.Instance.ClearRoundWinners();
                    SetGameplayState(GameplayState.NewRound);

                    break;
                case GameplayState.RoundOver:
                    TurnTimerManager.Instance.StopTimer();
                    InGameUI.Instance.StopTimer();
                    CoroutineQueueController.Instance.ClearQueue();
                    ScoreManager.Instance.AddRoundWinner(ScoreManager.Instance.GetRoundWinner());
                    if (ScoreManager.Instance.IsExistGameWinner())
                    {
                        SetGameplayState(GameplayState.GameOver);
                    }
                    else
                    {
                        CoroutineQueueController.Instance.AddCoroutine(InGameUI.Instance.ShowRoundOverAnimation());
                        CoroutineQueueController.Instance.AddCoroutine(ISetStateQueueProcess(GameplayState.NewRound));
                    }

                    break;
                case GameplayState.NewRound:
                    _figureCount = 0;
                    _aiService.StopBotTurnForce();
                    PlayerManager.Instance.ResetCurrentPlayer();
                    Field.Instance.Initialization(ScoreManager.Instance.GetRoundCount());
                    CoroutineQueueController.Instance.ClearQueue();
                    HistoryFactory.Instance.RestartGame();
                    EffectManager.Instance.ClearEffect();
                    foreach (var t in PlayerManager.Instance.Players)
                    {
                        CardPoolController.Instance.ResetHandPool(t);
                        foreach (CardModel card in t.FullDeckPool)
                        {
                            card.Info.CardBonusManacost = 0;
                        }
                    }

                    ManaManager.Instance.SetBonusMana(0);
                    ManaManager.Instance.ResetMana(ScoreManager.Instance.GetRoundCount());
                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();

                    CardPoolController.Instance.ChangeCurrentPlayerView(PlayerManager.Instance.GetCurrentPlayerOnDevice());
                    ScoreManager.Instance.ClearAllScore();
                    CardPoolController.Instance.UpdateCardPosition(false);

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
                CoroutineQueueController.Instance.AddCoroutine(ISetStateQueueProcess(state));
            }
        }

        private IEnumerator ISetStateQueueProcess(GameplayState state)
        {
            SetGameplayState(state);
            _isNewStateInQueue = false;
            yield return null;
        }

        public bool IsCurrentGameplayState(GameplayState state)
        {
            return _gameplayState == state;
        }
    }
}