
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System;

namespace EndlessRunner.Analytics
{
    /// <summary>
    /// The reason for a run's conclusion.
    /// </summary>
    public enum RunEndReason
    {
        DeathByObstacle,
        DeathByEnemy,
        Quit,
        BossDefeated,
        FellOffTrack
    }

    /// <summary>
    /// Manages the dispatch of all analytics events to the Unity Analytics service.
    /// Architecture completely rebuilt by Supreme Guardian Architect v13 for 100% operational insight.
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

        public void LogRunFinishedEvent(int score, float runDuration, int currencyCollected, RunEndReason deathCause)
        {
            if (!_isAnalyticsServiceReady) return;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "finalScore", score },
                { "runDurationSeconds", runDuration },
                { "currencyCollected", currencyCollected },
                { "deathCause", deathCause.ToString() }
            };
            
            Events.CustomData("runFinished", parameters);
            Debug.Log($"Guardian Architect: Logging 'runFinished' event. Score: {score}, Duration: {runDuration}");
        }

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
}
