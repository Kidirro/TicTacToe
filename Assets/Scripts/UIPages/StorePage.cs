using Analytic.Interfaces;
using Cards.Interfaces;
using IAPurchasing.Interfaces;
using UIElements;
using UnityEngine;
using Zenject;

namespace UIPages
{
    public class StorePage : MonoBehaviour
    {
        [Header("Store properties"), SerializeField]
        private AnimationFading _warningPopup;

        #region Dependency

        private IStoreEventsAnalyticService _storeEventsAnalyticService;
        private IIAPService _iapService;
        private IAdEventsAnalyticService _adEventsAnalyticService;
        private ICollectionService _collectionService;

        [Inject]
        private void Construct(IStoreEventsAnalyticService storeEventsAnalyticService, IIAPService iapService,
            IAdEventsAnalyticService adEventsAnalyticService, ICollectionService collectionService)
        {
            _storeEventsAnalyticService = storeEventsAnalyticService;
            _iapService = iapService;
            _adEventsAnalyticService = adEventsAnalyticService;
            _collectionService = collectionService;
        }

        #endregion


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