using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    [SerializeField]
    private float _angleStep;

    private void OnEnable()
    {
        StartCoroutine(IRotation());
    }


    IEnumerator IRotation()
    {
        while (true)
        {
            transform.rotation *= Quaternion.Euler(0, 0, _angleStep);

            yield return null;
        }
    }
}
