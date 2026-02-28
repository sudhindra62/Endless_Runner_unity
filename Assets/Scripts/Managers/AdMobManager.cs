using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdMobManager : MonoBehaviour
{
    public static AdMobManager Instance { get; private set; }
    private bool isInitialized = false;
    private RewardedAd rewardedAd;
    private Action onRewardEarned;

    [Header("Ad Unit IDs")]
    [SerializeField] private string rewardedAdUnitId_production = "ca-app-pub-YOUR_AD_UNIT_ID"; // Replace with your Production Ad Unit ID
    private string rewardedAdUnitId_test = "ca-app-pub-3940256099942544/5224354917";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        if (isInitialized) return;

        MobileAds.Initialize(initStatus =>
        {
            isInitialized = true;
            RequestRewardedAd();
        });
    }

    private void RequestRewardedAd()
    {
        string adUnitId = BuildSettings.Instance.IsProductionBuild ? rewardedAdUnitId_production : rewardedAdUnitId_test;
        this.rewardedAd = new RewardedAd(adUnitId);

        // Subscribe to events
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd(Action onRewardEarnedCallback)
    {
        this.onRewardEarned = onRewardEarnedCallback;
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        if (!BuildSettings.Instance.IsProductionBuild)
            Debug.Log("Rewarded ad opened.");
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        if (!BuildSettings.Instance.IsProductionBuild)
            Debug.Log($"User earned reward: {args.Amount} {args.Type}");
        onRewardEarned?.Invoke();
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if (!BuildSettings.Instance.IsProductionBuild)
            Debug.Log("Rewarded ad closed.");
        // Pre-load the next ad
        this.RequestRewardedAd();
    }
}
