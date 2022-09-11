using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class CollectionManager : MonoBehaviour
    {
        [Header("Cards"), SerializeField]
        private List<CardInfo> _cardsList = new List<CardInfo>();

        [SerializeField]
        private Transform _collectionParent;

        [SerializeField]
        private GameObject _cardPrefab;

        private List<CardCollection> _cardCollections = new List<CardCollection>();

        [Header("Texts"), SerializeField]
        private TextMeshProUGUI _cardUnlockValue;

        [SerializeField]
        private TextMeshProUGUI _moneyValue;


        private void Start()
        {
            if (_cardCollections.Count == 0) CreateCollectionList();
            CreateCardPull();

            UpdateCardState();
            UpdateTexts();
        }

        private void UpdateCardState()
        {
            foreach (CardCollection card in _cardCollections) card.UpdateUI();
        }

        public void UpdateTexts()
        {
            _cardUnlockValue.text = CoinManager.CoinPerUnlock.ToString();
            _moneyValue.text = CoinManager.AllCoins.ToString();
        }

        private void CreateCardPull()
        {
            CardManager.CardListClear();
            foreach (CardCollection card in _cardCollections)
            {
                if (card.IsUnlock) CardManager.CardListAdd(card.Info);
            }
        }     
        
        private void CreateCollectionList()
        {
            foreach (CardInfo card in _cardsList)
            {
                CardCollection cardCollection = Instantiate(_cardPrefab, _collectionParent).GetComponent<CardCollection>();
                cardCollection.Info = card;
                _cardCollections.Add(cardCollection);
            }
        }

        public void UnlockRandomCard()
        {
            if (CoinManager.AllCoins < CoinManager.CoinPerUnlock) return;
            List<CardCollection> _lockedList = new List<CardCollection>();
            foreach (CardCollection card in _cardCollections) if (!card.IsUnlock) _lockedList.Add(card);
            if (_lockedList.Count == 0) return;
            int valuerand = Random.Range(0, _lockedList.Count);
            _lockedList[valuerand].UnlockCard();
            _lockedList[valuerand].UpdateUI();
            CoinManager.AllCoins -= CoinManager.CoinPerUnlock;

            _moneyValue.text = CoinManager.AllCoins.ToString();
            CreateCardPull();
        }
    }
}