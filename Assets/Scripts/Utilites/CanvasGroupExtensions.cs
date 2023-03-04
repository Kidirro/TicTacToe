using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasGroupExtensions
{

    public static IEnumerator AlphaWithLerp(this CanvasGroup canvas, float initialAlpha, float finalAlpha, int countFrame, Action callback = null)
    {
        int frame = 0;
        while (frame <= countFrame)
        {
            var percentage = (float)frame / (float)countFrame;
            canvas.alpha = Mathf.Lerp(initialAlpha, finalAlpha, percentage);
            frame += 1;
            yield return null;
        }
        callback?.Invoke();
    }
}
