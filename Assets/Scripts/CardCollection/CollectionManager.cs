using System;
using System.Collections;
using System.Collections.Generic;
using Analytic.Interfaces;
using CardCollection.Interfaces;
using Cards.CustomType;
using Cards.Interfaces;
using Coin.Interfaces;
using TMPro;
using UIElements.Interfaces;
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

        [Header("Cards"), SerializeField]
        private List<CardInfo> _cardsList = new List<CardInfo>();

        private static List<CardInfo> _cardListStat = new List<CardInfo>();

        [SerializeField]
        private GameObject _cardPrefab;

        [Header("Deck properties"), SerializeField]
        private Transform _deckParent;

        [SerializeField]
        private int _minDeckPool;

        private List<CardCollectionUIObject> _cardDeck = new List<CardCollectionUIObject>();

        private static BoolArray _currentDeck;

        private static BoolArray _redactedDeck;

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
        private void Construct(ICardList cardList, ICoinService coinService,
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
            _cardListStat = new List<CardInfo>(_cardsList);
            for (int i = 0; i < _cardListStat.Count; i++)
            {
                _cardListStat[i].CardId = i;
            }

            if (_cardCollections.Count == 0)
                _cardCollections = _cardCollectionFactory.CreateCollectionUIList(_cardListStat, _collectionParent);
            if (_cardDeck.Count == 0)
                _cardDeck = _cardCollectionFactory.CreateCollectionUIList(_cardListStat, _deckParent);


            _currentDeck = new BoolArray();
            if (PlayerPrefs.HasKey("CurrentDeck"))
            {
                LoadCurrentDeck();
                CheckCurrentDeck();
                SaveCurrentDeck();
            }
            else
            {
                for (int i = 0; i < _cardCollections.Count; i++)
                {
                    _currentDeck.Array.Add(_cardCollections[i].IsUnlock);
                }

                SaveCurrentDeck();
            }

            _redactedDeck = _currentDeck;
            CreateCardPull();

            UpdateCollectionCardState();
            UpdateDeckCardState();
            UpdateTexts();
        }

        public int CountCardInDeck()
        {
            int countDeck = 0;
            foreach (bool i in _redactedDeck.Array)
            {
                if (i)
                {
                    countDeck += 1;
                }
            }

            return countDeck;
        }

        public void UpdateCollectionCardState()
        {
            foreach (CardCollectionUIObject card in _cardCollections)
            {
                card.UpdateUI();
                card.SetDeckState(true);
            }
        }

        public void UpdateDeckCardState()
        {
            foreach (CardCollectionUIObject card in _cardDeck)
            {
                card.UpdateUI();
                card.gameObject.SetActive(card.IsUnlock);
                card.SetDeckState(_currentDeck.Array[_cardListStat.IndexOf(card.Info)]);
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
            for (int i = 0; i < _cardsList.Count; i++)
            {
                if (_currentDeck.Array[i]) _cardList.CardListAdd(_cardsList[i]);
            }
        }

        public void UnlockRandomCard(bool isNeedUsekCoin = true)
        {
            if (isNeedUsekCoin && _coinService.GetCurrentMoney() < _coinService.GetCoinPerUnlock()) return;
            List<CardCollectionUIObject> _lockedList = new List<CardCollectionUIObject>();
            foreach (CardCollectionUIObject card in _cardCollections)
                if (!card.IsUnlock)
                    _lockedList.Add(card);
            if (_lockedList.Count == 0) return;
            int valuerand = UnityEngine.Random.Range(0, _lockedList.Count);
            CardCollectionUIObject currentCard = _lockedList[valuerand];
            currentCard.UnlockCard();
            currentCard.UpdateUI();
            _buyingAnimationController.ShowBuyingAnimation(currentCard.Info);
            if (isNeedUsekCoin)
            {
                _coinService.SetCurrentMoney(_coinService.GetCurrentMoney() - _coinService.GetCoinPerUnlock());
                _moneyValue.text = _coinService.GetCurrentMoney().ToString();
            }

            _currentDeck.Array[_cardsList.IndexOf(currentCard.Info)] = true;
            SaveCurrentDeck();
            CreateCardPull();
        }

        public void UnlockAllCard()
        {
            for (int i = 0; i < _cardCollections.Count; i++)
            {
                _buyingAnimationController.ShowBuyingAnimation(_cardCollections[i].Info);
                _cardCollections[i].UnlockCard();
                _cardCollections[i].UpdateUI();
            }
        }

        public static CardInfo GetCardFromId(int id)
        {
            return (id > 0 && id < _cardListStat.Count) ? _cardListStat[id] : null;
        }

        public static void PickCard(CardInfo card)
        {
            _redactedDeck.Array[_cardListStat.IndexOf(card)] = !_currentDeck.Array[_cardListStat.IndexOf(card)];
        }

        public static bool IsOnRedactedDeck(CardInfo card)
        {
            return _redactedDeck.Array[_cardListStat.IndexOf(card)];
        }

        private void LoadCurrentDeck()
        {
            _currentDeck = JsonUtility.FromJson<BoolArray>(PlayerPrefs.GetString("CurrentDeck"));
        }

        private void SaveCurrentDeck()
        {
            PlayerPrefs.SetString("CurrentDeck", JsonUtility.ToJson(_currentDeck));
        }

        private void CheckCurrentDeck()
        {
            for (int i = 0; i < _cardCollections.Count; i++)
            {
                if (i >= _currentDeck.Array.Count) _currentDeck.Array.Add(_cardCollections[i].IsUnlock);
                else _currentDeck.Array[i] = _cardCollections[i].IsUnlock && _currentDeck.Array[i];
            }
        }

        public void TrySaveDeck()
        {
            bool flagEmpty = false;
            foreach (bool i in _redactedDeck.Array)
            {
                if (i)
                {
                    flagEmpty = true;
                }
            }

            if (flagEmpty)
            {
                if (CountCardInDeck() >= _minDeckPool)
                {
                    _isEdit = false;
                    _currentDeck = _redactedDeck;
                    SaveCurrentDeck();
                    CreateCardPull();
                }
                else throw new Exception("Card count less minimum");
            }
            else throw new Exception("No one card in deck!");
        }

        public void StartTap(CardCollectionUIObject cardCollectionUIObject)
        {
            _startTimeTap = DateTime.Now;
            _fadeInCoroutine = IStartTap(cardCollectionUIObject);
            StartCoroutine(_fadeInCoroutine);
        }

        public void EndTap(CardCollectionUIObject cardCollectionUIObject)
        {
            StopCoroutine(_fadeInCoroutine);
            if ((DateTime.Now - _startTimeTap).TotalSeconds > TIME_TAP_VIEW)
            {
                StartCoroutine(IEndTap());
            }
            else
            {
                int value = (_redactedDeck.Array[_cardListStat.IndexOf(cardCollectionUIObject.Info)]) ? 1 : -1;
                if (_isEdit && CountCardInDeck() - value >= _minDeckPool) cardCollectionUIObject.PickCard();
            }
        }

        public void UpdateCardViewImage(CardInfo info)
        {
            string desc = "";
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


        private IEnumerator IStartTap(CardCollectionUIObject cardCollectionUIObject)
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

        private IEnumerator IEndTap()
        {
            while (_viewCardCanvas.alpha > 0)
            {
                _viewCardCanvas.alpha -= ALPHA_PER_STEP;
                yield return null;
            }

            _viewCardCanvas.alpha = 0;
            _viewCardCanvas.gameObject.SetActive(false);
        }

        [Serializable]
        public class BoolArray
        {
            public List<bool> Array;

            public BoolArray()
            {
                Array = new List<bool>();
            }
        }
    }
}