using System.Collections;

namespace Coroutine.Interfaces
{
    public interface ICoroutineService
    {
        public void AddCoroutine(IEnumerator coroutine);
        public void ClearQueue();

    }
}