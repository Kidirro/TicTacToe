namespace Network.Interfaces
{
    public interface ICheckEventNetworkService
    {
        public void RaiseEventMasterChecker();
        public void RaiseEventEndTurn();
        public void RaiseEventAwaitTime(float time);
    }
}