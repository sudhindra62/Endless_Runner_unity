using UnityEngine;

/// <summary>
/// Adapter that allows MissionData to be viewed as DailyMissionData
/// without changing existing data models.
/// ABSOLUTE SAFE MODE: no logic, no mutation.
/// </summary>
public static class DailyMissionDataAdapter
{
    public static DailyMissionData FromMissionData(MissionData source)
    {
        if (source == null) return null;

        // Create a runtime-only ScriptableObject wrapper
        var adapter = ScriptableObject.CreateInstance<DailyMissionData>();

        adapter.Type = source.MissionType;
        adapter.Description = source.Description;
        adapter.TargetValue = source.TargetValue;
        adapter.RewardType = source.rewardType;
        adapter.RewardAmount = source.rewardAmount;

        return adapter;
    }
}
