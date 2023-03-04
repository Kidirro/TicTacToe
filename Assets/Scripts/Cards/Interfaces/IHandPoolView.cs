using Cards.CustomType;

namespace Cards.Interfaces
{
    public interface IHandPoolView
    {
        public void ChangeCurrentPlayerView(PlayerInfo player);
        public void UpdateCardPosition(bool instantly = true, CardModel cardModel = null);
        public void UpdateCardUI();

    }
}