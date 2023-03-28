using GameTypeService.Enums;
using GameTypeService.Interfaces;

namespace GameTypeService
{
    public class GameTypeController:IGameTypeService
    {
        private GameType _gameType;
        
        public void SetGameType(GameType gameType)
        {
            _gameType = gameType;
        }

        public GameType GetGameType()
        {
            return _gameType;
        }
    }
}