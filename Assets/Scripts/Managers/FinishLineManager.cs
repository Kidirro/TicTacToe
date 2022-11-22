using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Managers
{

    public class FinishLineManager : Singleton<FinishLineManager>
    {

        public int CurrentGoalLine
        {
            get { return _currentGoalLine; }
        }

        private int _currentGoalLine = 3;

        private List<List<Vector2Int>> _lineForClearing = new List<List<Vector2Int>>();

        public void MasterChecker(int figure, bool isInQueue = true, bool isNeedEvent = true)
        {
            if (isInQueue) CoroutineManager.Instance.AddCoroutine(MasterChecker((CellFigure)figure, isNeedEvent));
            else StartCoroutine(MasterChecker((CellFigure)figure, isNeedEvent));

            Debug.Log("AddMasterChecker");
        }

        public IEnumerator MasterChecker(CellFigure figure, bool isNeedEvent)
        {
            if (isNeedEvent) NetworkEventManager.RaiseEventMasterChecker();
            List<Vector2Int> verticalList = new List<Vector2Int>();
            List<Vector2Int> horizontalList = new List<Vector2Int>();
            List<Vector2Int> diagonalRightList = new List<Vector2Int>();
            List<Vector2Int> diagonalLeftList = new List<Vector2Int>();

            List<List<Vector2Int>> linesFind = new List<List<Vector2Int>>();

            for (int x = 0; x < Field.Instance.FieldSize.x; x++)
            {
                for (int y = 0; y < Field.Instance.FieldSize.y; y++)
                {
                    Vector2Int curId = new Vector2Int(x, y);
                    if (Field.Instance.CellList[curId.x][curId.y].Figure == figure && !Field.Instance.CellList[curId.x][curId.y].IsCellClear)
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

            List<List<Vector2Int>> linesRes = new List<List<Vector2Int>>();
            foreach (List<Vector2Int> line in linesFind)
            {
                if (line.Count >= CurrentGoalLine && _lineForClearing.Find(x => x[0] == line[0] && x[x.Count - 1] == line[line.Count - 1]) == null)
                {
                    linesRes.Add(line);
                    _lineForClearing.Add(line);
                    foreach (Vector2Int cell in line)
                    {
                        Field.Instance.CellList[cell.x][cell.y].IsCellClear = true;
                    }
                }

            }

            if (linesRes.Count > 0) yield return StartCoroutine(IDrawFinishLine(linesRes));
            else
            {
                if (!Field.Instance.IsExistEmptyCell() && _lineForClearing.Count == 0 && GameplayManager.CurrentGameplayState != GameplayManager.GameplayState.GameOver)
                {
                    GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.RoundOver);
                }
            }
            yield return null;
        }

        private Vector2Int GetNextCellId(Vector2Int currentId, Vector2Int step)
        {
            Vector2Int nextId = currentId + step;
            if (nextId.x < 0 || nextId.y < 0 || nextId.x >= Field.Instance.FieldSize.x || nextId.y >= Field.Instance.FieldSize.y) return new Vector2Int(-1, -1);
            if (Field.Instance.CellList[nextId.x][nextId.y].IsCellClear) return new Vector2Int(-1, -1);
            if (Field.Instance.IsCellBlocked(nextId)) return new Vector2Int(-1, -1);
            if (Field.Instance.CellList[nextId.x][nextId.y].Figure != Field.Instance.CellList[currentId.x][currentId.y].Figure) return new Vector2Int(-1, -1);

            return nextId;
        }

        public IEnumerator IDrawFinishLine(List<List<Vector2Int>> linesFind)
        {
            Debug.Log("Begin Finish!");
            List<Vector2Int> uniqueCell = new List<Vector2Int>();
            foreach (List<Vector2Int> line in linesFind)
            {
                int i = 0;
                foreach (Vector2Int cell in line)
                {
                    if (uniqueCell.IndexOf(cell) == -1)
                    {
                        uniqueCell.Add(cell);
                        i += 1;
                    }
                    Field.Instance.CellList[cell.x][cell.y].IsCellClear = false;
                }
                Field.Instance.DrawFinishLine(line, i);
                _lineForClearing.Remove(line);
            }
            yield return StartCoroutine(CoroutineManager.Instance.IAwaitProcess(FinishLine.AnimationTime));

        }
    }
}