
using UnityEngine;
using System.Collections.Generic;

public enum MissionType
{
    CollectCurrency,
    RunDistance,
    UsePowerup
}

[System.Serializable]
public class EventMission
{
    public string missionID;
    public MissionType missionType;
    public float targetValue;
    public int rewardAmount;
    public bool isCompleted;
}

public class EventMissionManager : Singleton<EventMissionManager>
{
    public List<EventMission> activeMissions = new List<EventMission>();

    // This would be called by other game systems to report progress
    public void UpdateMissionProgress(MissionType type, float value)
    {
        foreach (var mission in activeMissions)
        {
            if (!mission.isCompleted && mission.missionType == type)
            {
                // In a real game, you would track progress and check for completion.
                // For simplicity, we'll just log it here.
                Debug.Log($"Progress on mission {mission.missionID}: {value}");

                if (value >= mission.targetValue)
                {
                    CompleteMission(mission);
                }
            }
        }
    }

    private void CompleteMission(EventMission mission)
    {
        mission.isCompleted = true;
        EventCurrencyManager.Instance.AddEventCurrency(mission.rewardAmount);
        Debug.Log($"Mission {mission.missionID} completed! Rewarded {mission.rewardAmount} event currency.");
    }
}
