using System.Collections.Generic;

namespace Cards.Interfaces
{
    public interface ICardList
    {
        public void CardListAdd(CardInfo card);
        public void CardListClear();

        public List<CardInfo> GetCardList();
    }
}