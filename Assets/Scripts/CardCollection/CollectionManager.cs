using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Analytic.Interfaces;
using CardCollection.Interfaces;
using Cards.CustomType;
using Cards.Interfaces;
using Coin.Interfaces;
using SaveSystem;
using TMPro;
using UIElements.Interfaces;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ICollectionUIService = Cards.Interfaces.ICollectionUIService;

namespace CardCollection
{
    public class CollectionManager : MonoBehaviour, ICollectionUIService, ICollectionService
    {
        [Header("Card view"), SerializeField]
        private CanvasGroup _viewCardCanvas;

        [SerializeField]
        private TextMeshProUGUI _viewManapoints;

        [SerializeField]
        private TextMeshProUGUI _viewCardDescription;

        [SerializeField]
        private Image _viewCardImage;

        [SerializeField]
        private List<GameObject> _viewBonusImageList = new List<GameObject>();

        private DateTime _startTimeTap = DateTime.MinValue;
        private const float TIME_TAP_VIEW = 0.5f;
        private const float ALPHA_PER_STEP = 0.05f;

        private const string CARD_INFO_PATH = "Cards/";

        private static Dictionary<int, CardInfo> _cardListStat = new Dictionary<int, CardInfo>();

        [Header("Deck properties"), SerializeField]
        private Transform _deckParent;

        [SerializeField]
        private int _minDeckPool;

        private List<CardCollectionUIObject> _cardDeck = new List<CardCollectionUIObject>();

        private List<int> _redactedDeck;

        private static CardBoolData _cardBoolUnlockData;
        private static BinarySaveSystem _cardUnlockSaveSystem;
        private const string CARD_UNLOCK_PATH = "UnlockData";

        private static CardListData _currentDeckData;
        private static BinarySaveSystem _currentDeckSaveSystem;
        private const string CARD_DECK_PATH = "DeckData";

        [Header("Collection properties"), SerializeField]
        private Transform _collectionParent;

        private List<CardCollectionUIObject> _cardCollections = new List<CardCollectionUIObject>();

        [Header("Texts"), SerializeField]
        private TextMeshProUGUI _cardUnlockValue;

        [SerializeField]
        private TextMeshProUGUI _moneyValue;

        private IEnumerator _fadeInCoroutine;

        private bool _isEdit;

        #region Dependency

        private ICardList _cardList;
        private ICoinService _coinService;
        private ICollectionEventsAnalyticService _collectionEventsAnalyticService;
        private IStoreEventsAnalyticService _storeEventsAnalyticService;
        private ICardCollectionFactory _cardCollectionFactory;
        private IBuyingAnimationController _buyingAnimationController;

        [Inject]
        private void Construct(
            ICardList cardList,
            ICoinService coinService,
            ICollectionEventsAnalyticService collectionEventsAnalyticService,
            IStoreEventsAnalyticService storeEventsAnalyticService,
            ICardCollectionFactory cardCollectionFactory,
            IBuyingAnimationController buyingAnimationController)
        {
            _cardList = cardList;
            _coinService = coinService;
            _collectionEventsAnalyticService = collectionEventsAnalyticService;
            _storeEventsAnalyticService = storeEventsAnalyticService;
            _cardCollectionFactory = cardCollectionFactory;
            _buyingAnimationController = buyingAnimationController;
        }

        #endregion

        private void Start()
        {
            _viewCardCanvas.gameObject.SetActive(false);
            _cardListStat = new Dictionary<int, CardInfo>();
            var resourcesCard = Resources.LoadAll<CardInfo>(CARD_INFO_PATH);
            foreach (var card in resourcesCard)
            {
                _cardListStat[card.CardId] = card;
            }


            if (_cardCollections.Count == 0)
                _cardCollections =
                    _cardCollectionFactory.CreateCollectionUIList(_cardListStat.Values.ToList(), _collectionParent);
            if (_cardDeck.Count == 0)
                _cardDeck = _cardCollectionFactory.CreateCollectionUIList(_cardListStat.Values.ToList(), _deckParent);

            LoadCardUnlockData();

            LoadCurrentDeckData();

            _redactedDeck = _currentDeckData.List;

            CreateCardPull();

            UpdateCollectionCardState();
            UpdateDeckCardState();
            UpdateTexts();
        }

        public int CountCardInDeck()
        {
            return _redactedDeck.Count;
        }

