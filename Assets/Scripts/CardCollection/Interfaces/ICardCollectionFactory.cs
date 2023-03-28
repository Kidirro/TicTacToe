using System.Collections.Generic;
using Cards.CustomType;
using UnityEngine;

namespace CardCollection.Interfaces
{
    public interface ICardCollectionFactory
    {
        public List<CardCollectionUIObject> CreateCollectionUIList(List<CardInfo> cardList, Transform parent);
    }
}