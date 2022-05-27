using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Cell : MonoBehaviour
{
    const float _scaleSpeed = 4;
    const float _positionSpeed = 4;

    public float CellSize
    {
        get { return _cellSize; }
    }
    private float _cellSize;

    private bool _isSizeCoroutineWork = false;

    public Vector2 Position
    {
        get { return _position; }
    }
    private Vector2 _position;
    private bool _isPositionCoroutineWork = false;

    public Vector2Int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    private Vector2Int _id;

    private RectTransform _transformRect;

    public CellState State
    {
        get { return _state; }
    }

    private CellState _state;

    private Image _image;

    void Awake()
    {
        _transformRect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void HighlightCell(CellState s)
    {
        if (_state != CellState.empty) return;
        _state = CellState.Highlighted;
        _image.sprite = ThemeManager.Instance.GetSprite(s);
        var cl = _image.color;
        cl.a = 0.15f;
        _image.color = cl;
    }
    public void UnHighlightCell()
    {
        if (_state != CellState.Highlighted) return;
        _state = CellState.empty;
        _image.sprite = ThemeManager.Instance.GetSprite(CellState.empty);
        var cl = _image.color;
        cl.a =1f;
        _image.color = cl;
    }

    public void SetState(int s)
    {
        _image.sprite = ThemeManager.Instance.GetSprite((CellState)s);
        Debug.LogFormat("{0}, {1} ", Id, (CellState)s);
        _state = (CellState)s;
        var cl = _image.color;
        cl.a =1f;
        _image.color = cl;
    }

    public void SetState(CellState s)
    {
        _image.sprite = ThemeManager.Instance.GetSprite(s);
        _state = s;
    }

    public void SetTransformSize(float reals, bool instantly = true)
    {
        _cellSize = reals;
        if (instantly) _transformRect.sizeDelta = new Vector2(_cellSize, _cellSize);
        else if (!_isSizeCoroutineWork) StartCoroutine(ScaleIEnumerator());
    }

    public void SetTransformParent(Transform parent)
    {
        _transformRect.SetParent(parent);
        SetTransformPosition(0, 0);
        _transformRect.localScale = Vector3.one;
    }

    public void SetTransformPosition(float x, float y, bool instantly = true)
    {
        _position = new Vector3(x, y, 0);
        if (instantly)
        {
            _transformRect.localPosition = _position;
        }
        else if (!_isPositionCoroutineWork) StartCoroutine(PositionIEnumerator());
    }

    public void Clicked()
    {
        TurnController.PlaceInCell(_id);
    }

    private IEnumerator ScaleIEnumerator()
    {
        _isSizeCoroutineWork = true;
        Field.Instance.CellAnimating = Field.Instance.CellAnimating + 1;
        float prevS = _cellSize;
        float step = (_cellSize - _transformRect.sizeDelta.x) / 100f * _scaleSpeed;
        int i = 0;
        while (i <= 100 / _scaleSpeed)
        {
            if (prevS != _cellSize)
            {
                prevS = _cellSize;
                step = (_cellSize - _transformRect.sizeDelta.x) / 100f * _scaleSpeed;
                i = 0;
            }
            _transformRect.sizeDelta = _transformRect.sizeDelta + new Vector2(step, step);
            i++;
            yield return null;
        }
        _transformRect.sizeDelta = new Vector2(_cellSize, _cellSize);
        Field.Instance.CellAnimating = Field.Instance.CellAnimating - 1;
        _isSizeCoroutineWork = false;
        yield break;
    }

    private IEnumerator PositionIEnumerator()
    {
        _isPositionCoroutineWork = true;
        Field.Instance.CellAnimating = Field.Instance.CellAnimating + 1;
        Vector2 prevPos = _position;

        Vector2 currentPosition = _transformRect.localPosition;
        Vector2 step = (prevPos - currentPosition) / 100f * _positionSpeed;
        int i = 0;
        while (i <= 100 / _positionSpeed)
        {
            currentPosition = _transformRect.localPosition;
            if (prevPos != _position)
            {
                prevPos = _position;

                step = (prevPos - currentPosition) / 100f * _positionSpeed;
                i = 0;
            }

            _transformRect.localPosition = currentPosition + step;
            i++;
            yield return null;
        }
        _transformRect.localPosition = _position;
        Field.Instance.CellAnimating = Field.Instance.CellAnimating - 1;
        _isPositionCoroutineWork = false;
        yield break;
    }


}

public enum CellState
{
    Highlighted = -2,
    block = -1,
    empty = 0,
    p1 = 1,
    p2 = 2
}
