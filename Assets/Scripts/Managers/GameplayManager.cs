using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    private static GameplayState _gameplayState =0;

    public static GameType TypeGame = 0;

    private void Start()
    {
        CheckGameplayState();
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
            case GameplayState.NewGame:
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
                UIController.Instance.Initialization();
                Field.Instance.Initialization();
                break;
            case GameplayState.NewTurn:
                TurnController.NewTurn();
                PlayerManager.Instance.NextPlayer();
                UIController.Instance.NewTurn(false);

                if (PlayerManager.Instance.GetCurrentPlayer().EntityType.Equals(PlayerType.AI))
                {
                    TurnController.TurnProcess(AIManager.Instance.GenerateNewTurn(Field.Instance.FieldSize));
                }
                break;

        }
    }

}

public enum GameplayState
{
    NewGame,

    NewTurn
}

public enum GameType
{
    SingleAI,

    SingleHuman
}
