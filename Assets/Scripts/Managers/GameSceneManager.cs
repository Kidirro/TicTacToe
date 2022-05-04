using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    private GameScene _currentScene=0;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        SetGameScene(GameScene.MainMenu);
    }

    public void SetGameScene(GameScene state)
    {
        GameSceneManager.Instance._currentScene = state;
        CheckState();
    }
    public void CheckState()
    {
       switch (_currentScene)
        {
            case GameScene.Game:
                if (!IsCurrentScene("GameplayScene"))
                {
                    SceneManager.LoadScene("GameplayScene");
                }
                break;
            case GameScene.MainMenu:

                if (!IsCurrentScene("MainMenu"))
                {
                    SceneManager.LoadScene("MainMenu");
                }
                break;
        }
    }

    private bool IsCurrentScene(string SceneName)
    {
        return SceneManager.GetActiveScene().name.Equals(SceneName);
    }

}

public enum GameScene
{
    None,
    
    MainMenu,

    Game
}
