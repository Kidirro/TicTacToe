using System.Collections;
using System.Collections.Generic;
using Coroutine.Interfaces;
using UnityEngine;

namespace Coroutine
{

    public class CoroutineQueueController: MonoBehaviour, ICoroutineService, ICoroutineAwaitService
    {

        private readonly Queue<IEnumerator> _coroutineQueue = new();

        private bool _isQueueEmpty = true;
        
        void Start()
        {
            StartCoroutine(CoroutineCoordinator());
        }

        IEnumerator CoroutineCoordinator()
        {
            while (true)
            {
                while (_coroutineQueue.Count > 0)
                {
                    _isQueueEmpty = false;
                    yield return StartCoroutine(_coroutineQueue.Dequeue());

                }
                yield return new WaitForEndOfFrame();
                _isQueueEmpty = true;
            }
        }

        public void AddCoroutine(IEnumerator coroutine)
        {
            Debug.Log("add coroutine");
            _isQueueEmpty = false;
            _coroutineQueue.Enqueue(coroutine);
        }

        public void AddAwaitTime(float time)
        {
            AddCoroutine(AwaitProcess(time));
        }

        public UnityEngine.Coroutine AwaitTime(float time)
        {
            return StartCoroutine(AwaitProcess(time));
        }

        private IEnumerator AwaitProcess(float time)
        {
            for (int i = 0; i < time; i++) yield return null;
        }

        public void ClearQueue()
        {
            _coroutineQueue.Clear();
        }

        public bool GetIsQueueEmpty()
        {
            return _isQueueEmpty;
        }
    }
}