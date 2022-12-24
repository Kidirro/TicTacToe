using System;
using System.Collections;
using System.Collections.Generic;
//using GoogleMobileAds.Api;
using UnityEngine;
using Yodo1.MAS;

namespace Managers
{
    public class AdsManager : Singleton<AdsManager>
    {
        private const string ADUnitRewardCheep = "ca-app-pub-8340576279106634/2054300816";
       
        private void Start()
        {
            Yodo1U3dMas.SetCOPPA(false);
            Yodo1U3dMas.SetGDPR(true);
            Yodo1U3dMas.SetCCPA(false);
            Yodo1U3dMas.InitializeMasSdk();

            Yodo1U3dMasCallback.OnSdkInitializedEvent += (success, error) =>
            {
                Debug.Log("[Yodo1 Mas] OnSdkInitializedEvent, success:" + success + ", error: " + error.ToString());
                Debug.Log(success
                    ? "[Yodo1 Mas] The initialization has succeeded"
                    : "[Yodo1 Mas] The initialization has failed");
            };

            InitializeRewardedAds();
            Debug.Log("Ads init successful");
        }

        #region Rewarded

   
        private void InitializeRewardedAds()
        {
            // Instantiate
            Yodo1U3dRewardAd.GetInstance();

            // Ad Events
            Yodo1U3dRewardAd.GetInstance().OnAdLoadedEvent += OnRewardAdLoadedEvent;
            Yodo1U3dRewardAd.GetInstance().OnAdLoadFailedEvent += OnRewardAdLoadFailedEvent;
            Yodo1U3dRewardAd.GetInstance().OnAdOpenedEvent += OnRewardAdOpenedEvent;
            Yodo1U3dRewardAd.GetInstance().OnAdOpenFailedEvent += OnRewardAdOpenFailedEvent;
            Yodo1U3dRewardAd.GetInstance().OnAdClosedEvent += OnRewardAdClosedEvent;
            Yodo1U3dRewardAd.GetInstance().OnAdEarnedEvent += OnRewardAdEarnedEvent;
        }

        private void OnRewardAdLoadedEvent(Yodo1U3dRewardAd ad)
        {
            Debug.Log("[Yodo1 Mas] OnRewardAdLoadedEvent event received");
        }

        private void OnRewardAdLoadFailedEvent(Yodo1U3dRewardAd ad, Yodo1U3dAdError adError)
        {
            Debug.Log("[Yodo1 Mas] OnRewardAdLoadFailedEvent event received with error: " + adError.ToString());
        }

        private void OnRewardAdOpenedEvent(Yodo1U3dRewardAd ad)
        {
            Debug.Log("[Yodo1 Mas] OnRewardAdOpenedEvent event received");
        }

        private void OnRewardAdOpenFailedEvent(Yodo1U3dRewardAd ad, Yodo1U3dAdError adError)
        {
            Debug.Log("[Yodo1 Mas] OnRewardAdOpenFailedEvent event received with error: " + adError.ToString());
            // Load the next ad
            Yodo1U3dRewardAd.GetInstance().LoadAd();
        }

        private void OnRewardAdClosedEvent(Yodo1U3dRewardAd ad)
        {
            Debug.Log("[Yodo1 Mas] OnRewardAdClosedEvent event received");
            // Load the next ad
            Yodo1U3dRewardAd.GetInstance().LoadAd();
        }

        private void OnRewardAdEarnedEvent(Yodo1U3dRewardAd ad)
        {
            Debug.Log("[Yodo1 Mas] OnRewardAdEarnedEvent event received");
            // Add your reward code here
        }
        
        #endregion
        
        

        //private IEnumerator TryToLoadRewardVideo(Action action = null)
        //{
        //    if (_rewardLoadingScreen == null) { _rewardLoadingScreen = Instantiate(_rewardLoadingScreenPrefab, GameObject.FindGameObjectWithTag("Boards").transform); }
        //    else
        //    {
        //        _rewardLoadingScreen.SetActive(true);
        //    }
        //    for (int i = 0; i < 5; i++)
        //    {
        //        if (_rewardedAd.IsLoaded())
        //        {
        //            _rewardedAd.Show();
        //            _actionEarn = action;
        //            _rewardLoadingScreen.SetActive(false);
        //            _load = null;
        //            yield break;
        //        }
        //        yield return new WaitForSeconds(0.5f);
        //    }
        //    _rewardLoadingScreen.SetActive(false);
        //    _load = null;
        //    if (_error == null) { _error = Instantiate(_errorPrefab, GameObject.FindGameObjectWithTag("Boards").transform); }
        //    else
        //    {
        //        _error.SetActive(true);
        //    }
        //    yield return new WaitForSeconds(2f);
        //    _error.SetActive(false);
        //}
    }
}