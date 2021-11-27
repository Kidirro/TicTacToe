using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer _line;
    private Vector2 _startPoint = Vector2.zero;
    private Vector2 _endPoint = Vector2.zero;

    private Coroutine _widthCorutine;

    public float Width
    {
        get { return _width; }
    }
    private float _width=0;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 2;
    }

    public void SetWidth(float s, bool instantly = true)
    {
        _width = s;
        if (instantly)
        {
            _line.endWidth = s;
            _line.startWidth = s;
        }
        else
        {
            if (_widthCorutine != null) StopCoroutine(_widthCorutine);
            _widthCorutine = StartCoroutine(WidthIEnumerator(s));
        }
        
          
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetWidth(_width + 1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetWidth(_width + 15,false);
        }
    }

    private IEnumerator WidthIEnumerator(float s)
    {
        float step = (s - _line.endWidth)/100f;
        for (int i=0;i<=100;i++)
        {
            _line.endWidth = _line.endWidth + step;
            _line.startWidth = _line.startWidth + step;
            yield return null; Debug.Log(_line.endWidth);
        }
        _line.endWidth = s;
        _line.startWidth = s;
    }
}
