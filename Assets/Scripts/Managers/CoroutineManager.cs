using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class CoroutineManager : Singleton<CoroutineManager>
    {

        public Queue<IEnumerator> _coroutineQueue = new Queue<IEnumerator>();

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
                    Debug.Log("Star tCoroutine");
                    yield return StartCoroutine(_coroutineQueue.Dequeue());

                    Debug.Log("End Coroutine");
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public void AddCoroutine(IEnumerator coroutine)
        {
            Debug.Log("Add coroutine");
            _coroutineQueue.Enqueue(coroutine);
        }

        IEnumerator TestCoroutine1()
        {
            Debug.Log("Coroutine 1 Started");
            yield return new WaitForSeconds(1);
            Debug.Log("Coroutine 1 Ended");
        }

        IEnumerator TestCoroutine2()
        {
            Debug.Log("Coroutine 2 Started");
            yield return new WaitForSeconds(2);
            Debug.Log("Coroutine 2 Ended");
        }
    }
}