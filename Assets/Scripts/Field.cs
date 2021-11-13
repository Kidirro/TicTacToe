using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<LineRenderer> _lineListHorizontal = new List<LineRenderer>();
    private List<LineRenderer> _lineListVertical = new List<LineRenderer>();
    private LineRenderer _lineFinish;

    private List<List<Cell>> _cellList = new List<List<Cell>>();

    private List<Vector4> _idClearList = new List<Vector4>();

    public List<List<Cell>> CellList
    {
        get { return _cellList; }
    }

    [SerializeField] private GameObject _cellPrefab;

    [Space]
    [Space]
    [Space]

    [SerializeField] private Material _lineMaterial;
    [SerializeField] private float _lineWidthPercent;

    private List<Vector4> _finishLineId = new List<Vector4>();

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        InitializeField();
        InitializeLine();
    }

    public void InitializeField()
    {
        if (_fieldSize.x ==0 || _fieldSize.y==0)_fieldSize = new Vector2Int(3, 3);
        float StartPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
        float EndPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;

        float StartPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y)).y;
        float EndPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y)).y;

        float cellSizeY = ((EndPositionY - StartPositionY) / _fieldSize.y);
        float cellSizeX = ((EndPositionX - StartPositionX) / _fieldSize.x);

        float cellSize = (cellSizeY < cellSizeX) ? cellSizeY : cellSizeX;

        float RemainX = (EndPositionX - StartPositionX- _fieldSize.x * cellSize) / 2;
        float RemainY = (EndPositionY - StartPositionY - _fieldSize.y * cellSize) / 2;

        Vector2 StartPositionMatrix = new Vector2(StartPositionX+RemainX+cellSize/2, StartPositionY + RemainY+ cellSize/2);
        _cellList = new List<List<Cell>>();

        for (int i = 0; i < _fieldSize.x; i++)
        {
            _cellList.Add(new List<Cell>());
            for (int j = 0; j < _fieldSize.y; j++)
            {
                GameObject newCellObject = Instantiate(_cellPrefab);
                Cell cell = newCellObject.GetComponent<Cell>();
                newCellObject.transform.position = new Vector2(StartPositionMatrix.x + i * cellSize, StartPositionMatrix.y + j * cellSize);
                newCellObject.name = "[" + i + "][" + j + "]cell";
                cell.Position = newCellObject.transform.position;
                cell.Id = new Vector2Int(i, j);
                newCellObject.transform.SetParent(_cellParent.transform);
                newCellObject.transform.localScale = new Vector2(cellSize * (1 - _borderPercent), cellSize * (1 - _borderPercent));

                _cellList[i].Add(cell);
            }
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
                _cellList[i][j].ChangeState(0);
            }
        }
    }

    private void InitializeLine()
    {
        int i = 0;
        int j = 0;

        int verticalSize = Mathf.Max(_lineListVertical.Count, _cellList.Count-1);
        int horizontalSize = Mathf.Max(_lineListHorizontal.Count, _cellList[0].Count-1);

        if (!_lineFinish)
        {
            GameObject newLine = new GameObject();
            newLine.name = "Finish Line";
            _lineFinish = newLine.AddComponent<LineRenderer>();
            newLine.transform.parent = _lineParent.transform;

            _lineFinish.material = _lineMaterial;
            _lineFinish.numCapVertices = 6;
            _lineFinish.SetColors(Color.red, Color.red);
            _lineFinish.positionCount = 0;
            _lineFinish.sortingOrder = 1;
        }
        else
        {
            _lineFinish.positionCount = 0;
        }
        
        while (i < verticalSize)
        {
            if (i >= _lineListVertical.Count)
            {
                GameObject newLine = new GameObject();
                LineRenderer LR = newLine.AddComponent<LineRenderer>();
                newLine.transform.position = Vector2.zero;
                newLine.transform.parent = _lineParent.transform;
                _lineListVertical.Add(LR);
                newLine.name = "Vertical Line " + i;

                _lineListVertical[i].material = _lineMaterial;
                _lineListVertical[i].numCapVertices = 6;
                _lineListVertical[i].SetColors(Color.black, Color.black);
            }

            if (i < _cellList.Count - 1)
            {
                Vector3[] points = new Vector3[2];
                points[0] = new Vector2(_cellList[i][0].Position.x * 0.5f +  _cellList[i+1][0].Position.x * 0.5f, _cellList[i][0].Position.y - _cellList[0][0].CellSize/2);
                points[1] = new Vector2(_cellList[i][_cellList[0].Count-1].Position.x * 0.5f + _cellList[i + 1][_cellList[0].Count - 1].Position.x * 0.5f, _cellList[i][_cellList[0].Count - 1].Position.y + _cellList[0][0].CellSize / 2);

                _lineListVertical[i].SetWidth(_cellList[0][0].CellSize * _borderPercent * _lineWidthPercent, _cellList[0][0].CellSize * _borderPercent * _lineWidthPercent);
                _lineListVertical[i].positionCount = 2;
                _lineListVertical[i].SetPositions(points);

            }
            else
            {

                _lineListVertical[i].positionCount = 0;
            }
            i++;
        }

        while (j < horizontalSize)
        {
            if (j >= _lineListHorizontal.Count)
            {
                GameObject newLine = new GameObject();
                LineRenderer LR = newLine.AddComponent<LineRenderer>();
                newLine.transform.position = Vector2.zero;
                newLine.transform.parent = _lineParent.transform;
                _lineListHorizontal.Add(LR);
                newLine.name = "Horizontal Line " + j;

                _lineListHorizontal[j].material = _lineMaterial;
                _lineListHorizontal[j].numCapVertices = 6;
                _lineListHorizontal[j].SetColors(Color.black, Color.black);

            }

            if (j < _cellList[0].Count - 1)
            {
                Vector3[] points = new Vector3[2];
                points[0] = new Vector2(_cellList[0][j].Position.x- _cellList[0][0].CellSize / 2, _cellList[0][j].Position.y * 0.5f + _cellList[0][j+1].Position.y * 0.5f);
                points[1] = new Vector2(_cellList[_cellList.Count - 1][j].Position.x + _cellList[0][0].CellSize / 2,_cellList[1][j].Position.y * 0.5f + _cellList[1][j+1].Position.y * 0.5f);

                _lineListHorizontal[j].SetWidth(_cellList[0][0].CellSize * _borderPercent * _lineWidthPercent, _cellList[0][0].CellSize * _borderPercent * _lineWidthPercent);
                _lineListHorizontal[j].positionCount = 2;
                _lineListHorizontal[j].SetPositions(points);
            }
            else
            {
                _lineListHorizontal[i].positionCount = 0;
            }
            j++;
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
        Color cl = _lineFinish.startColor;
        cl.a = s;

        _lineFinish.startColor = cl;
        _lineFinish.endColor = cl;
    }

    private void ClearCellOnLine(Vector2Int id1,Vector2Int id2)
    {
        _cellList[id1.x][id1.y].ChangeState(0);
        _cellList[(id1.x+id2.x)/2][(id1.y+id2.y)/2].ChangeState(0);
        _cellList[id2.x][id2.y].ChangeState(0);
    }


    public void AddNewId(Vector4 id)
    {
        _finishLineId.Add(id);
        Debug.Log(_finishLineId.Count);
        if (_finishLineId.Count == 1) StartCoroutine(FinishLineCleaning());
    }


    IEnumerator FinishLineCleaning()
    {
        while(_finishLineId.Count>0)
        {
            Debug.Log(_finishLineId[0]);
            Vector2Int id1 = new Vector2Int((int)_finishLineId[0].x, (int)_finishLineId[0].y);
            Vector2Int id2 = new Vector2Int((int)_finishLineId[0].z, (int)_finishLineId[0].w);
            Vector3[] points = new Vector3[2];
            points[0] = new Vector2(_cellList[id1.x][id1.y].Position.x, _cellList[id1.x][id1.y].Position.y);
            points[1] = new Vector2(_cellList[id2.x][id2.y].Position.x, _cellList[id2.x][id2.y].Position.y);
            SetAlphaFinishLine(0);
            _lineFinish.SetWidth(_cellList[0][0].CellSize * _borderPercent * _lineWidthPercent, _cellList[0][0].CellSize * _borderPercent * _lineWidthPercent);
            _lineFinish.positionCount = 2;
            _lineFinish.SetPositions(points);
            float j = 0;
            while (j < 10)
            {
                j++;
                SetAlphaFinishLine(j / 10);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
            }

            ClearCellOnLine(new Vector2Int((int)id1.x, (int)id1.y), new Vector2Int((int)id2.x, (int)id2.y));

            while (j >0)
            {
                j--;
                SetAlphaFinishLine( j/ 10);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
            }
            _finishLineId.Remove(_finishLineId[0]);
        }

        TurnController.UnlockTurn();
    }
}