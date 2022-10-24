using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class CoinManager : MonoBehaviour
    {

        public static int CoinPerWin = 20;

        public static int CoinPerUnlock = 50;

        public static int AllCoins
        {
            get => PlayerPrefs.GetInt("PlayerAllMoney", 0);
            set => PlayerPrefs.SetInt("PlayerAllMoney", value);
        }
    }
}