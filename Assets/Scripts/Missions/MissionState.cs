using System;
using UnityEngine;

[System.Serializable]
public class MissionState
{
    public DailyMissionData data;
    public int progress;
    public bool isComplete => progress >= data.goal;
    public bool isClaimed;

    // 🔹 ORIGINAL CONSTRUCTOR — KEPT
    public MissionState(DailyMissionData data)
    {
        this.data = data;
        this.progress = 0;
        this.isClaimed = false;
    }

    // 🔹 FIXED ADAPTER CONSTRUCTOR
    // Converts MissionData → DailyMissionData safely
    public MissionState(MissionStatus status)
    {
        // Create a runtime DailyMissionData adapter
        DailyMissionData adaptedData = ScriptableObject.CreateInstance<DailyMissionData>();

        adaptedData.name = status.Data.missionId;
        adaptedData.Description = status.Data.description;
        adaptedData.TargetValue = status.Data.goal;
        adaptedData.Type = status.Data.type;
        adaptedData.RewardType = status.Data.rewardType;
        adaptedData.RewardAmount = status.Data.rewardAmount;

        // 🔹 SAFE ADAPTER (ADDITIVE)
    this.data = DailyMissionDataAdapter.FromMissionData(status.Data);
    this.progress = status.CurrentProgress;
    this.isClaimed = status.IsClaimed;
    }

    // 🔹 ORIGINAL SYNC METHOD — KEPT
    public void SyncFrom(MissionStatus status)
    {
        this.progress = status.CurrentProgress;
        this.isClaimed = status.IsClaimed;
    }
}
