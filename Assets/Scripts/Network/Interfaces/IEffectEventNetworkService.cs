using System;
using UnityEngine;

namespace Network.Interfaces
{
    public interface IEffectEventNetworkService
    {
        public void RaiseEventAddEffect(Action action);
        public void RaiseEventAddEffect(int action);
        public void RaiseEventClearEffect(int action);

        public void RaiseEventUpdateEffect(int action, int value);
        public void RaiseEventAddFreezeEffect(Vector2Int position);
    }
}