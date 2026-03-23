
using UnityEngine;

public enum MissionType
{
    Jump,         // Jump a certain number of times
    Slide,        // Slide a certain number of times
    KillEnemies,
    Survive,
    PerformAction,
    ScorePoints,
    UsePowerup,
    CollectCurrency
}

[CreateAssetMenu(fileName = "New Mission", menuName = "Missions/Mission Data")]
public class MissionData : ScriptableObject
{
    public string missionId;
    public string missionName;
    [TextArea] public string missionDescription;
    public MissionType missionType;
    public int goal;
    public int currentProgress;
    public bool isCompleted;

    // --- Property Aliases for Architectural Sync (Folder 2/12) ---
    public string Description => missionDescription;
    public float TargetValue => goal;
    public int RewardAmount => rewardAmount;
    public string description { get => missionDescription; set => missionDescription = value; }

    [Header("Reward Parameters")]
    public string rewardId;
    public int rewardAmount;

    [Header("Rewards")]
    public int rewardXP;
    public int rewardCoins;
    public int rewardGems;
}
