using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    const float _widthSpeed = 4;
    const float _positionSpeed = 4;

    public LineRenderer LineRend
    {
        get { return _line; }
    }
    private LineRenderer _line;
   
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

    private IEnumerator WidthIEnumerator()
    {
        _isWidthCoroutineWork = true;
        float prevW = _width;
        float step = (_width - _line.endWidth)/100f * _widthSpeed;
        int i = 0;
        while (i<=100/ _widthSpeed)
        {
            if (prevW != _width)
            {
                prevW = _width;
                step = (_width - _line.endWidth) / 100f * _widthSpeed;
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
        Vector2 stepStart = (_startPoint - currentStartPoint) / 100f* _positionSpeed;
        Vector2 stepEnd = (_endPoint - currentEndPoint) / 100f* _positionSpeed;
        int i = 0;
        while (i <= 100/ _positionSpeed)
        {
            currentStartPoint = _line.GetPosition(0);
            currentEndPoint = _line.GetPosition(1);
            if (prevStartPos != _startPoint)
            {
                prevStartPos = _startPoint;
                stepStart = (_startPoint - currentStartPoint) / 100f* _positionSpeed;
                i = 0;
            } 
            if (prevEndPos != _endPoint)
            {
                prevEndPos = _endPoint;
                stepEnd = (_endPoint - currentEndPoint) / 100f * _positionSpeed;
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
