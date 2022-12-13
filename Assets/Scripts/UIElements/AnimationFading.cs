using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationFading : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private float _frameCount = 1;

    public float FrameCount 
        
    {
        get => _frameCount;
    }

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
        _animator.SetTrigger("FadeIn");
        StopAllCoroutines();
    }

    public void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
        Debug.Log("StartFadeOut");
        StartCoroutine(IDisable());
    }

    IEnumerator IDisable()
    {
        yield return CoroutineManager.Instance.IAwaitProcess(FrameCount);
        this.gameObject.SetActive(false);
    }

}