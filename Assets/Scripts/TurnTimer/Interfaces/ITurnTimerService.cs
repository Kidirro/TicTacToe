using System;

namespace TurnTimer.Interfaces
{
    public interface ITurnTimerService
    {
        public void StartNewTurnTimer(float turnTime);
        public void StartNewTurnTimer(PlayerType player, Action action=null);
        public float GetTimeLeft();

        public void StopTimer();
    }   
}