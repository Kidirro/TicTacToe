using System.Collections.Generic;
using UnityEngine;

namespace Field.Interfaces
{
    public interface IFieldFigureService
    {
        public CellFigure GetCellFigure(Vector2Int id);
        public CellFigure GetCellFigure(int x, int y);
        public void SetFigure(Vector2Int id, CellFigure figure, bool isQueue = true);
        public void SetFigure(int x, int y, CellFigure figure, bool isQueue = true);
        public bool IsCellEmpty(Vector2Int id);
        public bool IsCellEmpty(int x, int y);
        public bool IsCellBlocked(Vector2Int id);
        public bool IsCellBlocked(int x, int y);
        public bool IsCellEnableToPlace(Vector2Int id);
        public bool IsCellEnableToPlace(int x, int y);

        public bool GetIsCellClear(Vector2Int id);
        public bool GetIsCellClear(int x,int y);
        public void SetIsCellClear(Vector2Int id, bool state);
        public void SetIsCellClear(int x,int y, bool state);
        
        public void PlaceInCell(Vector2Int id, bool isNeedEvent = true, bool isQueue = true);
        public void PlaceInRandomCell(bool isNeedQueue = true);
        public void FreezeCell(Vector2Int id, Sprite sprite, bool isNeedEvent = true);
        public void FreezeCell(Vector2Int id, bool isNeedEvent = true);
        public void ResetSubStateWithPlaceFigure(Vector2Int position, bool isNeedEvent = true);
        public void ResetFigureWithPlaceSubState(Vector2Int position, Vector2Int areaSize, Sprite sprite, Color color,
            CellSubState cellSubState);

        public List<Cell> GetAllCellWithFigure(CellFigure figure);
        public List<Cell> GetAllCellWithSubState(CellSubState figureSubState);
    }
}