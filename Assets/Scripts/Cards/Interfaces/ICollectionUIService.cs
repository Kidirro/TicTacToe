using CardCollection;
using UnityEngine.EventSystems;

namespace Cards.Interfaces
{
    public interface ICollectionUIService
    {
        public void StartTap(CardCollectionUIObject cardCollectionUIObject, PointerEventData eventData);

        public void EndTap(CardCollectionUIObject cardCollectionUIObject, PointerEventData eventData);
        public void OnDrag(PointerEventData eventData);
    }
}