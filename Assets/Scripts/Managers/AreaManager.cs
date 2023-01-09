using UnityEngine;

namespace Managers
{

    public class AreaManager : MonoBehaviour
    {

        public static Vector4 GetArea(Vector2Int id, Vector2 areaSize)
        {
            Vector2Int leftDown = Vector2Int.zero;
            Vector2Int RightUp = Vector2Int.zero;
            Vector2Int FieldSize = Field.Instance.FieldSize;

            if (areaSize.x == -1)
            {
                leftDown.x = 0;
                RightUp.x = FieldSize.x - 1;
            }
            else
            {
                areaSize.x--;
                leftDown.x = (int)Mathf.Clamp(id.x - Mathf.Floor(areaSize.x / 2f), 0, FieldSize.x - 1);
                RightUp.x = (int)Mathf.Clamp(leftDown.x + areaSize.x, 0, FieldSize.x - 1);
                leftDown.x = (int)Mathf.Clamp(RightUp.x - areaSize.x, 0, FieldSize.x - 1);
            }


            if (areaSize.y == -1)
            {
                leftDown.y = 0;
                RightUp.y = FieldSize.y - 1;
            }
            else
            {
                areaSize.y--;
                leftDown.y = (int)Mathf.Clamp(id.y - Mathf.Floor(areaSize.y / 2f), 0, FieldSize.y - 1);
                RightUp.y = (int)Mathf.Clamp(leftDown.y + areaSize.y, 0, FieldSize.y - 1);
                leftDown.y = (int)Mathf.Clamp(RightUp.y - areaSize.y, 0, FieldSize.y - 1);
            }

            return new Vector4(leftDown.x, leftDown.y, RightUp.x, RightUp.y);
        }
    }
}