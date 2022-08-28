using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field : Singleton<Field>
{

    [SerializeField]
    private bool _isNeedGizmos;

    [Space]
    [Space]
    [Space]
    [Space]

    [SerializeField]
    private GameObject _lineParent;

    [SerializeField]
    private GameObject _cellParent;

    [Space]

    //In screen cord
    [SerializeField]
    private Vector2 _screenBorderX;

    [SerializeField]
    private Vector2 _screenBorderY;

    [SerializeField]
    private static Vector2Int _startFieldSize = new Vector2Int(3, 3);

    [SerializeField]
    private Vector2Int _growSizeMax = new Vector2Int(6, 6);


    private Vector2Int _fieldSize;

    public Vector2Int FieldSize
    {
        get { return _fieldSize; }
    }

    private List<Line> _lineListHorizontal = new List<Line>();
    private List<Line> _lineListVertical = new List<Line>();
    private List<FinishLine> _lineFinishEnabled = new List<FinishLine>();
    private float _cellSize;

    private List<List<Cell>> _cellList = new List<List<Cell>>();

    public List<List<Cell>> CellList
    {
        get { return _cellList; }
    }

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private GameObject _finishLinePrefab;

    [Space]
    [Space]
    [Space]

    [SerializeField] private float _lineWidthPercent;
    [SerializeField] private float _borderPercent;

    public float LineWidthPercent
    {
        get { return _lineWidthPercent; }
    }

    //In screen cord
    private float _startPositionX;
    private float _endPositionX;
    //In screen cord
    private float _startPositionY;
    private float _endPositionY;

    private float _remainX;
    private float _remainY;


    private bool _isFieldGrow_Dew = true;

    private int _cyclePerGrow = 2;
    private int _currenCycle = 0;


    [HideInInspector]
    public int CellAnimating = 0;


    public void Initialization()
    {
        StopAllCoroutines();
        CellAnimating = 0;
        InitializeField();

        _fieldSize = _startFieldSize;
        InitializeLine();
        NewCellSize(_fieldSize, false);
    }

    private void GetStartPosition()
    {
        _startPositionX = ScreenManager.Instance.GetWidth(_screenBorderX.x);
        _endPositionX = ScreenManager.Instance.GetWidth(ScreenManager.Instance.ScreenDefault.x - _screenBorderX.y);

        _startPositionY = ScreenManager.Instance.GetHeight(_screenBorderY.x);
        _endPositionY = ScreenManager.Instance.GetHeight(ScreenManager.Instance.ScreenDefault.y - _screenBorderY.y);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddLineLeft()
    {
        _fieldSize.x += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        LR.SetTransformParent(_lineParent.transform);
        _lineListVertical.Insert(0, LR);
        _lineListVertical[0].SetPositions(_lineListVertical[1].StartPoint - new Vector2(_cellList[0][0].CellSize, 0), _lineListVertical[1].EndPoint - new Vector2(_cellList[0][0].CellSize, 0));
        _lineListVertical[0].SetWidthWorldCord(0);
        for (int i = 0; i < _lineListVertical.Count; i++)
        {
            _lineListVertical[i].name = "Vertical Line " + i;
        }
        _cellList.Insert(0, new List<Cell>());
        for (int j = 0; j < _fieldSize.y; j++)
        {
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            cell.SetTransformParent(_cellParent.transform);
            _cellList[0].Add(cell);
            _cellList[0][j].SetTransformPosition((_cellList[1][j].Position - new Vector2(_cellList[0][0].CellSize, 0)).x, _cellList[1][j].Position.y);
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

    }

    public void AddLineRight()
    {
        _fieldSize.x += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        LR.SetTransformParent(_lineParent.transform);
        _lineListVertical.Add(LR);
        Vector2 newPosition1 = _lineListVertical[_lineListVertical.Count - 2].StartPoint + new Vector2(_cellList[0][0].CellSize, 0);
        Vector2 newPosition2 = _lineListVertical[_lineListVertical.Count - 2].EndPoint + new Vector2(_cellList[0][0].CellSize, 0);
        _lineListVertical[_lineListVertical.Count - 1].SetPositions(newPosition1, newPosition2);
        _lineListVertical[_lineListVertical.Count - 1].SetWidthWorldCord(0);
        for (int i = 0; i < _lineListVertical.Count; i++)
        {
            _lineListVertical[i].name = "Vertical Line " + i;
        }
        _cellList.Add(new List<Cell>());
        for (int j = 0; j < _fieldSize.y; j++)
        {
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            cell.SetTransformParent(_cellParent.transform);
            _cellList[_fieldSize.x - 1].Add(cell);
            _cellList[_fieldSize.x - 1][j].SetTransformPosition((_cellList[_fieldSize.x - 2][j].Position - new Vector2(_cellList[0][0].CellSize, 0)).x, _cellList[_fieldSize.x - 2][j].Position.y);
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

    }

    public void AddLineUp()
    {
        _fieldSize.y += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        LR.SetTransformParent(_lineParent.transform);
        _lineListHorizontal.Add(LR);
        Vector2 newPosition1 = _lineListHorizontal[_lineListHorizontal.Count - 2].StartPoint + new Vector2(0, _cellList[0][0].CellSize);
        Vector2 newPosition2 = _lineListHorizontal[_lineListHorizontal.Count - 2].EndPoint + new Vector2(0, _cellList[0][0].CellSize);
        _lineListHorizontal[_lineListHorizontal.Count - 1].SetPositions(newPosition1, newPosition2);
        _lineListHorizontal[_lineListHorizontal.Count - 1].SetWidthWorldCord(0);
        for (int i = 0; i < _lineListHorizontal.Count; i++)
        {
            _lineListHorizontal[i].name = "Horizontal Line " + i;
        }
        for (int j = 0; j < _fieldSize.x; j++)
        {
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            cell.SetTransformParent(_cellParent.transform);
            _cellList[j].Add(cell);
            _cellList[j][_fieldSize.y - 1].SetTransformPosition(_cellList[j][_fieldSize.y - 2].Position.x, _cellList[j][_fieldSize.y - 2].Position.y + new Vector2(0, _cellList[0][0].CellSize).y);
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
    }

    public void AddLineDown()
    {
        _fieldSize.y += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        LR.SetTransformParent(_lineParent.transform);
        _lineListHorizontal.Insert(0, LR);
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
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            cell.SetTransformParent(_cellParent.transform);
            _cellList[j].Insert(0, cell);
            _cellList[j][0].SetTransformPosition(_cellList[j][1].Position.x, _cellList[j][1].Position.y - new Vector2(0, _cellList[0][0].CellSize).y);
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
    }

    private float GetCellSize(int x_size = 3, int y_size = 3)
    {
        float cellSizeY = ((_endPositionY - _startPositionY) / y_size);
        float cellSizeX = ((_endPositionX - _startPositionX) / x_size);

        return (cellSizeY < cellSizeX) ? cellSizeY : cellSizeX;
    }

    private void InitializeField()
    {
        _currenCycle = 0;
        GetStartPosition();
        if (_cellList.Count != 0)
        {
            for (int i = 0; i < _fieldSize.x; i++)
            {
                for (int j = 0; j < _fieldSize.y; j++)
                {
                    Destroy(_cellList[i][j].gameObject);

                }
            }
        }
        _cellList = new List<List<Cell>>();
        for (int i = 0; i < _startFieldSize.x; i++)
        {
            _cellList.Add(new List<Cell>());
            for (int j = 0; j < _startFieldSize.y; j++)
            {
                GameObject newCellObject = Instantiate(_cellPrefab);
                Cell cell = newCellObject.GetComponent<Cell>();
                newCellObject.name = "[" + i + "][" + j + "]cell";
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
            GameObject newLine = Instantiate(_linePrefab);
            Line LR = newLine.GetComponent<Line>();
            LR.SetTransformParent(_lineParent.transform);
            LR.SetPositions(new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2), new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
            _lineListVertical.Add(LR);
            newLine.name = "Vertical Line " + i;


        }

        for (int j = 0; j < horizontalSize; j++)
        {
            GameObject newLine = Instantiate(_linePrefab);
            Line LR = newLine.GetComponent<Line>();
            LR.SetTransformParent(_lineParent.transform);
            LR.SetPositions(new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2), new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
            _lineListHorizontal.Add(LR);
            newLine.name = "Horizontal Line " + j;


        }
    }

    public void NewCellSize(Vector2Int VirtualfieldSize, bool instantly = true)
    {
        _cellSize = GetCellSize(VirtualfieldSize.x, VirtualfieldSize.y);
        _remainX = (_endPositionX - _startPositionX - _cellSize * _fieldSize.x) / 2;
        _remainY = (_endPositionY - _startPositionY - _cellSize * _fieldSize.y) / 2;
        Vector2 StartPositionMatrix = new Vector2(_startPositionX + _remainX + _cellSize / 2, _startPositionY + _remainY + _cellSize / 2);
        for (int i = 0; i < _fieldSize.x; i++)
        {
            for (int j = 0; j < _fieldSize.y; j++)
            {
                _cellList[i][j].SetTransformPosition(StartPositionMatrix.x + i * _cellSize, StartPositionMatrix.y + j * _cellSize, instantly);
                _cellList[i][j].SetTransformSize(_cellSize * (1 - _borderPercent), instantly);
            }
        }

        for (int i = 0; i < _lineListVertical.Count; i++)
        {
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[i][0].Position.x * 0.5f + _cellList[i + 1][0].Position.x * 0.5f, _cellList[i][0].Position.y - _cellList[0][0].CellSize / 2);
            points[1] = new Vector2(_cellList[i][_cellList[0].Count - 1].Position.x * 0.5f + _cellList[i + 1][_cellList[0].Count - 1].Position.x * 0.5f, _cellList[i][_cellList[0].Count - 1].Position.y + _cellList[0][0].CellSize / 2);

            _lineListVertical[i].SetWidthScreenCord(_cellSize * _lineWidthPercent / ScreenManager.Instance.GetWidthRatio(), instantly);
            _lineListVertical[i].SetPositions(points[0], points[1], instantly);
        }

        for (int i = 0; i < _lineListHorizontal.Count; i++)
        {
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[0][i].Position.x - _cellSize / 2, _cellList[0][i].Position.y * 0.5f + _cellList[0][i + 1].Position.y * 0.5f);
            points[1] = new Vector2(_cellList[_cellList.Count - 1][i].Position.x + _cellSize / 2, _cellList[1][i].Position.y * 0.5f + _cellList[1][i + 1].Position.y * 0.5f);

            _lineListHorizontal[i].SetWidthScreenCord(_cellSize * _lineWidthPercent / ScreenManager.Instance.GetWidthRatio(), instantly);
            _lineListHorizontal[i].SetPositions(points[0], points[1], instantly);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NewCellSize(_fieldSize, false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            NewCellSize(_fieldSize + Vector2Int.one, false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddLineLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddLineRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddLineUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddLineDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var kk = _cellList[0][0];
            _cellList[0][0] = _cellList[0][1];
            _cellList[0][1] = kk;

            kk = _cellList[0][1];
            _cellList[0][1] = _cellList[1][1];
            _cellList[1][1] = kk;

            kk = _cellList[1][1];
            _cellList[1][1] = _cellList[1][0];
            _cellList[1][0] = kk;
            NewCellSize(_fieldSize, false);
        }

    }

    public void SwapVerticalLines(int fl, int sl, bool instantly = true)
    {
        for (int i = 0; i < _fieldSize.y; i++)
        {
            var kk = _cellList[fl][i];
            _cellList[fl][i] = _cellList[sl][i];
            _cellList[sl][i] = kk;
            _cellList[sl][i].Id = new Vector2Int(sl, i);
            _cellList[fl][i].Id = new Vector2Int(fl, i);
        }

        NewCellSize(_fieldSize, instantly);

    }

    public void SwapHorizontalLines(int fl, int sl, bool instantly = true)
    {
        for (int i = 0; i < _fieldSize.x; i++)
        {
            var kk = _cellList[i][fl];
            _cellList[i][fl] = _cellList[i][sl];
            _cellList[i][sl] = kk;
            _cellList[i][sl].Id = new Vector2Int(i, sl);
            _cellList[i][fl].Id = new Vector2Int(i, fl);
        }

        NewCellSize(_fieldSize, instantly);


    }

    private void OnDrawGizmos()
    {
        if (_isNeedGizmos)
        {
            float StartPositionX = Camera.main.ScreenToWorldPoint(new Vector3(ScreenManager.Instance.GetWidth(_screenBorderX.x), 0)).x;
            float EndPositionX = Camera.main.ScreenToWorldPoint(new Vector2(ScreenManager.Instance.GetWidth(ScreenManager.Instance.ScreenDefault.x - _screenBorderX.y), 0)).x;

            float StartPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, ScreenManager.Instance.GetHeight(_screenBorderY.x))).y;
            float EndPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, ScreenManager.Instance.GetHeight(ScreenManager.Instance.ScreenDefault.y - _screenBorderY.y))).y;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(StartPositionX, StartPositionY), new Vector2(EndPositionX, StartPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, StartPositionY), new Vector2(EndPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, EndPositionY), new Vector2(StartPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(StartPositionX, EndPositionY), new Vector2(StartPositionX, StartPositionY));
        }
    }

    public void DrawFinishLine(List<Vector2Int> ids, int score = 0)
    {
        if (_lineFinishEnabled.Count == 0) CreateFinishLine();
        FinishLine FL = _lineFinishEnabled[0];
        _lineFinishEnabled.Remove(FL);
        FL.SetWidthScreenCord(_cellSize * _lineWidthPercent / ScreenManager.Instance.GetWidthRatio());
        FL.SetPositions(CellList[ids[0].x][ids[0].y].Position, CellList[ids[ids.Count - 1].x][ids[ids.Count - 1].x].Position);
        StartCoroutine(FL.FinishLineCleaning(ids, score));
    }

    private void CreateFinishLine()
    {
        GameObject newLine = Instantiate(_finishLinePrefab);
        FinishLine LR = newLine.GetComponent<FinishLine>();
        LR.SetTransformParent(_lineParent.transform);
        LR.SetPositions(Vector2Int.zero, Vector2.zero);
        _lineFinishEnabled.Add(LR);
    }

    public void AddToFinishLineList(FinishLine FL)
    {
        _lineFinishEnabled.Add(FL);
    }

    public bool CheckIsInField(Vector2 pos)
    {
        return pos.x >= (_startPositionX + _remainX) & pos.x <= (_endPositionX - _remainX) &
               pos.y >= (_startPositionY + _remainY) & pos.y <= (_endPositionY - _remainY);
    }

    public bool IsInFieldHeight(float h)
    {

        return h >= (_startPositionY + _remainY);
    }

    /// <summary>
    ///при выходе за границы при включенном флаге - последн€€ точка
    /// </summary>
    /// <param name="pos"> Position</param>
    /// <param name="IsFindBorder">ѕрив выходе за границы: крайн€€ точка (true) или (-1,-1) (false) </param>
    /// <returns></returns>
    public Vector2Int GetIdFromPosition(Vector2 pos, bool IsFindBorder)
    {
        Vector2Int pos_final = Vector2Int.zero;
        pos_final.y = (int)Mathf.Clamp((float)Math.Floor((pos.y - (_startPositionY + _remainY)) / _cellSize), 0, _fieldSize.y - 1);
        pos_final.x = (int)Mathf.Clamp((float)Math.Floor((pos.x - (_startPositionX + _remainX)) / _cellSize), 0, _fieldSize.x - 1);
        if (CheckIsInField(pos) || IsFindBorder) return pos_final;
        else return (new Vector2Int(-1, -1));
    }

    public void ResetSubStateZone(Vector2Int Position, Vector2Int AreaSize)
    {
        Vector4 CurrentArea = AreaManager.GetArea(Position, AreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                CellList[x][y].ResetSubState();
            }
        }
    }

    public void SetSubStateZone(Vector2Int Position, Vector2Int AreaSize, Sprite sprite, Color color, CellSubState cellSubState)
    {
        Vector4 CurrentArea = AreaManager.GetArea(Position, AreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                CellList[x][y].SetSubState(sprite, color, cellSubState);
            }
        }
    }

    public void UnhighlightZone(Vector2Int Position, Vector2Int AreaSize)
    {
        Vector4 CurrentArea = AreaManager.GetArea(Position, AreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                CellList[x][y].UnhighlightCell();
            }
        }
    }

    public void HighlightZone(Vector2Int Position, Vector2Int AreaSize, Sprite sprite, Color color)
    {
        Vector4 CurrentArea = AreaManager.GetArea(Position, AreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                CellList[x][y].HighlightCell(sprite, color);
            }
        }
    }



    public bool IsZoneEmpty(Vector2Int Position, Vector2Int AreaSize)
    {
        Vector4 CurrentArea = AreaManager.GetArea(Position, AreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                if (!IsCellEmpty(x, y)) return false;
            }
        }
        return true;
    }

    public bool IsCellEmpty(Vector2Int id)
    {
        return CellList[id.x][id.y].Figure == CellFigure.none;
    }
    
    public bool IsCellEmpty(int x, int y)
    {
        return CellList[x][y].Figure == CellFigure.none;
    }

    public bool IsCellBlocked(Vector2Int id)
    {
        return CellList[id.x][id.y].SubState == CellSubState.block;
    }

    public bool IsCellBlocked(int x, int y)
    {
        return CellList[x][y].SubState == CellSubState.block;
    }

    public bool IsCellEnableToPlace(Vector2Int id)
    {
        return IsCellEmpty(id) && !IsCellBlocked(id);
    }

    public bool IsCellEnableToPlace(int x, int y)
    {
        return IsCellEmpty(x,y) && !IsCellBlocked(x,y);
    }

    public Cell GetNextCell(Vector2Int currentId, Vector2Int step)
    {
        Vector2Int nextId = currentId + step;
        if (nextId.x < 0 || nextId.y < 0 || nextId.x >= Field.Instance.FieldSize.x || nextId.y >= Field.Instance.FieldSize.y) return null;
        if (Field.Instance.CellList[nextId.x][nextId.y].Figure == CellFigure.none) return null;

        return Field.Instance.CellList[nextId.x][nextId.y];
    }

    public static void SetStartSize(Vector2Int newSize)
    {
        _startFieldSize = newSize;
    }

    public void GrowField()
    {
        if (_isFieldGrow_Dew)
        {
            _currenCycle += 1;
            if (_currenCycle == _cyclePerGrow)
            {
             
                if (_fieldSize.y < _growSizeMax.y)
                {
                    AddLineDown();
                    AddLineRight();
                }
                _currenCycle = 0;
            }
        }
    }

    public bool IsExistEmptyCell()
    {
        for (int i = 0; i < _cellList.Count; i++)
        {
            for (int j = 0; j < _cellList[i].Count; j++)
            {
                if (IsCellEnableToPlace(i,j)) return true;
            }
        }
        return false;
    }
}