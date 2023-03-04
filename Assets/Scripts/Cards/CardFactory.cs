using System.Collections.Generic;
using Cards.CustomType;
using Cards.Interfaces;
using Mana;
using UnityEngine;
using Zenject;

namespace Cards
{
    public class CardFactory : ICardFactory
    {
        private CardModel _cardModelPrefab;
        private readonly ICardList _cardList;
        private const string CARD_PREFAB_PATH = "Card";

        private readonly DiContainer _diContainer;

        private void Load()
        {
            _cardModelPrefab = Resources.Load<CardModel>(CARD_PREFAB_PATH);
        }

        public CardFactory(DiContainer diContainer, ICardList cardList)
        {
            _diContainer = diContainer;
            _cardList = cardList;
            Load();
        }

        public List<CardModel> CreateDeck()
        {
            List<CardModel> newDeck = new List<CardModel>();
            foreach (CardInfo cardInfo in _cardList.GetCardList())
            {
                for (int i = 0; i < cardInfo.CardCount; i++)
                {
                    CardModel cardModel = _diContainer.InstantiatePrefabForComponent<CardModel>(_cardModelPrefab);
                    //card.name = card.Info.CardName;
                    Debug.Log(cardModel);
                    cardModel.SetCardInfo(cardInfo);
                    cardModel.Info.CardBonusManacost = 0;
                    cardModel.gameObject.SetActive(false);
                    //card.SetTransformParent(transform);
                    newDeck.Add(cardModel);
                }
            }

            return newDeck;
        }
    }
}