using System.Collections;
using Coroutine.Interfaces;
using UnityEngine;
using Zenject;

namespace UIElements
{
    [RequireComponent(typeof(Animator))]
    public class AnimationFading : MonoBehaviour
    {
        private Animator _animator;

        [SerializeField] private float _frameCount= 1;

        #region Dependency

        private ICoroutineAwaitService _coroutineAwaitService;
        private static readonly int fadeIn = Animator.StringToHash("FadeIn");
        private static readonly int fadeOut = Animator.StringToHash("FadeOut");

        [Inject]
        private void Construct(ICoroutineAwaitService coroutineAwaitService)
        {
            _coroutineAwaitService = coroutineAwaitService;
        }

        #endregion
 

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
            _animator.SetTrigger(fadeIn);
            StopAllCoroutines();
        }

        public void FadeOut()
        {
            _animator.SetTrigger(fadeOut);
            Debug.Log("StartFadeOut");
            StartCoroutine(IDisable());
        }

        IEnumerator IDisable()
        {
            yield return _coroutineAwaitService.AwaitTime(FrameCount);
            this.gameObject.SetActive(false);
        }
    }
}