        public void UpdateCollectionCardState()
        {
            foreach (CardCollectionUIObject card in _cardCollections)
            {
                card.UpdateUI(IsCardUnlock(card.Info));
                card.SetDeckState(true);
            }
        }

        public void UpdateDeckCardState()
        {
            foreach (CardCollectionUIObject card in _cardDeck)
            {
                card.UpdateUI(IsCardUnlock(card.Info));
                card.gameObject.SetActive(IsCardUnlock(card.Info));
                card.SetDeckState(IsOnDeck(card.Info));
            }
        }

        public void UpdateTexts()
        {
            _cardUnlockValue.text = _coinService.GetCoinPerUnlock().ToString();
            _moneyValue.text = _coinService.GetCurrentMoney().ToString();
        }

        private void CreateCardPull()
        {
            _cardList.CardListClear();

            foreach (var card in _currentDeckData.List)
            {
                _cardList.CardListAdd(_cardListStat[card]);
            }
        }

        public void UnlockRandomCard(bool isNeedUsekCoin = true)
        {
            if (isNeedUsekCoin && _coinService.GetCurrentMoney() < _coinService.GetCoinPerUnlock()) return;
            var lockedList = _cardCollections.Where(card => !IsCardUnlock(card.Info)).ToList();
            if (lockedList.Count == 0) return;
            int valueRand = UnityEngine.Random.Range(0, lockedList.Count);
            CardCollectionUIObject currentCard = lockedList[valueRand];

            _cardBoolUnlockData.BoolData[currentCard.Info.CardId] = true;
            SaveCardUnlockData();

            currentCard.UpdateUI(true);
            _buyingAnimationController.ShowBuyingAnimation(currentCard.Info);
            if (isNeedUsekCoin)
            {
                _coinService.SetCurrentMoney(_coinService.GetCurrentMoney() - _coinService.GetCoinPerUnlock());
                _moneyValue.text = _coinService.GetCurrentMoney().ToString();
            }

            _currentDeckData.List.Add(currentCard.Info.CardId);
            SaveCurrentDeckData();
            CreateCardPull();
        }

        public void UnlockAllCard()
        {
            List<KeyValuePair<int, bool>> keyList = _cardBoolUnlockData.BoolData.Where(x => !x.Value).ToList();

            for (int i = 0; i < keyList.Count; i++)
            {
                _buyingAnimationController.ShowBuyingAnimation(_cardListStat[keyList[i].Key]);
                _cardBoolUnlockData.BoolData[keyList[i].Key] = true;
                _currentDeckData.List.Add(keyList[i].Key);
            }

            SaveCurrentDeckData();
            SaveCardUnlockData();
        }

        private void LoadCardUnlockData()
        {
            _cardUnlockSaveSystem ??= new(CARD_UNLOCK_PATH);
            _cardBoolUnlockData = (CardBoolData) _cardUnlockSaveSystem.Load();
            if (_cardBoolUnlockData == null)
            {
                _cardBoolUnlockData = new CardBoolData();
                foreach (var card in _cardListStat.Values)
                {
                    //Синхронизация со старыми сейвами
                    if (PlayerPrefs.HasKey("IsCard" + card.CardName + "Unlocked"))
                    {
                        _cardBoolUnlockData.BoolData[card.CardId] =
                            PlayerPrefs.GetInt("IsCard" + card.name + "Unlocked", 0) == 1;
                        PlayerPrefs.DeleteKey("IsCard" + card.name + "Unlocked");
                    }
                    else
                    {
                        _cardBoolUnlockData.BoolData[card.CardId] = card.IsDefaultUnlock;
                    }
                }

                SaveCardUnlockData();
            }
        }

        private void SaveCardUnlockData()
        {
            _cardUnlockSaveSystem ??= new(CARD_UNLOCK_PATH);
            _cardUnlockSaveSystem.Save(_cardBoolUnlockData);
        }

        private void LoadCurrentDeckData()
        {
            _currentDeckSaveSystem ??= new BinarySaveSystem(CARD_DECK_PATH);
            _currentDeckData = (CardListData) _currentDeckSaveSystem.Load();

            if (_currentDeckData != null) return;

            _currentDeckData = new CardListData();
            foreach (var card in _cardBoolUnlockData.BoolData.Where(card => card.Value))
            {
                _currentDeckData.List.Add(card.Key);
            }
            SaveCurrentDeckData();
        }

