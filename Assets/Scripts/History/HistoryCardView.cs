using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using History.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace History
{
    public class HistoryCardView : MonoBehaviour, IHistoryViewService
    {
        private DateTime _startTimeTap = DateTime.MinValue;
        private const float TIME_TAP_VIEW = 0.5f;
        private const float ALPHA_PER_STEP = 0.05f;

        private IEnumerator _fadeInCoroutine;

        [Header("Card view"), SerializeField]
        private CanvasGroup _viewCardCanvas;

        [SerializeField]
        private TextMeshProUGUI _viewManapoints;

        [SerializeField]
        private TextMeshProUGUI _viewCardDescription;

        [SerializeField]
        private Image _viewCardImage;

        [SerializeField]
        private List<GameObject> _viewBonusImageList = new();


        public void StartTap(CardInfo cardCollection, PlayerInfo player)
        {
            _startTimeTap = DateTime.Now;
            _fadeInCoroutine = IStartTap(cardCollection, player);
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

        private void UpdateCardViewImage(CardInfo info, PlayerInfo player)
        {
            string desc = "";
            desc = I2.Loc.LocalizationManager.TryGetTranslation(info.CardDescription, out desc)
                ? I2.Loc.LocalizationManager.GetTranslation(info.CardDescription)
                : info.CardDescription;
            _viewCardDescription.text = desc;
            _viewCardImage.sprite = (player.SideId == 1) ? info.CardImageP1 : info.CardImageP2;
            _viewManapoints.text = info.CardManacost.ToString();
            for (int i = 0; i < _viewBonusImageList.Count; i++)
            {
                _viewBonusImageList[i].SetActive(i == (int) info.CardBonus);
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
    }
}