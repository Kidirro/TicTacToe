using System.Collections;

namespace Coroutine.Interfaces
{
    public interface ICoroutineAwaitService
    {
        public void AddAwaitTime(float time);

        public UnityEngine.Coroutine AwaitTime(float time);

    }
}