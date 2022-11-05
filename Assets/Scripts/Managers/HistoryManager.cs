using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{

    public class HistoryManager : Singleton<HistoryManager>
    {

        [Header("Prefabs"), SerializeField]
        private GameObject _historySegmentPrefabP1;

        [SerializeField]
        private GameObject _historySegmentPrefabP2;
        
        
        [SerializeField]
        private GameObject _historyCardPrefab;

        [Space, SerializeField]
        private RectTransform _historyContentParent;

        private List<GameObject> _historySegmentObjectsList = new List<GameObject>();

        private List<HistoryUnit> _historyUnitInfoList = new List<HistoryUnit>();

        private bool _isNewTurn = true;

        public void AddHistoryNewTurn(PlayerInfo player)
        {
            _historyUnitInfoList.Add(new HistoryUnit { HistoryType = HistoryUnit.HistoryUnitTypes.NewTurn, HistoryPlayer = player });
            _isNewTurn = true;
        }

        public void AddHistoryCard(PlayerInfo player, CardInfo card)
        {
            if (_isNewTurn)
            {
                _historySegmentObjectsList.Add(Instantiate((player.SideId == 1) ? _historySegmentPrefabP1 : _historySegmentPrefabP2, _historyContentParent));                
                _historySegmentObjectsList[_historySegmentObjectsList.Count-1].transform.SetSiblingIndex(0);
                if (_historySegmentObjectsList.Count == 7)
                {
                    Destroy(_historySegmentObjectsList[0].gameObject);
                    _historySegmentObjectsList.RemoveAt(0);
                }
                _isNewTurn = false;
            }

            GameObject historyUnit = Instantiate(_historyCardPrefab, _historySegmentObjectsList[_historySegmentObjectsList.Count-1].transform);
            historyUnit.transform.GetChild(0).GetComponent<Image>().sprite = (player.SideId == 1) ? card.CardImageP1 : card.CardImageP2;
            historyUnit.transform.SetSiblingIndex(0);
            Debug.Log("TryStartCoroutine");
            StartCoroutine(IChangePosition());
        }

        private void Update()
        {            
            if (Input.GetKeyDown(KeyCode.I))
            {
                for (int i = 0; i < _historyUnitInfoList.Count; i++)
                {
                    if (_historyUnitInfoList[i].HistoryType == HistoryUnit.HistoryUnitTypes.NewTurn) Debug.LogFormat("HistoryUnit id: {0}, type: {1}, player: {2}", i, _historyUnitInfoList[i].HistoryType, _historyUnitInfoList[i].HistoryPlayer.SideId);
                    else Debug.LogFormat("HistoryUnit id: {0}, type: {1}, player: {2}, card: {3}", i, _historyUnitInfoList[i].HistoryType, _historyUnitInfoList[i].HistoryPlayer.SideId, _historyUnitInfoList[i].HistoryCard.CardName);
                }
            }
        }


        public void RestartGame()
        {
            _historyUnitInfoList = new List<HistoryUnit>();
            for (int i=0; i< _historySegmentObjectsList.Count; i++)
            {
                Destroy(_historySegmentObjectsList[i]);
            }
            _historySegmentObjectsList = new List<GameObject>();
            _isNewTurn = true;
        }

        IEnumerator IChangePosition()
        {

            float startWidth = _historyContentParent.rect.width;
            while(startWidth == _historyContentParent.rect.width)            yield return null;
            _historyContentParent.anchoredPosition = new Vector2(_historyContentParent.rect.width / 2, _historyContentParent.anchoredPosition.y);
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