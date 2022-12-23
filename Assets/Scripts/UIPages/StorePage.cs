using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

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

    public void BuyBetaBundle() => IAPManager.BuyProductID(IAPManager.Instance.BetatestBundle,
        delegate { CollectionManager.Instance.UnlockAllCard(); });

    public void UnlockCardWithRewardAd() =>
        AdsManager.Instance.ShowRewardedAd(delegate
        {
            for (int i = 0; i < 5; i++)
            {
                CollectionManager.Instance.UnlockRandomCard(false);
            }
        });
}