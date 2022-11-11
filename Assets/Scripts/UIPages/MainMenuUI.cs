using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class MainMenuUI : Singleton<MainMenuUI>
{
    [SerializeField]
    private TextMeshProUGUI _moneyValue;

    private int _growTurnDEV = 3;
    private int _growManaDEV = 1;

    [Header("Pages"), SerializeField]
    private GameObject _mainMenu;

    [Header("Multiplayer button properties"), SerializeField]
    private Button _multiplayerButton;

    [SerializeField]
    private GameObject _multiplayerNotAvaibleObject;

    private bool _showPre = false;



    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        UpdateNetworkUI(MasterConecctorManager.IsConnected);
    }

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
        Debug.Log("asdasd");
        GameplayManager.TypeGame = GameplayManager.GameType.SingleAI;
        GameSceneManager.Instance.SetGameScene(GameSceneManager.GameScene.Game);
    }


    public void OnHumanButtonStart()
    {
        GameplayManager.TypeGame = GameplayManager.GameType.SingleHuman;
        GameSceneManager.Instance.SetGameScene(GameSceneManager.GameScene.Game);
    }

    public void OnMultiplierOButtonStart()
    {
        GameplayManager.TypeGame = GameplayManager.GameType.MultiplayerHuman;
        GameSceneManager.Instance.SetGameScene(GameSceneManager.GameScene.Game);
    }

    public void DEV_ChangeFieldSize(int i)
    {
        Field.SetStartSize(new Vector2Int(i, i));
    }

    public void ChangeMultiplayerButtonState(bool state)
    {
        _multiplayerButton.interactable = state;
        _multiplayerNotAvaibleObject.SetActive(!state);
    }

    public void UpdateNetworkUI(bool isConnected)
    {
        ChangeMultiplayerButtonState(isConnected);
    }

    public void SetGrowTurn(string value)
    {
        _growTurnDEV = int.Parse(value);
    }

    public void SetMaxManaGrow(string value)
    {
        _growManaDEV = int.Parse(value);
    }

    public void DEV_ChangePreShow()
    {
        _showPre = !_showPre;
    }

    public void DEV_SetCoinPerWin(string text)
    {
        CoinManager.CoinPerWin = int.Parse(text);
    }

    public void DEV_SetCoinPerCard(string text)
    {
        CoinManager.CoinPerUnlock = int.Parse(text);
    }
    public void DEV_PointsPerWin(string text)
    {
        ScoreManager._scoreForWin = int.Parse(text);
    }

    public void PlayButtonClick()
    {
        OnAIButtonStart();
    }

    public void OnDiscordClick()
    {
        Application.OpenURL("https://discord.gg/DK7M2QYZ");
    }
}
