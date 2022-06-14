using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    private Vector2 _screenDefault = new Vector2(720, 1280);

    public Vector2 ScreenDefault
    {
        get { return _screenDefault; }
    }


    public Vector2 GetVector(Vector2 vector)
    {
        return new Vector2(GetWidth(vector.x), GetHeight(vector.y));
    }


    public float GetHeight(float h)
    {
        return h * GetHeightRatio();
    }

    public float GetWidth(float w)
    {
        return w * GetWidthRatio();
    }

    public float GetHeightRatio()
    {
        return Camera.main.pixelHeight / _screenDefault.y;
    }


    public float GetWidthRatio()
    {
        return Camera.main.pixelWidth / _screenDefault.x;
    }

}
