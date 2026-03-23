using System;
using System.Collections.Generic;

/// <summary>
/// Defines the rewards for a specific league tier for both weekly and seasonal cadences.
/// </summary>
[Serializable]
public class LeagueTierRewardConfig
{
    public string TierName;
    public int TierLevel;

    // Weekly rewards are granted at the end of each competitive week.
    public List<string> WeeklyRewardIDs; // IDs for RewardManager

    // Seasonal rewards are granted at the end of the entire season.
    public List<string> SeasonalRewardIDs; // IDs for RewardManager, e.g., exclusive skins/trails

    public string SeasonalExclusiveBadgeFrameID; // An ID for a cosmetic frame
}
