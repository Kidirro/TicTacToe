using Cards;

namespace History.Interfaces
{
    public interface IHistoryService
    {
        public void AddHistoryNewTurn(PlayerInfo player);
        public void AddHistoryCard(PlayerInfo player, CardInfo card);
        public void ClearHistory();
        
    }
}