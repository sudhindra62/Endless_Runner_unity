
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System;

/// <summary>
/// The reason for a run's conclusion.
/// </summary>
public enum RunEndReason
{
    DeathByObstacle,
    DeathByEnemy,
    Quit,
    BossDefeated
}

/// <summary>
/// Manages the dispatch of all analytics events to the Unity Analytics service.
/// Architecture completely rebuilt by Supreme Guardian Architect v12 for 100% operational insight.
/// This is the central nervous system of the project's data pipeline. NO STUB LOGIC.
/// </summary>
public class AnalyticsManager : Singleton<AnalyticsManager>
{
    private bool _isAnalyticsServiceReady = false;

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            _isAnalyticsServiceReady = true;
            Debug.Log("Guardian Architect: Unity Analytics service initialized and ready.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Guardian Architect FATAL_ERROR: Failed to initialize Unity Analytics. Reason: {e.Message}");
        }
    }

    // --- CORE GAMEPLAY EVENTS ---

    /// <summary>
    /// Logs the conclusion of a player's run.
    /// </summary>
    /// <param name="score">The final score achieved.</param>
    /// <param name="runDuration">Total duration of the run in seconds.</param>
    /// <param name="currencyCollected">Total amount of standard currency collected.</param>
    /// <param name="deathCause">The reason for the run's end.</param>
    public void LogRunFinishedEvent(int score, float runDuration, int currencyCollected, RunEndReason deathCause)
    {
        if (!_isAnalyticsServiceReady) return;

        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "finalScore", score },
            { "runDurationSeconds", runDuration },
            { "currencyCollected", currencyCollected },
            { "deathCause", deathCause.ToString() } // Enum converted to string for analytics
        };
        
        Events.CustomData("runFinished", parameters);
        Debug.Log($"Guardian Architect: Logging 'runFinished' event. Score: {score}, Duration: {runDuration}");
    }

    /// <summary>
    /// Logs when a power-up is activated by the player.
    /// </summary>
    /// <param name="powerUpType">The type of power-up used.</param>
    public void LogPowerUpActivatedEvent(string powerUpType)
    {
        if (!_isAnalyticsServiceReady) return;
        
        Events.CustomData("powerUpActivated", new Dictionary<string, object>
        {
            { "powerUpType", powerUpType }
        });
        Debug.Log($"Guardian Architect: Logging 'powerUpActivated' event. Type: {powerUpType}");
    }
    
    // --- MONETIZATION EVENTS ---

    /// <summary>
    /// Logs the start of an advertisement.
    /// This is crucial for tracking ad pressure and frequency.
    /// </summary>
    /// <param name="adType">The type of ad shown (e.g., "Interstitial", "Rewarded").</param>
    /// <param name="placementName">The specific placement ID for the ad.</param>
    public void LogAdStartedEvent(string adType, string placementName)
    {
        if (!_isAnalyticsServiceReady) return;

        Events.CustomData("adStarted", new Dictionary<string, object>
        {
            { "adType", adType },
            { "adPlacement", placementName }
        });
        Debug.Log($"Guardian Architect: Logging 'adStarted' event. Type: {adType}, Placement: {placementName}");
    }
    
    /// <summary>
    /// Logs the completion of a rewarded advertisement.
    /// </summary>
    /// <param name="placementName">The specific placement ID for the ad.</param>
    /// <param name="rewardType">The type of reward granted (e.g., "Revive", "BonusCoins").</param>
    public void LogRewardedAdCompletedEvent(string placementName, string rewardType)
    {
        if (!_isAnalyticsServiceReady) return;

        Events.CustomData("rewardedAdCompleted", new Dictionary<string, object>
        {
            { "adPlacement", placementName },
            { "rewardType", rewardType }
        });
        Debug.Log($"Guardian Architect: Logging 'rewardedAdCompleted' event. Placement: {placementName}, Reward: {rewardType}");
    }

    /// <summary>
    /// Logs a successful In-App Purchase.
    /// </summary>
    /// <param name="productId">The unique identifier of the purchased product.</param>
    /// <param name="price">The cost of the product.</param>
    /// <param name="currencyCode">The ISO currency code (e.g., "USD").</param>
    public void LogIapPurchaseEvent(string productId, float price, string currencyCode)
    {
        if (!_isAnalyticsServiceReady) return;

        Events.CustomData("iapPurchase", new Dictionary<string, object>
        {
            { "productId", productId },
            { "price", price },
            { "currencyCode", currencyCode }
        });
        Debug.Log($"Guardian Architect: Logging 'iapPurchase' event. Product: {productId}, Price: {price} {currencyCode}");
    }

    // --- PLAYER PROGRESSION & BEHAVIOR ---

    /// <summary>
    /// Logs the unlocking of a cosmetic item.
    /// </summary>
    /// <param name="itemId">The ID of the unlocked cosmetic.</param>
    /// <param name="unlockMethod">How the item was unlocked (e.g., "Purchased", "QuestReward", "Gacha").</param>
    public void LogCosmeticUnlockedEvent(string itemId, string unlockMethod)
    {
        if (!_isAnalyticsServiceReady) return;

        Events.CustomData("cosmeticUnlocked", new Dictionary<string, object>
        {
            { "itemId", itemId },
            { "unlockMethod", unlockMethod }
        });
        Debug.Log($"Guardian Architect: Logging 'cosmeticUnlocked' event. Item: {itemId}, Method: {unlockMethod}");
    }
}
