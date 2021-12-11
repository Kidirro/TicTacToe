using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field : MonoBehaviour
{
    static public Field Instance => _instance;
    private static Field _instance;

    [SerializeField] private bool _isNeedGizmos;


    [Space]
    [Space]
    [Space]
    [Space]

    [SerializeField] private GameObject _lineParent;
    [SerializeField] private GameObject _cellParent;

    [Space]

    [SerializeField] private Vector2 _defaultScreenResolution;
    [SerializeField] private Vector2 _screenBorderX;
    [SerializeField] private Vector2 _screenBorderY;

    [SerializeField] private float _borderPercent = 0.1f;

    [SerializeField] private Vector2Int _fieldSize;
    private List<Line> _lineListHorizontal = new List<Line>();
    private List<Line> _lineListVertical = new List<Line>();
    private Line _lineFinish;

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

    private float _startPositionX;
    private float _endPositionX;

    private float _startPositionY;
    private float _endPositionY;

    private List<Vector4> _finishLineId = new List<Vector4>();

    public int CellAnimating=0;

    private void Awake()
    {
        _instance = this;

    }

    private void Start()
    {
        InitializeField();
        InitializeLine();
        NewCellSize(_fieldSize,false);
    }

    private void GetStartPosition()
    {
        _startPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
        _endPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;

        _startPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y)).y;
        _endPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y)).y;

    }

    private void AddLineLeft()
    {
        _fieldSize.x += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        newLine.transform.parent = _lineParent.transform;
        _lineListVertical.Insert(0, LR);
        _lineListVertical[0].SetPositions(_lineListVertical[1].StartPoint - new Vector2(_cellList[0][0].CellSize, 0), _lineListVertical[1].EndPoint - new Vector2(_cellList[0][0].CellSize, 0));
        _lineListVertical[0].SetWidth(0);
        for (int i = 0; i < _lineListVertical.Count; i++)
        {
            _lineListVertical[i].name = "Vertical Line " + i;
        }
        _cellList.Insert(0, new List<Cell>());
        for (int j = 0; j < _fieldSize.y; j++)
        {
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            newCellObject.transform.SetParent(_cellParent.transform);
            cell.SetColliderSize(1 + _borderPercent * 0.5f);
            _cellList[0].Add(cell);
            _cellList[0][j].SetTransformPosition((_cellList[1][j].Position - new Vector2(_cellList[0][0].CellSize, 0)).x, _cellList[1][j].Position.y);
        }

        for(int i = 0; i < _fieldSize.x; i++)
        {
            for (int j = 0; j < _fieldSize.y; j++)
            {

                _cellList[i][j].Id = new Vector2Int(i, j);
                _cellList[i][j].name = "[" + i + "][" + j + "]cell";
            }
        }
        NewCellSize(_fieldSize,false);

    }

    private void AddLineRight()
    {
        _fieldSize.x += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        newLine.transform.parent = _lineParent.transform;
        _lineListVertical.Add(LR);
        Vector2 newPosition1 = _lineListVertical[_lineListVertical.Count - 2].StartPoint + new Vector2(_cellList[0][0].CellSize, 0);
        Vector2 newPosition2 = _lineListVertical[_lineListVertical.Count - 2].EndPoint + new Vector2(_cellList[0][0].CellSize, 0);
        _lineListVertical[_lineListVertical.Count-1].SetPositions(newPosition1, newPosition2);
        _lineListVertical[_lineListVertical.Count - 1].SetWidth(0);
        for (int i = 0; i < _lineListVertical.Count; i++)
        {
            _lineListVertical[i].name = "Vertical Line " + i;
        }
        _cellList.Add(new List<Cell>());
        for (int j = 0; j < _fieldSize.y; j++)
        {
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            newCellObject.transform.SetParent(_cellParent.transform);
            cell.SetColliderSize(1 + _borderPercent * 0.5f);
            _cellList[_fieldSize.x-1].Add(cell);
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

    private void AddLineUp()
    {
        _fieldSize.y += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        newLine.transform.parent = _lineParent.transform;
        _lineListHorizontal.Add(LR);
        Vector2 newPosition1 = _lineListHorizontal[_lineListHorizontal.Count - 2].StartPoint + new Vector2(0, _cellList[0][0].CellSize);
        Vector2 newPosition2 = _lineListHorizontal[_lineListHorizontal.Count - 2].EndPoint + new Vector2(0, _cellList[0][0].CellSize);
        _lineListHorizontal[_lineListHorizontal.Count - 1].SetPositions(newPosition1, newPosition2);
        _lineListHorizontal[_lineListHorizontal.Count - 1].SetWidth(0);
        for (int i = 0; i < _lineListHorizontal.Count; i++)
        {
            _lineListHorizontal[i].name = "Horizontal Line " + i;
        }
        for (int j = 0; j < _fieldSize.x; j++)
        {
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            newCellObject.transform.SetParent(_cellParent.transform);
            cell.SetColliderSize(1 + _borderPercent * 0.5f);
            _cellList[j].Add(cell);
            _cellList[j][_fieldSize.y-1].SetTransformPosition(_cellList[j][_fieldSize.y - 2].Position .x, _cellList[j][_fieldSize.y - 2].Position.y + new Vector2(0, _cellList[0][0].CellSize).y);
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

    private void AddLineDown()
    {
        _fieldSize.y += 1;
        GameObject newLine = Instantiate(_linePrefab);
        Line LR = newLine.GetComponent<Line>();
        newLine.transform.position = Vector2.zero;
        newLine.transform.parent = _lineParent.transform;
        _lineListHorizontal.Insert(0,LR);
        Vector2 newPosition1 = _lineListHorizontal[1].StartPoint - new Vector2(0, _cellList[0][0].CellSize);
        Vector2 newPosition2 = _lineListHorizontal[1].EndPoint - new Vector2(0, _cellList[0][0].CellSize);
        _lineListHorizontal[0].SetPositions(newPosition1, newPosition2);
        _lineListHorizontal[0].SetWidth(0);
        for (int i = 0; i < _lineListHorizontal.Count; i++)
        {
            _lineListHorizontal[i].name = "Horizontal Line " + i;
        }
        for (int j = 0; j < _fieldSize.x; j++)
        {
            GameObject newCellObject = Instantiate(_cellPrefab);
            Cell cell = newCellObject.GetComponent<Cell>();
            newCellObject.transform.SetParent(_cellParent.transform);
            cell.SetColliderSize(1 + _borderPercent * 0.5f);
            _cellList[j].Insert(0,cell);
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

    private float GetCellSize(int x_size=3,int y_size=3)
    {
        float cellSizeY = ((_endPositionY - _startPositionY) / y_size);
        float cellSizeX = ((_endPositionX - _startPositionX) / x_size);

        return (cellSizeY < cellSizeX) ? cellSizeY : cellSizeX;
    }

    public void InitializeField()
    {
        GetStartPosition();
        _cellList = new List<List<Cell>>();

        for (int i = 0; i < _fieldSize.x; i++)
        {
            _cellList.Add(new List<Cell>());
            for (int j = 0; j < _fieldSize.y; j++)
            {
                GameObject newCellObject = Instantiate(_cellPrefab);
                Cell cell = newCellObject.GetComponent<Cell>();
                newCellObject.name = "[" + i + "][" + j + "]cell";
                cell.Id = new Vector2Int(i, j);
                newCellObject.transform.SetParent(_cellParent.transform);
                cell.SetColliderSize(1+_borderPercent*0.5f);

                _cellList[i].Add(cell);
            }
        }
    }

    private void InitializeLine()
    {
        int i = 0;
        int j = 0;

        int verticalSize = Mathf.Max(_lineListVertical.Count, _cellList.Count - 1);
        int horizontalSize = Mathf.Max(_lineListHorizontal.Count, _cellList[0].Count - 1);

        if (!_lineFinish)
        {
            GameObject newLine = Instantiate(_linePrefab);
            newLine.name = "Finish Line";
            newLine.transform.parent = _lineParent.transform;
            _lineFinish = newLine.GetComponent<Line>();
            _lineFinish.LineRend.SetColors(Color.red, Color.red);
            _lineFinish.LineRend.sortingOrder = 1;
            _lineFinish.LineRend.material = _finishLineMaterial;
        }
        _lineFinish.LineRend.positionCount = 0;

        while (i < verticalSize)
        {
            if (i >= _lineListVertical.Count)
            {
                GameObject newLine = Instantiate(_linePrefab);
                Line LR = newLine.GetComponent<Line>();
                newLine.transform.position = Vector2.zero;
                newLine.transform.parent = _lineParent.transform;
                _lineListVertical.Add(LR);
                newLine.name = "Vertical Line " + i;
            }

            if (i >= _cellList.Count - 1)
            {

                _lineListVertical[i].LineRend.positionCount = 0;
            }
            i++;
        }

        while (j < horizontalSize)
        {
            if (j >= _lineListHorizontal.Count)
            {
                GameObject newLine = Instantiate(_linePrefab);
                Line LR = newLine.GetComponent<Line>();
                newLine.transform.position = Vector2.zero;
                newLine.transform.parent = _lineParent.transform;
                _lineListHorizontal.Add(LR);
                newLine.name = "Horizontal Line " + j;
            }

            if (j >= _cellList[0].Count - 1)
            {
                _lineListHorizontal[i].LineRend.positionCount = 0;
            }
            j++;
        }
    }


    public void NewCellSize(Vector2Int VirtualfieldSize,bool instantly = true)
    {
        float cellSize = GetCellSize(VirtualfieldSize.x, VirtualfieldSize.y);
        float RemainX = (_endPositionX - _startPositionX - _fieldSize.x * cellSize) / 2;
        float RemainY = (_endPositionY - _startPositionY - _fieldSize.y * cellSize) / 2;
        Vector2 StartPositionMatrix = new Vector2(_startPositionX + RemainX + cellSize / 2, _startPositionY + RemainY + cellSize / 2);
        for (int i = 0; i < _fieldSize.x; i++)
        {
            for (int j = 0; j < _fieldSize.y; j++)
            {
                _cellList[i][j].SetTransformPosition(StartPositionMatrix.x + i * cellSize, StartPositionMatrix.y + j * cellSize, instantly);
                _cellList[i][j].SetTransformSize(cellSize * (1 - _borderPercent), cellSize, instantly);
            }
        }

        for (int i =0; i < _lineListVertical.Count; i++)
        {
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[i][0].Position.x * 0.5f + _cellList[i + 1][0].Position.x * 0.5f, _cellList[i][0].Position.y - _cellList[0][0].CellSize / 2);
            points[1] = new Vector2(_cellList[i][_cellList[0].Count - 1].Position.x * 0.5f + _cellList[i + 1][_cellList[0].Count - 1].Position.x * 0.5f, _cellList[i][_cellList[0].Count - 1].Position.y + _cellList[0][0].CellSize / 2);

            _lineListVertical[i].SetWidth(_cellList[0][0].CellSize * _borderPercent * _lineWidthPercent, instantly);
            _lineListVertical[i].SetPositions(points[0], points[1], instantly);
        }

        for (int i = 0; i < _lineListHorizontal.Count; i++)
        {
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[0][i].Position.x - _cellList[0][0].CellSize / 2, _cellList[0][i].Position.y * 0.5f + _cellList[0][i + 1].Position.y * 0.5f);
            points[1] = new Vector2(_cellList[_cellList.Count - 1][i].Position.x + _cellList[0][0].CellSize / 2, _cellList[1][i].Position.y * 0.5f + _cellList[1][i + 1].Position.y * 0.5f);

            _lineListHorizontal[i].SetWidth(_cellList[0][0].CellSize * _borderPercent * _lineWidthPercent, instantly);
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
        for(int i = 0; i < _cellList.Count; i++)
        {
            for(int j = 0; j < _cellList[i].Count; j++)
            {
                _cellList[i][j].SetState(0);
            }
        }
    }

    public  void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NewCellSize(_fieldSize,false);
        } 
        if (Input.GetKeyDown(KeyCode.K))
        {
            NewCellSize(_fieldSize+Vector2Int.one,false);
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
            _cellList[0][0]=_cellList[0][1];
            _cellList[0][1] = kk;

            kk = _cellList[0][1];
            _cellList[0][1] = _cellList[1][1];
            _cellList[1][1] = kk;

            kk = _cellList[1][1];
            _cellList[1][1] = _cellList[1][0];
            _cellList[1][0] = kk;
            NewCellSize(_fieldSize, false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwapVerticalLines(0, 1);
        }
    }

    private void SwapVerticalLines(int fl, int sl)
    {
        for (int i = 0; i < _fieldSize.y; i++)
        {
            var kk = _cellList[fl][i];
            _cellList[fl][i] = _cellList[sl][i];
            _cellList[sl][i] = kk;
            _cellList[sl][i].Id = new Vector2Int(sl, i);
            _cellList[fl][i].Id = new Vector2Int(fl, i);
        }

        NewCellSize(_fieldSize, false);

        TurnController.MasterChecker(new Vector2Int(fl, 0));
        TurnController.MasterChecker(new Vector2Int(sl, 0),false);
        for (int i = 1; i < _fieldSize.y; i++)
        {
            TurnController.MasterChecker(new Vector2Int(fl, i),false);
            TurnController.MasterChecker(new Vector2Int(sl, i),false);
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
        if (_cellList[id1.x][id1.y].State != 0)
        {
            _cellList[id1.x][id1.y].SetState(0);
            res++;
        }
        Vector2Int nextValue = new Vector2Int(id1.x + (int)Math.Sign(id2.x - id1.x), id1.y + (int)Math.Sign(id2.y - id1.y));
        while (nextValue != id2)
        {
            if (_cellList[nextValue.x][nextValue.y].State != 0) _cellList[nextValue.x][nextValue.y].SetState(0);
            nextValue = new Vector2Int(nextValue.x + (int)Math.Sign(id2.x - id1.x), nextValue.y + (int)Math.Sign(id2.y - id1.y));
            res++;
        }
        if (_cellList[id2.x][id2.y].State != 0)
        {
            _cellList[id2.x][id2.y].SetState(0);
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
        while (CellAnimating != 0) {
            yield return null;
        }
        _lineFinish.LineRend.positionCount = 2;
        while (_finishLineId.Count > 0)
        {
            Vector2Int id1 = new Vector2Int((int)_finishLineId[0].x, (int)_finishLineId[0].y);
            Vector2Int id2 = new Vector2Int((int)_finishLineId[0].z, (int)_finishLineId[0].w);
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[id1.x][id1.y].Position.x, _cellList[id1.x][id1.y].Position.y);
            points[1] = new Vector2(_cellList[id2.x][id2.y].Position.x, _cellList[id2.x][id2.y].Position.y);
            SetAlphaFinishLine(0);
            _lineFinish.SetWidth(_cellList[0][0].CellSize * _borderPercent * _lineWidthPercent);
            _lineFinish.SetPositions(points[0], points[1]);
            float j = 0;
            while (j < 10)
            {
                j++;
                SetAlphaFinishLine(j / 10);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
            }

            UIController.AddScore(TurnController.CurrentPlayer, ClearCellOnLine(new Vector2Int((int)id1.x, (int)id1.y), new Vector2Int((int)id2.x, (int)id2.y)));
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
}