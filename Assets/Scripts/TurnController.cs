using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    static private int _currentPlayer = 1;

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

            Field.Instance.CellList[id.x][id.y].ChangeState(_currentPlayer);
            _cellStateCopy = new List<List<CellState>>();
            for (int i =0;i<Field.Instance.CellList.Count;i++)
            {
                _cellStateCopy.Add(new List<CellState>());
                for(int j = 0; j < Field.Instance.CellList[i].Count; j++)
                {
                    _cellStateCopy[i].Add(Field.Instance.CellList[i][j].State);
                }
            }


            MasterChecker(id);

            _currentPlayer = (_currentPlayer == 1) ? 2 : 1;
        }
    }

    static private void MasterChecker(Vector2Int turnId)
    {

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
                    case 0:
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
                    case 2:
                        if (turnId.x > 1)
                        {
                            flagX = true;
                            startX = turnId.x-2;

                        }
                        break;
                }

                switch (y)
                {
                    case 0:
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
                    case 2:
                        if (turnId.y > 1)
                        {
                            flagY = true;
                            startY = turnId.y-2;

                        }
                        break;
                }

                if (flagX && flagY)
                {
                    Debug.Log(""+x +" "+y);
                    CheckField(startX,startY, turnId);
                }

            }
        }
    }

    static private void CheckField(int stepX, int stepY, Vector2Int turnId)
    {
        Vector4 DiagonalResult = Vector4.one;
        Vector4 LineResult = Vector4.one;

        while (DiagonalResult != Vector4.zero || LineResult != Vector4.zero)
        {
            DiagonalResult = CheckDiagonal(stepX, stepY);
            LineResult = CheckVerticalHorizontal(stepX, stepY);
            if (DiagonalResult != Vector4.zero)
            {
                _isPossibilityOfMove = false;
                Field.Instance.AddNewId(DiagonalResult);
                ClearLine(new Vector2Int((int)DiagonalResult.x, (int)DiagonalResult.y), new Vector2Int((int)DiagonalResult.z, (int)DiagonalResult.w), turnId);

            }
            if (LineResult != Vector4.zero)
            {
                _isPossibilityOfMove = false;
                Field.Instance.AddNewId(LineResult);

                ClearLine(new Vector2Int((int)LineResult.x, (int)LineResult.y), new Vector2Int((int)LineResult.z, (int)LineResult.w), turnId);
            }
        }
    }

    static private Vector4 CheckDiagonal(int StartX,int StartY)
    {

        int DefValR = (int)_cellStateCopy[StartX][StartY];
        bool toright = DefValR == 1 || DefValR==2;

        int DefValL = (int)_cellStateCopy[StartX+2][StartY];
        bool toleft = DefValL == 1 || DefValL == 2;
        int i = 0;
        while (i<3 & (toleft || toright))
        {
            toright = toright && (int)_cellStateCopy[StartX + i][StartY + i] == DefValR;

            toleft = toleft && (int)_cellStateCopy[StartX + 2 - i][StartY + i] == DefValL;
            i++;
        }
        if (toright) return new Vector4(StartX, StartY, StartX + i - 1, StartY + i - 1);
        else if (toleft) return new Vector4(StartX + 2, StartY, StartX + 3 - i, StartY + i - 1);
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
            if (rowFlag) return new Vector4(StartX + i, StartY, StartX + i, StartY + 2);
            else if (colFlag) return new Vector4(StartX, StartY + i, StartX + 2, StartY + i);

            i++;
        }
        return Vector4.zero;
    }


   static private void ClearLine(Vector2Int id1,Vector2Int id2, Vector2Int idDefault)
    {
        Vector2Int centralCell = new Vector2Int((id1.x + id2.x) / 2, (id1.y + id2.y) / 2);
        Debug.Log(centralCell);
        Debug.Log(idDefault);
        Debug.Log(idDefault==centralCell);
        if (id1 != idDefault)  _cellStateCopy[id1.x][id1.y]= 0;
        if ( centralCell != idDefault ) _cellStateCopy[centralCell.x][centralCell.y] = 0;
        if (id2 != idDefault) _cellStateCopy[id2.x][id2.y] = 0;
    }


    static public void Restart()
    {
        _isGamePlaying = true;
    }

    static public void UnlockTurn()
    {
        _isPossibilityOfMove = true;
    }
}
