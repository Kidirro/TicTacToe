using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class FieldCellLineManager : Singleton<FieldCellLineManager>
{
    public bool IsGamePlayimg
    {
        get { return _isGamePlaying; }
    }

    public int CurrentGoalLine
    {
        get { return _currentGoalLine; }
    }

    private int _currentGoalLine = 3;

    private bool _isGamePlaying = true;

    private bool _isPossibilityOfMove = true;

    public void PlaceInCell(Vector2Int id)
    {
        if (Field.Instance.IsCellEmpty(id) && _isGamePlaying && _isPossibilityOfMove && !Field.Instance.IsCellBlocked(id))
        {
            Field.Instance.CellList[id.x][id.y].SetFigure(PlayerManager.Instance.GetCurrentPlayer().SideId);
        }
    }

    public void MasterChecker(CellFigure figure)
    {
        List<Vector2Int> verticalList = new List<Vector2Int>();
        List<Vector2Int> horizontalList = new List<Vector2Int>();
        List<Vector2Int> diagonalRightList = new List<Vector2Int>();
        List<Vector2Int> diagonalLeftList = new List<Vector2Int>();

        List<List<Vector2Int>> linesFind = new List<List<Vector2Int>>();

        List<Vector2Int> uniqueCell = new List<Vector2Int>();

        for (int x = 0; x < Field.Instance.FieldSize.x; x++)
        {
            for (int y = 0; y < Field.Instance.FieldSize.y; y++)
            {
                Vector2Int curId = new Vector2Int(x, y);
                if (Field.Instance.CellList[curId.x][curId.y].Figure == figure)
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
                            Debug.LogFormat("Finded Line. Start at:{0}, End at:{1}, with count{2} ", newLine[0], newLine[newLine.Count - 1], newLine.Count);
                            linesFind.Add(newLine);
                        }
                    }
                }

                curId = new Vector2Int(x, Field.Instance.FieldSize.y - y - 1);
                if (Field.Instance.CellList[curId.x][curId.y].Figure == figure)
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
                            Debug.LogFormat("Finded Line. Start at:{0}, End at:{1}, with count{2} ", newLine[0], newLine[newLine.Count - 1], newLine.Count);
                            linesFind.Add(newLine);
                        }
                    }
                }
            }
        }

        foreach (List<Vector2Int> line in linesFind)
        {
            if (line.Count < CurrentGoalLine) continue;
            int i = 0;
            foreach (Vector2Int cell in line)
            {
                if (uniqueCell.IndexOf(cell) == -1)
                {
                    uniqueCell.Add(cell);
                    i += 1;
                }
            }
            Field.Instance.DrawFinishLine(line, i);
        }
        Debug.LogFormat("All cell deleted {0}", uniqueCell.Count);
    }

    private Vector2Int GetNextCellId(Vector2Int currentId, Vector2Int step)
    {
        Vector2Int nextId = currentId + step;
        if (nextId.x < 0 || nextId.y < 0 || nextId.x >= Field.Instance.FieldSize.x || nextId.y >= Field.Instance.FieldSize.y) return new Vector2Int(-1, -1);
        if (Field.Instance.IsCellEmpty(nextId)) return new Vector2Int(-1, -1);
        if (Field.Instance.IsCellBlocked(nextId)) return new Vector2Int(-1, -1);
        if (Field.Instance.CellList[nextId.x][nextId.y].Figure != Field.Instance.CellList[currentId.x][currentId.y].Figure) return new Vector2Int(-1, -1);

        return nextId;
    }


    public void Restart()
    {
        _isGamePlaying = true;
    }

    public void NewTurn(bool IsEvent = false)
    {

        _isPossibilityOfMove = true;
        //_enableManaPoint = 3;
        //if (IsEvent == false) NetworkEvent.RaiseEventEndTurn();
    }

    public bool CheckCanTurn()
    {
        return _isGamePlaying;//&& CheckIsCurrentPlayer();
    }
}
