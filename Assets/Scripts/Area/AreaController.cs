using Area.Interfaces;
using Managers;
using UnityEngine;

namespace Area
{
    public class AreaController : IAreaService
    {

        public Vector4 GetArea(Vector2Int id, Vector2 areaSize)
        {
            Vector2Int leftDown = Vector2Int.zero;
            Vector2Int rightUp = Vector2Int.zero;
            Vector2Int fieldSize = Field.Instance.FieldSize;

            if (areaSize.x == -1)
            {
                leftDown.x = 0;
                rightUp.x = fieldSize.x - 1;
            }
            else
            {
                areaSize.x--;
                leftDown.x = (int)Mathf.Clamp(id.x - Mathf.Floor(areaSize.x / 2f), 0, fieldSize.x - 1);
                rightUp.x = (int)Mathf.Clamp(leftDown.x + areaSize.x, 0, fieldSize.x - 1);
                leftDown.x = (int)Mathf.Clamp(rightUp.x - areaSize.x, 0, fieldSize.x - 1);
            }


            if (areaSize.y == -1)
            {
                leftDown.y = 0;
                rightUp.y = fieldSize.y - 1;
            }
            else
            {
                areaSize.y--;
                leftDown.y = (int)Mathf.Clamp(id.y - Mathf.Floor(areaSize.y / 2f), 0, fieldSize.y - 1);
                rightUp.y = (int)Mathf.Clamp(leftDown.y + areaSize.y, 0, fieldSize.y - 1);
                leftDown.y = (int)Mathf.Clamp(rightUp.y - areaSize.y, 0, fieldSize.y - 1);
            }

            return new Vector4(leftDown.x, leftDown.y, rightUp.x, rightUp.y);
        }
    }
}