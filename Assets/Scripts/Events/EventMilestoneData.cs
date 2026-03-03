using System;

/// <summary>
/// Defines the structure for tracking community-wide progress towards a collective goal.
/// </summary>
[Serializable]
public class EventMilestoneData
{
    public string milestoneID;
    public string description;
    public MilestoneType type;
    public double targetValue;
    public string globalRewardID; // e.g., XP Boost for all players for 24h
}

public enum MilestoneType
{
    TotalDistanceRun,
    TotalBossDefeats,
    TotalNearMisses,
    TotalCoinsCollected
}
