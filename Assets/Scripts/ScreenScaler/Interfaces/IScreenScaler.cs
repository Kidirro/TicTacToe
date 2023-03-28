using UnityEngine;

namespace ScreenScaler.Interfaces
{
    public interface IScreenScaler
    {
        public Vector2 GetScreenDefault();
        public Vector2 GetVector(Vector2 vector);
        public float GetHeight(float h);
        public float GetWidth(float w);
        public float GetHeightRatio();
        public float GetWidthRatio();
    }
}