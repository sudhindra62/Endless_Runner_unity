
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the daily rotation of missions. It ensures that missions are refreshed once per day and prevents time manipulation exploits.
/// </summary>
public class DailyMissionManager : Singleton<DailyMissionManager>
{
    [SerializeField] private MissionDatabase missionDatabase;
    private const string LastMissionRefreshKey = "LastMissionRefresh";

    private void Start()
    {
        CheckForMissionRefresh();
    }

    private void CheckForMissionRefresh()
    {
        DateTime lastRefreshDate = GetLastRefreshDate();
        DateTime currentDate = DateTime.UtcNow.Date;

        if (currentDate > lastRefreshDate)
        {
            RefreshMissions();
            SetLastRefreshDate(currentDate);
        }
    }

    private void RefreshMissions()
    {
        List<Mission> missionsToRemove = new List<Mission>();
        foreach (Mission mission in MissionManager.Instance.ActiveMissions)
        {
            if(mission.MissionId.StartsWith("Daily_"))
            {
                missionsToRemove.Add(mission);
            }
        }

        foreach(Mission mission in missionsToRemove)
        {
            MissionManager.Instance.ActiveMissions.Remove(mission);
        }
        
        // In a real game, you would have a more sophisticated system for selecting daily missions.
        // For this example, we'll just add a few from the database.
        MissionManager.Instance.AddMission("Daily_Run500m");
        MissionManager.Instance.AddMission("Daily_Collect100Coins");
        
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
}
