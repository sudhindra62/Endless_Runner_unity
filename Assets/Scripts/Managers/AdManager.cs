
using System;
using UnityEngine;
using GoogleMobileAds.Api;

/// <summary>
/// Authoritative singleton for managing all ad interactions, including rewarded and interstitial ads.
/// It ensures that ads are shown correctly, handles ad callbacks, and prevents duplicate reward granting.
/// Rewards are always granted through the RewardManager to maintain a single, secure reward pathway.
/// </summary>
public class AdManager : Singleton<AdManager>
{
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;

    private const string RewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917"; // Test Ad ID
    private const string InterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test Ad ID

    private bool _isRewardedAdLoaded = false;
    private bool _isInterstitialAdLoaded = false;

    private Action _onRewardedAdCompleted;

    protected override void Awake()
    {
        base.Awake();
        InitializeAds();
    }

    private void InitializeAds()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads initialized.");
            LoadRewardedAd();
            LoadInterstitialAd();
        });
    }

    private void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        var adRequest = new AdRequest();
        RewardedAd.Load(RewardedAdUnitId, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load with error: " + error.GetMessage());
                _isRewardedAdLoaded = false;
                return;
            }

            _rewardedAd = ad;
            _isRewardedAdLoaded = true;
            Debug.Log("Rewarded ad loaded successfully.");
        });
    }

    private void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var adRequest = new AdRequest();
        InterstitialAd.Load(InterstitialAdUnitId, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load with error: " + error.GetMessage());
                _isInterstitialAdLoaded = false;
                return;
            }

            _interstitialAd = ad;
            _isInterstitialAdLoaded = true;
            Debug.Log("Interstitial ad loaded successfully.");
        });
    }

    public void ShowRewardedAd(Action onRewardedAdCompleted)
    {
        if (!_isRewardedAdLoaded)
        {
            Debug.LogWarning("Rewarded ad is not ready yet.");
            return;
        }

        _onRewardedAdCompleted = onRewardedAdCompleted;

        _rewardedAd.OnAdFullScreenContentClosed += OnRewardedAdClosed;
        _rewardedAd.Show(reward =>
        {
            // The reward is granted here, but the callback is invoked in OnRewardedAdClosed to ensure proper flow.
            Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
        });
    }

    private void OnRewardedAdClosed()
    {
        _rewardedAd.OnAdFullScreenContentClosed -= OnRewardedAdClosed;
        _onRewardedAdCompleted?.Invoke();
        _onRewardedAdCompleted = null;
        LoadRewardedAd(); // Pre-load the next ad
    }

    public void ShowInterstitialAd()
    {
        if (!_isInterstitialAdLoaded)
        {
            Debug.LogWarning("Interstitial ad is not ready yet.");
            return;
        }

        _interstitialAd.OnAdFullScreenContentClosed += OnInterstitialAdClosed;
        _interstitialAd.Show();
    }

    private void OnInterstitialAdClosed()
    {
        _interstitialAd.OnAdFullScreenContentClosed -= OnInterstitialAdClosed;
        LoadInterstitialAd(); // Pre-load the next ad
    }
}
