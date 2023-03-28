using System;
using System.Collections;
using System.Collections.Generic;
using Coroutine.Interfaces;
using Field.Interfaces;
using FinishLine.Interfaces;
using ScreenScaler.Interfaces;
using UnityEngine;
using Zenject;

namespace FinishLine
{
    public class FinishLineManager : MonoBehaviour, IFinishLineService, IFinishLineControllerService
    {
        private const int CURRENT_GOAL_LINE = 3;
        private const float LINE_WIDTH_PERCENT = 0.2f;

        [SerializeField]
        private Transform _lineParent;

        private readonly List<List<Vector2Int>> _lineForClearing = new List<List<Vector2Int>>();
        private readonly List<FinishLineObject> _lineFinishEnabled = new();

        private Action _networkEventAction;
        private Action<GameplayState> _newGameplayStateAction;
        private Predicate<GameplayState> _isGameplayEqual;

        #region Dependency

        private ICoroutineService _coroutineService;
        private ICoroutineAwaitService _coroutineAwaitService;
        private IFieldService _fieldService;
        private IFieldFigureService _fieldFigureService;
        private IScreenScaler _screenScaler;
        private IFinishLineFactoryService _finishLineFactoryService;

        [Inject]
        private void Construct(
            ICoroutineService coroutineService, 
            ICoroutineAwaitService coroutineAwaitService,
            IFieldService fieldService,
            IFieldFigureService fieldFigureService,
            IScreenScaler screenScaler,
            IFinishLineFactoryService finishLineFactoryService)
        {
            _coroutineService = coroutineService;
            _coroutineAwaitService = coroutineAwaitService;
            _fieldService = fieldService;
            _fieldFigureService = fieldFigureService;
            _screenScaler = screenScaler;
            _finishLineFactoryService = finishLineFactoryService;
        }

        #endregion


        public void MasterChecker(int figure, bool isInQueue = true, bool isNeedEvent = true)
        {
            if (isInQueue) _coroutineService.AddCoroutine(MasterChecker((CellFigure) figure, isNeedEvent));
            //else StartCoroutine(MasterChecker((CellFigure)figure, isNeedEvent));
        }

        public void SetNetworkEventAction(Action action)
        {
            _networkEventAction = action;
        }

        public void SetNewGameState(Action<GameplayState> action)
        {
            _newGameplayStateAction = action;
        }

        public void SetPredicateIsEqualGameState(Predicate<GameplayState> action)
        {
            _isGameplayEqual = action;
        }

        private IEnumerator MasterChecker(CellFigure figure, bool isNeedEvent)
        {
            if (isNeedEvent) _networkEventAction?.Invoke();
             //_checkEventNetworkService.RaiseEventMasterChecker();
            List<Vector2Int> verticalList = new List<Vector2Int>();
            List<Vector2Int> horizontalList = new List<Vector2Int>();
            List<Vector2Int> diagonalRightList = new List<Vector2Int>();
            List<Vector2Int> diagonalLeftList = new List<Vector2Int>();

            List<List<Vector2Int>> linesFind = new List<List<Vector2Int>>();

            for (int x = 0; x < _fieldService.GetFieldSize().x; x++)
            {
                for (int y = 0; y < _fieldService.GetFieldSize().y; y++)
                {
                    Vector2Int curId = new Vector2Int(x, y);
                    if (
                        _fieldFigureService.GetCellFigure(curId) == figure &&
                        !_fieldFigureService.GetIsCellClear(curId))
                    {
                        if (verticalList.IndexOf(curId) == -1)
                        {
                            //Debug.LogFormat("Current id : {0}", curId);
                            Vector2Int step = new Vector2Int(0, 1);
                            Vector2Int nextVal = GetNextCellId(curId, step);
                            if (nextVal != new Vector2Int(-1, -1))
                            {
                                List<Vector2Int> newLine = new List<Vector2Int>();
                                Vector2Int curIdLocal = curId;
                                newLine.Add(curIdLocal);
                                verticalList.Add(curIdLocal);
                                while (nextVal != new Vector2Int(-1, -1))
                                {
                                    newLine.Add(nextVal);
                                    verticalList.Add(nextVal);
                                    curIdLocal = nextVal;
                                    nextVal = GetNextCellId(curIdLocal, step);
                                }

                                linesFind.Add(newLine);
                            }
                        }

                        if (horizontalList.IndexOf(curId) == -1)
                        {
                            Vector2Int step = new Vector2Int(1, 0);
                            Vector2Int nextVal = GetNextCellId(curId, step);
                            if (nextVal != new Vector2Int(-1, -1))
                            {
                                List<Vector2Int> newLine = new List<Vector2Int>();
                                Vector2Int curIdLocal = curId;
                                newLine.Add(curIdLocal);
                                horizontalList.Add(curIdLocal);
                                while (nextVal != new Vector2Int(-1, -1))
                                {
                                    newLine.Add(nextVal);
                                    horizontalList.Add(nextVal);
                                    curIdLocal = nextVal;
                                    nextVal = GetNextCellId(curIdLocal, step);
                                }

                                linesFind.Add(newLine);
                            }
                        }

                        if (diagonalRightList.IndexOf(curId) == -1)
                        {
                            //Debug.LogFormat("Current id : {0}", curId);
                            Vector2Int step = new Vector2Int(1, 1);
                            Vector2Int nextVal = GetNextCellId(curId, step);
                            if (nextVal != new Vector2Int(-1, -1))
                            {
                                List<Vector2Int> newLine = new List<Vector2Int>();
                                Vector2Int curIdLocal = curId;
                                newLine.Add(curIdLocal);
                                diagonalRightList.Add(curIdLocal);
                                while (nextVal != new Vector2Int(-1, -1))
                                {
                                    newLine.Add(nextVal);
                                    diagonalRightList.Add(nextVal);
                                    curIdLocal = nextVal;
                                    nextVal = GetNextCellId(curIdLocal, step);
                                }

                                Debug.LogFormat("Finded Line. Start at:{0}, End at:{1}, with count{2} ", newLine[0],
                                    newLine[newLine.Count - 1], newLine.Count);
                                linesFind.Add(newLine);
                            }
                        }
                    }

                    curId = new Vector2Int(x, _fieldService.GetFieldSize().y - y - 1);
                    if (_fieldFigureService.GetCellFigure(curId) == figure)
                    {
                        if (diagonalLeftList.IndexOf(curId) == -1)
                        {
                            //Debug.LogFormat("Current id : {0}", curId);
                            Vector2Int step = new Vector2Int(1, -1);
                            Vector2Int nextVal = GetNextCellId(curId, step);
                            if (nextVal != new Vector2Int(-1, -1))
                            {
                                List<Vector2Int> newLine = new List<Vector2Int>();
                                Vector2Int curIdLocal = curId;
                                newLine.Add(curIdLocal);
                                diagonalLeftList.Add(curIdLocal);
                                while (nextVal != new Vector2Int(-1, -1))
                                {
                                    newLine.Add(nextVal);
                                    diagonalLeftList.Add(nextVal);
                                    curIdLocal = nextVal;
                                    nextVal = GetNextCellId(curIdLocal, step);
                                }

                                Debug.LogFormat("Finded Line. Start at:{0}, End at:{1}, with count{2} ", newLine[0],
                                    newLine[newLine.Count - 1], newLine.Count);
                                linesFind.Add(newLine);
                            }
                        }
                    }
                }
            }

            List<List<Vector2Int>> linesRes = new List<List<Vector2Int>>();
            foreach (List<Vector2Int> line in linesFind)
            {
                if (line.Count >= CURRENT_GOAL_LINE &&
                    _lineForClearing.Find(x => x[0] == line[0] && x[^1] == line[^1]) == null)
                {
                    linesRes.Add(line);
                    _lineForClearing.Add(line);
                    foreach (Vector2Int cell in line)
                    {
                        _fieldFigureService.SetIsCellClear(cell, true);
                    }
                }
            }

            if (linesRes.Count > 0) yield return  StartCoroutine(IDrawFinishLine(linesRes));
            else
            {
                if (!_fieldService.IsExistEmptyCell() && _lineForClearing.Count == 0 &&
                    !_isGameplayEqual(GameplayState.GameOver))
                {
                    _newGameplayStateAction(GameplayState.RoundOver);
                }
            }

            yield return null;
        }

