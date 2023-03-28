namespace Network.Interfaces
{
    public interface IManaEventNetworkService
    {
        public void RaiseEventIncreaseMana(int value, bool isOverMax = false);
        public void RaiseEventAddBonusMana(int value);
        public void RaiseEventIncreaseMaxMana(int value);
        public void RaiseEventRestoreMana(int value);
        

    }
}