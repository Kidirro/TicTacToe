using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{

    private Queue<IEnumerator> _coroutineQueue = new Queue<IEnumerator>();

    void Start()
    {
        StartCoroutine(CoroutineCoordinator());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) _coroutineQueue.Enqueue(TestCoroutine1());
        if (Input.GetKeyDown(KeyCode.Mouse1)) _coroutineQueue.Enqueue(TestCoroutine2());
    }

    IEnumerator CoroutineCoordinator()
    {
        while (true)
        {
            while (_coroutineQueue.Count > 0)
                yield return StartCoroutine(_coroutineQueue.Dequeue());
            yield return null;
        }
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
