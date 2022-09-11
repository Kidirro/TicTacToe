using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;



public class Cell : MonoBehaviour
{
    const float _scaleSpeed = 4;
    const float _positionSpeed = 4;
    const float _figureSpeed = 4;

    public static float AnimationTime
    {

        get => 100 / Mathf.Max(_scaleSpeed, _positionSpeed, _figureSpeed) * Time.deltaTime;
    }


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
    private bool _isFigureCoroutineWork = false;

    public Vector2Int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    private Vector2Int _id;

    private RectTransform _transformRect;

    public CellFigure Figure
    {
        get { return _figure; }
    }

    private CellFigure _figure;

    public CellSubState SubState
    {
        get { return _subState; }
    }

    private CellSubState _subState;

    [HideInInspector]
    public bool IsCellClear = false;

    private Image _image;
    private Image _subImage;
    private Image _highlightImage;

    void Awake()
    {
        _transformRect = GetComponent<RectTransform>();
        _subImage = transform.GetChild(0).GetComponent<Image>();
        _highlightImage = transform.GetChild(1).GetComponent<Image>();
        _image = transform.GetChild(2).GetComponent<Image>();
    }

    public void SetFigure(int s, bool isQueue = true)
    {
        _figure = (CellFigure)s;
        switch ((CellFigure)s)
        {
            case CellFigure.none:
                if (isQueue) CoroutineManager.Instance.AddCoroutine(IFigureFillProcess((CellFigure)s, true));
                else StartCoroutine(IFigureFillProcess((CellFigure)s, true));
                break;
            case CellFigure.p1:
                _image.fillMethod = Image.FillMethod.Vertical;
                _image.fillOrigin = 0;
                if (isQueue) CoroutineManager.Instance.AddCoroutine(IFigureFillProcess((CellFigure)s, false));
                else StartCoroutine(IFigureFillProcess((CellFigure)s, false));
                break;
            case CellFigure.p2:
                _image.fillMethod = Image.FillMethod.Radial360;
                _image.fillOrigin = 0;
                if (isQueue) CoroutineManager.Instance.AddCoroutine(IFigureFillProcess((CellFigure)s, false));
                else StartCoroutine(IFigureFillProcess((CellFigure)s, false));
                break;

        }
    }

    public void SetFigure(CellFigure s, bool isQueue = true)
    {
        _figure = s;
        switch (s)
        {
            case CellFigure.none:
                if (isQueue) CoroutineManager.Instance.AddCoroutine(IFigureFillProcess(s, true));
                else StartCoroutine(IFigureFillProcess(s, true));
                break;
            case CellFigure.p1:
                _image.fillMethod = Image.FillMethod.Vertical;
                _image.fillOrigin = 0;
                if (isQueue) CoroutineManager.Instance.AddCoroutine(IFigureFillProcess(s, false));
                else StartCoroutine(IFigureFillProcess(s, false));
                break;
            case CellFigure.p2:
                _image.fillMethod = Image.FillMethod.Radial360;
                _image.fillOrigin = 0;
                if (isQueue) CoroutineManager.Instance.AddCoroutine(IFigureFillProcess(s, false));
                else StartCoroutine(IFigureFillProcess(s, false));
                break;

        }
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

    public void SetSubState(Sprite sprite, Color color, CellSubState cellSubState, bool isQueue = true)
    {
        if (_subState == cellSubState && _subImage.sprite == sprite) return;
        _subState = cellSubState;
        if (isQueue) CoroutineManager.Instance.AddCoroutine(ISubStateFillProcess(sprite, color, false));
        else StartCoroutine(ISubStateFillProcess(sprite, color, false));
    }

    public void ResetSubState(bool isQueue = true)
    {
        _subState = CellSubState.none;
        Color color = Color.white;
        color.a = 0;
        if (isQueue) CoroutineManager.Instance.AddCoroutine(ISubStateFillProcess(null, color, true));
        else StartCoroutine(ISubStateFillProcess(null, color, true));
    }

    public void HighlightCell(Sprite sprite, Color color)
    {
        _highlightImage.sprite = sprite;
        _highlightImage.color = color;
    }

    public void UnhighlightCell()
    {
        _highlightImage.sprite = null;
        Color color = Color.white;
        color.a = 0;
        _highlightImage.color = color;
    }

    private IEnumerator ScaleIEnumerator()
    {
        _isSizeCoroutineWork = true;
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
            _transformRect.sizeDelta += new Vector2(step, step);
            i++;
            yield return null;
        }
        _transformRect.sizeDelta += new Vector2(step, step);
        _isSizeCoroutineWork = false;
        yield break;
    }

    private IEnumerator PositionIEnumerator()
    {
        _isPositionCoroutineWork = true;
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
        _isPositionCoroutineWork = false;
        yield break;
    }

    private IEnumerator IFigureFillProcess(CellFigure s, bool reverse)
    {

        if (!reverse)
        {
            _image.sprite = ThemeManager.Instance.GetSprite(s);

        }
        _isFigureCoroutineWork = true;
        _image.fillAmount = (reverse) ? 1 : 0;
        float step = (1) / 100f * _figureSpeed;
        int i = 0;
        while (i <= 100 / _figureSpeed)
        {
            _image.fillAmount += (reverse) ? -step : step;
            i++;
            yield return null;
        }
        _image.fillAmount = (reverse) ? 0 : 1;
        if (reverse) _image.sprite = ThemeManager.Instance.GetSprite(s);
        _isFigureCoroutineWork = false;
        yield break;
        //_figure = (CellFigure)s;

    }

    private IEnumerator ISubStateFillProcess(Sprite s, Color32 color, bool reverse)
    {

        if (!reverse)
        {
            _subImage.sprite = s;
            _subImage.color = color;
        }

        _isFigureCoroutineWork = true;
        _subImage.fillAmount = (reverse) ? 1 : 0;
        float step = (1) / 100f * _figureSpeed;
        int i = 0;
        while (i <= 100 / _figureSpeed)
        {
            _subImage.fillAmount += (reverse) ? -step : step;
            i++;
            yield return null;
        }
        _subImage.fillAmount = (reverse) ? 0 : 1;
        if (reverse)
        {
            _subImage.sprite = s;
            _subImage.color = color;
        }
        _isFigureCoroutineWork = false;
        yield break;
        //_figure = (CellFigure)s;
    }
}

public enum CellSubState
{
    none,
    block
}

public enum CellFigure
{
    none = 0,
    p1 = 1,
    p2 = 2
}

