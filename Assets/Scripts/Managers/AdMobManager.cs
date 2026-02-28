
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdMobManager : MonoBehaviour
{
    private bool isInitialized = false;
    private RewardedAd rewardedAd;
    private BuildSettings buildSettings;

    [Header("Ad Unit IDs")]
    // TODO: Add your production Ad Unit ID here
    private string rewardedAdUnitId_production = "";
    private string rewardedAdUnitId_test = "ca-app-pub-3940256099942544/5224354917";

    void Awake()
    {
        ServiceLocator.Register<AdMobManager>(this);
    }

    void Start()
    {
        buildSettings = ServiceLocator.Get<BuildSettings>();
        if (buildSettings == null)
        {
            Debug.LogError("BuildSettings not found in ServiceLocator. AdMobManager requires it for initialization.");
            return;
        }

        Initialize();
    }

    void OnDestroy()
    {
        ServiceLocator.Unregister<AdMobManager>();
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }

    private void Initialize()
    {
        if (isInitialized || !buildSettings.AdsEnabled) return;

        MobileAds.Initialize(initStatus =>
        {
            isInitialized = true;
            LoadRewardedAd();
        });
    }

    private void LoadRewardedAd()
    {
        if (this.rewardedAd != null)
        {
            this.rewardedAd.Destroy();
            this.rewardedAd = null;
        }

        string adUnitId = buildSettings.IsProductionBuild ? rewardedAdUnitId_production : rewardedAdUnitId_test;
        var adRequest = new AdRequest.Builder().Build();

        Debug.Log("Loading rewarded ad...");
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                this.rewardedAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    public void ShowRewardedAd(Action onRewardEarnedCallback)
    {
        if (!buildSettings.AdsEnabled) 
        {
            Debug.LogWarning("Ads are disabled. Cannot show rewarded ad.");
            onRewardEarnedCallback?.Invoke(); // still call callback to not stall game flow
            return;
        }
        
        if (this.rewardedAd != null && this.rewardedAd.CanShowAd())
        {
            this.rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
                onRewardEarnedCallback?.Invoke();
            });
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready yet. Simulating reward for seamless gameplay.");
            onRewardEarnedCallback?.Invoke(); // still call callback to not stall game flow
            LoadRewardedAd(); // Attempt to load a new ad for next time
        }
    }
    
    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
            LoadRewardedAd();
        };
    }
}
