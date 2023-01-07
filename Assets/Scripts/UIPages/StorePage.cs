using Managers;
using UnityEngine;

namespace UIPages
{
    public class StorePage : MonoBehaviour
    {
        [Header("Store properties"), SerializeField]
        private AnimationFading _warningPopup;

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
            AnalitycManager.Player_Try_Purchase(IAPManager.Instance.BetatestBundle);
            IAPManager.BuyProductID(IAPManager.Instance.BetatestBundle,
                delegate
                {
                    CollectionManager.Instance.UnlockAllCard();
                    AnalitycManager.Player_Bought_Bundle(IAPManager.Instance.BetatestBundle);
                });
        }

        public void UnlockCardWithRewardAd()
        {
            for (int i = 0; i < 5; i++)
            {
                CollectionManager.Instance.UnlockRandomCard(false);
            }
        }

        public void FindAdd() => AnalitycManager.Player_Try_Watch_Add();
        
        public void WatchedAdd() => AnalitycManager.Player_Watched_Add();

        public void Player_Open_Store()
        {
            AnalitycManager.Player_Open_Store();
        }
    }
}