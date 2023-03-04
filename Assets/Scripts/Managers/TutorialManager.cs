using System;
using System.Collections.Generic;
using Analytic;
using GameScene;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{

    public class TutorialManager : MonoBehaviour
    {
        public static bool IsTutorialShowed
        {
            get => PlayerPrefs.GetInt("IsTutorialShowed", 0) == 1;
            private set => PlayerPrefs.SetInt("IsTutorialShowed", value?1:0); 
        }

        private int _currentPage = 0;

        [SerializeField]
        private List<UnityEvent> _pageAction;

        private void Start()
        {
            AnalyticController.Player_Start_Tutorial();
            GameSceneManager.Instance.BeginLoadGameScene(GameSceneManager.GameScene.MainMenu);
            InvokePageAction(_currentPage);
        }

        private void InvokePageAction(int page)
        {
            _pageAction[page].Invoke();
        }

        public void NextPage()
        {
            _currentPage += 1;
            if (_currentPage >= _pageAction.Count)
            {
                IsTutorialShowed = true;
                AnalyticController.Player_Complete_Tutorial();
                Debug.Log($"TutorialManager.IsTutorialShowed {TutorialManager.IsTutorialShowed}" );
                GameSceneManager.Instance.BeginTransaction();
            }
            else InvokePageAction(_currentPage);
        }
    }
}