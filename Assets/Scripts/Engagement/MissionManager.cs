using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Manages the assignment and progress tracking of persistent missions.
/// This system works similarly to Daily Missions but is intended for ongoing objectives
/// that do not reset daily.
/// </summary>
public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    // Event fired when mission data changes (progress, completion, claim)
    public static event Action OnMissionsUpdated;

    [Header("Mission Configuration")]
    [Tooltip("A list of all possible missions that can be assigned.")]
    [SerializeField] private List<ProjectMissionData> allMissions = new List<ProjectMissionData>();

    [Tooltip("The current set of active missions for the player.")]
    [SerializeField] private List<ProjectMissionData> activeMissions = new List<ProjectMissionData>();

    // PlayerPrefs key for saving mission state
    private const string MissionsKey = "ActiveMissions";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMissions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Public API

    public List<ProjectMissionData> GetActiveMissions() => activeMissions;

    /// <summary>
    /// Reports progress towards a specific mission type.
    /// </summary>
    public void UpdateMissionProgress(MissionType type, int amount)
    {
        bool updated = false;
        foreach (var mission in activeMissions.Where(m => m.type == type))
        {
            updated = true;
        }

        if (updated)
        {
            SaveMissions();
            OnMissionsUpdated?.Invoke();
        }
    }

    /// <summary>
    /// Claims the reward for a completed mission and replaces it with a new one if available.
    /// </summary>
    public void ClaimMissionReward(string missionID)
    {
        ProjectMissionData mission = activeMissions.FirstOrDefault(m => m.missionId == missionID);

        if (mission != null)
        {
            // Award currency
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddCoins(mission.rewardCoins);
                CurrencyManager.Instance.AddGems(mission.rewardGems);
            }
            
            // Replace the claimed mission with a new one
            ReplaceMission(missionID);

            SaveMissions();
            OnMissionsUpdated?.Invoke();
        }
    }

    #endregion

    #region Persistence

    [Serializable]
    private class MissionDataList
    {
        public List<ProjectMissionData> list = new List<ProjectMissionData>();
    }

    private void SaveMissions()
    {
        MissionDataList wrapper = new MissionDataList { list = activeMissions };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(MissionsKey, json);
        PlayerPrefs.Save();
    }

    private void LoadMissions()
    {
        string json = PlayerPrefs.GetString(MissionsKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            MissionDataList wrapper = JsonUtility.FromJson<MissionDataList>(json);
            activeMissions = wrapper.list ?? new List<ProjectMissionData>();
        }
        else
        { 
            // If no saved data, assign a default set of missions
            AssignInitialMissions();
        }

        // Ensure the mission list is never empty if there are missions available
        if (activeMissions.Count == 0 && allMissions.Count > 0)
        {
            AssignInitialMissions();
        }
    }

    /// <summary>
    /// Assigns the first few missions from the main list as the starting set.
    /// </summary>
    private void AssignInitialMissions()
    {
        activeMissions = allMissions.Take(3).Select(m => new ProjectMissionData { missionId = m.missionId, rewardCoins = m.rewardCoins, rewardGems = m.rewardGems, type = m.type, goal = m.goal, rewardAmount = m.rewardAmount, rewardType = m.rewardType }).ToList();
        SaveMissions();
    }

    /// <summary>
    /// Replaces a claimed mission with a new, uncompleted one from the main pool.
    /// </summary>
    private void ReplaceMission(string completedMissionID)
    {
        activeMissions.RemoveAll(m => m.missionId == completedMissionID);

        // Find a new mission that is not already active
        ProjectMissionData newMission = allMissions.FirstOrDefault(m => !activeMissions.Any(am => am.missionId == m.missionId));

        if (newMission != null)
        {
            activeMissions.Add(new ProjectMissionData { missionId = newMission.missionId, rewardCoins = newMission.rewardCoins, rewardGems = newMission.rewardGems, type = newMission.type, goal = newMission.goal, rewardAmount = newMission.rewardAmount, rewardType = newMission.rewardType });
        }
    }

    #endregion
}
