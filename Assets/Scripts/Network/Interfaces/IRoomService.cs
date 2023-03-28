using System;

namespace Network.Interfaces
{
    public interface IRoomService
    {
        public bool GetIsOwnRoom();
        public int GetCurrentPlayerSide();
        public void LeaveRoom(bool isPreExit);

        public void SetPlayerLeaveAction(Action<bool,int> action);
    }
}