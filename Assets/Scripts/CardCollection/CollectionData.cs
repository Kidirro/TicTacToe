using System.Collections.Generic;
using System.Linq;
using CardCollection.Interfaces;
using Cards.CustomType;

namespace CardCollection
{
    public class CollectionData : ICollectionData
    {
        private Dictionary<int, CardInfo> _cardListStat = new Dictionary<int, CardInfo>();
        private bool _isInited = false; 
        
        public CardInfo GetCardFromId(int id)
        {
            return _cardListStat[id];
        }

        public List<CardInfo> GetCardList()
        {
            return _cardListStat.Values.ToList();
        }

        public void AddCard(CardInfo card)
        {
            _cardListStat[card.CardId] = card;
        }

        public bool IsInit()
        {
            return _isInited;
        }

        public void Initialize()
        {
            _isInited = true;
        }
    }
}