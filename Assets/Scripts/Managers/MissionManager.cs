
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Manages active missions, tracks progress, and grants rewards.
/// Acts as the central hub for all mission-related logic.
/// </summary>
public class MissionManager : Singleton<MissionManager>
{
    [Header("Mission Pool")]
    [SerializeField] private MissionDatabase missionDatabase;
    [SerializeField] private int concurrentMissions = 3;

    private List<MissionData> activeMissions = new List<MissionData>();

    public static event Action<MissionData> OnMissionProgress;
    public static event Action<MissionData> OnMissionCompleted;

    private void Start()
    {
        LoadActiveMissions();
        // SubscribeToGameEvents();
    }

    private void OnDestroy()
    {
        // UnsubscribeFromGameEvents();
    }

    /// <summary>
    /// Called by other game systems to report an event.
    /// </summary>
    public void ReportEvent(MissionType type, int amount)
    {
        foreach (var mission in activeMissions)
        {
            if (mission.missionType == type && !mission.isCompleted)
            {
                mission.currentProgress += amount;
                OnMissionProgress?.Invoke(mission);

                if (mission.currentProgress >= mission.goal)
                {
                    CompleteMission(mission);
                }
            }
        }
        SaveActiveMissions();
    }

    private void CompleteMission(MissionData mission)
    {
        if (mission.isCompleted) return;

        mission.isCompleted = true;

        // Grant Rewards
        PlayerProgression.Instance.AddXP(mission.rewardXP);
        CurrencyManager.Instance.AddCoins(mission.rewardCoins);
        CurrencyManager.Instance.AddGems(mission.rewardGems);

        OnMissionCompleted?.Invoke(mission);
        Debug.Log($"Mission Completed: {mission.missionName}");

        // Replace the completed mission with a new one
        ReplaceCompletedMission(mission);
    }

    private void ReplaceCompletedMission(MissionData completedMission)
    {
        activeMissions.Remove(completedMission);
        
        List<MissionData> availableMissions = missionDatabase.allMissions
            .Except(activeMissions)
            .ToList();

        if (availableMissions.Count > 0)
        {
            MissionData newMission = Instantiate(availableMissions[UnityEngine.Random.Range(0, availableMissions.Count)]);
            newMission.currentProgress = 0;
            newMission.isCompleted = false;
            activeMissions.Add(newMission);
        }
    }

    public List<MissionData> GetActiveMissions() => activeMissions;

    private void LoadActiveMissions() 
    { 
        // For simplicity, we'll just grab the first few from the database
        // In a real game, we would load the saved mission state
        activeMissions = missionDatabase.allMissions.Take(concurrentMissions).Select(m => Instantiate(m)).ToList();
        foreach(var mission in activeMissions)
        {
            mission.currentProgress = 0;
            mission.isCompleted = false;
        }
    }
    private void SaveActiveMissions() 
    { 
        // In a real game, we would save the active mission state to a file
    }
    // private void SubscribeToGameEvents() { ... }
    // private void UnsubscribeFromGameEvents() { ... }
}
