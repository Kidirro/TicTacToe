using Field.Interfaces;
using UnityEngine;
using Zenject;

namespace Field
{
    public class FieldFactory:IFieldFactoryService
    {
        private Cell _cellPrefab;
        private const string CELL_PREFAB_PATH = "Cell";

        private Line _linePrefab;
        private const string LINE_PREFAB_PATH = "Line";

        #region Dependecy

        private readonly DiContainer _diContainer;

        public FieldFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            Load();
        }

        #endregion

        private void Load()
        {
            _cellPrefab = Resources.Load<Cell>(CELL_PREFAB_PATH);
            _linePrefab = Resources.Load<Line>(LINE_PREFAB_PATH);
        }

        public Cell InstantiateCell()
        {
            return _diContainer.InstantiatePrefabForComponent<Cell>(_cellPrefab);
        }
        
        public Line InstantiateLine()
        {
            return _diContainer.InstantiatePrefabForComponent<Line>(_linePrefab);
        }
        
    }
}