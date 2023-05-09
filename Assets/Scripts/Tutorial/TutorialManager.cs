using System.Collections.Generic;
using Analytic;
using Analytic.Interfaces;
using GameScene;
using GameScene.Interfaces;
using GameState.Interfaces;
using Tutorial.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        private int _currentPage = 0;

        [SerializeField]
        private List<UnityEvent> _pageAction;

        #region Dependency

        private IGameSceneService _gameSceneService;
        private ITutorialEventsEventsAnalyticService _tutorialEventsEventsAnalyticService;
        private ITutorialCompleteService _tutorialCompleteService;

        [Inject]
        private void Construct(
            IGameSceneService gameSceneService, 
            ITutorialCompleteService tutorialCompleteService,
            ITutorialEventsEventsAnalyticService tutorialEventsEventsAnalyticService)
        {
            Debug.Log($"Binding...");
            _gameSceneService = gameSceneService;
            _tutorialCompleteService = tutorialCompleteService;
            _tutorialEventsEventsAnalyticService = tutorialEventsEventsAnalyticService;
            Debug.Log($"Binded! {gameSceneService}, {tutorialCompleteService}, {tutorialEventsEventsAnalyticService}");
        }

        #endregion

        private void Start()
        {
            //_tutorialEventsEventsAnalyticService.Player_Start_Tutorial();
            _gameSceneService.BeginLoadGameScene(GameSceneManager.GameScene.MainMenu);
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
                _tutorialCompleteService.SetIsTutorialComplete(true);
                _tutorialEventsEventsAnalyticService.Player_Complete_Tutorial();
                Debug.Log($"TutorialManager.IsTutorialShowed {_tutorialCompleteService.GetIsTutorialComplete()}");
                _gameSceneService.BeginTransaction();
            }
            else InvokePageAction(_currentPage);
        }
    }
}