
using UnityEngine;
using System;

/// <summary>
/// Tracks player session data relevant to ad scheduling, such as session duration and runs completed.
/// This data is used by the SmartAdScheduler to make intelligent decisions.
/// </summary>
public class AdSessionTracker : Singleton<AdSessionTracker>
{
    private DateTime sessionStartTime;
    private int runsThisSession = 0;
    private int rewardedAdsWatchedThisSession = 0;

    private const string RUNS_SINCE_LAST_AD_KEY = "RunsSinceLastAd";
    public int RunsSinceLastInterstitial { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        sessionStartTime = DateTime.UtcNow;
        LoadRunCount();
    }

    private void OnEnable()
    {
        // GameFlowController.OnRunEnded += IncrementRunCount;
        // AdMobManager.OnRewardedAdCompleted += HandleRewardedAdView;
    }

    private void OnDisable()
    {
        // GameFlowController.OnRunEnded -= IncrementRunCount;
        // AdMobManager.OnRewardedAdCompleted -= HandleRewardedAdView;
    }

    private void LoadRunCount()
    {
        RunsSinceLastInterstitial = PlayerPrefs.GetInt(RUNS_SINCE_LAST_AD_KEY, 0);
    }

    private void IncrementRunCount()
    {
        runsThisSession++;
        RunsSinceLastInterstitial++;
        PlayerPrefs.SetInt(RUNS_SINCE_LAST_AD_KEY, RunsSinceLastInterstitial);
    }

    public void ResetRunCount()
    {
        RunsSinceLastInterstitial = 0;
        PlayerPrefs.SetInt(RUNS_SINCE_LAST_AD_KEY, 0);
    }

    private void HandleRewardedAdView(object adData) // Assuming ad manager passes some data
    {
        rewardedAdsWatchedThisSession++;
    }

    public float GetSessionLengthMinutes()
    {
        return (float)(DateTime.UtcNow - sessionStartTime).TotalMinutes;
    }

    public int GetRewardedAdsWatched()
    {
        return rewardedAdsWatchedThisSession;
    }
}
