using Cards.CustomType;
using UnityEngine;

namespace Cards.Interfaces
{
    public interface ICardActionService
    {
        public bool InvokeActionWithCheck(CardModel cardModel, Vector2Int chosenCell);
    }
}