using Analytic.Interfaces;
using Cards.Interfaces;
using Coin.Interfaces;
using GameScene;
using GameScene.Interfaces;
using GameTypeService.Enums;
using GameTypeService.Interfaces;
using TMPro;
using Tutorial.Interfaces;
using UIPages.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIPages
{
    public class MainMenuUI : MonoBehaviour,IMainMenuUIService
    {
        [SerializeField]
        private TextMeshProUGUI _moneyValue;

        [Header("Multiplayer button properties"), SerializeField]
        private Button _multiplayerButton;

        [SerializeField]
        private GameObject _multiplayerNotAvailableObject;

        [Header("Tutorial area properties"), SerializeField]
        private GameObject _tutorialShowedArea;

        [SerializeField]
        private GameObject _tutorialNotShowedArea;

        #region Dependency

        private ICardList _cardList;
        private ICoinService _coinService;
        private IMatchEventsAnalyticService _matchEventsAnalyticService;
        private IGameSceneService _gameSceneService;
        private IGameTypeService _gameTypeService;
        private ITutorialCompleteService _tutorialCompleteService;

        [Inject]
        private void Construct(ICardList cardList, ICoinService coinService,
            IMatchEventsAnalyticService matchEventsAnalyticService, IGameSceneService gameSceneService,
            IGameTypeService gameTypeService, 
            ITutorialCompleteService tutorialCompleteService)
        {
            _cardList = cardList;
            _coinService = coinService;
            _matchEventsAnalyticService = matchEventsAnalyticService;
            _gameSceneService = gameSceneService;
            _gameTypeService = gameTypeService;
            _tutorialCompleteService = tutorialCompleteService;
        }

        #endregion

        private void Start()
        {
            UpdateNetworkUI(false);
            Debug.Log($"TutorialManager.IsTutorialShowed {_tutorialCompleteService.GetIsTutorialComplete()}");
            _tutorialShowedArea.SetActive(_tutorialCompleteService.GetIsTutorialComplete());
            _tutorialNotShowedArea.SetActive(!_tutorialCompleteService.GetIsTutorialComplete());
        }

        private void OnEnable()
        {
            UpdateTexts();
        }

        public void UpdateTexts()
        {
            _moneyValue.text = _coinService.GetCurrentMoney().ToString();
        }

        public void OnAIButtonStart()
        {
            _matchEventsAnalyticService.Player_Start_Match(GameType.SingleAI, _cardList.GetCardList());
            _gameTypeService.SetGameType(GameType.SingleAI);
            _gameSceneService.BeginLoadGameScene(GameSceneManager.GameScene.Game);
            _gameSceneService.BeginTransaction();
        }


        public void OnHumanButtonStart()
        {
            _matchEventsAnalyticService.Player_Start_Match(GameType.SingleHuman, _cardList.GetCardList());
            _gameTypeService.SetGameType(GameType.SingleHuman);
            _gameSceneService.BeginLoadGameScene(GameSceneManager.GameScene.Game);
            _gameSceneService.BeginTransaction();
        }

        public void OnTutorialButtonClick()
        {
            _gameSceneService.BeginLoadGameScene(GameSceneManager.GameScene.Tutorial);
            _gameSceneService.BeginTransaction();
        }

        private void ChangeMultiplayerButtonState(bool state)
        {
            _multiplayerButton.interactable = state;
            _multiplayerNotAvailableObject.SetActive(!state);
        }

        public void UpdateNetworkUI(bool isConnected)
        {
            ChangeMultiplayerButtonState(isConnected);
        }

        public void PlayButtonClick()
        {
            OnAIButtonStart();
        }

        public void OnDiscordClick()
        {
            Application.OpenURL("https://discord.gg/4PJbjRZtkU");
        }
    }
}