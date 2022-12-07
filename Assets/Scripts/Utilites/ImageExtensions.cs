using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    public static IEnumerator AlphaWithLerp(this Image image, float initialAlpha, float finalAlpha, int countFrame)
    {
        float percentage = 0;
        int frame = 0;
        while (frame <= countFrame)
        {
            percentage = (float)frame / (float)countFrame;
            Color color = image.color;
            color.a = Mathf.Lerp(initialAlpha, finalAlpha, percentage);
            image.color = color;
            frame += 1;
            yield return null;
        }
    }
}
