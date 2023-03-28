using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.CustomType;
using History.Interfaces;
using UnityEngine;
using Zenject;

namespace History
{
    public class HistoryFactory : MonoBehaviour, IHistoryService
    {
        [Header("Prefabs"), SerializeField]
        private GameObject _historySegmentPrefabP1;

        [SerializeField]
        private GameObject _historySegmentPrefabP2;


        [SerializeField]
        private GameObject _historyCardPrefab;

        [Space, SerializeField]
        private RectTransform _historyContentParent;

        private List<GameObject> _historySegmentObjectsList = new();

        private List<HistoryUnit> _historyUnitInfoList = new();

        private bool _isNewTurn = true;

        #region Dependecy

        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        #endregion

        public void AddHistoryNewTurn(PlayerInfo player)
        {
            _historyUnitInfoList.Add(new HistoryUnit
                {HistoryType = HistoryUnit.HistoryUnitTypes.NewTurn, HistoryPlayer = player});
            _isNewTurn = true;
        }

        public void AddHistoryCard(PlayerInfo player, CardInfo card)
        {
            if (_isNewTurn)
            {
                _historySegmentObjectsList.Add(_diContainer.InstantiatePrefab(
                    (player.SideId == 1) ? _historySegmentPrefabP1 : _historySegmentPrefabP2, _historyContentParent));
                _historySegmentObjectsList[^1].transform.SetSiblingIndex(0);
                if (_historySegmentObjectsList.Count == 7)
                {
                    Destroy(_historySegmentObjectsList[0].gameObject);
                    _historySegmentObjectsList.RemoveAt(0);
                }

                _isNewTurn = false;
            }

            GameObject historyUnit =
                _diContainer.InstantiatePrefab(_historyCardPrefab, _historySegmentObjectsList[^1].transform);
            historyUnit.GetComponent<HistoryCell>().SetCard(player, card);


            historyUnit.transform.SetSiblingIndex(0);
            Debug.Log("TryStartCoroutine");
            StartCoroutine(IChangePosition());
        }

        public void ClearHistory()
        {
            _historyUnitInfoList = new List<HistoryUnit>();
            for (int i = 0; i < _historySegmentObjectsList.Count; i++)
            {
                Destroy(_historySegmentObjectsList[i]);
            }

            _historySegmentObjectsList = new List<GameObject>();
            _isNewTurn = true;
        }

        IEnumerator IChangePosition()
        {
            float startWidth = _historyContentParent.rect.width;
            while (startWidth == _historyContentParent.rect.width) yield return null;
            _historyContentParent.anchoredPosition = new Vector2(_historyContentParent.rect.width / 2,
                _historyContentParent.anchoredPosition.y);
            Debug.Log("end thread");
        }

        public class HistoryUnit
        {
            public CardInfo HistoryCard;

            public HistoryUnitTypes HistoryType;

            public PlayerInfo HistoryPlayer;

            public enum HistoryUnitTypes
            {
                Card,
                NewTurn
            }
        }
    }
}