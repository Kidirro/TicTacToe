using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _moneyValue;


    private void OnEnable()
    {
        UpdateTexts();
    }

    public void UpdateTexts()
    {

        _moneyValue.text = CoinManager.AllCoins.ToString();
    }

    public void OnAIButtonStart()
    {
        GameplayManager.TypeGame = GameType.SingleAI;
        GameSceneManager.Instance.SetGameScene(GameScene.Game);
    }


    public void OnHumanButtonStart()
    {
        GameplayManager.TypeGame = GameType.SingleHuman;
        GameSceneManager.Instance.SetGameScene(GameScene.Game);
    }
}
