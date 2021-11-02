using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
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
    [SerializeField] private Vector2 _screenBorderX;
    [SerializeField] private Vector2 _screenBorderY;

    [SerializeField] private float _borderPercent = 0.1f;

    [SerializeField] private Vector2Int _fieldSize;
    private List<LineRenderer> _lineListHorizontal = new List<LineRenderer>();
    private List<LineRenderer> _lineListVertical = new List<LineRenderer>();
    private List<List<Cell>> _cellList = new List<List<Cell>>(); //Заглушка        
    [SerializeField] private GameObject _cellPrefab;

    private void Start()
    {
        InitializeField();
    }

    private void InitializeField()
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

        Debug.Log("RemainY:"+RemainY);
        Debug.Log("RemainX:" + RemainX);
        Debug.Log(cellSize);

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
                cell.Id = new Vector2(i, j);
                newCellObject.transform.SetParent(_cellParent.transform);
                cell.ChangeSize(cellSize * (1 - _borderPercent));
                newCellObject.transform.localScale = new Vector2(cellSize * (1 - _borderPercent), cellSize * (1 - _borderPercent));

                _cellList[i].Add(cell);
            }
        }

        InitializeLine();
    }

    private void InitializeLine()
    {
        int i = 0;
        int j = 0;

        int verticalSize = Mathf.Max(_lineListVertical.Count, _cellList.Count-1);
        int horizontalSize = Mathf.Max(_lineListHorizontal.Count, _cellList[0].Count-1);

        while (i < verticalSize)
        {
            if (i >= _lineListVertical.Count)
            {
                GameObject newLine = new GameObject();
                LineRenderer LR = newLine.AddComponent<LineRenderer>();
                newLine.transform.position = Vector2.zero;
                newLine.transform.parent = _lineParent.transform;
                _lineListVertical.Add(LR);
            }

            if (i < _cellList.Count - 1)
            {
                Vector3[] points = new Vector3[2];
                points[0] = new Vector2(_cellList[i][0].Position.x * 0.5f +  _cellList[i+1][0].Position.x * 0.5f, _cellList[i][0].Position.y - _cellList[0][0].CellSize/2);
                points[1] = new Vector2(_cellList[i][_cellList[0].Count-1].Position.x * 0.5f + _cellList[i + 1][_cellList[0].Count - 1].Position.x * 0.5f, _cellList[i][_cellList[0].Count - 1].Position.y + _cellList[0][0].CellSize / 2);
                _lineListVertical[i].positionCount = 2;
                _lineListVertical[i].SetPositions(points);
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
            }

            if (j < _cellList[0].Count - 1)
            {
                Vector3[] points = new Vector3[2];
                points[0] = new Vector2(_cellList[0][j].Position.x- _cellList[0][0].CellSize / 2, _cellList[0][j].Position.y * 0.5f + _cellList[0][j+1].Position.y * 0.5f);
                points[1] = new Vector2(_cellList[_cellList.Count - 1][j].Position.x + _cellList[0][0].CellSize / 2,_cellList[1][j].Position.y * 0.5f + _cellList[1][j+1].Position.y * 0.5f);
                _lineListHorizontal[j].positionCount = 2;
                _lineListHorizontal[j].SetPositions(points);
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
}


/*
_cellData = new List<List<Cell>>();
_cellGameObjects = new List<List<GameObject>>();
for (int i = 0; i < _matrixSize; i++)
{
    _cellData.Add(new List<Cell>());
    _cellGameObjects.Add(new List<GameObject>());
    for (int j = 0; j < _matrixSize; j++)
    {
        GameObject newCellObject = Instantiate(_cellPrefab);
        newCellObject.transform.position = new Vector2(StartPositionMatrix.x + i * _cellSize, StartPositionMatrix.y + j * _cellSize);
        newCellObject.name = "[" + i + "][" + j + "]cell";
        newCellObject.transform.SetParent(this.transform);
        newCellObject.transform.localScale = new Vector2(_cellSize, _cellSize);

        Cell newCellData = new Cell(cellType.plane, newCellObject.transform);
        _cellData[i].Add(newCellData);
        _cellGameObjects[i].Add(newCellObject);
    }
*/