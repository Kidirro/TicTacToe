using System.Collections.Generic;
using CardCollection.Interfaces;
using Cards.CustomType;
using UnityEngine;
using Zenject;

namespace CardCollection
{
    public class CardCollectionFactory:ICardCollectionFactory
    {

        private CardCollectionUIObject _cardPrefab;
        private const string PREFAB_PATH = "CollectionItem";



        #region Dependency

        private DiContainer _diContainer;


        public CardCollectionFactory(DiContainer diDiContainer)
        {
            _diContainer = diDiContainer;
            Load();
        }

        #endregion

        private void Load()
        {
            _cardPrefab = Resources.Load<CardCollectionUIObject>(PREFAB_PATH);
            Debug.Log($"Prefab loaded!! {_cardPrefab}");
        }

        public List<CardCollectionUIObject> CreateCollectionUIList(List<CardInfo> cardList, Transform parent)
        {
            List<CardCollectionUIObject> result = new List<CardCollectionUIObject>();
            for (int i = 0; i < cardList.Count; i++)
            {
                CardCollectionUIObject cardCollectionUIObject =
                    _diContainer.InstantiatePrefabForComponent<CardCollectionUIObject>(_cardPrefab, parent);
                cardCollectionUIObject.Info = cardList[i];
                result.Add(cardCollectionUIObject);
            }

            return result;
        }
    }
}