using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Managers;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _moneyValue;

    private int _growTurnDEV = 3;
    private int _growManaDEV = 1;

    [Header("Pages"), SerializeField]
    private GameObject _mainMenu;

    [SerializeField]
    private GameObject _preSettings;

    private bool _showPre = false;

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

    public void DEV_ChangeFieldSize(int i)
    {
        Field.SetStartSize(new Vector2Int(i, i));
    }

    public void ChangeGrowFieldState()
    {
//        Field._isFieldGrow_Dew = !Field._isFieldGrow_Dew;
        //Field._cyclePerGrow = _growTurnDEV;
    }

    public void ChangeGrowManaState()
    {
        //ManaManager._isManaGrow = !ManaManager._isManaGrow;
        //ManaManager.Manapool = 1;
        //ManaManager._maxManaGrow = _growManaDEV;
    }

    public void SetMana(string value)
    {
     //   ManaManager.Manapool = int.Parse(value);
    }

    public void SetGrowTurn(string value)
    {
        _growTurnDEV = int.Parse(value);
    }

    public void SetMaxManaGrow(string value)
    {
        _growManaDEV = int.Parse(value);
    }

    public void DEV_clearAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
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
    
    public void ShowPreSettingMenu(bool state)
    {
        _mainMenu.SetActive(!state);
        _preSettings.SetActive(state);
    }

    public void PlayButtonClick()
    {
        Debug.Log(_showPre);
        if (_showPre) ShowPreSettingMenu(true);
        else OnAIButtonStart();
    }
}
