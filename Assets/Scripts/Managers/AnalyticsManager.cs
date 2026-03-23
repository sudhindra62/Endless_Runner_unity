using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System;
using EndlessRunner.Events;

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
/// Manages the dispatch of all analytics events.
/// Global scope Singleton for project-wide accessibility.
/// Integrated with Unity Services.
/// </summary>
public class AnalyticsManager : Singleton<AnalyticsManager>
{
    private bool _isAnalyticsServiceReady = false;

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            _isAnalyticsServiceReady = true;
            Debug.Log("[AnalyticsManager] Unity Analytics service initialized.");
        }
        catch (Exception e)
        {
            Debug.LogError($"[AnalyticsManager] Failed to initialize Unity Analytics: {e.Message}");
        }
    }

    public void TrackEvent(string eventName, Dictionary<string, object> parameters)
    {
        if (!_isAnalyticsServiceReady) return;
        AnalyticsService.Instance.CustomData(eventName, parameters);
        Debug.Log($"[AnalyticsManager] Logging event: {eventName}");
    }

    public void LogRunFinishedEvent(int score, float runDuration, int currencyCollected, RunEndReason deathCause)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "finalScore", score },
            { "runDurationSeconds", runDuration },
            { "currencyCollected", currencyCollected },
            { "deathCause", deathCause.ToString() }
        };
        TrackEvent("runFinished", parameters);
    }

    public void LogPowerUpActivatedEvent(string powerUpType)
    {
        TrackEvent("powerUpActivated", new Dictionary<string, object> { { "powerUpType", powerUpType } });
    }

    public void LogAdStartedEvent(string adType, string placementName)
    {
        TrackEvent("adStarted", new Dictionary<string, object> { { "adType", adType }, { "adPlacement", placementName } });
    }

    public void TrackThemeSelectedEvent(string themeName)
    {
        TrackEvent("theme_selected", new Dictionary<string, object> { { "theme_name", themeName } });
    }

    public void TrackObstacleHitEvent(string obstacleType, float playerSpeed)
    {
        TrackEvent("obstacle_hit", new Dictionary<string, object> { { "obstacle_type", obstacleType }, { "player_speed", playerSpeed } });
    }

    public void LogEvent(string eventName, Dictionary<string, object> parameters) => TrackEvent(eventName, parameters);

    public void LogPurchase(string itemID, int price)
    {
        TrackEvent("item_purchased", new Dictionary<string, object> { { "item_id", itemID }, { "price", price } });
    }

    public void LogLevelStart(int level)
    {
        TrackEvent("level_start", new Dictionary<string, object> { { "level", level } });
    }

    public void LogLevelEnd(int level, bool success)
    {
        TrackEvent("level_end", new Dictionary<string, object> { { "level", level }, { "success", success } });
    }

    public void SetUserProperty(string key, string value)
    {
        if (!_isAnalyticsServiceReady) return;
        // Placeholder: would set user property via analytics service
        Debug.Log($"[AnalyticsManager] Setting user property: {key} = {value}");
    }
}
