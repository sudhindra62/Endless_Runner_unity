using UnityEngine;
using System;

/// <summary>
/// A centralized data model for Missions.
/// Includes both definition and progress tracking for the active session.
/// Global scope.
/// </summary>
[Serializable]
public class Mission
{
    public string missionId;
    public string description;
    public MissionType type;
    public float currentProgress;
    public bool isCompleted;
    public int score; // Mission-based score tracking
    public float targetValue; // ADDED: Missing field causing CS0103

    // --- Property Aliases for Architectural Sync (Folder 2) ---
    public string Description => description;
    public float TargetValue => targetValue;

    [Header("Rewards")]
    public int coinReward;
    public int gemReward;
    public int xpReward;
    [NonSerialized] private MissionData runtimeData;

    public MissionData Data
    {
        get
        {
            if (runtimeData == null)
            {
                runtimeData = ScriptableObject.CreateInstance<MissionData>();
            }

            runtimeData.missionId = missionId;
            runtimeData.missionName = description;
            runtimeData.missionDescription = description;
            runtimeData.missionType = type;
            runtimeData.goal = Mathf.RoundToInt(targetValue);
            runtimeData.currentProgress = Mathf.RoundToInt(currentProgress);
            runtimeData.isCompleted = isCompleted;
            runtimeData.rewardCoins = coinReward;
            runtimeData.rewardGems = gemReward;
            runtimeData.rewardXP = xpReward;
            runtimeData.rewardAmount = coinReward;

            return runtimeData;
        }
    }

    public float CurrentProgress => currentProgress;
    public bool IsClaimed => isCompleted;

    public Mission(string id, string desc, MissionType type, float target)
    {
        this.missionId = id;
        this.description = desc;
        this.type = type;
        this.targetValue = target;
        this.currentProgress = 0;
        this.isCompleted = false;
    }

    public Mission() { }

    public void UpdateProgress(float amount)
    {
        if (isCompleted) return;

        currentProgress += amount;
        if (currentProgress >= targetValue)
        {
            currentProgress = targetValue;
            isCompleted = true;
            Debug.Log($"[MissionSystem] Mission completed: {description}");
            GameEvents.TriggerMissionCompleted(missionId);
        }
    }
}
