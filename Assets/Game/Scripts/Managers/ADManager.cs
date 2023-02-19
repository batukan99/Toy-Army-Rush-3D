using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;
using UnityEngine;
using UnityEngine.Events;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

namespace Game.Managers
{
    public class ADManager : MonoBehaviour, IProvidable
    {
        [SerializeField] private bool UseRealAds = false;
        [SerializeField] public int InterstitialAdFrequency = 2;
        
        #if UNITY_ANDROID
        // These ad units are configured to always serve test ads.
        private string _rewardedTestUnitId = "ca-app-pub-3940256099942544/5224354917";
        private string _rewardedRealUnitId = "?";
        private string _rewardedAdUnitId => UseRealAds ? _rewardedRealUnitId: _rewardedTestUnitId;
        private string _interTestUnitId = "ca-app-pub-3940256099942544/1033173712";
        private string _interRealUnitId = "?";
        private string _interAdUnitId => UseRealAds ? _interRealUnitId: _interTestUnitId;
        #elif UNITY_IPHONE
        private string _rewardedTestUnitId = "ca-app-pub-3940256099942544/1712485313";
        private string _rewardedRealUnitId = "?";
        private string _rewardedAdUnitId => UseRealAds ? _rewardedRealUnitId: _rewardedTestUnitId;

        private string _interTestUnitId = "ca-app-pub-3940256099942544/4411468910";
        private string _interRealUnitId = "?";
        private string _interAdUnitId => UseRealAds ? _interRealUnitId: _interTestUnitId;
        #else
        private string _rewardedAdUnitId = "unused";
        #endif

        private RewardedAd rewardedAd;
        private InterstitialAd interstitialAd;
        private bool hasRewardEarned;

        public void Start()
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
                LoadRewardedAd();
                LoadInterstitialAd();
            });
        }

        public void LoadInterstitialAd()
        {
            // Clean up the old ad before loading a new one.
            if (interstitialAd != null)
            {
                    interstitialAd.Destroy();
                    interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder()
                    .AddKeyword("unity-admob-sample")
                    .Build();

            // send the request to load the ad.
            InterstitialAd.Load(_interAdUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                        "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                                + ad.GetResponseInfo());

                    interstitialAd = ad;
                });
        }

        public void ShowInterstitialAd()
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }
        private void RegisterInterEventHandlers(InterstitialAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Interstitial ad full screen content opened.");
            };
        }
        private void RegisterInterReloadHandler(InterstitialAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                            "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
        }

        public void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            hasRewardEarned = false;
            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();

            // send the request to load the ad.
            RewardedAd.Load(_rewardedAdUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                        "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                                + ad.GetResponseInfo());

                    rewardedAd = ad;
                    RegisterRewardEventHandlers(ad);
                    RegisterRewardReloadHandler(ad);
                });
        }
        public void ShowRewardedAd()
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    // TODO: Reward the user.
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }
        private void RegisterRewardEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                hasRewardEarned = true;
                print("OnAdPaid: " +hasRewardEarned);
                Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };
        }

        private void RegisterRewardReloadHandler(RewardedAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                NotifyRewardedEvent();
                Debug.Log("Rewarded Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                            "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };
        }

        private void NotifyRewardedEvent() 
        {
            print("notify: " +hasRewardEarned);
            EventBase.NotifyListeners(EventType.AdRewarded, hasRewardEarned);
        }



        private void OnGameOverEvent(bool status)
        {
            if (status)
            {

            }
        }

        private void OnEnable()
        {
            ManagerProvider.Register(this);
            EventBase.StartListening(EventType.GameOver, (UnityAction<bool>) OnGameOverEvent);
        }
        private void OnDisable()
        {
            EventBase.StopListening(EventType.GameOver, (UnityAction<bool>) OnGameOverEvent);
        }
    }

}
