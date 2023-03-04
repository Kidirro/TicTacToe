using System.Collections.Generic;

namespace Players.Interfaces
{
    public interface IPlayerService
    {
        public void AddPlayer(PlayerType type);
        public void AddPlayer(PlayerType type, int side);
        public PlayerInfo GetCurrentPlayer();
        public void NextPlayer();
        public PlayerInfo GetNextPlayer();
        public void ResetCurrentPlayer();
        public int GetCurrentSideOnDevice();
        public PlayerInfo GetCurrentPlayerOnDevice();
        public List<PlayerInfo> GetPlayers();

    }
}