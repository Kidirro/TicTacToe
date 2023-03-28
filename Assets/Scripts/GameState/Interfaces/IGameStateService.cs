namespace GameState.Interfaces
{
    public interface IGameStateService
    {
        public GameplayState GetCurrentGameplayState();
        public void SetGameplayState(GameplayState state);
        public void SetGamePlayStateQueue(GameplayState state);
        public bool IsCurrentGameplayState(GameplayState state);
        public bool GetIsOnline();
    }
}