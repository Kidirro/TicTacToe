using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    private static GameplayState _gameplayState = GameplayState.NewGame;

    public static GameType TypeGame = 0;

    private void Start()
    {
        ChangeGameplayState(GameplayState.NewGame);
        GameSceneManager.Instance.SetGameScene(GameScene.Game);
    }

    public void ChangeGameplayState(GameplayState state)
    {
        _gameplayState = state;
        CheckGameplayState();
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
                        PlayerManager.Instance.AddPlayer(PlayerType.AI);
                        break;
                    case GameType.SingleHuman:
                        PlayerManager.Instance.AddPlayer(PlayerType.Human);
                        PlayerManager.Instance.AddPlayer(PlayerType.Human);
                        break;
                }
                ThemeManager.Instance.Initialization();
                UIController.Instance.Initialization();
                Field.Instance.Initialization();
                SlotManager.Instance.Initialization();
                ChangeGameplayState(GameplayState.None);
                break;

            case GameplayState.NewTurn:
                TurnController.NewTurn();
                PlayerManager.Instance.NextPlayer();
                UIController.Instance.NewTurn(false);

                if (PlayerManager.Instance.GetCurrentPlayer().EntityType.Equals(PlayerType.AI))
                {
                    TurnController.TurnProcess(AIManager.Instance.GenerateNewTurn(Field.Instance.FieldSize));
                    ChangeGameplayState(GameplayState.NewTurn);
                }
                else
                {
                    ChangeGameplayState(GameplayState.None);
                }
                break;
            case GameplayState.GameOver:
                UIController.Instance.StateGameOverPanel(true);
                break;
            case GameplayState.RestartGame:
                Field.Instance.Initialization();
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
