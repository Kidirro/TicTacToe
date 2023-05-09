using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.CustomType;
using History.Interfaces;
using TMPro;
using UIElements;
using UnityEngine;
using UnityEngine.UI;

namespace History
{
    public class HistoryCardView : CardViewBase, IHistoryViewService
    {
        private DateTime _startTimeTap = DateTime.MinValue;
        private const float TIME_TAP_VIEW = 0.5f;
        private const float ALPHA_PER_STEP = 0.05f;

        private IEnumerator _fadeInCoroutine;
        
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