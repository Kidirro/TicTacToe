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
    private List<GameObject> _playerOneRPList;

    [SerializeField]
    private TextMeshProUGUI _playerTwoScorText;

    [SerializeField]
    private List<GameObject> _playerTwoRPList;

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

    [Space, Header("RoundOver Properties"), SerializeField]
    private AnimationFading _roundOverPanel;

    [SerializeField]
    private Image _roundOverLogo;

    [SerializeField]
    private GameObject _windrawRoundOverBG;

    [SerializeField]
    private GameObject _loseRoundOverBG;

    [SerializeField]
    private TextMeshProUGUI _winnerRoundOverText;

    [SerializeField]
    private GameObject _winnerRoundOverArea;

    [SerializeField]
    private GameObject _drawRoundOverArea;

    [SerializeField]
    private List<GameObject> _p1RoundOverPoints;

    [SerializeField]
    private List<GameObject> _p2RoundOverPoints;

    [Space, Header("Timer Properties"), SerializeField]
    private GameObject _timerPanel;

    [SerializeField]
    private Image _timerFilledImg;

    [SerializeField]
    private float _timerEnableTime;

    [SerializeField]
    private float _timerStartAnimationTime;

    private Coroutine _timerCoroutine;

    [Header("New turn banner properties"), SerializeField]
    private Animator _animatorNewTurn;

    [Header("Pause menu Properties"), SerializeField]
    private GameObject _pauseMenuReplyBTN;

    [Header("Paper new turn properties"), SerializeField]
    private AnimationFading _paperNewTurnAnimation;

    [SerializeField]
    private Image _paperNewTurnPlayerImage;

    private void UpdateReplyState()
    {
        _pauseMenuReplyBTN.SetActive(!GameplayManager.IsOnline);
        _gameOverReplyBTN.SetActive(!GameplayManager.IsOnline);
    }

    public void NewTurn()
    {
        StopTimer();
        ClearTriggers();
        _animatorNewTurn.SetTrigger("NewTurn");
        _timerCoroutine = StartCoroutine(ITimerProcess());
        ;
        _newTurnBTN.SetActive(SlotManager.Instance.IsCurrentPlayerOnSlot);
        Debug.Log($"Current side : {_newTurnBTN.activeSelf}");
    }

    public void SetSideBannerTurn(int side)
    {
        ClearTriggers();
        _animatorNewTurn.SetTrigger((side == 1) ? "XTurn" : "OTurn");
    }

    private void ClearTriggers()
    {
        _animatorNewTurn.ResetTrigger("NewTurn");
        _animatorNewTurn.ResetTrigger("XTurn");
        _animatorNewTurn.ResetTrigger("OTurn");
    }

    public void UpdateScore()
    {
        _playerOneScoreText.text = ScoreManager.Instance.GetScore(1).ToString();
        _playerTwoScorText.text = ScoreManager.Instance.GetScore(2).ToString();

        UpdatePlayerRP();
    }

    private void UpdatePlayerRP()
    {
        for (int i = 0; i < _playerOneRPList.Count; i++)
        {
            _playerOneRPList[i].SetActive(i < ScoreManager.Instance.GetCountRoundWin(1));
        }

        for (int i = 0; i < _playerTwoRPList.Count; i++)
        {
            _playerTwoRPList[i].SetActive(i < ScoreManager.Instance.GetCountRoundWin(2));
        }
    }

    public void ReturnHome()
    {
        if (!GameplayManager.IsCurrentGameplayState(GameplayManager.GameplayState.GameOver))
        {
            AnalitycManager.Player_Lose_Match(GameplayManager.TypeGame, CardManager.CardList);
            AnalitycManager.Player_Leave_Match(GameplayManager.TypeGame, CardManager.CardList);
        }

        if (GameplayManager.IsOnline) RoomManager.LeaveRoom(true);
        GameSceneManager.Instance.BeginTransaction();
    }

    public void EndButtonPressed()
    {
        if (GameplayManager.IsOnline && PlayerManager.Instance.GetCurrentPlayer().SideId !=
            Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber) return;
        GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);
        NetworkEventManager.RaiseEventEndTurn();
    }

    public void StateGameOverPanel(bool state, int value = 0)
    {
        UpdateReplyState();
        if (state) _gameOverPanel.FadeIn();
        else _gameOverPanel.FadeOut();
        if (state)
        {
            int currentWinner = ScoreManager.Instance.GetGameWinner();
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

            _windrawBG.SetActive(isWin || currentWinner == -1);
            _loseBG.SetActive(!(isWin || currentWinner == -1));
            _moneyValue.text = "+" + value.ToString();

            _winnerText.text = (currentWinner == -1) ? "Draw" :
                (isWin) ? "You\nwin" : "You\nlose";

            _drawArea.SetActive(ScoreManager.Instance.GetGameWinner() == -1);
            _winnerArea.SetActive(ScoreManager.Instance.GetGameWinner() != -1);
            for (int i = 0; i < _bgList.Count; i++)
            {
                _bgList[i].SetActive(i == Mathf.Max(0, currentWinner));
            }

            if (ScoreManager.Instance.GetGameWinner() != -1)
                _gameOverLogo.sprite =
                    ThemeManager.Instance.GetSprite((CellFigure) ScoreManager.Instance.GetGameWinner());
        }
    }

    public void RestartGame()
    {
        if (GameplayManager.IsOnline) return;
        if (_gameOverPanel.gameObject.activeSelf) StateGameOverPanel(false);
        GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.RestartGame);
    }


    public void StopTimer()
    {
        if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
        _timerCoroutine = null;
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

    public IEnumerator ShowRoundOverAnimation()
    {
        _roundOverPanel.FadeIn();
        int currentWinner = ScoreManager.Instance.GetRoundWinner();
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

        _windrawRoundOverBG.SetActive(isWin || currentWinner == -1);
        _loseRoundOverBG.SetActive(!(isWin || currentWinner == -1));

        _winnerRoundOverText.text = (currentWinner == -1) ? "Draw" :
            (isWin) ? "You\nwin" : "You\nlose";

        _drawRoundOverArea.SetActive(ScoreManager.Instance.GetRoundWinner() == -1);
        _winnerRoundOverArea.SetActive(ScoreManager.Instance.GetRoundWinner() != -1);
        for (int i = 0; i < _bgList.Count; i++)
        {
            _bgList[i].SetActive(i == Mathf.Max(0, currentWinner));
        }

        if (ScoreManager.Instance.GetRoundWinner() != -1)
            _roundOverLogo.sprite =
                ThemeManager.Instance.GetSprite((CellFigure) ScoreManager.Instance.GetRoundWinner());

        int p1Count = ScoreManager.Instance.GetCountRoundWin(1);
        int p2Count = ScoreManager.Instance.GetCountRoundWin(2);

        for (int i = 0; i < _p1RoundOverPoints.Count; i++)
        {
            _p1RoundOverPoints[i].SetActive(i < p1Count);
        }

        for (int i = 0; i < _p2RoundOverPoints.Count; i++)
        {
            _p2RoundOverPoints[i].SetActive(i < p2Count);
        }

        Transform currentPoint = null;

        if (ScoreManager.Instance.GetRoundWinner() == 1)
        {
            currentPoint = _p1RoundOverPoints[p1Count - 1].transform;
            currentPoint.localScale = Vector2.zero;
        }
        else if (ScoreManager.Instance.GetRoundWinner() == 2)
        {
            currentPoint = _p2RoundOverPoints[p2Count - 1].transform;
            currentPoint.localScale = Vector2.zero;
        }


        yield return StartCoroutine(CoroutineManager.Instance.IAwaitProcess((_roundOverPanel.FrameCount)));
        yield return new WaitForSeconds(0.5f);
        if (currentPoint != null)
            yield return StartCoroutine(currentPoint.ScaleWithLerp(Vector2.zero, Vector2.one, 20));
        yield return new WaitForSeconds(1f);
        _roundOverPanel.FadeOut();
        yield return StartCoroutine(CoroutineManager.Instance.IAwaitProcess((_roundOverPanel.FrameCount)));
    }

    public IEnumerator IShowNewTurnAnimation(CellFigure cellFigure)
    {
        _paperNewTurnPlayerImage.sprite = ThemeManager.Instance.GetSprite(cellFigure);
        _paperNewTurnAnimation.FadeIn();
        Debug.Log("BegunAnim");
        yield return StartCoroutine(CoroutineManager.Instance.IAwaitProcess((_paperNewTurnAnimation.FrameCount)));
        yield return new WaitForSeconds(0.5f);
        _paperNewTurnAnimation.FadeOut();
        yield return StartCoroutine(CoroutineManager.Instance.IAwaitProcess((_paperNewTurnAnimation.FrameCount)));
    }
}