using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        // Переписать залупу

        private static AsyncOperation _asyncOperation = null;

        private string _currentAsyncLoad = "";

        public void BeginLoadGameScene(GameScene state)
        {
            if (_currentAsyncLoad != "")
            {
                AsyncOperation ao  =  SceneManager.UnloadSceneAsync(_currentAsyncLoad);
            }

            switch (state)
            {
                case GameScene.Game:
                    _currentAsyncLoad = "GameplayScene";
                    break;
                case GameScene.MainMenu:
                    _currentAsyncLoad = "MainMenu";
                    break;
                case GameScene.Tutorial:
                    _currentAsyncLoad = "TutorialScene";
                    break;
            }

            _asyncOperation = SceneManager.LoadSceneAsync(_currentAsyncLoad);
            _asyncOperation.allowSceneActivation = false;
        }

        public void BeginTransaction()
        {
            _currentAsyncLoad = "";
            _asyncOperation.allowSceneActivation = true;
            _asyncOperation = null;
        }

        public enum GameScene
        {
            MainMenu,
            Game,
            Tutorial
        }
    }
}