using UnityEngine;

namespace Field.Interfaces
{
    public interface IFieldZoneService
    {
        
        public void UnhighlightZone(Vector2Int position, Vector2Int areaSize);
        public void HighlightZone(Vector2Int position, Vector2Int areaSize, Sprite sprite, Color color);
        public bool IsZoneEmpty(Vector2Int position, Vector2Int areaSize);
        public bool IsZoneEnableToPlace(Vector2Int position, Vector2Int areaSize);
    }
}