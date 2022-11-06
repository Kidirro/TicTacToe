using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class AnimationFading : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private float _fadeOutTimeAwait = 1;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        _animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        StartCoroutine(IDisable());
        _animator.SetTrigger("FadeOut");

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            FadeIn();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            FadeOut();
        }
    }

    IEnumerator IDisable()
    {
        yield return new  WaitForSecondsRealtime(_fadeOutTimeAwait);
        this.gameObject.SetActive(false);
    }
}
