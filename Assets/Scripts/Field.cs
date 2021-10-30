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
        //_fieldSize = new Vector2Int(3, 3);
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
                cell.Id = new Vector2(i, j);
                newCellObject.transform.SetParent(this.transform);
                cell.ChangeSize(cellSize * (1 - _borderPercent));
                newCellObject.transform.localScale = new Vector2(cellSize * (1 - _borderPercent), cellSize * (1 - _borderPercent));

                _cellList[i].Add(cell);
            }
        }
    }

    private void InitializeLine()
    {
        int i = 0;
        int j = 0;

        int size

        while ()
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