        private Vector2Int GetNextCellId(Vector2Int currentId, Vector2Int step)
        {
            Vector2Int nextId = currentId + step;
            if (nextId.x < 0 || nextId.y < 0 || nextId.x >= _fieldService.GetFieldSize().x ||
                nextId.y >= _fieldService.GetFieldSize().y) return new Vector2Int(-1, -1);
            if (_fieldFigureService.GetIsCellClear(nextId)) return new Vector2Int(-1, -1);
            if (_fieldFigureService.IsCellBlocked(nextId)) return new Vector2Int(-1, -1);
            if (_fieldFigureService.GetCellFigure(nextId) !=
                _fieldFigureService.GetCellFigure(currentId)) return new Vector2Int(-1, -1);

            return nextId;
        }

        private IEnumerator IDrawFinishLine(List<List<Vector2Int>> linesFind)
        {
            List<Vector2Int> uniqueCell = new List<Vector2Int>();
            foreach (List<Vector2Int> line in linesFind)
            {
                int i = 0;
                foreach (Vector2Int cell in line)
                {
                    if (uniqueCell.IndexOf(cell) == -1)
                    {
                        uniqueCell.Add(cell);
                        i += 1;
                    }

                    _fieldFigureService.SetIsCellClear(cell, false);
                }

                DrawFinishLine(line, i);
                _lineForClearing.Remove(line);
            }

            yield return _coroutineAwaitService.AwaitTime(FinishLineObject.FINISH_COUNT_FRAME*2);
        }

        public void DrawFinishLine(List<Vector2Int> ids, int score = 0)
        {
            if (_lineFinishEnabled.Count == 0) CreateFinishLine();
            FinishLineObject fl = _lineFinishEnabled[0];
            _lineFinishEnabled.Remove(fl);
            fl.SetAlphaFinishLine(0);
            fl.SetWidthScreenCord(_fieldService.GetCellSize() * LINE_WIDTH_PERCENT / _screenScaler.GetWidthRatio());
            fl.SetPositions(_fieldService.GetCellPosition(ids[0]), _fieldService.GetCellPosition(ids[^1]));
            //_coroutineService.AddCoroutine(fl.FinishLineCleaning(ids, score));
            StartCoroutine(fl.FinishLineCleaning(ids, score));
        }

        private void CreateFinishLine()
        {
            FinishLineObject lr = _finishLineFactoryService.InstantiateFinishLine();
            lr.SetTransformParent(_lineParent.transform);
            lr.SetPositions(Vector2Int.zero, Vector2.zero);
            _lineFinishEnabled.Add(lr);
        }

        public void AddToFinishLineList(FinishLineObject fl)
        {
            _lineFinishEnabled.Add(fl);
        }
    }
}