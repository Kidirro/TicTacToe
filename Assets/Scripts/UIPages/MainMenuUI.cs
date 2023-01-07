using System;
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

    [Header("Tutorial area properties"), SerializeField]
    private GameObject _tutorialShowedArea;
    [SerializeField]
    private GameObject _tutorialNotShowedArea;

    private bool _showPre = false;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        UpdateNetworkUI(MasterConectorManager.IsConnected);
        Debug.Log($"TutorialManager.IsTutorialShowed {TutorialManager.IsTutorialShowed}" );
        _tutorialShowedArea.SetActive(TutorialManager.IsTutorialShowed);
        _tutorialNotShowedArea.SetActive(!TutorialManager.IsTutorialShowed);
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
        AnalitycManager.Player_Start_Match(GameplayManager.GameType.SingleAI, CardManager.CardList);
        GameplayManager.TypeGame = GameplayManager.GameType.SingleAI;
        GameSceneManager.Instance.BeginLoadGameScene(GameSceneManager.GameScene.Game);
        GameSceneManager.Instance.BeginTransaction();
    }


    public void OnHumanButtonStart()
    {
        AnalitycManager.Player_Start_Match(GameplayManager.GameType.SingleHuman, CardManager.CardList);
        GameplayManager.TypeGame = GameplayManager.GameType.SingleHuman;
        GameSceneManager.Instance.BeginLoadGameScene(GameSceneManager.GameScene.Game);
        GameSceneManager.Instance.BeginTransaction();
    }

    public void OnTutorialButtonClick()
    {
        GameSceneManager.Instance.BeginLoadGameScene(GameSceneManager.GameScene.Tutorial);
        GameSceneManager.Instance.BeginTransaction();
    }

    public void DEV_ChangeFieldSize(int i)
    {
        Field.SetStartSize(new Vector2Int(i, i));
    }

    private void ChangeMultiplayerButtonState(bool state)
    {
        _multiplayerButton.interactable = state;
        _multiplayerNotAvaibleObject.SetActive(!state);
    }

    public void UpdateNetworkUI(bool isConnected)
    {
        ChangeMultiplayerButtonState(isConnected);
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

    public void PlayButtonClick()
    {
        OnAIButtonStart();
    }

    public void OnDiscordClick()
    {
        Application.OpenURL("https://discord.gg/4PJbjRZtkU");
    }
}