using System;
using System.Collections.Generic;

/// <summary>
/// Defines a single tier of rewards within a Live Event, based on player performance (e.g., score).
/// </summary>
[Serializable]
public class EventRewardTier
{
    public string tierID;
    public long scoreThreshold;
    public List<string> rewardIDs; // List of reward IDs managed by RewardManager
    public string exclusiveBadgeID;
    public string leaderboardBorderID;
}
