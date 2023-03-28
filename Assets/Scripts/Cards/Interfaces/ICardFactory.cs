using System.Collections.Generic;
using Cards.CustomType;

namespace Cards.Interfaces
{
    public interface ICardFactory
    {
        public List<CardModel> CreateDeck(int side);
    }
}