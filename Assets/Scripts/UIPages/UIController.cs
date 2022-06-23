using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : Singleton<UIController>
{

    /// <summary>
    /// Требуется доработка и разбитие на класс карт!
    /// 
    /// </summary>

    [SerializeField] private bool _isNeedGizmos;

    [SerializeField] private TextMeshProUGUI _playerOneScoreText;

    [SerializeField] private TextMeshProUGUI _playerTwoScorText;


    [Space]
    [Header("LOGS")]
    [SerializeField] private TurnHistorySingleCell _logPref;
    [SerializeField] private Transform _logParent;
    [SerializeField] private Vector2 _defaultScreenResolution;
    [SerializeField] private Vector2 _screenBorderX;
    [SerializeField] private Vector2 _screenBorderY;


    [Space, Header("GameOver Properties"),SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Image _gameOverLogo;
    private List<TurnHistorySingleCell> _listLog = new List<TurnHistorySingleCell>();
    private float _startPositionX;
    private float _endPositionX;

    private float _startPositionY;
    private float _endPositionY;
    private float _logSize;
    private float _logBeginX;

    public void Initialization()
    {
        _playerOneScoreText.text = "0";
        _playerTwoScorText.text = "0";
        InitializeLog();
    }



    private void InitializeLog()
    {/*
        GetStartPosition();
        InitializeCellSize();
        for (int i = 0; i < 6; i++)
        {
            TurnHistorySingleCell vr = Instantiate(_logPref);
            vr.transform.SetParent(_logParent);
            _listLog.Add(vr);
            _listLog[i].SetSprite((i % 2 == 0) ? CellFigure.p1 : CellFigure.p2);
            //_listLog[i].SetTransformPosition(_logBeginX + _logSize * (i), _startPositionY * 0.5f + _endPositionY * 0.5f);
        }
        NewTurn(true);
        int currentPlayer = ((int)_listLog.Count / 2) - 1;*/
    }

    private void OnDrawGizmos()
    {
        if (_isNeedGizmos)
        {
            float StartPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
            float EndPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;
            float EndPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * ((_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y))).y;
            float StartPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * ((_screenBorderY.x / _defaultScreenResolution.y)))).y;


            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(StartPositionX, StartPositionY), new Vector2(EndPositionX, StartPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, StartPositionY), new Vector2(EndPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, EndPositionY), new Vector2(StartPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(StartPositionX, EndPositionY), new Vector2(StartPositionX, StartPositionY));
        }
    }

    public void NewTurn(bool instantly = true)
    {
        /*  int currentPlayer = ((int)_listLog.Count / 2) - 1;
          for (int i = 0; i < _listLog.Count - 1; i++)
          {
              if (i == currentPlayer)
              {
                  _listLog[i].SetAlpha(1, instantly);
                  _listLog[i].SetTransformSize(_logSize, instantly);
              }
              else
              {
                  _listLog[i].SetAlpha(Mathf.Abs((1.0f) / (float)(currentPlayer - i)), instantly);
                  _listLog[i].SetTransformSize(Mathf.Abs((1.0f) / (float)(currentPlayer - i)) * _logSize, instantly);
              }
              _listLog[i].SetTransformPosition(_logBeginX + _logSize * (i), _startPositionY * 0.5f + _endPositionY * 0.5f, instantly);

          }
          var k = _listLog[0];

          _listLog[_listLog.Count - 1].SetAlpha(0);
          _listLog[_listLog.Count - 1].SetTransformSize(0);
          _listLog[_listLog.Count - 1].SetTransformPosition(_logBeginX + _logSize * (_listLog.Count - 1), _startPositionY * 0.5f + _endPositionY * 0.5f, instantly);
          _listLog.RemoveAt(0);
          _listLog.Add(k);*/
    }

    public void UpdateScore()
    {
        _playerOneScoreText.text = ScoreManager.Instance.GetScore(1).ToString();
        _playerTwoScorText.text = ScoreManager.Instance.GetScore(2).ToString();
    }

    private void InitializeCellSize()
    {
        float cellSizeY = ((_endPositionY - _startPositionY) / 1f);
        float cellSizeX = ((_endPositionX - _startPositionX) / 5f);

        _logSize = (cellSizeY < cellSizeX) ? cellSizeY : cellSizeX;

        _logBeginX = _startPositionX + (_endPositionX - _startPositionX - 4 * _logSize) / 2;
    }

    private void GetStartPosition()
    {
        _startPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
        _endPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;

        _startPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y)).y;
        _endPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y)).y;

    }


    public void ReturnHome()
    {
        GameSceneManager.Instance.SetGameScene(GameScene.MainMenu);
    }

    public void EndButtonPressed()
    {
        if (TurnController.Instance.CheckCanTurn())
        {
            GameplayManager.Instance.SetGameplayState(GameplayState.NewTurn);
        }
    }

    public void StateGameOverPanel(bool state)
    {
        _gameOverPanel.SetActive(state);
        _gameOverLogo.sprite = ThemeManager.Instance.GetSprite((CellFigure) ScoreManager.Instance.GetWinner());
        Debug.Log(ThemeManager.Instance.GetSprite((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId));
    }

    public void RestartGame()
    {
        StateGameOverPanel(false);
        GameplayManager.Instance.SetGameplayState(GameplayState.RestartGame);
    }
}
