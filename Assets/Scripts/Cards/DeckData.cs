using System.Collections.Generic;
using Cards.Interfaces;

namespace Cards
{
    public class DeckData : ICardList
    {
        private List<CardInfo> _cardList = new();

        public void CardListAdd(CardInfo card)
        {
            _cardList ??= new List<CardInfo>();
            if (_cardList.IndexOf(card) == -1)
            {
                _cardList.Add(card);
            }
        }

        public void CardListClear()
        {
            _cardList = new List<CardInfo>();
        }

        public List<CardInfo> GetCardList()
        {
            return _cardList;
        }
    }
}