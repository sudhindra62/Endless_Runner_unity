
using UnityEngine;
using System;

/// <summary>
/// Authoritative, consolidated manager for remote configuration.
/// This script is the single source of truth for fetching and providing remote values.
/// It is based on the superior mock implementation from the original 'RemoteConfigBridge' in the Managers folder,
/// which provides essential test data for the Live Event system and includes a necessary OnRemoteConfigUpdated event.
/// All other duplicate or placeholder config files have been deprecated and removed.
/// </summary>
public class RemoteConfigManager : Singleton<RemoteConfigManager>
{
    // This event is crucial for a real implementation, allowing other systems to react to config changes.
    public static event Action OnRemoteConfigUpdated;

    /// <summary>
    /// In a real implementation, this would connect to a service like Firebase Remote Config.
    /// For now, it returns a hardcoded sample JSON for a live event to allow for testing.
    /// </summary>
    public string GetString(string key, string defaultValue)
    {
        if (key == "liveEvent")
        {
            return "{\"eventID\":\"CommunityRun_2024_07\",\"eventName\":\"Community Kilometre Run!\",\"startTime\":\"2024-07-20T00:00:00Z\",\"endTime\":\"2024-07-27T00:00:00Z\",\"eventType\":\"CommunityDistanceChallenge\",\"gameplayModifierList\":[{\"modifierID\":\"DoubleCoins\",\"targetManager\":\"ScoreManager\",\"parameter\":\"coinMultiplier\",\"value\":2.0,\"cap\":2.0}],\"rewardTierList\":[{\"tierID\":\"Bronze\",\"scoreThreshold\":10000,\"rewardIDs\":[\"small_coin_pouch\"]}],\"communityMilestoneTargets\":[{\"milestoneID\":\"TotalDistance_100k\",\"description\":\"Run 100,000km as a community\",\"type\":\"TotalDistanceRun\",\"targetValue\":100000000,\"globalRewardID\":\"24h_XP_Boost\"}],\"isStackableWithRunModifiers\":true,\"isStackableWithBossMode\":false,\"visualThemeID\":\"VibrantCity\",\"musicOverrideID\":\"EventTheme_Upbeat\"}";
        }
        return defaultValue;
    }

    /// <summary>
    /// In a real implementation, this would connect to a service like Firebase Remote Config.
    /// For now, it simulates some progress for the community challenge.
    /// </summary>
    public double GetDouble(string key, double defaultValue)
    {
        if (key == "communityChallenge_currentDistance")
        {
            return 55000000.0;
        }
        return defaultValue;
    }
    
    /// <summary>
    /// Simulates fetching new config and notifying listeners.
    /// In a real implementation, this would be called after a successful fetch from Firebase.
    /// </summary>
    public void FetchAndActivate()
    {
        Debug.Log("Simulating fetch and activation of remote config...");
        // In a real scenario, you would update your local config cache here.
        OnRemoteConfigUpdated?.Invoke();
    }
}
