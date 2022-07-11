using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardTips : MonoBehaviour
{
    [SerializeField]
    private RectTransform _tipRect;

    [SerializeField]
    private TextMeshProUGUI _tipsText;

    [SerializeField]
    private CanvasGroup _tipCanvas;

    [SerializeField, Space]
    private Vector2Int _tipsPos;
    [SerializeField]
    private float _awaitTime;

    private bool _isPositionCoroutineWork = false;
    private float _positionSpeed = 4;
    private Vector2 _tipPosition;

    private bool _isAlphaCoroutineWork = false;
    private float _alphaSpeed = 4;
    private float _tipAlpha;

    public void ShowTip(string textTip, bool instantly = true)
    {
        if (!this.gameObject.activeSelf) this.gameObject.SetActive(true);
        _tipsText.text = textTip;
        _tipPosition = new Vector2(0, ScreenManager.Instance.GetHeight(_tipsPos.y));
        _tipAlpha = 1;
        if (instantly)
        {
            _tipRect.localPosition = _tipPosition;
            _tipCanvas.alpha = _tipAlpha;
        }
        else
        {/*
            if (!_isPositionCoroutineWork)
                StartCoroutine(IPositionIEnumerator(_awaitTime));*/

            _tipRect.localPosition = _tipPosition;
            if (!_isAlphaCoroutineWork)
                StartCoroutine(IAlphaIEnumerator(_awaitTime));
        }
    }

    public void HideTip(bool instantly)
    {
        _tipPosition = new Vector2(0, ScreenManager.Instance.GetHeight(_tipsPos.x));
        _tipAlpha = 0;

        if (instantly)
        {
            _tipRect.localPosition = new Vector2(0, ScreenManager.Instance.GetHeight(_tipsPos.x));
            _tipCanvas.alpha = 0;
            this.gameObject.SetActive(false);
        }
        else
        {/*
            if (!_isPositionCoroutineWork && this.gameObject.activeSelf)
                StartCoroutine(IPositionIEnumerator(0));*/

            _tipRect.localPosition = _tipPosition;
            if (!_isAlphaCoroutineWork)
                StartCoroutine(IAlphaIEnumerator(0));
        }
    }


    private IEnumerator IPositionIEnumerator(float aTime)
    {
        yield return new WaitForSecondsRealtime(aTime);

        _isPositionCoroutineWork = true;

        float countStep = 100f / _positionSpeed;

        Vector2 prevPos = _tipPosition;

        Vector2 currentPosition = _tipRect.localPosition;
        Vector2 step = (prevPos - currentPosition) / countStep;
        int i = 0;
        while (i <= countStep)
        {
            currentPosition = _tipRect.localPosition;
            if (prevPos != _tipPosition)
            {
                prevPos = _tipPosition;

                step = (prevPos - currentPosition) / countStep;
                i = 0;
            }

            _tipRect.localPosition = currentPosition + step;
            i++;
            yield return null;
        }
        _tipRect.localPosition = _tipPosition;
        _isPositionCoroutineWork = false;
        yield break;
    }
    private IEnumerator IAlphaIEnumerator(float aTime)
    {
        yield return new WaitForSecondsRealtime(aTime);

        _isPositionCoroutineWork = true;
        float countStep = 100f / _positionSpeed;
        float prevAlpha = _tipAlpha;
        float currentAlpha = _tipCanvas.alpha;

        float step = (prevAlpha - currentAlpha) / countStep;

        int i = 0;
        while (i <= countStep)
        {
            currentAlpha = _tipCanvas.alpha;
            if (prevAlpha != _tipAlpha)
            {
                prevAlpha = _tipAlpha;

                step = (prevAlpha - currentAlpha) / countStep;
                i = 0;
            }

            _tipCanvas.alpha = currentAlpha + step;
            i++;
            yield return null;
        }
        _tipCanvas.alpha = _tipAlpha;
        _isAlphaCoroutineWork = false;
        yield break;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _isPositionCoroutineWork = false;
        _isAlphaCoroutineWork = false;
    }

}
