using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    const float _widthCountFrame = 25;
    const float _positionCountFrame = 25;

    public LineRenderer LineRend
    {
        get { return _line; }
    }
    private LineRenderer _line;
    
    public float Alpha
    {
        get { return _alpha; }
    }
    private float _alpha=1;

    public Vector2 StartPoint
    {
        get { return _startPoint; }
    }
    private Vector2 _startPoint = Vector2.zero;

    public Vector2 EndPoint
    {
        get { return _endPoint; }
    }
    private Vector2 _endPoint = Vector2.zero;
    bool _isPositionCoroutineWork = false;



    public float Width
    {
        get { return _width; }
    }
    private float _width=0;
    bool _isWidthCoroutineWork = false;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 2;
        _startPoint = _line.GetPosition(0);
        _endPoint = _line.GetPosition(1);
    }

    private float ScreenToWorld(float s)
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(s, s)).x;
    }

    public void SetColor(Color s)
    {
        _line.startColor = s;
        _line.endColor = s;
    }

    public void SetWidthScreenCord(float s, bool instantly = true)
    {
        _width = s/250;
        if (instantly)
        {
            _line.endWidth = _width;
            _line.startWidth = _width;
        }
        else
        {
            if (!_isWidthCoroutineWork) StartCoroutine(WidthIEnumerator());
        } 
    } 

    public void SetWidthWorldCord(float s, bool instantly = true)
    {
        _width = s;
        if (instantly)
        {
            _line.endWidth = _width;
            _line.startWidth = _width;
        }
        else
        {
            if (!_isWidthCoroutineWork) StartCoroutine(WidthIEnumerator());
        } 
    }

    public void SetPositions(Vector2 StartPos,Vector2 EndPos, bool instantly = true)
    {
        _startPoint = StartPos;
        _endPoint = EndPos;
        if (instantly)
        {
            _line.SetPosition(0,_startPoint);
            _line.SetPosition(1, _endPoint);
        }
        else
        {
            if (!_isPositionCoroutineWork) StartCoroutine(PositionIEnumerator());
        }
    }

    public void SetTransformParent(Transform parent, bool instantly = true)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        
    }


    public void SetAlphaFinishLine(float s)
    {
        Color cl = _line.startColor;
        cl.a = s;
        _alpha = s;

        _line.startColor = cl;
        _line.endColor = cl;
    }

    private IEnumerator WidthIEnumerator()
    {
        _isWidthCoroutineWork = true;
        float prevW = _width;
        float step = (_width - _line.endWidth)/_widthCountFrame;
        int i = 0;
        while (i< _widthCountFrame)
        {
            if (prevW != _width)
            {
                prevW = _width;
                step = (_width - _line.endWidth) / _widthCountFrame;
                i = 0;
            }
            _line.endWidth = _line.endWidth + step;
            _line.startWidth = _line.startWidth + step;
            i++;
            yield return null; 
        }
        _line.endWidth = _width;
        _line.startWidth = _width;
        _isWidthCoroutineWork = false;
        yield break;
    }

    private IEnumerator PositionIEnumerator()
    {
        _isPositionCoroutineWork = true;
        Vector2 prevStartPos = _startPoint;
        Vector2 prevEndPos = _endPoint;

        Vector2 currentStartPoint = _line.GetPosition(0);
        Vector2 currentEndPoint = _line.GetPosition(1);
        Vector2 stepStart = (_startPoint - currentStartPoint) / _positionCountFrame;
        Vector2 stepEnd = (_endPoint - currentEndPoint) / _positionCountFrame;
        int i = 0;
        while (i < _positionCountFrame)
        {
            currentStartPoint = _line.GetPosition(0);
            currentEndPoint = _line.GetPosition(1);
            if (prevStartPos != _startPoint)
            {
                prevStartPos = _startPoint;
                stepStart = (_startPoint - currentStartPoint) / _positionCountFrame;
                i = 0;
            } 
            if (prevEndPos != _endPoint)
            {
                prevEndPos = _endPoint;
                stepEnd = (_endPoint - currentEndPoint) / _positionCountFrame;
                i = 0;
            }
            _line.SetPosition(0, currentStartPoint + stepStart);
            _line.SetPosition(1, currentEndPoint + stepEnd);
            i++;
            yield return null;
        }
        _line.SetPosition(0, _startPoint);
        _line.SetPosition(1, _endPoint);
        
        _isPositionCoroutineWork = false;
        yield break;
    }
}
