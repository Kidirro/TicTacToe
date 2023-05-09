using System.Collections.Generic;
using Cards.CustomType;
using UIElements.Interfaces;
using UnityEngine;

namespace UIElements
{
    public class BuyingAnimationController : CardViewBase, IBuyingAnimationController
    {
        [SerializeField]
        private Animator _animator;

        private bool _fadeInOver;
        private static readonly int nextState = Animator.StringToHash("NextState");
        private static readonly int resetState = Animator.StringToHash("Reset");
        private static readonly int idleState = Animator.StringToHash("Base Layer.CardBuyingAnimationIdle");

        private List<CardInfo> _cardInfos = new List<CardInfo>();
        
        public void OnFadeOutOver()
        {
            if (_cardInfos.Count > 0)
            {
                _animator.SetTrigger(resetState);
                StartAnimation(_cardInfos[0]);
            }
            else
            {
                _animator.gameObject.SetActive(false);
                _fadeInOver = false;
            }
        }

        public void OnFadeInOver()
        {
            _fadeInOver = true;
        }

        public void OnBgClick()
        {
            if (_fadeInOver)
                _animator.SetTrigger(nextState);
            else
            {
                _animator.Play(idleState);
                _fadeInOver = true;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnBgClick();
            }
        }

        private void StartAnimation(CardInfo cardInfo)
        {
            _cardInfos.Remove(cardInfo);
            UpdateCardViewImage(cardInfo, new PlayerInfo());
            _fadeInOver = false;
            _animator.gameObject.SetActive(true);
        }

        public void ShowBuyingAnimationWithReset(List<CardInfo> cardInfos)
        {
            _cardInfos = cardInfos;
            StartAnimation(_cardInfos[0]);
        }
        
        public void ShowBuyingAnimation(CardInfo cardInfos)
        {
            _cardInfos.Add(cardInfos);
            if (!_animator.gameObject.activeSelf) StartAnimation(cardInfos);
        }

    }
}