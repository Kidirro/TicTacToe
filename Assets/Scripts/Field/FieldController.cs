using System;
using System.Collections;
using System.Collections.Generic;
using AI.Interfaces;
using Area.Interfaces;
using Coroutine.Interfaces;
using Field.Interfaces;
using Network.Interfaces;
using Players.Interfaces;
using ScreenScaler.Interfaces;
using Theme.Interfaces;
using UnityEngine;
using Zenject;

namespace Field
{
    public class FieldController : MonoBehaviour, IFieldService, IFieldFigureService, IFieldZoneService
    {
        [SerializeField]
        private bool _isNeedGizmos;

        [Space, Space, Space, Space, Header("Parent properties"), SerializeField]
        private GameObject _lineParent;

        [SerializeField]
        private GameObject _cellParent;

        [Space, Header("Screen properties"), SerializeField]

        //In screen cord
        private Vector2 _screenBorderX;

        [SerializeField]
        private Vector2 _screenBorderY;

        [SerializeField]
        private int _startFieldSize = 3;

        private const int GROW_PER_ROUND = 1;

        private Vector2Int _fieldSize;

        private List<Line> _lineListHorizontal = new();
        private List<Line> _lineListVertical = new();
        private float _cellSize;

        private List<List<Cell>> _cellList = new();

        [Space, Space, Space, SerializeField]
        private float _lineWidthPercent;

        [SerializeField]
        private float _borderPercent;

        //In screen cord
        private float _startPositionX;

        private float _endPositionX;

        //In screen cord
        private float _startPositionY;
        private float _endPositionY;

        private float _remainX;
        private float _remainY;

        #region Dependecy

        private IScreenScaler _screenScaler;
        private ICoroutineService _coroutineService;
        private ICoroutineAwaitService _coroutineAwaitService;
        private IAreaService _areaService;
        private IPlayerService _playerService;
        private IFigureEventNetworkService _figureEventNetworkService;
        private IFreezeEventNetworkService _freezeEventNetworkService;
        private IThemeService _themeService;
        private IAIService _aiService;
        private IFieldFactoryService _fieldFactoryService;
        
        [Inject]
        public void Construct(IScreenScaler screenScaler, ICoroutineService coroutineService,
            ICoroutineAwaitService coroutineAwaitService, IAreaService areaService, IPlayerService playerService,
            IFigureEventNetworkService figureEventNetworkService, IFreezeEventNetworkService freezeEventNetworkService,
            IThemeService themeService, IAIService aiService, IFieldFactoryService fieldFactoryService)
        {
            _screenScaler = screenScaler;
            Debug.Log($"_screenScaler {screenScaler.GetScreenDefault()}");
            Debug.Log($"_screenScaler {_screenScaler.GetScreenDefault()}");
            _coroutineService = coroutineService;
            _coroutineAwaitService = coroutineAwaitService;
            _areaService = areaService;
            _playerService = playerService;
            _figureEventNetworkService = figureEventNetworkService;
            _freezeEventNetworkService = freezeEventNetworkService;
            _themeService = themeService;
            _aiService = aiService;
            _fieldFactoryService = fieldFactoryService;
        }

        #endregion


        public Vector2Int GetFieldSize()
        {
            return _fieldSize;
        }

        public void Initialization(int round = 0)
        {
            StopAllCoroutines();
            Debug.Log($"_screenScaler {_screenScaler.GetScreenDefault()}");
            int size = _startFieldSize + round * GROW_PER_ROUND;

            _fieldSize = new Vector2Int(size, size);
            InitializeField();
            InitializeLine();
            NewCellSize(_fieldSize, false);
        }

