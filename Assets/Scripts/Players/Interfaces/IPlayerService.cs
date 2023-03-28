using System.Collections.Generic;
using Cards.CustomType;
using GameTypeService.Enums;

namespace Players.Interfaces
{
    public interface IPlayerService
    {
        public void AddPlayer(PlayerType type,List<CardModel> deck);
        public void AddPlayer(PlayerType type, int side, List<CardModel> deck);
        public PlayerInfo GetCurrentPlayer();
        public void NextPlayer();
        public PlayerInfo GetNextPlayer();
        public void ResetCurrentPlayer();
        public int GetCurrentSideOnDevice();
        public PlayerInfo GetCurrentPlayerOnDevice();
        public List<PlayerInfo> GetPlayers();
        public void SetGameType(GameType gameType);

        public void SetOnlineId(int id);

    }
}