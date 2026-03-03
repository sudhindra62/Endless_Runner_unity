using UnityEngine;
using System;

/// <summary>
/// Placeholder for the Remote Config service. This bridge provides the necessary
/// methods and events for the LiveEventManager to function.
/// In a real implementation, this would connect to a service like Firebase Remote Config.
/// </summary>
public class RemoteConfigBridge : Singleton<RemoteConfigBridge>
{
    public static event Action OnRemoteConfigUpdated;

    // Method to simulate fetching a string from a remote service.
    public string GetString(string key, string defaultValue)
    {
        if (key == "liveEvent")
        {
            // Return a sample JSON for a live event to allow for testing.
            return "{\"eventID\":\"CommunityRun_2024_07\",\"eventName\":\"Community Kilometre Run!\",\"startTime\":\"2024-07-20T00:00:00Z\",\"endTime\":\"2024-07-27T00:00:00Z\",\"eventType\":\"CommunityDistanceChallenge\",\"gameplayModifierList\":[{\"modifierID\":\"DoubleCoins\",\"targetManager\":\"ScoreManager\",\"parameter\":\"coinMultiplier\",\"value\":2.0,\"cap\":2.0}],\"rewardTierList\":[{\"tierID\":\"Bronze\",\"scoreThreshold\":10000,\"rewardIDs\":[\"small_coin_pouch\"]}],\"communityMilestoneTargets\":[{\"milestoneID\":\"TotalDistance_100k\",\"description\":\"Run 100,000km as a community\",\"type\":\"TotalDistanceRun\",\"targetValue\":100000000,\"globalRewardID\":\"24h_XP_Boost\"}],\"isStackableWithRunModifiers\":true,\"isStackableWithBossMode\":false,\"visualThemeID\":\"VibrantCity\",\"musicOverrideID\":\"EventTheme_Upbeat\"}";
        }
        return defaultValue;
    }

    // Method to simulate fetching a double from a remote service.
    public double GetDouble(string key, double defaultValue)
    {
        if (key == "communityChallenge_currentDistance")
        {
            return 55000000.0; // Simulate some progress
        }
        return defaultValue;
    }
}