        private void GetStartPosition()
        {
            Debug.Log($"_screenBorderX {_screenBorderX.x}");
            _startPositionX = _screenScaler.GetWidth(_screenBorderX.x);
            _endPositionX = _screenScaler.GetWidth(_screenScaler.GetScreenDefault().x - _screenBorderX.y);

            _startPositionY = _screenScaler.GetHeight(_screenBorderY.x);
            _endPositionY = _screenScaler.GetHeight(_screenScaler.GetScreenDefault().y - _screenBorderY.y);
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator AddLineLeft()
        {
            _fieldSize.x += 1; 
            Line lr=_fieldFactoryService.InstantiateLine();
            lr.transform.position = Vector2.zero;
            lr.SetTransformParent(_lineParent.transform);
            _lineListVertical.Insert(0, lr);
            _lineListVertical[0]
                .SetPositions(_lineListVertical[1].StartPoint - new Vector2(_cellList[0][0].CellSize, 0),
                    _lineListVertical[1].EndPoint - new Vector2(_cellList[0][0].CellSize, 0));
            _lineListVertical[0].SetWidthWorldCord(0);
            for (int i = 0; i < _lineListVertical.Count; i++)
            {
                _lineListVertical[i].name = "Vertical Line " + i;
            }

            _cellList.Insert(0, new List<Cell>());
            for (int j = 0; j < _fieldSize.y; j++)
            {
                Cell cell = _fieldFactoryService.InstantiateCell();
                cell.SetTransformParent(_cellParent.transform);
                _cellList[0].Add(cell);
                _cellList[0][j]
                    .SetTransformPosition((_cellList[1][j].Position - new Vector2(_cellList[0][0].CellSize, 0)).x,
                        _cellList[1][j].Position.y);
            }

            for (int i = 0; i < _fieldSize.x; i++)
            {
                for (int j = 0; j < _fieldSize.y; j++)
                {
                    _cellList[i][j].Id = new Vector2Int(i, j);
                    _cellList[i][j].name = "[" + i + "][" + j + "]cell";
                }
            }

            NewCellSize(_fieldSize, false);
            yield return _coroutineAwaitService.AwaitTime(Cell.AnimationTime);
        }

        public IEnumerator AddLineRight()
        {
            _fieldSize.x += 1;
            Line lr = _fieldFactoryService.InstantiateLine();
            lr.transform.position = Vector2.zero;
            lr.SetTransformParent(_lineParent.transform);
            _lineListVertical.Add(lr);
            Vector2 newPosition1 = _lineListVertical[^2].StartPoint +
                                   new Vector2(_cellList[0][0].CellSize, 0);
            Vector2 newPosition2 = _lineListVertical[^2].EndPoint +
                                   new Vector2(_cellList[0][0].CellSize, 0);
            _lineListVertical[^1].SetPositions(newPosition1, newPosition2);
            _lineListVertical[^1].SetWidthWorldCord(0);
            for (int i = 0; i < _lineListVertical.Count; i++)
            {
                _lineListVertical[i].name = "Vertical Line " + i;
            }

            _cellList.Add(new List<Cell>());
            for (int j = 0; j < _fieldSize.y; j++)
            {
                Cell cell = _fieldFactoryService.InstantiateCell();
                cell.SetTransformParent(_cellParent.transform);
                _cellList[_fieldSize.x - 1].Add(cell);
                _cellList[_fieldSize.x - 1][j].SetTransformPosition(
                    (_cellList[_fieldSize.x - 2][j].Position - new Vector2(_cellList[0][0].CellSize, 0)).x,
                    _cellList[_fieldSize.x - 2][j].Position.y);
            }

            for (int i = 0; i < _fieldSize.x; i++)
            {
                for (int j = 0; j < _fieldSize.y; j++)
                {
                    _cellList[i][j].Id = new Vector2Int(i, j);
                    _cellList[i][j].name = "[" + i + "][" + j + "]cell";
                }
            }

            NewCellSize(_fieldSize, false);
            yield return _coroutineAwaitService.AwaitTime(Cell.AnimationTime);
        }

        public IEnumerator AddLineUp()
        {
            _fieldSize.y += 1;
            Line lr = _fieldFactoryService.InstantiateLine();
            lr.transform.position = Vector2.zero;
            lr.SetTransformParent(_lineParent.transform);
            _lineListHorizontal.Add(lr);
            Vector2 newPosition1 = _lineListHorizontal[_lineListHorizontal.Count - 2].StartPoint +
                                   new Vector2(0, _cellList[0][0].CellSize);
            Vector2 newPosition2 = _lineListHorizontal[_lineListHorizontal.Count - 2].EndPoint +
                                   new Vector2(0, _cellList[0][0].CellSize);
            _lineListHorizontal[_lineListHorizontal.Count - 1].SetPositions(newPosition1, newPosition2);
            _lineListHorizontal[_lineListHorizontal.Count - 1].SetWidthWorldCord(0);
            for (int i = 0; i < _lineListHorizontal.Count; i++)
            {
                _lineListHorizontal[i].name = "Horizontal Line " + i;
            }

            for (int j = 0; j < _fieldSize.x; j++)
            {
                Cell cell = _fieldFactoryService.InstantiateCell();
                cell.SetTransformParent(_cellParent.transform);
                _cellList[j].Add(cell);
                _cellList[j][_fieldSize.y - 1].SetTransformPosition(_cellList[j][_fieldSize.y - 2].Position.x,
                    _cellList[j][_fieldSize.y - 2].Position.y + new Vector2(0, _cellList[0][0].CellSize).y);
            }

            for (int i = 0; i < _fieldSize.x; i++)
            {
                for (int j = 0; j < _fieldSize.y; j++)
                {
                    _cellList[i][j].Id = new Vector2Int(i, j);
                    _cellList[i][j].name = "[" + i + "][" + j + "]cell";
                }
            }

            NewCellSize(_fieldSize, false);
            yield return _coroutineAwaitService.AwaitTime(Cell.AnimationTime);
        }

        public IEnumerator AddLineDown()
        {
            _fieldSize.y += 1;
            Line lr = _fieldFactoryService.InstantiateLine();
            lr.transform.position = Vector2.zero;
            lr.SetTransformParent(_lineParent.transform);
            _lineListHorizontal.Insert(0, lr);
            Vector2 newPosition1 = _lineListHorizontal[1].StartPoint - new Vector2(0, _cellList[0][0].CellSize);
            Vector2 newPosition2 = _lineListHorizontal[1].EndPoint - new Vector2(0, _cellList[0][0].CellSize);
            _lineListHorizontal[0].SetPositions(newPosition1, newPosition2);
            _lineListHorizontal[0].SetWidthWorldCord(0);
            for (int i = 0; i < _lineListHorizontal.Count; i++)
            {
                _lineListHorizontal[i].name = "Horizontal Line " + i;
            }

            for (int j = 0; j < _fieldSize.x; j++)
            {
                Cell cell = _fieldFactoryService.InstantiateCell();
                cell.SetTransformParent(_cellParent.transform);
                _cellList[j].Insert(0, cell);
                _cellList[j][0].SetTransformPosition(_cellList[j][1].Position.x,
                    _cellList[j][1].Position.y - new Vector2(0, _cellList[0][0].CellSize).y);
            }

            for (int i = 0; i < _fieldSize.x; i++)
            {
                for (int j = 0; j < _fieldSize.y; j++)
                {
                    _cellList[i][j].Id = new Vector2Int(i, j);
                    _cellList[i][j].name = "[" + i + "][" + j + "]cell";
                }
            }

            NewCellSize(_fieldSize, false);
            yield return _coroutineAwaitService.AwaitTime(Cell.AnimationTime);
        }

        private float GetCellSize(int xSize = 3, int ySize = 3)
        {
            float cellSizeY = ((_endPositionY - _startPositionY) / ySize);
            float cellSizeX = ((_endPositionX - _startPositionX) / xSize);

            return (cellSizeY < cellSizeX) ? cellSizeY : cellSizeX;
        }

        private void InitializeField()
        {
            GetStartPosition();
            if (_cellList.Count != 0)
            {
                for (int i = 0; i < _cellList.Count; i++)
                {
                    for (int j = 0; j < _cellList[i].Count; j++)
                    {
                        Destroy(_cellList[i][j].gameObject);
                    }
                }
            }

            _cellList = new List<List<Cell>>();
            for (int i = 0; i < _fieldSize.x; i++)
            {
                _cellList.Add(new List<Cell>());
                for (int j = 0; j < _fieldSize.y; j++)
                {
                    Cell cell = _fieldFactoryService.InstantiateCell();
                    cell.name = "[" + i + "][" + j + "]cell";
                    cell.Id = new Vector2Int(i, j);
                    cell.SetTransformParent(_cellParent.transform);
                    cell.SetTransformPosition(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
                    _cellList[i].Add(cell);
                }
            }
        }

        private void InitializeLine()
        {
            if (_lineListVertical.Count != 0)
            {
                foreach (Line line in _lineListVertical)
                {
                    Destroy(line.gameObject);
                }
            }

            if (_lineListHorizontal.Count != 0)
            {
                foreach (Line line in _lineListHorizontal)
                {
                    Destroy(line.gameObject);
                }
            }

            _lineListHorizontal = new List<Line>();
            _lineListVertical = new List<Line>();

            int verticalSize = _fieldSize.x - 1;
            int horizontalSize = _fieldSize.y - 1;


            for (int i = 0; i < verticalSize; i++)
            {
                Line lr = _fieldFactoryService.InstantiateLine();
                lr.SetTransformParent(_lineParent.transform);
                lr.SetPositions(new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2),
                    new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
                _lineListVertical.Add(lr);
                lr.name = "Vertical Line " + i;
            }

            for (int j = 0; j < horizontalSize; j++)
            {
                Line lr = _fieldFactoryService.InstantiateLine();
                lr.SetTransformParent(_lineParent.transform);
                var main = Camera.main;
                lr.SetPositions(new Vector2(main.pixelWidth / 2, main.pixelHeight / 2),
                    new Vector2(main.pixelWidth / 2, main.pixelHeight / 2));
                _lineListHorizontal.Add(lr);
                lr.name = "Horizontal Line " + j;
            }
        }

        private void NewCellSize(Vector2Int virtualfieldSize, bool instantly = true)
        {
            _cellSize = GetCellSize(virtualfieldSize.x, virtualfieldSize.y);
            _remainX = (_endPositionX - _startPositionX - _cellSize * _fieldSize.x) / 2;
            _remainY = (_endPositionY - _startPositionY - _cellSize * _fieldSize.y) / 2;
            Vector2 startPositionMatrix = new Vector2(_startPositionX + _remainX + _cellSize / 2,
                _startPositionY + _remainY + _cellSize / 2);
            for (int i = 0; i < _fieldSize.x; i++)
            {
                for (int j = 0; j < _fieldSize.y; j++)
                {
                    _cellList[i][j].SetTransformPosition(startPositionMatrix.x + i * _cellSize,
                        startPositionMatrix.y + j * _cellSize, instantly);
                    _cellList[i][j].SetTransformSize(_cellSize * (1 - _borderPercent), instantly);
                }
            }

            for (int i = 0; i < _lineListVertical.Count; i++)
            {
                Vector3[] points = new Vector3[2];
                points[0] = new Vector2(_cellList[i][0].Position.x * 0.5f + _cellList[i + 1][0].Position.x * 0.5f,
                    _cellList[i][0].Position.y - _cellList[0][0].CellSize / 2);
                points[1] = new Vector2(
                    _cellList[i][_cellList[0].Count - 1].Position.x * 0.5f +
                    _cellList[i + 1][_cellList[0].Count - 1].Position.x * 0.5f,
                    _cellList[i][_cellList[0].Count - 1].Position.y + _cellList[0][0].CellSize / 2);

                _lineListVertical[i]
                    .SetWidthScreenCord(
                        _cellSize * _lineWidthPercent / _screenScaler.GetWidthRatio(),
                        instantly);
                _lineListVertical[i].SetPositions(points[0], points[1], instantly);
            }

            for (int i = 0; i < _lineListHorizontal.Count; i++)
            {
                Vector3[] points = new Vector3[2];
                points[0] = new Vector2(_cellList[0][i].Position.x - _cellSize / 2,
                    _cellList[0][i].Position.y * 0.5f + _cellList[0][i + 1].Position.y * 0.5f);
                points[1] = new Vector2(_cellList[_cellList.Count - 1][i].Position.x + _cellSize / 2,
                    _cellList[1][i].Position.y * 0.5f + _cellList[1][i + 1].Position.y * 0.5f);

                _lineListHorizontal[i]
                    .SetWidthScreenCord(
                        _cellSize * _lineWidthPercent / _screenScaler.GetWidthRatio(),
                        instantly);
                _lineListHorizontal[i].SetPositions(points[0], points[1], instantly);
            }
        }


        public void SwapVerticalLines(int fl, int sl, bool instantly = true)
        {
            for (int i = 0; i < _fieldSize.y; i++)
            {
                (_cellList[fl][i], _cellList[sl][i]) = (_cellList[sl][i], _cellList[fl][i]);
                _cellList[sl][i].Id = new Vector2Int(sl, i);
                _cellList[fl][i].Id = new Vector2Int(fl, i);
            }

            NewCellSize(_fieldSize, instantly);
        }

        public void SwapHorizontalLines(int fl, int sl, bool instantly = true)
        {
            for (int i = 0; i < _fieldSize.x; i++)
            {
                (_cellList[i][fl], _cellList[i][sl]) = (_cellList[i][sl], _cellList[i][fl]);
                _cellList[i][sl].Id = new Vector2Int(i, sl);
                _cellList[i][fl].Id = new Vector2Int(i, fl);
            }

            NewCellSize(_fieldSize, instantly);
        }

        private void OnDrawGizmos()
        {
            if (!_isNeedGizmos) return;
            float startPositionX = Camera.main
                .ScreenToWorldPoint(
                    new Vector3(_screenScaler.GetWidth(_screenBorderX.x), 0)).x;
            float endPositionX = Camera.main.ScreenToWorldPoint(new Vector2(
                _screenScaler.GetWidth(_screenScaler.GetScreenDefault().x - _screenBorderX.y), 0)).x;

            float startPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0,
                _screenScaler.GetHeight(_screenBorderY.x))).y;
            float endPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0,
                _screenScaler.GetHeight(
                    _screenScaler.GetScreenDefault().y - _screenBorderY.y))).y;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(startPositionX, startPositionY), new Vector2(endPositionX, startPositionY));
            Gizmos.DrawLine(new Vector2(endPositionX, startPositionY), new Vector2(endPositionX, endPositionY));
            Gizmos.DrawLine(new Vector2(endPositionX, endPositionY), new Vector2(startPositionX, endPositionY));
            Gizmos.DrawLine(new Vector2(startPositionX, endPositionY), new Vector2(startPositionX, startPositionY));
        }

      
        public bool CheckIsInField(Vector2 pos)
        {
            return pos.x >= (_startPositionX + _remainX) & pos.x <= (_endPositionX - _remainX) &
                   pos.y >= (_startPositionY + _remainY) & pos.y <= (_endPositionY - _remainY);
        }

        public List<Cell> GetAllCellWithFigure(CellFigure figure)
        {
            List<Cell> result = new List<Cell>();
            for (int i = 0; i < _cellList.Count; i++)
            {
                result.AddRange(_cellList[i].FindAll(x => x.Figure == figure));
            }

            return result;
        }

        public List<Cell> GetAllCellWithSubState(CellSubState figureSubState)
        {
                List<Cell> result = new List<Cell>();
                for (int i = 0; i < _cellList.Count; i++)
                {
                    result.AddRange(_cellList[i].FindAll(x => x.SubState == figureSubState));
                }

                return result;
        }

        public bool IsInFieldHeight(float h)
        {
            return h >= (_startPositionY + _remainY);
        }

        public List<Cell> GetAllEmptyNeighbours(Cell cell)
        {
            List<Cell> cells = new List<Cell>();
            Vector2Int startPos = cell.Id;
            Cell checkedPos = IsOnField(startPos, Vector2Int.down);
            if (checkedPos != null && IsCellEnableToPlace(checkedPos.Id))
                cells.Add(_cellList[checkedPos.Id.x][checkedPos.Id.y]);

            checkedPos = IsOnField(startPos, Vector2Int.up);
            if (checkedPos != null && IsCellEnableToPlace(checkedPos.Id))
                cells.Add(_cellList[checkedPos.Id.x][checkedPos.Id.y]);

            checkedPos = IsOnField(startPos, Vector2Int.right);
            if (checkedPos != null && IsCellEnableToPlace(checkedPos.Id))
                cells.Add(_cellList[checkedPos.Id.x][checkedPos.Id.y]);

            checkedPos = IsOnField(startPos, Vector2Int.left);
            if (checkedPos != null && IsCellEnableToPlace(checkedPos.Id))
                cells.Add(_cellList[checkedPos.Id.x][checkedPos.Id.y]);

            return cells;
        }

        /// <summary>
        ///??? ?????? ?? ??????? ??? ?????????? ????? - ????????? ?????
        /// </summary>
        /// <param name="pos"> Position</param>
        /// <param name="isFindBorder">???? ?????? ?? ???????: ??????? ????? (true) ??? (-1,-1) (false) </param>
        /// <returns></returns>
        public Vector2Int GetIdFromPosition(Vector2 pos, bool isFindBorder)
        {
            Vector2Int posFinal = Vector2Int.zero;
            posFinal.y = (int) Mathf.Clamp((float) Math.Floor((pos.y - (_startPositionY + _remainY)) / _cellSize), 0,
                _fieldSize.y - 1);
            posFinal.x = (int) Mathf.Clamp((float) Math.Floor((pos.x - (_startPositionX + _remainX)) / _cellSize), 0,
                _fieldSize.x - 1);
            if (CheckIsInField(pos) || isFindBorder) return posFinal;
            else return (new Vector2Int(-1, -1));
        }

        public void ResetSubStateZone(Vector2Int position, Vector2Int areaSize, bool isQueue = true)
        {
            Vector4 currentArea = _areaService.GetArea(position, areaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    _cellList[x][y].ResetSubState(isQueue);
                }
            }
        }

        private void SetSubStateZone(Vector2Int position, Vector2Int areaSize, Sprite sprite, Color color,
            CellSubState cellSubState, bool isQueue = true)
        {
            Vector4 currentArea = _areaService.GetArea(position, areaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    _cellList[x][y].SetSubState(sprite, color, cellSubState, isQueue);
                }
            }
        }

        public void UnhighlightZone(Vector2Int position, Vector2Int areaSize)
        {
            Vector4 currentArea = _areaService.GetArea(position, areaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    _cellList[x][y].UnhighlightCell();
                }
            }
        }

        public void HighlightZone(Vector2Int position, Vector2Int areaSize, Sprite sprite, Color color)
        {
            Vector4 currentArea = _areaService.GetArea(position, areaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    _cellList[x][y].HighlightCell(sprite, color);
                }
            }
        }

        public bool IsZoneEmpty(Vector2Int position, Vector2Int areaSize)
        {
            Vector4 currentArea = _areaService.GetArea(position, areaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    if (!IsCellEmpty(x, y)) return false;
                }
            }

            return true;
        }

        public bool IsZoneEnableToPlace(Vector2Int position, Vector2Int areaSize)
        {
            Vector4 currentArea = _areaService.GetArea(position, areaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    if (!IsCellEnableToPlace(x, y)) return false;
                }
            }

            return true;
        }

        public Cell GetCellLink(Vector2Int id)
        {
            return _cellList[id.x][id.y];
        }

        public Cell GetCellLink(int x, int y)
        {
            return _cellList[x][y];

            }

        public CellFigure GetCellFigure(Vector2Int id)
        {
            return _cellList[id.x][id.y].Figure;
        }

        public CellFigure GetCellFigure(int x, int y)
        {
            return _cellList[x][y].Figure;
        }

 

        public bool IsCellEmpty(Vector2Int id)
        {
            return _cellList[id.x][id.y].Figure == CellFigure.None;
        }

        public bool IsCellEmpty(int x, int y)
        {
            return _cellList[x][y].Figure == CellFigure.None;
        }

        public bool IsCellBlocked(Vector2Int id)
        {
            return _cellList[id.x][id.y].SubState == CellSubState.Freeze;
        }

        public bool IsCellBlocked(int x, int y)
        {
            return _cellList[x][y].SubState == CellSubState.Freeze;
        }

        public bool IsCellEnableToPlace(Vector2Int id)
        {
            return IsCellEmpty(id) && !IsCellBlocked(id);
        }

        public bool IsCellEnableToPlace(int x, int y)
        {
            return IsCellEmpty(x, y) && !IsCellBlocked(x, y);
        }

        public Cell GetNextCell(Vector2Int currentId, Vector2Int step)
        {
            Vector2Int nextId = currentId + step;
            if (IsOnField(currentId, step) == null) return null;
            if (_cellList[nextId.x][nextId.y].Figure == CellFigure.None) return null;
            return _cellList[nextId.x][nextId.y];
        }

        public float GetCellSize()
        {
            return _cellSize;
        }

        public Vector2 GetCellPosition(Vector2Int id)
        {
            return _cellList[id.x][id.y].Position;
        }

        public Vector2 GetCellPosition(int x, int y)
        {
            return _cellList[x][y].Position;
        }

        public Cell IsOnField(Vector2Int currentId, Vector2Int step)
        {
            Vector2Int nextId = currentId + step;
            if (nextId.x < 0 || nextId.y < 0 || nextId.x >= _fieldSize.x ||
                nextId.y >= _fieldSize.y) return null;
            return _cellList[nextId.x][nextId.y];
        }
        
        public bool IsExistEmptyCell()
        {
            for (int i = 0; i < _cellList.Count; i++)
            {
                for (int j = 0; j < _cellList[i].Count; j++)
                {
                    if (IsCellEnableToPlace(i, j)) return true;
                }
            }

            return false;
        }

        public bool GetIsCellClear(Vector2Int id)
        {
            return _cellList[id.x][id.y].GetIsCellClear();
        }

        public bool GetIsCellClear(int x, int y)
        {
            return _cellList[x][y].GetIsCellClear();
        }

        public void SetIsCellClear(Vector2Int id, bool state)
        {
            _cellList[id.x][id.y].SetIsCellClear(state);
        }

        public void SetIsCellClear(int x, int y, bool state)
        {
            _cellList[x][y].SetIsCellClear(state);
        }

        public void PlaceInCell(Vector2Int id, bool isNeedEvent = true, bool isQueue = true)
        {
            if (IsCellEnableToPlace(id))
            {
                SetFigure(id, (CellFigure)_playerService.GetCurrentPlayer().SideId, isQueue: isQueue);
                if (isNeedEvent) _figureEventNetworkService.RaiseEventPlaceInCell(id);
            }
        }

        public void SetFigure(Vector2Int id, CellFigure figure,bool  isQueue = true)
        {
            if (IsCellEnableToPlace(id)^ figure ==CellFigure.None)
            {
                Debug.Log($"Current figure {figure}");
                _cellList[id.x][id.y].SetFigure(figure, isQueue: isQueue);
            }
        }
        
        public void SetFigure(int x, int y, CellFigure figure, bool isQueue = true)
        {   
            if (IsCellEnableToPlace(new Vector2Int(x,y))^ figure ==CellFigure.None)
            {
                _cellList[x][y].SetFigure(figure, isQueue: isQueue);
            }
        }
        
        public void PlaceInRandomCell(bool isNeedQueue = true)
        {
            if (isNeedQueue) _coroutineService.AddCoroutine(IPlaceInRandomCell());
            else StartCoroutine(IPlaceInRandomCell());
        }

        public void FreezeCell(Vector2Int id, Sprite sprite, bool isNeedEvent = true)
        {
            if (_cellList[id.x][id.y].Figure == CellFigure.None)
            {
                SetSubStateZone(id,
                    Vector2Int.one,
                    sprite,
                    Color.black,
                    CellSubState.Freeze, false);
            }
            else
            {
                ResetFigureWithPlaceSubState(id,
                    Vector2Int.one,
                    sprite,
                    Color.black,
                    CellSubState.Freeze);
            }

            if (isNeedEvent) _freezeEventNetworkService.RaiseEventFreezeCell(id);
        }

        public void FreezeCell(Vector2Int id, bool isNeedEvent = true)
        {
            if (_cellList[id.x][id.y].Figure == CellFigure.None)
            {
                SetSubStateZone(id,
                    Vector2Int.one,
                    _themeService.GetSprite(CellSubState.Freeze),
                    Color.black,
                    CellSubState.Freeze, false);
            }
            else
            {
                ResetFigureWithPlaceSubState(id,
                    Vector2Int.one,
                    _themeService.GetSprite(CellSubState.Freeze),
                    Color.black,
                    CellSubState.Freeze);
            }

            if (isNeedEvent) _freezeEventNetworkService.RaiseEventFreezeCell(id);
        }

        public void ResetSubStateWithPlaceFigure(Vector2Int position, bool isNeedEvent = true)
        {
            _cellList[position.x][position.y]
                .ResetSubStateWithPlace((CellFigure) _playerService.GetCurrentPlayer().SideId);
            if (isNeedEvent) _freezeEventNetworkService.RaiseEventUnFreezeCell(position);
        }

        public void ResetFigureWithPlaceSubState(Vector2Int position, Vector2Int areaSize, Sprite sprite, Color color,
            CellSubState cellSubState)
        {
            Vector4 currentArea = _areaService.GetArea(position, areaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    _cellList[x][y].ResetFigureWithPlaceState(sprite, color, cellSubState);
                }
            }
        }

        private IEnumerator IPlaceInRandomCell()
        {
            Vector2Int id = _aiService.GenerateRandomPosition(_fieldSize);
            Debug.Log(id);
            if (id != new Vector2Int(-1, -1))
            {
                PlaceInCell(id, isQueue: false);
                yield return _coroutineAwaitService.AwaitTime(Cell.AnimationTime);
            }
            else
            {
                yield return null;
            }
        }
    }
}