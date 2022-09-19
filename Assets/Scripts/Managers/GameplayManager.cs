using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class GameplayManager : Singleton<GameplayManager>
    {
        private static GameplayState _gameplayState = GameplayState.NewGame;

        public static GameType TypeGame = 0;

        private bool _isNewStateInQueue = false;

        private void Start()
        {
            SetGameplayState(GameplayState.NewGame);
            GameSceneManager.Instance.SetGameScene(GameSceneManager.GameScene.Game);
        }

        public void SetGameplayState(GameplayState state)
        {
            _gameplayState = state;
            CheckGameplayState();
        }

        public void AddScore(int value, int sideId)
        {
            ScoreManager.Instance.AddScore(sideId, value);
            if (ScoreManager.Instance.IsExistWinner())
            {
                SetGameplayState(GameplayState.GameOver);
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
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.AI);
                            ScoreManager.Instance.AddPlayer(2);
                            break;
                        case GameType.SingleHuman:
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(1);
                            PlayerManager.Instance.AddPlayer(PlayerType.Human);
                            ScoreManager.Instance.AddPlayer(2);
                            break;
                    }
                    SlotManager.Instance.NewTurn(PlayerManager.Instance.GetCurrentPlayer());
                    ThemeManager.Instance.Initialization();
                    InGameUI.Instance.Initialization();
                    Field.Instance.Initialization();

                    HistoryManager.Instance.RestartGame();
                    ManaManager.Instance.ResetMana();
                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();

                    TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType);

                    InGameUI.Instance.NewTurn();
                    SetGameplayState(GameplayState.None);
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



                    if (PlayerManager.Instance.Players[0].Equals(PlayerManager.Instance.GetCurrentPlayer()))
                    {
                        CoroutineManager.Instance.AddCoroutine(Field.Instance.GrowField());

                        ManaManager.Instance.GrowMana();
                    }

                   CoroutineManager.Instance.AddCoroutine(EffectManager.Instance.UpdateEffectTurn());
                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();

                    SlotManager.Instance.NewTurn(PlayerManager.Instance.GetCurrentPlayer());
                    TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType);
                    InGameUI.Instance.NewTurn();

                    if (PlayerManager.Instance.GetCurrentPlayer().EntityType.Equals(PlayerType.AI))
                    {
                        Debug.Log("AI TURN START");
                        Field.Instance.PlaceInCell(AIManager.Instance.GenerateNewTurn(PlayerManager.Instance.GetCurrentPlayer().SideId));
                        Debug.LogFormat("Current tactic : {0}", AIManager.Instance.BotAggression);

                        FinishLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);

                        SetGamePlayStateQueue(GameplayState.NewTurn);
                        //SetGameplayState(GameplayState.NewTurn);
                    }
                    else
                    {
                        SetGameplayState(GameplayState.None);
                    }

                    break;
                case GameplayState.GameOver:
                    int valueMoney = 0;
                    if (GameplayManager.TypeGame == GameType.SingleAI && ScoreManager.Instance.GetWinner() != -1 && PlayerManager.Instance.Players[ScoreManager.Instance.GetWinner() - 1].EntityType == PlayerType.Human) valueMoney = CoinManager.CoinPerWin;
                    else if (GameplayManager.TypeGame == GameType.SingleAI && ScoreManager.Instance.GetWinner() == -1) valueMoney = CoinManager.CoinPerWin / 2;

                    CoinManager.AllCoins += valueMoney;
                    InGameUI.Instance.StateGameOverPanel(true, valueMoney);
                    break;
                case GameplayState.RestartGame:

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
                    SlotManager.Instance.NewTurn(PlayerManager.Instance.GetCurrentPlayer());
                    ScoreManager.Instance.ResetAllScore();
                    Field.Instance.Initialization();
                    SlotManager.Instance.UpdateCardPosition(false);


                    TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType);

                    ManaManager.Instance.ResetMana();
                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();
                    InGameUI.Instance.UpdateScore();
                    break;


            }
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

            RestartGame
        }

        public enum GameType
        {
            SingleAI,

            SingleHuman
        }
    }
}