using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{

    public const int CoinPerWin = 500;

    public const int CoinPerUnlock = 450;

    public static int AllCoins
    {
        get => PlayerPrefs.GetInt("PlayerAllMoney", 2000);
        set => PlayerPrefs.SetInt("PlayerAllMoney", value);
    }
}
