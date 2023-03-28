using System;

namespace Network.Interfaces
{
    public interface INetworkEventService
    {
        public void SetIsOnline(bool state);
        public void SetNewTurnAction(Action action);
    }
}