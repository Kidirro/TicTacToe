using UnityEngine;

namespace Network.Interfaces
{
    public interface IFreezeEventNetworkService
    {
        public void RaiseEventFreezeCell(Vector2Int id);

        public void RaiseEventUnFreezeCell(Vector2Int id);
    }
}