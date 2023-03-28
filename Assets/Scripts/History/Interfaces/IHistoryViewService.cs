using Cards;
using Cards.CustomType;

namespace History.Interfaces
{
    public interface IHistoryViewService
    {
        public void StartTap(CardInfo cardCollection, PlayerInfo player);
        public void EndTap();
    }
}