using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field : Singleton<Field>
{

    [SerializeField] private bool _isNeedGizmos;


    [Space]
    [Space]
    [Space]
    [Space]

    [SerializeField] private GameObject _lineParent;
    [SerializeField] private GameObject _cellParent;

    [Space]

    [SerializeField] private Vector2 _defaultScreenResolution;
    //In screen cord
    [SerializeField] private Vector2 _screenBorderX;
    [SerializeField] private Vector2 _screenBorderY;

    [SerializeField] private Vector2Int _startFieldSize;
    private Vector2Int _fieldSize;

    public Vector2Int FieldSize
    {
        get { return _fieldSize; }
    }

    private List<Line> _lineListHorizontal = new List<Line>();
    private List<Line> _lineListVertical = new List<Line>();
    private Line _lineFinish;
    private float _cellSize;

    private List<List<Cell>> _cellList = new List<List<Cell>>();

    public List<List<Cell>> CellList
    {
        get { return _cellList; }
    }

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private Material _finishLineMaterial;

    [Space]
    [Space]
    [Space]

    [SerializeField] private float _lineWidthPercent;
    [SerializeField] private float _borderPercent;


    //In screen cord
    private float _startPositionX;
    private float _endPositionX;
    //In screen cord
    private float _startPositionY;
    private float _endPositionY;

    private float _remainX;
    private float _remainY;

    private List<Vector4> _finishLineId = new List<Vector4>();

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
        /* _startPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
         _endPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;

         _startPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y)).y;
         _endPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y)).y;*/
        _startPositionX = Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x);
        _endPositionX = Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x);

        _startPositionY = Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y;
        _endPositionY = Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y;

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

                    _cellList[i].Add(cell);
                }
            }
        
    }

    private void InitializeLine()
    {
        if (_lineListVertical.Count != 0)
        {
            foreach (Line line in _lineListVertical) {
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
        int horizontalSize = _fieldSize.y- 1;

        if (!_lineFinish)
        {
            GameObject newLine = Instantiate(_linePrefab);
            newLine.name = "Finish Line";
            _lineFinish = newLine.GetComponent<Line>();
            _lineFinish.SetTransformParent(_lineParent.transform);
            _lineFinish.LineRend.startColor = Color.red;
            _lineFinish.LineRend.endColor = Color.red;
            _lineFinish.LineRend.sortingOrder = 1;
            _lineFinish.LineRend.material = _finishLineMaterial;
        }
        _lineFinish.LineRend.positionCount = 0;

        for (int i = 0; i <verticalSize; i++)
        {
                GameObject newLine = Instantiate(_linePrefab);
                Line LR = newLine.GetComponent<Line>();
                LR.SetTransformParent(_lineParent.transform);
                _lineListVertical.Add(LR);
                newLine.name = "Vertical Line " + i;
            

        }

        for  (int j =0; j < horizontalSize; j++)
        {
                GameObject newLine = Instantiate(_linePrefab);
                Line LR = newLine.GetComponent<Line>();
                LR.SetTransformParent(_lineParent.transform);
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

            _lineListVertical[i].SetWidthScreenCord(_cellList[0][0].CellSize * _lineWidthPercent, instantly);
            _lineListVertical[i].SetPositions(points[0], points[1], instantly);
        }

        for (int i = 0; i < _lineListHorizontal.Count; i++)
        {
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[0][i].Position.x - _cellSize / 2, _cellList[0][i].Position.y * 0.5f + _cellList[0][i + 1].Position.y * 0.5f);
            points[1] = new Vector2(_cellList[_cellList.Count - 1][i].Position.x + _cellSize / 2, _cellList[1][i].Position.y * 0.5f + _cellList[1][i + 1].Position.y * 0.5f);

            _lineListHorizontal[i].SetWidthScreenCord(_cellSize * _lineWidthPercent, instantly);
            _lineListHorizontal[i].SetPositions(points[0], points[1], instantly);
        }
    }

    public void RestartGame()
    {
        ResetField();
        InitializeLine();
    }

    private void ResetField()
    {
        for (int i = 0; i < _cellList.Count; i++)
        {
            for (int j = 0; j < _cellList[i].Count; j++)
            {
                _cellList[i][j].SetState(CellState.empty);
            }
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (Line line in _lineListVertical) Destroy(line.gameObject);
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

/*        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log(GetIdFromPosition(Input.mousePosition, false));
            Debug.Log(AreaManager.GetArea(GetIdFromPosition(Input.mousePosition, false), new Vector2Int(1, 1)));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(GetIdFromPosition(Input.mousePosition, false));
        }*/
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

        TurnController.MasterChecker(new Vector2Int(fl, 0));
        TurnController.MasterChecker(new Vector2Int(sl, 0), false);
        for (int i = 1; i < _fieldSize.y; i++)
        {
            TurnController.MasterChecker(new Vector2Int(fl, i), false);
            TurnController.MasterChecker(new Vector2Int(sl, i), false);
        }

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

        TurnController.MasterChecker(new Vector2Int(0, fl));
        TurnController.MasterChecker(new Vector2Int(0, sl), false);
        for (int i = 1; i < _fieldSize.x; i++)
        {
            TurnController.MasterChecker(new Vector2Int(i, fl), false);
            TurnController.MasterChecker(new Vector2Int(i, sl), false);
        }

    }

    private void OnDrawGizmos()
    {
        if (_isNeedGizmos)
        {
            float StartPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
            float EndPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;

            float StartPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y)).y;
            float EndPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y)).y;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(StartPositionX, StartPositionY), new Vector2(EndPositionX, StartPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, StartPositionY), new Vector2(EndPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, EndPositionY), new Vector2(StartPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(StartPositionX, EndPositionY), new Vector2(StartPositionX, StartPositionY));
        }
    }

    private void SetAlphaFinishLine(float s)
    {
        Color cl = _lineFinish.LineRend.startColor;
        cl.a = s;

        _lineFinish.LineRend.startColor = cl;
        _lineFinish.LineRend.endColor = cl;
    }

    private int ClearCellOnLine(Vector2Int id1, Vector2Int id2)
    {
        int res = 0;
        if (_cellList[id1.x][id1.y].State != CellState.empty)
        {
            _cellList[id1.x][id1.y].SetState(CellState.empty);
            res++;
        }
        Vector2Int nextValue = new Vector2Int(id1.x + (int)Math.Sign(id2.x - id1.x), id1.y + (int)Math.Sign(id2.y - id1.y));
        while (nextValue != id2)
        {
            if (_cellList[nextValue.x][nextValue.y].State != CellState.empty) _cellList[nextValue.x][nextValue.y].SetState(CellState.empty);
            nextValue = new Vector2Int(nextValue.x + (int)Math.Sign(id2.x - id1.x), nextValue.y + (int)Math.Sign(id2.y - id1.y));
            res++;
        }
        if (_cellList[id2.x][id2.y].State != CellState.empty)
        {
            _cellList[id2.x][id2.y].SetState(CellState.empty);
            res++;
        }
        return res;
    }


    public void AddNewFinishId(Vector4 id)
    {
        _finishLineId.Add(id);
        if (_finishLineId.Count == 1) StartCoroutine(FinishLineCleaning());
    }


    IEnumerator FinishLineCleaning()
    {
        while (CellAnimating != 0)
        {
            yield return null;
        }
        _lineFinish.LineRend.positionCount = 2;
        int current_player = PlayerManager.Instance.GetCurrentPlayer().SideId;
        while (_finishLineId.Count > 0)
        {
            Vector2Int id1 = new Vector2Int((int)_finishLineId[0].x, (int)_finishLineId[0].y);
            Vector2Int id2 = new Vector2Int((int)_finishLineId[0].z, (int)_finishLineId[0].w);
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[id1.x][id1.y].Position.x, _cellList[id1.x][id1.y].Position.y);
            points[1] = new Vector2(_cellList[id2.x][id2.y].Position.x, _cellList[id2.x][id2.y].Position.y);
            SetAlphaFinishLine(0);
            _lineFinish.SetWidthScreenCord(_cellList[0][0].CellSize * _lineWidthPercent);
            _lineFinish.SetPositions(points[0], points[1]);
            float j = 0;
            while (j < 10)
            {
                j++;
                SetAlphaFinishLine(j / 10);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
            }

            UIController.AddScore(current_player, ClearCellOnLine(new Vector2Int((int)id1.x, (int)id1.y), new Vector2Int((int)id2.x, (int)id2.y)));
            while (j > 0)
            {
                j--;
                SetAlphaFinishLine(j / 10);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
            }
            _finishLineId.Remove(_finishLineId[0]);

        }

        _lineFinish.LineRend.positionCount = 0;
    }

    public bool CheckIsInField(Vector2 pos)
    {
        return pos.x >= (_startPositionX + _remainX) & pos.x <= (_endPositionX - _remainX) &
               pos.y >= (_startPositionY + _remainY) & pos.y <= (_endPositionY - _remainY);
    }

    /// <summary>
    ///��� ������ �� ������� ��� ���������� ����� - ��������� �����
    /// </summary>
    /// <param name="pos"> Position</param>
    /// <param name="IsFindBorder">���� ������ �� �������: ������� ����� (true) ��� (-1,-1) (false) </param>
    /// <returns></returns>
    public Vector2Int GetIdFromPosition(Vector2 pos, bool IsFindBorder)
    {
        Vector2Int pos_final = Vector2Int.zero;
        pos_final.y = (int)Mathf.Clamp((float)Math.Floor((pos.y - (_startPositionY + _remainY)) / _cellSize), 0, _fieldSize.y - 1);
        pos_final.x = (int)Mathf.Clamp((float)Math.Floor((pos.x - (_startPositionX + _remainX)) / _cellSize), 0, _fieldSize.x - 1);
        if (CheckIsInField(pos) || IsFindBorder) return pos_final;
        else return (new Vector2Int(-1, -1));
    }
}