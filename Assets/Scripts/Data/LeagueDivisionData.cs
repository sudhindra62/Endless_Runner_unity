using System;
using System.Collections.Generic;

/// <summary>
/// Data structure for a specific league division and its scoring thresholds.
/// Global scope.
/// </summary>
[Serializable]
public class LeagueDivisionData
{
    public string TierName;         // e.g., "Gold"
    public int TierLevel;           // e.g., 2 for "Gold II"
    public int Division;            // e.g., 1, 2, 3
    public long EntryScore;         // Minimum score to be in this division
    public long PromotionScore;     // Score needed to enter promotion zone
    public long DemotionScore;      // Score below which enters demotion zone
    public long PromotionThreshold; // Top X% are promoted
    public long DemotionThreshold;  // Bottom Y% are demoted
    public LeagueRewardData WeeklyReward;
}

[Serializable]
public class LeagueRewardData
{
    public string RewardId;
    public List<CurrencyReward> Currencies;
    public string ItemId; 
}

[Serializable]
public struct CurrencyReward
{
    public string CurrencyType; 
    public int Amount;
}

[Serializable]
public class LeagueTierDefinition
{
    public List<LeagueDivisionData> Divisions;
}
