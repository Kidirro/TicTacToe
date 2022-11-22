using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{

    public static IEnumerator ScaleWithLerp(this Transform transform, Vector3 initialScale, Vector3 finalScale, int countFrame)
    {
        float percentage = 0;
        int frame = 0;
        while (frame <= countFrame)
        {
            percentage = (float)frame / (float)countFrame;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, percentage);
            frame += 1;
            yield return null;
        }
    }
}
