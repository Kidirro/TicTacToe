using CardCollection;

namespace Cards.Interfaces
{
    public interface ICollectionUIService
    {
        public void StartTap(CardCollectionUIObject cardCollectionUIObject);

        public void EndTap(CardCollectionUIObject cardCollectionUIObject);
    }
}