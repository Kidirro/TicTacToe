using System.Collections.Generic;
using Cards.CustomType;

namespace CardCollection.Interfaces
{
    public interface ICollectionData
    {
        public CardInfo GetCardFromId(int id);
        
        public List<CardInfo> GetCardList();
        
        public void AddCard(CardInfo card);

        public bool IsInit();
        public void Initialize();
    }
}