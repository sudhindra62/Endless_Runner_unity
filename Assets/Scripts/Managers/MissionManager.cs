
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages player missions and tracks their progress.
/// This system is responsible for loading, saving, and updating mission objectives.
/// Created by Supreme Guardian Architect v12.
/// </summary>

// Defines a single mission's structure
[System.Serializable]
public class Mission
{
    public string missionId;
    public string description;
    public int requiredCount;
    [System.NonSerialized] public int currentCount; // Runtime progress
    public bool isCompleted;
    public int rewardAmount;

    public void LoadProgress()
    {
        currentCount = PlayerPrefs.GetInt("MissionProgress_" + missionId, 0);
        isCompleted = PlayerPrefs.GetInt("MissionCompleted_" + missionId, 0) == 1;
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("MissionProgress_" + missionId, currentCount);
        PlayerPrefs.SetInt("MissionCompleted_" + missionId, isCompleted ? 1 : 0);
    }

    public void UpdateProgress(int amount)
    {
        if (isCompleted) return;

        currentCount += amount;
        if (currentCount >= requiredCount)
        {
            currentCount = requiredCount;
            Complete();
        }
    }

    private void Complete()
    {
        isCompleted = true;
        Debug.Log($"Guardian Architect Log: Mission ''{missionId}'' completed!");
        // You would typically grant a reward here
        FindObjectOfType<ScoreManager>()?.AddCoins(rewardAmount);
        SaveProgress();
        // Maybe show a UI notification
    }
}

public class MissionManager : Singleton<MissionManager>
{
    [Header("Mission Database")]
    [SerializeField] private List<Mission> allMissions = new List<Mission>();

    void Start()
    {
        LoadAllMissions();
    }

    private void LoadAllMissions()
    {
        foreach (var mission in allMissions)
        {
            mission.LoadProgress();
        }
        Debug.Log("Guardian Architect Log: All mission progress loaded.");
    }

    private void SaveAllMissions()
    {
        foreach (var mission in allMissions)
        {
            mission.SaveProgress();
        }
        PlayerPrefs.Save();
        Debug.Log("Guardian Architect Log: All mission progress saved.");
    }

    /// <summary>
    /// Updates the progress of a specific mission type.
    /// </summary>
    /// <param name="missionTypeIdentifier">The identifier for the mission type (e.g., "runDistance", "collectCoins").</param>
    /// <param name="amount">The amount to add to the progress.</param>
    public void UpdateMissionProgress(string missionTypeIdentifier, int amount)
    {
        foreach (var mission in allMissions)
        {
            // This logic assumes the missionId is structured like "runDistance_1", "collectCoins_3", etc.
            if (!mission.isCompleted && mission.missionId.StartsWith(missionTypeIdentifier))
            {
                mission.UpdateProgress(amount);
            }
        }
    }

    // Save progress when the application quits
    void OnApplicationQuit()
    {
        SaveAllMissions();
    }
    
    // Example: A method to get a list of currently active (non-completed) missions for UI display
    public List<Mission> GetActiveMissions()
    {
        List<Mission> activeMissions = new List<Mission>();
        foreach (var mission in allMissions)
        {
            if (!mission.isCompleted)
            {
                activeMissions.Add(mission);
            }
        }
        return activeMissions;
    }
}
