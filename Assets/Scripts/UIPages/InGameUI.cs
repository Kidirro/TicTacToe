using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class InGameUI : Singleton<InGameUI>
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

    [SerializeField]
    private TextMeshProUGUI _winnerText;

    [SerializeField]
    private GameObject _winnerArea;

    [SerializeField]
    private GameObject _drawArea;


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

    public void NewTurn()
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
        GameSceneManager.Instance.SetGameScene(GameSceneManager.GameScene.MainMenu);
    }

    public void EndButtonPressed()
    {
        GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);
    }

    public void StateGameOverPanel(bool state, int value = 0)
    {
        _gameOverPanel.SetActive(state);
        if (state)
        {
            _winnerText.text = (ScoreManager.Instance.GetWinner() != -1) ? "Winner!" : "Draw!";
            _moneyValue.text = "+" + value.ToString();
            _drawArea.SetActive(ScoreManager.Instance.GetWinner() == -1);
            _winnerArea.SetActive(ScoreManager.Instance.GetWinner() != -1);
            if (ScoreManager.Instance.GetWinner() != -1) _gameOverLogo.sprite = ThemeManager.Instance.GetSprite((CellFigure)ScoreManager.Instance.GetWinner());

        }
        //      Debug.Log(ThemeManager.Instance.GetSprite((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId));
    }

    public void RestartGame()
    {
        StateGameOverPanel(false);
        GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.RestartGame);
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
            _timerFilledImg.fillAmount = TurnTimerManager.Instance.TimeLeft / _timerStartAnimationTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
