
    using GameTypeService.Enums;

    namespace GameTypeService.Interfaces
    {
        public interface IGameTypeService
        {
            public void SetGameType(GameType gameType);
            public GameType GetGameType();
        }
    }
