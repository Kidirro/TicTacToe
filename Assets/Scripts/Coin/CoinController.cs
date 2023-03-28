using Coin.Interfaces;
using UnityEngine;

namespace Coin
{
    public class CoinController : ICoinService
    {
        private const int COIN_PER_WIN = 20;
        private const int COIN_PER_UNLOCK = 50;
        private int _currentMoney = -1;

        public int GetCurrentMoney()
        {
            if (_currentMoney == -1) _currentMoney = PlayerPrefs.GetInt("PlayerAllMoney", 0);
            return _currentMoney;
        }

        public void SetCurrentMoney(int value)
        {
            _currentMoney = value;
            PlayerPrefs.SetInt("PlayerAllMoney", _currentMoney);
        }

        public int GetCoinPerWin()
        {
            return COIN_PER_WIN;
        }

        public int GetCoinPerUnlock()
        {
            return COIN_PER_UNLOCK;
        }
    }


}