namespace Coin.Interfaces
{
    public interface ICoinService
    {
        public int GetCurrentMoney();

        public void SetCurrentMoney(int value);

        public int GetCoinPerWin();

        public int GetCoinPerUnlock();
    }
}