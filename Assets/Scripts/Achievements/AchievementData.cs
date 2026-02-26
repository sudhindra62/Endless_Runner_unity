using UnityEngine;
using System;

/// <summary>
/// Defines the type of reward granted by an achievement or mission.
/// </summary>
[Serializable]
public enum EngagementRewardType
{
    Coins,
    Gems,
    Chest
}

/// <summary>
/// A serializable data container that defines a single achievement.
/// These are used in a list within the AchievementManager.
/// </summary>
[Serializable]
public class AchievementData
{
    public string AchievementID;
    public string Title;
    public string Description;
    public long TargetValue; // Use long for potentially large numbers like total coins

    [Header("Reward")]
    public EngagementRewardType RewardType;
    public int RewardAmount;
}