        private void SaveCurrentDeckData()
        {
            _currentDeckSaveSystem ??= new BinarySaveSystem(CARD_DECK_PATH);
            _currentDeckSaveSystem.Save(_currentDeckData);
        }

        public bool IsCardUnlock(CardInfo cardInfo)
        {
            return _cardBoolUnlockData.BoolData[cardInfo.CardId];
        }

        public static CardInfo GetCardFromId(int id)
        {
            return _cardListStat[id];
        }

        private void PickCard(CardInfo card)
        {
            if (_redactedDeck.Exists(x => x == card.CardId))
            {
                _redactedDeck.Remove(card.CardId);
            }
            else
            {
                _redactedDeck.Add(card.CardId);
            }
        }

        private bool IsOnRedactedDeck(CardInfo card)
        {
            return _redactedDeck.Exists(x => x == card.CardId);
        }

        private bool IsOnDeck(CardInfo card)
        {
            return _currentDeckData.List.Exists(x => x == card.CardId);
        }

        public void TrySaveDeck()
        {
            if (CountCardInDeck() >= _minDeckPool)
            {
                _isEdit = false;
                _currentDeckData.List = _redactedDeck;
                SaveCurrentDeckData();
                CreateCardPull();
            }
            else throw new Exception("Card count less minimum");
        }

        public void StartTap(CardCollectionUIObject cardCollectionUIObject)
        {
            _startTimeTap = DateTime.Now;
            _fadeInCoroutine = StartTapProcess(cardCollectionUIObject);
            StartCoroutine(_fadeInCoroutine);
        }

        public void EndTap(CardCollectionUIObject cardCollectionUIObject)
        {
            StopCoroutine(_fadeInCoroutine);
            if ((DateTime.Now - _startTimeTap).TotalSeconds > TIME_TAP_VIEW)
            {
                StartCoroutine(EndTapProcess());
            }
            else
            {
                int value = IsOnRedactedDeck(cardCollectionUIObject.Info) ? 1 : -1;

                if (!_isEdit || CountCardInDeck() - value < _minDeckPool) return;

                PickCard(cardCollectionUIObject.Info);
                cardCollectionUIObject.SetDeckState(IsOnRedactedDeck(cardCollectionUIObject.Info));
            }
        }

        private void UpdateCardViewImage(CardInfo info)
        {
            string desc;
            desc = I2.Loc.LocalizationManager.TryGetTranslation(info.CardDescription, out desc)
                ? I2.Loc.LocalizationManager.GetTranslation(info.CardDescription)
                : info.CardDescription;
            _viewCardDescription.text = desc;
            _viewCardImage.sprite = info.CardImageP1;
            _viewManapoints.text = info.CardManacost.ToString();
            for (int i = 0; i < _viewBonusImageList.Count; i++)
            {
                _viewBonusImageList[i].SetActive(i == (int) info.CardBonus);
            }
        }

        public void StartEditPull()
        {
            _isEdit = true;
        }

        public void Player_Open_Collection()
        {
            _collectionEventsAnalyticService.Player_Open_Collection();
        }

        public void Player_Bought_Random_Card()
        {
            if (_coinService.GetCurrentMoney() < _coinService.GetCoinPerUnlock()) return;

            _storeEventsAnalyticService.Player_Bought_Random_Card();
        }

        public void Player_Open_Deckbuild()
        {
            _collectionEventsAnalyticService.Player_Open_Deckbuild();
        }


        private IEnumerator StartTapProcess(CardCollectionUIObject cardCollectionUIObject)
        {
            yield return new WaitForSeconds(TIME_TAP_VIEW);

            UpdateCardViewImage(cardCollectionUIObject.Info);
            _viewCardCanvas.gameObject.SetActive(true);
            _viewCardCanvas.alpha = 0;
            while (_viewCardCanvas.alpha < 1)
            {
                _viewCardCanvas.alpha += ALPHA_PER_STEP;
                yield return null;
            }
        }

        private IEnumerator EndTapProcess()
        {
            while (_viewCardCanvas.alpha > 0)
            {
                _viewCardCanvas.alpha -= ALPHA_PER_STEP;
                yield return null;
            }

            _viewCardCanvas.alpha = 0;
            _viewCardCanvas.gameObject.SetActive(false);
        }
    }
}