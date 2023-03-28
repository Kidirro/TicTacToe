using System.Collections.Generic;
using Cards.CustomType;

namespace Cards.Interfaces
{
    public interface ICardList
    {
        public void CardListAdd(CardInfo card);
        public void CardListClear();

        public List<CardInfo> GetCardList();
    }
}