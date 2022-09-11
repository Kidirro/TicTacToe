using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class HistoryManager : Singleton<HistoryManager>
    {

        [SerializeField]
        private GameObject _historyUnitPrefab;

        [SerializeField]
        private Transform _historyContetParent;

        private List<GameObject> _historyUnitObjectsList = new List<GameObject>();
        private List<HistoryUnit> _historyUnitInfoList = new List<HistoryUnit>();

        public void AddHistoryNewTurn(PlayerInfo player)
        {
            GameObject historyUnit = Instantiate(_historyUnitPrefab, _historyContetParent);
            _historyUnitObjectsList.Add(historyUnit);
            _historyUnitInfoList.Add(new HistoryUnit { HistoryType = HistoryUnit.HistoryUnitTypes.NewTurn, HistoryPlayer = player });
        }

        public void AddHistoryCard(PlayerInfo player, CardInfo card)
        {
            GameObject historyUnit = Instantiate(_historyUnitPrefab, _historyContetParent);
            _historyUnitObjectsList.Add(historyUnit);
            _historyUnitInfoList.Add(new HistoryUnit { HistoryType = HistoryUnit.HistoryUnitTypes.Card, HistoryPlayer = player, HistoryCard = card });
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
            for (int i=0; i< _historyUnitObjectsList.Count; i++)
            {
                Destroy(_historyUnitObjectsList[i]);
            }
            _historyUnitObjectsList = new List<GameObject>();
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