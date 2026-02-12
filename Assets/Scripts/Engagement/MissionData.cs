using System;
using UnityEngine;

/// <summary>
/// A serializable data class that defines the structure and state of a single mission.
/// Plain data container – no runtime logic.
/// </summary>
[Serializable]
public partial class MissionData
{
    // =======================
    // CORE STORED DATA
    // =======================

    public string MissionID;
    public string Description;
    public MissionType MissionType;
    public int TargetValue;
    public int CurrentProgress;
    public bool IsCompleted;
    public bool IsClaimed;

    [Header("Reward")]
    public int RewardCoins;
    public int RewardGems;

    // =======================
    // 🔹 COMPATIBILITY ALIASES
    // =======================

    public MissionType type => MissionType;
    public int goal => TargetValue;

    public MissionRewardType rewardType =>
        RewardCoins > 0 ? MissionRewardType.Coins : MissionRewardType.Gems;

    public int rewardAmount =>
        RewardCoins > 0 ? RewardCoins : RewardGems;

    public string description => Description;
    public string missionId => MissionID;

    // =======================
    // CONSTRUCTORS
    // =======================

    public MissionData(
        string id,
        string desc,
        MissionType missionType,
        int target,
        int coins,
        int gems)
    {
        MissionID = id;
        Description = desc;
        MissionType = missionType;
        TargetValue = target;
        CurrentProgress = 0;
        RewardCoins = coins;
        RewardGems = gems;
        IsCompleted = false;
        IsClaimed = false;
    }

    // Required for serialization
    public MissionData() { }
}
