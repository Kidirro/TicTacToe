using UnityEngine;

namespace Area.Interfaces
{
    public interface IAreaService
    {
        public Vector4 GetArea(Vector2Int id, Vector2 areaSize);
    }
}