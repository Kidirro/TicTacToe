using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasGroupExtensions
{

    public static IEnumerator AlphaWithLerp(this CanvasGroup canvas, float initialAlpha, float finalAlpha, int countFrame)
    {
        float percentage = 0;
        int frame = 0;
        while (frame <= countFrame)
        {
            percentage = (float)frame / (float)countFrame;
            canvas.alpha = Mathf.Lerp(initialAlpha, finalAlpha, percentage);
            frame += 1;
            yield return null;
        }
    }
}
