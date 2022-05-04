using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnAIButtonStart()
    {
        GameplayManager.TypeGame = GameType.SingleAI;
        GameSceneManager.Instance.SetGameScene(GameScene.Game);
    }


    public void OnHumanButtonStart()
    {
        GameplayManager.TypeGame = GameType.SingleHuman;
        GameSceneManager.Instance.SetGameScene(GameScene.Game);
    }
}
