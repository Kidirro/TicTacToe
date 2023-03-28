using System.Collections;
using System.Collections.Generic;
using Field.Interfaces;
using FinishLine.Interfaces;
using GameState.Interfaces;
using Players.Interfaces;
using Score.Interfaces;
using UIPages.Interfaces;
using UnityEngine;
using Zenject;

namespace FinishLine
{
    public class FinishLineObject : Line
    {
        #region Dependency

        private IPlayerService _playerService;
        private IScoreService _scoreService;
        private IScoreWinnerService _scoreWinnerService;
        private IGameStateService _gameStateService;
        private IFieldService _fieldService;
        private IFieldFigureService _fieldFigureService;
        private IFinishLineControllerService _finishLineControllerService;
        private IInGameUIService _inGameUIService;

        [Inject]
        private void Construct(IPlayerService playerService,
            IScoreService scoreService,
            IFieldService fieldService,
            IFieldFigureService fieldFigureService,
            IFinishLineControllerService finishLineControllerService,
            IScoreWinnerService scoreWinnerService,
            IGameStateService gameStateService,
            IInGameUIService inGameUIService)
        {
            _playerService = playerService;
            _scoreService = scoreService;
            _fieldService = fieldService;
            _fieldFigureService = fieldFigureService;
            _finishLineControllerService = finishLineControllerService;
            _gameStateService = gameStateService;
            _scoreWinnerService = scoreWinnerService;
            _inGameUIService = inGameUIService;
        }

        #endregion


        public const float FINISH_COUNT_FRAME = 25;

        // public static float AnimationTime
        // {
        //     get => FINISH_COUNT_FRAME * 2;
        // }


        public IEnumerator FinishLineCleaning(List<Vector2Int> ids, int score)
        {
            int currentPlayer = _playerService.GetCurrentPlayer().SideId;
            Debug.Log($"_playerService.GetCurrentPlayer().SideId {_playerService.GetCurrentPlayer().SideId}");
            Vector2Int id1 = new Vector2Int(ids[0].x, ids[0].y);
            Vector2Int id2 = new Vector2Int(ids[^1].x, ids[^1].y);

            SetAlphaFinishLine(0);
            SetPositions(_fieldService.GetCellPosition(id1), _fieldService.GetCellPosition(id2));
            float j = 0;
            while (j < FINISH_COUNT_FRAME)
            {
                j++;
                SetAlphaFinishLine(j / FINISH_COUNT_FRAME);
                yield return null;
            }

            _scoreService.AddScore(currentPlayer,score);
            _inGameUIService.UpdateScore(_scoreService.GetScore(1), _scoreService.GetScore(2));

            if (_scoreWinnerService.IsExistRoundWinner() &&
                _gameStateService.GetCurrentGameplayState() != GameplayState.GameOver)
            {
                _gameStateService.SetGamePlayStateQueue(GameplayState.RoundOver);
            }

            foreach (Vector2Int id in ids)
            {
                _fieldFigureService.SetFigure(id, CellFigure.None, isQueue: false);
            }

            while (j > 0)
            {
                j--;
                SetAlphaFinishLine(j / FINISH_COUNT_FRAME);
                yield return null;
            }

            _finishLineControllerService.AddToFinishLineList(this);
        }
    }
}