using System;

/// <summary>
/// Defines the structure for tracking community-wide progress towards a collective goal.
/// </summary>
[Serializable]
public class EventMilestoneData
{
    public string MilestoneID;
    public string Description;
    public MilestoneType type;
    public double targetValue;
    public float CurrentGlobalValue;
    public bool IsCompleted;
    public string globalRewardID; // e.g., XP Boost for all players for 24h
}
