using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{

    public class HistoryManager : Singleton<HistoryManager>
    {

        [Header("Card view"), SerializeField]
        private CanvasGroup _viewCardCanvas;

        [SerializeField]
        private TextMeshProUGUI _viewManapoints;

        [SerializeField]
        private TextMeshProUGUI _viewCardDescription;

        [SerializeField]
        private Image _viewCardImage;

        [SerializeField]
        private List<GameObject> _viewBonusImageList = new List<GameObject>();

        private DateTime _startTimeTap = DateTime.MinValue;
        private const float TIME_TAP_VIEW = 0.5f;
        private const float ALPHA_PER_STEP = 0.05f;

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

        private IEnumerator _fadeInCoroutine;


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
                _historySegmentObjectsList[_historySegmentObjectsList.Count - 1].transform.SetSiblingIndex(0);
                if (_historySegmentObjectsList.Count == 7)
                {
                    Destroy(_historySegmentObjectsList[0].gameObject);
                    _historySegmentObjectsList.RemoveAt(0);
                }
                _isNewTurn = false;
            }

            GameObject historyUnit = Instantiate(_historyCardPrefab, _historySegmentObjectsList[_historySegmentObjectsList.Count - 1].transform);
            historyUnit.GetComponent<HistoryCell>().SetCard(player, card);


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
            for (int i = 0; i < _historySegmentObjectsList.Count; i++)
            {
                Destroy(_historySegmentObjectsList[i]);
            }
            _historySegmentObjectsList = new List<GameObject>();
            _isNewTurn = true;
        }

        public void StartTap(CardInfo cardCollection, PlayerInfo player)
        {
            _startTimeTap = DateTime.Now;
            _fadeInCoroutine = IStartTap(cardCollection,player);
            StartCoroutine(_fadeInCoroutine);
        }

        public void EndTap()
        {
            StopCoroutine(_fadeInCoroutine);
            if ((DateTime.Now - _startTimeTap).TotalSeconds > TIME_TAP_VIEW)
            {
                StartCoroutine(IEndTap());
            }
        }

        public void UpdateCardViewImage(CardInfo info, PlayerInfo player)
        {
            string desc = "";
            desc = I2.Loc.LocalizationManager.TryGetTranslation(info.CardDescription, out desc) ? I2.Loc.LocalizationManager.GetTranslation(info.CardDescription) : info.CardDescription;
            _viewCardDescription.text = desc;
            _viewCardImage.sprite = (player.SideId ==1)? info.CardImageP1: info.CardImageP2;
            _viewManapoints.text = info.CardManacost.ToString();
            for (int i = 0; i < _viewBonusImageList.Count; i++)
            {
                _viewBonusImageList[i].SetActive(i == (int)info.CardBonus);
            }
        }

        private IEnumerator IStartTap(CardInfo cardCollection, PlayerInfo player)
        {
            yield return new WaitForSeconds(TIME_TAP_VIEW);

            UpdateCardViewImage(cardCollection, player);
            _viewCardCanvas.gameObject.SetActive(true);
            _viewCardCanvas.alpha = 0;
            while (_viewCardCanvas.alpha < 1)
            {
                _viewCardCanvas.alpha += ALPHA_PER_STEP;
                yield return null;
            }

        }

        private IEnumerator IEndTap()
        {
            while (_viewCardCanvas.alpha > 0)
            {
                _viewCardCanvas.alpha -= ALPHA_PER_STEP;
                yield return null;
            }
            _viewCardCanvas.alpha = 0;
            _viewCardCanvas.gameObject.SetActive(false);

        }


        IEnumerator IChangePosition()
        {
            float startWidth = _historyContentParent.rect.width;
            while (startWidth == _historyContentParent.rect.width) yield return null;
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