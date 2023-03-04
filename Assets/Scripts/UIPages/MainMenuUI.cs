using System;
using System.Collections;
using System.Collections.Generic;
using Analytic;
using Cards;
using Cards.Interfaces;
using Coin;
using GameScene;
using GameState;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;
using Network;
using Zenject;

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


    private ICardList _cardList;

    [Inject]
    private void Construct(ICardList cardList)
    {
        _cardList = cardList;
    }
    
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Vibration.Init();
        UpdateNetworkUI(MasterConectorManager.isConnected);
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
        _moneyValue.text = CoinController.AllCoins.ToString();
    }

    public void OnAIButtonStart()
    {
        AnalyticController.Player_Start_Match(GameplayManager.GameType.SingleAI, _cardList.GetCardList());
        GameplayManager.TypeGame = GameplayManager.GameType.SingleAI;
        GameSceneManager.Instance.BeginLoadGameScene(GameSceneManager.GameScene.Game);
        GameSceneManager.Instance.BeginTransaction();
    }


    public void OnHumanButtonStart()
    {
        AnalyticController.Player_Start_Match(GameplayManager.GameType.SingleHuman, _cardList.GetCardList());
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
        CoinController.coinPerWin = int.Parse(text);
    }

    public void DEV_SetCoinPerCard(string text)
    {
        CoinController.coinPerUnlock = int.Parse(text);
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