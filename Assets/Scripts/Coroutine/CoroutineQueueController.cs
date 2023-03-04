using System.Collections;
using System.Collections.Generic;
using Coroutine.Interfaces;
using UnityEngine;

namespace Coroutine
{

    public class CoroutineQueueController: MonoBehaviour, ICoroutineService, ICoroutineAwaitService
    {

        private readonly Queue<IEnumerator> _coroutineQueue = new();

        public static bool isQueueEmpty = true;

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
                    isQueueEmpty = false;
                    Debug.Log("BegunAnim");
                    yield return StartCoroutine(_coroutineQueue.Dequeue());

                }
                yield return new WaitForEndOfFrame();
                isQueueEmpty = true;
            }
        }

        public void AddCoroutine(IEnumerator coroutine)
        {
            isQueueEmpty = false;
            _coroutineQueue.Enqueue(coroutine);
            Debug.Log($"BegunAnim {_coroutineQueue.Count}");
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
    }
}