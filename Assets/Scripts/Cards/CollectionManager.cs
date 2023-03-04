using System;
using System.Collections;
using System.Collections.Generic;
using Analytic;
using Cards.Interfaces;
using Coin;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards
{
    public class CollectionManager : Singleton<CollectionManager>
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

        private List<CardCollectionUI> _cardDeck = new List<CardCollectionUI>();

        private static BoolArray _currentDeck;

        private static BoolArray _redactedDeck;

        [Header("Collection properties"), SerializeField]
        private Transform _collectionParent;

        private List<CardCollectionUI> _cardCollections = new List<CardCollectionUI>();

        [Header("Texts"), SerializeField]
        private TextMeshProUGUI _cardUnlockValue;

        [SerializeField]
        private TextMeshProUGUI _moneyValue;

        private IEnumerator _fadeInCoroutine;

        private bool _isEdit = false;

        #region Dependency

        private ICardList _cardList;

        [Inject]
        private void Construct(ICardList cardList)
        {
            _cardList = cardList;
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

            if (_cardCollections.Count == 0) CreateCollectionList();
            if (_cardDeck.Count == 0) CreateDeckList();


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
            foreach (CardCollectionUI card in _cardCollections)
            {
                card.UpdateUI();
                card.SetDeckState(true);
            }
        }

        public void UpdateDeckCardState()
        {
            foreach (CardCollectionUI card in _cardDeck)
            {
                card.UpdateUI();
                card.gameObject.SetActive(card.IsUnlock);
                card.SetDeckState(_currentDeck.Array[_cardListStat.IndexOf(card.Info)]);
            }
        }

        public void UpdateTexts()
        {
            _cardUnlockValue.text = CoinController.coinPerUnlock.ToString();
            _moneyValue.text = CoinController.AllCoins.ToString();
        }

        private void CreateCardPull()
        {
            _cardList.CardListClear();
            for (int i = 0; i < _cardsList.Count; i++)
            {
                if (_currentDeck.Array[i]) _cardList.CardListAdd(_cardsList[i]);
            }
        }

        private void CreateCollectionList()
        {
            for (int i = 0; i < _cardListStat.Count; i++)
            {
                CardCollectionUI cardCollectionUI =
                    Instantiate(_cardPrefab, _collectionParent).GetComponent<CardCollectionUI>();
                cardCollectionUI.Info = _cardListStat[i];
                _cardCollections.Add(cardCollectionUI);
            }
        }

        private void CreateDeckList()
        {
            for (int i = 0; i < _cardListStat.Count; i++)
            {
                CardCollectionUI cardCollectionUI =
                    Instantiate(_cardPrefab, _deckParent).GetComponent<CardCollectionUI>();
                cardCollectionUI.Info = _cardListStat[i];
                _cardDeck.Add(cardCollectionUI);
            }
        }

        public void UnlockRandomCard(bool isNeedUsekCoin = true)
        {
            if (isNeedUsekCoin && CoinController.AllCoins < CoinController.coinPerUnlock) return;
            List<CardCollectionUI> _lockedList = new List<CardCollectionUI>();
            foreach (CardCollectionUI card in _cardCollections)
                if (!card.IsUnlock)
                    _lockedList.Add(card);
            if (_lockedList.Count == 0) return;
            int valuerand = UnityEngine.Random.Range(0, _lockedList.Count);
            _lockedList[valuerand].UnlockCard();
            _lockedList[valuerand].UpdateUI();
            if (isNeedUsekCoin)
            {
                CoinController.AllCoins -= CoinController.coinPerUnlock;
                _moneyValue.text = CoinController.AllCoins.ToString();
            }

            _currentDeck.Array[_cardsList.IndexOf(_lockedList[valuerand].Info)] = true;
            SaveCurrentDeck();
            CreateCardPull();
        }

        public void UnlockAllCard()
        {
            for (int i = 0; i < _cardCollections.Count; i++)
            {
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

        public void StartTap(CardCollectionUI cardCollectionUI)
        {
            _startTimeTap = DateTime.Now;
            _fadeInCoroutine = IStartTap(cardCollectionUI);
            StartCoroutine(_fadeInCoroutine);
        }

        public void EndTap(CardCollectionUI cardCollectionUI)
        {
            StopCoroutine(_fadeInCoroutine);
            if ((DateTime.Now - _startTimeTap).TotalSeconds > TIME_TAP_VIEW)
            {
                StartCoroutine(IEndTap());
            }
            else
            {
                int value = (_redactedDeck.Array[_cardListStat.IndexOf(cardCollectionUI.Info)]) ? 1 : -1;
                if (_isEdit && CountCardInDeck() - value >= _minDeckPool) cardCollectionUI.PickCard();
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
            AnalyticController.Player_Open_Collection();
        }

        public void Player_Bought_Random_Card()
        {
            if (CoinController.AllCoins < CoinController.coinPerUnlock) return;
            AnalyticController.Player_Bought_Random_Card();
        }

        public void Player_Open_Deckbuild()
        {
            AnalyticController.Player_Open_Deckbuild();
        }


        private IEnumerator IStartTap(CardCollectionUI cardCollectionUI)
        {
            yield return new WaitForSeconds(TIME_TAP_VIEW);

            UpdateCardViewImage(cardCollectionUI.Info);
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