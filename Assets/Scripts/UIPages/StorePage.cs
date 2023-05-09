using System;
using Analytic.Interfaces;
using Cards.Interfaces;
using Coin.Interfaces;
using IAPurchasing.Interfaces;
using TMPro;
using UIElements;
using UnityEngine;
using Zenject;

namespace UIPages
{
    public class StorePage : MonoBehaviour
    {
        [Header("Store properties"), SerializeField]
        private AnimationFading _warningPopup;

        [SerializeField]
        private TextMeshProUGUI _randomCardPrice;
        
        #region Dependency

        private IStoreEventsAnalyticService _storeEventsAnalyticService;
        private IIAPService _iapService;
        private IAdEventsAnalyticService _adEventsAnalyticService;
        private ICollectionService _collectionService;
        private ICoinService _coinService;

        [Inject]
        private void Construct(
            IStoreEventsAnalyticService storeEventsAnalyticService,
            IIAPService iapService,
            IAdEventsAnalyticService adEventsAnalyticService,
            ICollectionService collectionService,
            ICoinService coinService)
        {
            _storeEventsAnalyticService = storeEventsAnalyticService;
            _iapService = iapService;
            _adEventsAnalyticService = adEventsAnalyticService;
            _collectionService = collectionService;
            _coinService = coinService;
        }

        #endregion

        private void Start()
        {
            _randomCardPrice.text = _coinService.GetCoinPerUnlock().ToString();
        }

        private bool _isWarningPopupShowed
        {
            get => PlayerPrefs.GetInt("IsWarningPopupShowed", 0) == 1;
            set => PlayerPrefs.SetInt("IsWarningPopupShowed", value ? 1 : 0);
        }

        public void ShowWarningPopup()
        {
            if (_isWarningPopupShowed) return;
            _isWarningPopupShowed = true;
            _warningPopup.FadeIn();
        }

        public void BuyBetaBundle()
        {
            _storeEventsAnalyticService.Player_Try_Purchase(_iapService.GetBetatestBundleId());
            _iapService.BuyProductID(_iapService.GetBetatestBundleId(),
                delegate
                {
                    _collectionService.UnlockAllCard();
                    _storeEventsAnalyticService.Player_Bought_Bundle(_iapService.GetBetatestBundleId());
                });
        }

        public void UnlockCardWithRewardAd()
        {
            for (int i = 0; i < 5; i++)
            {
                _collectionService.UnlockRandomCard(false);
            }
        }

        public void FindAdd() => _adEventsAnalyticService.Player_Try_Watch_Add();

        public void WatchedAdd() => _adEventsAnalyticService.Player_Watched_Add();

        public void Player_Open_Store()
        {
            _storeEventsAnalyticService.Player_Open_Store();
        }
    }
}