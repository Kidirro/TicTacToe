using FinishLine.Interfaces;
using UnityEngine;
using Zenject;

namespace FinishLine
{
    public class FinishLineFactory:IFinishLineFactoryService
    {
        private FinishLineObject _finishLinePrefab;
        private const string FINISH_LINE_PREFAB_PATH = "FinishLine";

        #region Dependecy

        private readonly DiContainer _diContainer;

        public FinishLineFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            Load();
        }

        #endregion

        private void Load()
        {
            _finishLinePrefab = Resources.Load<FinishLineObject>(FINISH_LINE_PREFAB_PATH);
        }

        public FinishLineObject InstantiateFinishLine()
        {
            return _diContainer.InstantiatePrefabForComponent<FinishLineObject>(_finishLinePrefab);
        }
    }
}