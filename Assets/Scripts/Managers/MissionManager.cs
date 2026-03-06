
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// The SUPREME manager for all missions. It handles active missions, progress tracking, rewards, and daily mission refresh logic.
/// This script has absorbed all functionality from the redundant DailyMissionManager.
/// </summary>
public class MissionManager : Singleton<MissionManager>
{
    [Header("Mission Configuration")]
    [SerializeField] private MissionDatabase missionDatabase;
    [SerializeField] private int concurrentMissions = 3;

    private List<MissionData> activeMissions = new List<MissionData>();

    public static event Action<MissionData> OnMissionProgress;
    public static event Action<MissionData> OnMissionCompleted;
    public static event Action OnDailyMissionsRefreshed;

    // ◈ MERGED: Daily mission refresh logic ◈
    private const string LastMissionRefreshKey = "LastMissionRefresh";

    private void Start()
    {
        CheckForDailyMissionRefresh();
        LoadActiveMissions();
        // SubscribeToGameEvents();
    }

    private void OnDestroy()
    {
        // UnsubscribeFromGameEvents();
    }

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

        if (PlayerProgression.Instance != null)
        {
            PlayerProgression.Instance.AddXP(mission.rewardXP, "MissionComplete");
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(mission.rewardCoins);
            CurrencyManager.Instance.AddGems(mission.rewardGems);
        }

        OnMissionCompleted?.Invoke(mission);
        Debug.Log($"Mission Completed: {mission.missionName}");

        if (!mission.isDaily)
        {
            ReplaceCompletedMission(mission);
        }
    }

    private void ReplaceCompletedMission(MissionData completedMission)
    {
        activeMissions.Remove(completedMission);
        
        List<MissionData> availableMissions = missionDatabase.allMissions
            .Where(m => !m.isDaily)
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
        // In a real game, this would load the state from a save file
        // For now, we will just populate with a mix of daily and regular missions

        // Clear any existing missions that aren't daily
        activeMissions.RemoveAll(m => !m.isDaily);

        // Add regular missions until we reach the concurrent limit
        int regularMissionCount = activeMissions.Count(m => !m.isDaily);
        while(regularMissionCount < concurrentMissions)
        {
            ReplaceCompletedMission(null);
            regularMissionCount++;
        }
    }

    private void SaveActiveMissions() 
    { 
        // In a real game, this would save the mission state
    }

    #region Daily Mission Refresh
    private void CheckForDailyMissionRefresh()
    {
        DateTime lastRefreshDate = GetLastRefreshDate();
        DateTime currentDate = DateTime.UtcNow.Date;

        if (currentDate > lastRefreshDate)
        {
            RefreshDailyMissions();
            SetLastRefreshDate(currentDate);
        }
    }

    private void RefreshDailyMissions()
    {
        // Remove all existing daily missions
        activeMissions.RemoveAll(m => m.isDaily);

        // Get a new set of daily missions
        List<MissionData> dailyMissions = missionDatabase.allMissions
            .Where(m => m.isDaily)
            .OrderBy(m => UnityEngine.Random.value)
            .Take(2) // Example: Take 2 new daily missions
            .ToList();

        foreach (var mission in dailyMissions)
        {
            MissionData newMission = Instantiate(mission);
            newMission.currentProgress = 0;
            newMission.isCompleted = false;
            activeMissions.Add(newMission);
        }

        OnDailyMissionsRefreshed?.Invoke();
        Debug.Log("Daily missions have been refreshed.");
    }

    private DateTime GetLastRefreshDate()
    {
        string dateString = PlayerPrefs.GetString(LastMissionRefreshKey, null);
        if (string.IsNullOrEmpty(dateString))
        {
            return DateTime.MinValue;
        }
        return DateTime.Parse(dateString);
    }

    private void SetLastRefreshDate(DateTime date)
    {
        PlayerPrefs.SetString(LastMissionRefreshKey, date.ToString());
        PlayerPrefs.Save();
    }
    #endregion
}
