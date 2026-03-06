
using System;
using System.Collections.Generic;

[Serializable]
public class LiveEventData
{
    public string EventID;
    public string EventName;
    public ulong StartTimeUTC;
    public ulong EndTimeUTC;
    public EventType Type;
    public List<GameplayModifier> GameplayModifiers;
    public List<EventRewardTier> RewardTiers;
    public List<EventMilestoneData> CommunityMilestones;
    public bool IsStackableWithRunModifiers;
    public bool IsStackableWithBossMode;
    public string VisualThemeID;
    public string MusicOverrideID;
}

[Serializable]
public enum EventType
{
    DoubleCoinFestival,
    DoubleGemStorm,
    GravityShiftWeek,
    BossRushEvent,
    CommunityDistanceChallenge,
    RareDropBoostWeek,
    NoShieldHardcoreWeek,
    SpeedMania,
    ComboFestival,
    ObstacleInvasion
}

[Serializable]
public class GameplayModifier
{
    public string TargetManager; // e.g., "DifficultyManager", "ScoreManager"
    public string ModifierField; // e.g., "DifficultyMultiplier"
    public float Value;
}

[Serializable]
public class EventRewardTier
{
    public string RewardID;
    public string Description;
    public int Requirement; // e.g., score, distance
    public bool IsClaimed;
}

[Serializable]
public class EventMilestoneData
{
    public string MilestoneID;
    public string Description;
    public float TargetValue;
    public float CurrentGlobalValue;
    public bool IsCompleted;
}
