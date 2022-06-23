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
        if (ScoreManager.Instance.GetWinner()!=-1)
        {
            SetGameplayState(GameplayState.GameOver);
        }
        UIController.Instance.UpdateScore();
    }

    public void CheckGameplayState()
    {
        switch (_gameplayState)
        {
            case GameplayState.None:
                break;

            case GameplayState.NewGame:
                CardManager.Instance.Initialization();
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
                SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
                SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
                SlotManager.Instance.UpdateCardPosition(false);
                ThemeManager.Instance.Initialization();
                UIController.Instance.Initialization();
                Field.Instance.Initialization();

                ManaManager.Instance.ResetMana();
                ManaManager.Instance.UpdateManaUI();

                SetGameplayState(GameplayState.None);
                break;

            case GameplayState.NewTurn:
                TurnController.Instance.NewTurn();
                PlayerManager.Instance.NextPlayer();
                UIController.Instance.NewTurn(false);
                ManaManager.Instance.SetBonusMana(0);

                EffectManager.Instance.UpdateEffectTurn();


                ManaManager.Instance.ResetMana();
                ManaManager.Instance.UpdateManaUI();

                if (PlayerManager.Instance.GetCurrentPlayer().EntityType.Equals(PlayerType.AI))
                {
                    TurnController.Instance.PlaceInCell(AIManager.Instance.GenerateNewTurn(PlayerManager.Instance.GetCurrentPlayer().SideId));
                    Debug.LogFormat("Current tactic : {0}",AIManager.Instance.BotAggression);

                    TurnController.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
                    SetGameplayState(GameplayState.NewTurn);
                }
                else
                {
                    SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
                    SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
                    SlotManager.Instance.ResetRechanher();
                    SlotManager.Instance.UpdateCardPosition(false);
                    SetGameplayState(GameplayState.None);
                }
                break;
            case GameplayState.GameOver:
                UIController.Instance.StateGameOverPanel(true);
                break;
            case GameplayState.RestartGame:
                for (int i =0; i< PlayerManager.Instance.Players.Count;i++)
                {
                    SlotManager.Instance.ResetHandPool(PlayerManager.Instance.Players[i]);
                }
                ScoreManager.Instance.ResetAllScore();
                UIController.Instance.UpdateScore();
                Field.Instance.Initialization();
                SlotManager.Instance.UpdateCardPosition();
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
