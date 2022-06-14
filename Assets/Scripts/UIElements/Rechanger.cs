using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rechanger : MonoBehaviour
{

    public float Height
    {
        get { return _rectTransform.rect.height; }
    }

    private RectTransform _rectTransform;

    private bool _isPositionCoroutineWork = false;
    private Vector2 _rechangerPosition;
    private float _positionSpeed = 4;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width*Camera.main.pixelWidth/720f, _rectTransform.rect.height * Camera.main.pixelHeight / 1280);
    }

    public void Show()
    {
        SetTransformPosition(Camera.main.pixelWidth / 2, Height / 2, false);
    }

    public void Hide()
    {
        SetTransformPosition(Camera.main.pixelWidth / 2, -Height / 2, false);
    }

    private void SetTransformPosition(float x, float y, bool instantly = true)
    {
        _rechangerPosition = new Vector2(x, y);
        if (instantly) _rectTransform.localPosition = _rechangerPosition;
        else if (!_isPositionCoroutineWork) StartCoroutine(PositionIEnumerator());
    }

    private IEnumerator PositionIEnumerator()
    {
        _isPositionCoroutineWork = true;

        float countStep = 100f / _positionSpeed;

        Vector2 prevPos = _rechangerPosition;


        Vector2 currentPosition = _rectTransform.localPosition;
        Vector2 step = (prevPos - currentPosition) / countStep;
        int i = 0;
        while (i <= countStep)
        {
            currentPosition = _rectTransform.localPosition;
            if (prevPos != _rechangerPosition)
            {
                prevPos = _rechangerPosition;

                step = (prevPos - currentPosition) / countStep;
                i = 0;
            }

            _rectTransform.localPosition = currentPosition + step;
            i++;
            yield return null;
        }
        _rectTransform.localPosition = _rechangerPosition;
        _isPositionCoroutineWork = false;
        yield break;
    }
}
