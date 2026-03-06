
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Manages the rewarded advertisement boost system. Handles ad watching, boost activation, and tracking.
/// Integrates with AdMobManager, RewardManager, CurrencyManager, and others.
/// </summary>
public class RewardedBoostManager : Singleton<RewardedBoostManager>
{
    [Header("Configuration")]
    [Tooltip("All available boosts that can be offered to the player.")]
    public List<BoostRewardData> availableBoosts;
    [Tooltip("Maximum number of boost-related ads a player can watch per day.")]
    public int maxAdsPerDay = 10;

    [Header("Live Data")]
    public List<BoostActivationController> activeBoosts = new List<BoostActivationController>();

    public static event Action OnActiveBoostsChanged;

    private int adsWatchedToday;
    private const string ADS_WATCHED_KEY = "AdsWatchedToday";
    private const string LAST_AD_RESET_KEY = "LastAdReset";

    protected override void Awake()
    {
        base.Awake();
        LoadAdWatchCount();
    }

    private void OnEnable()
    {
        // Assumed integration with AdMobManager or equivalent
        // AdMobManager.OnRewardedAdCompleted += HandleRewardedAdSuccess;
        // GameFlowController.OnRunStarted += OnRunStarted;
        // GameFlowController.OnRunEnded += OnRunEnded;
    }

    private void OnDisable()
    {
        // AdMobManager.OnRewardedAdCompleted -= HandleRewardedAdSuccess;
        // GameFlowController.OnRunStarted -= OnRunStarted;
        // GameFlowController.OnRunEnded -= OnRunEnded;
    }

    private void LoadAdWatchCount()
    {
        DateTime now = DateTime.Now;
        DateTime lastReset = DateTime.Parse(PlayerPrefs.GetString(LAST_AD_RESET_KEY, now.ToString()));
        if (now.Date > lastReset.Date)
        {
            adsWatchedToday = 0;
            PlayerPrefs.SetString(LAST_AD_RESET_KEY, now.ToString());
            PlayerPrefs.SetInt(ADS_WATCHED_KEY, 0);
        }
        else
        {
            adsWatchedToday = PlayerPrefs.GetInt(ADS_WATCHED_KEY, 0);
        }
    }

    public bool CanWatchAd()
    {
        return adsWatchedToday < maxAdsPerDay;
    }

    public void RequestAdForBoost(BoostRewardData boostData)
    {
        if (!CanWatchAd()) return;
        
        // This is where you would call your Ad SDK
        // AdMobManager.Instance.ShowRewardedVideo(boostData);
    }

    private void HandleRewardedAdSuccess(BoostRewardData boostData) // This would be the callback from the ad manager
    {
        adsWatchedToday++;
        PlayerPrefs.SetInt(ADS_WATCHED_KEY, adsWatchedToday);

        ActivateBoost(boostData);
    }

    public void ActivateBoost(BoostRewardData boostData)
    {
        if (boostData.durationType == BoostDurationType.Immediate)
        {
            GrantImmediateReward(boostData);
            return;
        }

        BoostActivationController existingBoost = activeBoosts.FirstOrDefault(b => b.BoostData.boostType == boostData.boostType);
        if (existingBoost != null)
        {
            existingBoost.ExtendDuration(boostData.durationValue);
        }
        else
        {
            activeBoosts.Add(new BoostActivationController(boostData));
        }
        OnActiveBoostsChanged?.Invoke();
    }

    private void GrantImmediateReward(BoostRewardData boostData)
    {
        switch (boostData.boostType)
        {
            case BoostType.BonusChest:
                if (RewardManager.Instance != null && boostData.immediateRewardPrefab != null)
                {
                    RewardManager.Instance.GrantReward(boostData.immediateRewardPrefab);
                }
                break;
            case BoostType.ExtraBonusRun:
                // Assuming BonusRunManager exists as per feature registry
                // BonusRunManager.Instance.GrantBonusRun();
                break;
        }
    }

    private void OnRunStarted()
    {
        // Potentially apply start-of-run effects here
    }

    private void OnRunEnded()
    {
        // Decrement run-based boosts
        List<BoostActivationController> boostsToUpdate = activeBoosts.Where(b => b.BoostData.durationType == BoostDurationType.Runs).ToList();
        foreach (var boost in boostsToUpdate)
        {
            boost.DecrementDuration();
        }

        // Remove expired boosts
        activeBoosts.RemoveAll(b => b.IsExpired());
        OnActiveBoostsChanged?.Invoke();
    }

    public float GetBoostValue(BoostType type)
    {
        float totalValue = 1f; // Default to 1 (no effect)
        var boosts = activeBoosts.Where(b => b.BoostData.boostType == type && !b.IsExpired()).ToList();
        foreach (var boost in boosts)
        {
            totalValue *= boost.BoostData.effectValue;
        }
        
        // Event integration for dynamic boost increase
        // if (EventManager.Instance.IsEventActive("TripleCoinWeekend")){
        //     if(type == BoostType.DoubleCoins) totalValue *= 1.5f; // From 2x to 3x
        // }

        return totalValue;
    }
}
