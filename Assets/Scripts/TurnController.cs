using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnController : MonoBehaviour
{
    static private int _currentPlayer = 1;
    static public int CurrentPlayer
    {
        get { return _currentPlayer; }
    }

    static public bool IsGamePlayimg
    {
        get { return _isGamePlaying; }
    }

    static private bool _isGamePlaying=true;
    static private bool _isPossibilityOfMove = true;

    static private List<List<CellState>> _cellStateCopy = new List<List<CellState>>();

    static public void TurnProcess(Vector2Int id)
    {
        if (Field.Instance.CellList[id.x][id.y].State == 0 && _isGamePlaying && _isPossibilityOfMove)
        {
            _isPossibilityOfMove = false;
            Field.Instance.CellList[id.x][id.y].SetState(_currentPlayer);

            MasterChecker(id);
        }
    }

    static public void MasterChecker(Vector2Int turnId,bool IsNeedUpdate = true)
    {
        bool lineFind = false;
        if (IsNeedUpdate)
        {
            _cellStateCopy = new List<List<CellState>>();
            for (int i = 0; i < Field.Instance.CellList.Count; i++)
            {
                _cellStateCopy.Add(new List<CellState>());
                for (int j = 0; j < Field.Instance.CellList[i].Count; j++)
                {
                    _cellStateCopy[i].Add(Field.Instance.CellList[i][j].State);

                }
            }
        }

        for (int x = 0; x < _cellStateCopy[0].Count; x++)
        {
            for(int y = 0; y < _cellStateCopy.Count; y++)
            {
                bool flagX = false;
                bool flagY = false;
                int startX = 0;
                int startY = 0;

                switch (x)
                {
                    case 2:
                        if (turnId.x <_cellStateCopy.Count-2)
                        {
                            flagX = true;
                            startX = turnId.x;
                            
                        }
                        break;
                    case 1:
                        if (turnId.x < _cellStateCopy.Count - 1 && turnId.x>0)
                        {
                            flagX = true;
                            startX = turnId.x-1;

                        }
                        break;
                    case 0:
                        if (turnId.x > 1)
                        {
                            flagX = true;
                            startX = turnId.x-2;

                        }
                        break;
                }

                switch (y)
                {
                    case 2:
                        if (turnId.y < _cellStateCopy[0].Count - 2)
                        {
                            flagY = true;
                            startY = turnId.y;

                        }
                        break;
                    case 1:
                        if (turnId.y < _cellStateCopy[0].Count - 1 && turnId.y > 0)
                        {
                            flagY = true;
                            startY = turnId.y-1;

                        }
                        break;
                    case 0:
                        if (turnId.y > 1)
                        {
                            flagY = true;
                            startY = turnId.y-2;

                        }
                        break;
                }

                if (flagX && flagY)
                {
                    lineFind = CheckField(startX, startY, turnId) || lineFind;
                }

            }
        }
    }

    static private bool CheckField(int stepX, int stepY, Vector2Int turnId)
    {
        Vector4 DiagonalResult = Vector4.one;
        Vector4 LineResult = Vector4.one;
        bool lineFind = false;

        while (DiagonalResult != Vector4.zero || LineResult != Vector4.zero)
        {
            DiagonalResult = CheckDiagonal(stepX, stepY);
            LineResult = CheckVerticalHorizontal(stepX, stepY);
            if (DiagonalResult != Vector4.zero)
            {
                _isPossibilityOfMove = false;
                Field.Instance.AddNewFinishId(DiagonalResult);
                ClearLine(new Vector2Int((int)DiagonalResult.x, (int)DiagonalResult.y), new Vector2Int((int)DiagonalResult.z, (int)DiagonalResult.w), turnId);
                lineFind = true;

            }
            if (LineResult != Vector4.zero)
            {
                _isPossibilityOfMove = false;
                Field.Instance.AddNewFinishId(LineResult);

                ClearLine(new Vector2Int((int)LineResult.x, (int)LineResult.y), new Vector2Int((int)LineResult.z, (int)LineResult.w), turnId);
                lineFind = true;
            }
        }
        return lineFind;
    }

    static private Vector4 CheckDiagonal(int StartX, int StartY)
    {
        int DefValR = (int)_cellStateCopy[StartX][StartY];
        bool toright = DefValR == 1 || DefValR == 2;

        int DefValL = (int)_cellStateCopy[StartX + 2][StartY];
        bool toleft = DefValL == 1 || DefValL == 2;
        int i = 0;
        while (i < 3 & (toleft || toright))
        {
            toright = toright && (int)_cellStateCopy[StartX + i][StartY + i] == DefValR;

            toleft = toleft && (int)_cellStateCopy[StartX + i][StartY +2 - i] == DefValL;
            i++;
        }
        if (toright)
        {
            i--;
            while ((i + StartY <= _cellStateCopy[0].Count - 1) && (i + StartX <= _cellStateCopy.Count - 1) && toright)
            {
                toright = toright && (int)_cellStateCopy[StartX + i][StartY + i] == DefValR;
                if (!toright) i -= 1;
                else i++;
            }
            if (i + StartY == _cellStateCopy[0].Count || i + StartX == _cellStateCopy.Count) i--;
            return new Vector4(StartX, StartY, StartX + i, StartY + i);
        }
        else if (toleft)
        {
            i--;
            Debug.Log(StartX);
            Debug.Log(_cellStateCopy.Count);

            while ((i + StartX < _cellStateCopy.Count) && ( StartY+2-i >= 0) && toleft)
            {
                toleft = toleft && (int)_cellStateCopy[StartX + i][StartY + 2-i] == DefValL;
                if (!toleft) i -= 1;
                else i++;
            }

            if (i + StartX == _cellStateCopy.Count || StartY + 2 - i == -1) i--;
            return new Vector4(StartX, StartY + 2, StartX + i , StartY + 2 -i);
        }
        else return Vector4.zero;
                
    }


    static private Vector4 CheckVerticalHorizontal(int StartX, int StartY)
    {
        int i = 0;
        while (i < 3)
        {

            int DefValR = (int)_cellStateCopy[StartX + i][StartY];
            bool rowFlag = DefValR == 1 || DefValR == 2;

            int DefValC = (int)_cellStateCopy[StartX][StartY+i];
            bool colFlag = DefValC == 1 || DefValC == 2;
            int j = 0;
            while (j < 3 & (rowFlag || colFlag))
            {
                rowFlag = rowFlag && (int)_cellStateCopy[StartX + i][StartY + j] == DefValR;
                colFlag = colFlag && (int)_cellStateCopy[StartX + j][StartY + i] == DefValC;

                j++;
            }
            if (rowFlag)
            {
                j--;
                while (j + StartY <= _cellStateCopy[0].Count - 1 && rowFlag)
                {
                    rowFlag = rowFlag && (int)_cellStateCopy[StartX + i][StartY + j] == DefValR;
                    if (!rowFlag) j -= 1;
                    else j++;
                }

                if (j + StartY == _cellStateCopy[0].Count) j--;
                return new Vector4(StartX + i, StartY, StartX + i, StartY + j);

            }
            else if (colFlag)
            {
                j--;
                while(j+StartX<=_cellStateCopy.Count-1 && colFlag)
                {
                    colFlag = colFlag && (int)_cellStateCopy[StartX + j][StartY + i] == DefValC;
                    if (!colFlag) j -= 1;
                    else j++;
                }
                if (j + StartX == _cellStateCopy.Count) j--;
                return new Vector4(StartX, StartY + i, StartX + j, StartY + i);
            }

            i++;
        }
        return Vector4.zero;
    }


   static private void ClearLine(Vector2Int id1,Vector2Int id2, Vector2Int idDefault)
    {
        if (id1 != idDefault)  _cellStateCopy[id1.x][id1.y]= 0;
        Vector2Int nextValue = new Vector2Int(id1.x + (int) Math.Sign(id2.x - id1.x), id1.y + (int)Math.Sign(id2.y - id1.y));
        while (nextValue != id2)
        {
            if (nextValue != idDefault) _cellStateCopy[nextValue.x][nextValue.y]=0;
             nextValue = new Vector2Int(nextValue.x + (int)Math.Sign(id2.x - id1.x), nextValue.y + (int)Math.Sign(id2.y - id1.y));
        }
        if (id2 != idDefault) _cellStateCopy[id2.x][id2.y] = 0;
    }


    static public void Restart()
    {
        _isGamePlaying = true;
    }

    static public void NewTurn()
    {
        _isPossibilityOfMove = true;

        _currentPlayer = (_currentPlayer == 1) ? 2 : 1;
    }
}
