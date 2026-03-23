using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Specialized manager for time-locked daily missions.
/// </summary>
public class DailyMissionManager : Singleton<DailyMissionManager>
{
    public static event Action OnMissionsRefreshed;
    public static event Action OnMissionsUpdated;
    public static event Action<Mission, float> OnMissionProgress;
    
    private DateTime lastRefreshTime;
    private readonly List<Mission> activeMissions = new List<Mission>();

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.Register(this);
    }

    public void RefreshMissions()
    {
        Debug.Log("Guardian Architect: Refreshing daily missions.");
        lastRefreshTime = DateTime.Now;
        OnMissionsRefreshed?.Invoke();
        OnMissionsUpdated?.Invoke();
    }

    public bool AreMissionsReady()
    {
        return DateTime.Now > lastRefreshTime.AddDays(1);
    }

    public void ClaimDailyReward(int dayIndex)
    {
        Debug.Log($"[DailyMission] Claimed reward for day {dayIndex}");
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Data.claimedDailyRewards[dayIndex.ToString()] = true;
            SaveManager.Instance.SaveGame();
        }
    }

    // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
    
    public void ClaimDailyReward(long dayIndex)
    {
        ClaimDailyReward((int)System.Math.Min(dayIndex, int.MaxValue));
    }

    public DateTime GetLastRefreshTime()
    {
        return lastRefreshTime;
    }

    public TimeSpan GetTimeSinceLastRefresh()
    {
        return DateTime.Now - lastRefreshTime;
    }

    public bool IsMissionRefreshAvailable()
    {
        return GetTimeSinceLastRefresh().TotalHours >= 24;
    }

    public List<Mission> GetActiveMissions()
    {
        return new List<Mission>(activeMissions);
    }

    public void ClaimMissionReward(Mission mission)
    {
        if (mission == null) return;
        mission.isCompleted = true;
        activeMissions.Remove(mission);
        OnMissionsUpdated?.Invoke();
    }

    public void ClaimMissionReward(string missionId)
    {
        Mission mission = activeMissions.Find(m => m != null && m.missionId == missionId);
        if (mission != null)
        {
            ClaimMissionReward(mission);
        }
    }

    public void ReportMissionProgress(Mission mission, float progress)
    {
        if (mission == null) return;
        if (!activeMissions.Contains(mission))
        {
            activeMissions.Add(mission);
        }

        mission.currentProgress = progress;
        OnMissionProgress?.Invoke(mission, progress);
    }
}
