using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    private static GameplayState _gameplayState = GameplayState.NewGame;

    public static GameType TypeGame = 0;

    private void Start()
    {
        SetGameplayState(GameplayState.NewGame);
        GameSceneManager.Instance.SetGameScene(GameScene.Game);
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
        UIManager.Instance.UpdateScore();
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
                UIManager.Instance.Initialization();
                Field.Instance.Initialization();

                ManaManager.Instance.ResetMana();
                ManaManager.Instance.ResetCurrentMana();
                ManaManager.Instance.UpdateManaUI();

                TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType);

                UIManager.Instance.NewTurn(false);
                SetGameplayState(GameplayState.None);
                break;

            case GameplayState.NewTurn:
                FieldCellLineManager.Instance.NewTurn();
                foreach (Card card in PlayerManager.Instance.GetCurrentPlayer().FullDeckPool)
                {
                    card.Info.CardBonusManacost = 0;
                }
                PlayerManager.Instance.NextPlayer();
                ManaManager.Instance.SetBonusMana(0);


                EffectManager.Instance.UpdateEffectTurn();

                if (PlayerManager.Instance.Players[0].Equals(PlayerManager.Instance.GetCurrentPlayer()))
                {
                    Field.Instance.GrowField();

                    ManaManager.Instance.GrowMana();
                }

                ManaManager.Instance.ResetCurrentMana();
                ManaManager.Instance.UpdateManaUI();

                if (PlayerManager.Instance.GetCurrentPlayer().EntityType.Equals(PlayerType.AI))
                {
                    FieldCellLineManager.Instance.PlaceInCell(AIManager.Instance.GenerateNewTurn(PlayerManager.Instance.GetCurrentPlayer().SideId));
                    Debug.LogFormat("Current tactic : {0}", AIManager.Instance.BotAggression);

                    FieldCellLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
                    //SetGameplayState(GameplayState.NewTurn);
                }
                else
                {
                    SetGameplayState(GameplayState.None);
                }

                SlotManager.Instance.NewTurn(PlayerManager.Instance.GetCurrentPlayer());
                TurnTimerManager.Instance.StartNewTurnTimer(PlayerManager.Instance.GetCurrentPlayer().EntityType);
                UIManager.Instance.NewTurn(false);
                break;
            case GameplayState.GameOver:
                int valueMoney = 0;
                if (GameplayManager.TypeGame == GameType.SingleAI && ScoreManager.Instance.GetWinner() != -1 && PlayerManager.Instance.Players[ScoreManager.Instance.GetWinner() - 1].EntityType == PlayerType.Human) valueMoney = CoinManager.CoinPerWin;
                else if (GameplayManager.TypeGame == GameType.SingleAI && ScoreManager.Instance.GetWinner() == -1) valueMoney = CoinManager.CoinPerWin / 2;

                CoinManager.AllCoins += valueMoney;
                UIManager.Instance.StateGameOverPanel(true, valueMoney);
                break;
            case GameplayState.RestartGame:
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
                ManaManager.Instance.ResetCurrentMana();
                ManaManager.Instance.UpdateManaUI();
                UIManager.Instance.UpdateScore();
                break;


        }
    }

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
