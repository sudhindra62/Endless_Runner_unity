
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authoritative singleton for managing all mission progress.
/// It subscribes to game events to update mission progress and is the sole authority for completing missions.
/// </summary>
public class MissionManager : Singleton<MissionManager>
{
    public static event Action<Mission> OnMissionCompleted;

    [SerializeField] private MissionDatabase missionDatabase;
    public List<Mission> ActiveMissions { get; private set; } = new List<Mission>();

    private const string ActiveMissionsKey = "ActiveMissions";
    private const string CompletedMissionsKey = "CompletedMissions";

    private HashSet<string> _completedMissions = new HashSet<string>();

    protected override void Awake()
    {
        base.Awake();
        LoadMissionData();
    }

    private void OnEnable()
    {
        // Subscribe to game events here
        // Example: ScoreManager.OnScoreIncreased += HandleScoreIncreased;
    }

    private void OnDisable()
    {
        // Unsubscribe from game events here
        // Example: ScoreManager.OnScoreIncreased -= HandleScoreIncreased;
    }

    private void LoadMissionData()
    {
        // Load completed missions
        string completedMissionsString = PlayerPrefs.GetString(CompletedMissionsKey, string.Empty);
        if (!string.IsNullOrEmpty(completedMissionsString))
        {
            _completedMissions = new HashSet<string>(completedMissionsString.Split(','));
        }

        // Load active missions
        string activeMissionsString = PlayerPrefs.GetString(ActiveMissionsKey, string.Empty);
        if (!string.IsNullOrEmpty(activeMissionsString))
        {
            string[] activeMissionIds = activeMissionsString.Split(',');
            foreach (string missionId in activeMissionIds)
            {
                Mission mission = missionDatabase.GetMissionById(missionId);
                if (mission != null && !IsMissionCompleted(missionId))
                {
                    ActiveMissions.Add(mission);
                }
            }
        }
    }

    private void SaveMissionData()
    {
        PlayerPrefs.SetString(CompletedMissionsKey, string.Join(",", _completedMissions));
        
        List<string> activeMissionIds = new List<string>();
        foreach(Mission mission in ActiveMissions)
        {
            activeMissionIds.Add(mission.MissionId);
        }

        PlayerPrefs.SetString(ActiveMissionsKey, string.Join(",", activeMissionIds));
        PlayerPrefs.Save();
    }

    public void UpdateMissionProgress(string missionId, int amount)
    {
        Mission mission = ActiveMissions.Find(m => m.MissionId == missionId);
        if (mission != null)
        {
            mission.CurrentProgress += amount;
            if (mission.CurrentProgress >= mission.RequiredAmount)
            {
                CompleteMission(mission);
            }
            SaveMissionData();
        }
    }

    private void CompleteMission(Mission mission)
    {
        if (IsMissionCompleted(mission.MissionId)) return;

        _completedMissions.Add(mission.MissionId);
        ActiveMissions.Remove(mission);
        OnMissionCompleted?.Invoke(mission);
        RewardManager.Instance.GrantMissionReward(mission.MissionId, mission.CoinReward, mission.GemReward, mission.XpReward);
        SaveMissionData();
        Debug.Log($"Mission {mission.MissionId} completed!");
    }

    public bool IsMissionCompleted(string missionId)
    {
        return _completedMissions.Contains(missionId);
    }
    
    public void AddMission(string missionId)
    {
        if(ActiveMissions.Find(m => m.MissionId == missionId) != null) return;

        Mission mission = missionDatabase.GetMissionById(missionId);
        if (mission != null && !IsMissionCompleted(missionId))
        {
            ActiveMissions.Add(mission);
            SaveMissionData();
        }
    }
}
