using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class InGameUI : Singleton<InGameUI>
{
    public bool IsGameOverShowed
    {
        get => _gameOverPanel.gameObject.activeSelf;
    }


    [SerializeField]
    private TextMeshProUGUI _playerOneScoreText;

    [SerializeField]
    private TextMeshProUGUI _playerTwoScorText;

    [SerializeField]
    private GameObject _newTurnBTN;


    [Space, Header("GameOver Properties"), SerializeField]
    private AnimationFading _gameOverPanel;

    [SerializeField]
    private Image _gameOverLogo;

    [SerializeField]
    private GameObject _windrawBG;

    [SerializeField]
    private GameObject _loseBG;

    [SerializeField]
    private List<GameObject> _bgList;

    [SerializeField]
    private GameObject _gameOverReplyBTN;

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

    [Header("New turn banner properties"), SerializeField]
    private Animator _animatorNewTurn;


    [Header ("Pause menu Properties"),SerializeField]
    private GameObject _pauseMenuReplyBTN;

     public void UpdateRerplyState()
    {
        _pauseMenuReplyBTN.SetActive(!GameplayManager.IsOnline);
        _gameOverReplyBTN.SetActive(!GameplayManager.IsOnline);
    }

    public void Initialization()
    {
        _playerOneScoreText.text = "0";
        _playerTwoScorText.text = "0";
    }

    public void NewTurn()
    {
        StopTimer();
        _animatorNewTurn.SetTrigger("NewTurn");
        StartCoroutine(ITimerProcess());
        _newTurnBTN.SetActive(SlotManager.Instance.IsCurrentPlayerOnSlot);
    }

    public void UpdateScore()
    {
        _playerOneScoreText.text = ScoreManager.Instance.GetScore(1).ToString();
        _playerTwoScorText.text = ScoreManager.Instance.GetScore(2).ToString();
    }

    public void ReturnHome()
    {
        if (GameplayManager.IsOnline) RoomManager.LeaveRoom(true);
        GameSceneManager.Instance.SetGameScene(GameSceneManager.GameScene.MainMenu);
    }

    public void EndButtonPressed()
    {
        if (GameplayManager.IsOnline && PlayerManager.Instance.GetCurrentPlayer().SideId != Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber) return;
        GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);
        NetworkEventManager.RaiseEventEndTurn();
    }

    public void StateGameOverPanel(bool state, int value = 0)
    {
        UpdateRerplyState();
        if (state) _gameOverPanel.FadeIn();        
        else _gameOverPanel.FadeOut();
        if (state)
        {
            int currentWinner = ScoreManager.Instance.GetWinner();
            bool isWin = false;
            if (currentWinner != -1)
            {
                switch (GameplayManager.TypeGame)
                {
                    case GameplayManager.GameType.MultiplayerHuman:
                        isWin = currentWinner == RoomManager.GetCurrentPlayerSide();
                        break;
                    case GameplayManager.GameType.SingleAI:
                        isWin = PlayerManager.Instance.Players[currentWinner - 1].EntityType == PlayerType.Human;
                        break;
                    case GameplayManager.GameType.SingleHuman:
                        isWin = true;
                        break;
                }
            }
            _windrawBG.SetActive(isWin || currentWinner ==-1);
            _loseBG.SetActive(!(isWin||currentWinner==-1));
            _moneyValue.text = "+" + value.ToString();

                _winnerText.text = (currentWinner==-1)? I2.Loc.LocalizationManager.GetTranslation("Draw"):
                (isWin) ? I2.Loc.LocalizationManager.GetTranslation("YouWin") : I2.Loc.LocalizationManager.GetTranslation("YouLose");
            
            _drawArea.SetActive(ScoreManager.Instance.GetWinner() == -1);
            _winnerArea.SetActive(ScoreManager.Instance.GetWinner() != -1);
            for (int i = 0; i < _bgList.Count; i++)
            {
                _bgList[i].SetActive(i == Mathf.Max(0, currentWinner));
            }
            if (ScoreManager.Instance.GetWinner() != -1) _gameOverLogo.sprite = ThemeManager.Instance.GetSprite((CellFigure)ScoreManager.Instance.GetWinner());
        }
    }

    public void RestartGame()
    {
        if (GameplayManager.IsOnline) return;
        if (_gameOverPanel.gameObject.activeSelf) StateGameOverPanel(false);
        GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.RestartGame);
    }

    private IEnumerator ITimerProcess()
    {
        _timerPanel.SetActive(false);

        while (_timerEnableTime < TurnTimerManager.Instance.TimeLeft) yield return null;

        _timerFilledImg.fillAmount = 1;
        _timerPanel.SetActive(true);

        while (_timerStartAnimationTime < TurnTimerManager.Instance.TimeLeft) yield return null;

        while (TurnTimerManager.Instance.TimeLeft >= 0)
        {
            _timerFilledImg.fillAmount = TurnTimerManager.Instance.TimeLeft / _timerStartAnimationTime;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

}
