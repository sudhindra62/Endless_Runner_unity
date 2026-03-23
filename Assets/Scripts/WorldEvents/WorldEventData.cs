
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldEventData
{
    public string eventId;
    public string eventName;
    public DateTime startTime;
    public DateTime endTime;
    public List<WorldEventType> modifierTypes;
    public RewardBoost rewardBoost;
    public string visualThemeId;
    public int riskTier;
    public bool isStackableWithRunModifier;
    public bool IsLaneBased;
    public bool IsActive;
}

[Serializable]
public class RewardBoost
{
    public float coinMultiplier = 1f;
    public float xpMultiplier = 1f;
    public float leaguePointsMultiplier = 1f;
    public float rareDropChanceBonus = 0f;
}

public enum WorldEventType
{
    DoubleCoins,
    DoubleGems,
    GravityShift,
    BossRush,
    SpeedFestival,
    RareDropBoost,
    ZombieMode,
    DarkVisionMode,
    ComboFestival,
    NoPowerUpsChallenge
}
