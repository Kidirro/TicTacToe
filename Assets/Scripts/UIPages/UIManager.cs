using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{ 

    [SerializeField]
    private TextMeshProUGUI _playerOneScoreText;

    [SerializeField]
    private TextMeshProUGUI _playerTwoScorText;    
    
    [SerializeField]
    private GameObject _newTurnBTN;


    [Space, Header("GameOver Properties"), SerializeField]    
    private GameObject _gameOverPanel;

    [SerializeField]
    private Image _gameOverLogo;

    [SerializeField]
    private TextMeshProUGUI _moneyValue;


    [Space, Header("Timer Properties"), SerializeField]
    private GameObject _timerPanel;

    [SerializeField]
    private Image _timerFilledImg;

    [SerializeField]
    private float _timerEnableTime;

    [SerializeField]
    private float _timerStartAnimationTime;

    public void Initialization()
    {
        _playerOneScoreText.text = "0";
        _playerTwoScorText.text = "0";
    }

    public void NewTurn(bool instantly = true)
    {
        StopAllCoroutines();
        StartCoroutine(ITimerProcess());
        _newTurnBTN.SetActive(PlayerManager.Instance.GetCurrentPlayer().EntityType != PlayerType.AI);
    }

    public void UpdateScore()
    {
        _playerOneScoreText.text = ScoreManager.Instance.GetScore(1).ToString();
        _playerTwoScorText.text = ScoreManager.Instance.GetScore(2).ToString();
    }

    public void ReturnHome()
    {
        GameSceneManager.Instance.SetGameScene(GameScene.MainMenu);
    }

    public void EndButtonPressed()
    {
        if (FieldCellLineManager.Instance.CheckCanTurn())
        {
            GameplayManager.Instance.SetGameplayState(GameplayState.NewTurn);
        }
    }

    public void StateGameOverPanel(bool state)
    {
        _gameOverPanel.SetActive(state);
        if (state)
        {
            _gameOverLogo.sprite = ThemeManager.Instance.GetSprite((CellFigure)ScoreManager.Instance.GetWinner());

            int moneyValue = (GameplayManager.TypeGame == GameType.SingleAI && PlayerManager.Instance.Players[ScoreManager.Instance.GetWinner() - 1].EntityType == PlayerType.Human) ? CoinManager.CoinPerWin : 0;

            _moneyValue.text = "+" + moneyValue.ToString();
        }
        //      Debug.Log(ThemeManager.Instance.GetSprite((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId));
    }

    public void RestartGame()
    {
        StateGameOverPanel(false);
        GameplayManager.Instance.SetGameplayState(GameplayState.RestartGame);
    }

    private IEnumerator ITimerProcess()
    {
        _timerPanel.SetActive(false);
        yield return new WaitForSeconds(TurnTimerManager.Instance.TimeLeft - _timerEnableTime);
        _timerFilledImg.fillAmount = 1;
        _timerPanel.SetActive(true);

        yield return new WaitForSeconds(TurnTimerManager.Instance.TimeLeft - _timerStartAnimationTime);
        while (TurnTimerManager.Instance.TimeLeft >= 0)
        {
            _timerFilledImg.fillAmount =TurnTimerManager.Instance.TimeLeft / _timerStartAnimationTime   ;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
