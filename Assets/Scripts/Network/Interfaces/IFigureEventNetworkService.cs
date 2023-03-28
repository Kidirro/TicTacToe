using UnityEngine;

namespace Network.Interfaces
{
    public interface IFigureEventNetworkService
    {
        public void RaiseEventPlaceInCell(Vector2Int id);
    }
}