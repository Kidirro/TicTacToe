using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{

    public class GameSceneManager : Singleton<GameSceneManager>
    {
        private GameScene _currentScene = 0;

        public void SetGameScene(GameScene state)
        {
            _currentScene = state;
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



        public enum GameScene
        {
            MainMenu,

            Game
        }
    }
}