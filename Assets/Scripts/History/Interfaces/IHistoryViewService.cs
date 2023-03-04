using Cards;

namespace History.Interfaces
{
    public interface IHistoryViewService
    {
        public void StartTap(CardInfo cardCollection, PlayerInfo player);
        public void EndTap();
    }
}