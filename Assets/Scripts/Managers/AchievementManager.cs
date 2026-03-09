
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Manages all achievement-related logic, including loading, tracking, and unlocking achievements.
/// This is the central authority for the achievement system, created by Supreme Guardian Architect v12.
/// It uses a data-driven approach, loading all AchievementData ScriptableObjects from the project resources.
/// </summary>
public class AchievementManager : Singleton<AchievementManager>
{
    // --- PUBLIC EVENTS ---
    public static event Action<AchievementData> OnAchievementUnlocked;

    // --- PRIVATE STATE ---
    private Dictionary<string, AchievementData> _achievements; // All available achievements, keyed by ID
    private Dictionary<string, int> _achievementProgress; // Player'''s current progress, keyed by ID
    private const string ProgressKeyPrefix = "AchievementProgress_"; // Prefix for PlayerPrefs

    protected override void Awake()
    {
        base.Awake();
        InitializeManager();
    }

    /// <summary>
    /// Initializes the manager by loading all achievement data from the project'''s Resources folder.
    /// </summary>
    private void InitializeManager()
    {
        _achievements = new Dictionary<string, AchievementData>();
        _achievementProgress = new Dictionary<string, int>();

        // --- DATA-DRIVEN LOADING: Load all AchievementData assets from Resources/Achievements ---
        var achievementAssets = Resources.LoadAll<AchievementData>("Achievements");

        if (achievementAssets.Length == 0)
        {
            Debug.LogWarning("Guardian Architect Warning: No AchievementData assets found in Resources/Achievements. The achievement system will be inactive.");
            return;
        }

        foreach (var achievement in achievementAssets)
        {
            if (!_achievements.ContainsKey(achievement.id))
            {
                _achievements.Add(achievement.id, achievement);
                LoadProgress(achievement.id);
            }
            else
            {
                Debug.LogError($"Guardian Architect FATAL_ERROR: Duplicate achievement ID found: '{achievement.id}'. Achievement IDs must be unique.");
            }
        }

        Debug.Log($"Guardian Architect: Achievement Manager initialized. Loaded {achievementAssets.Length} achievements.");
    }

    /// <summary>
    /// Adds progress to a specific achievement.
    /// If the progress meets or exceeds the required value, the achievement is unlocked.
    /// </summary>
    /// <param name="achievementId">The unique ID of the achievement to progress.</param>
    /// <param name="amount">The amount of progress to add.</param>
    public void AddProgress(string achievementId, int amount)
    {
        if (!_achievements.ContainsKey(achievementId))
        {
            Debug.LogWarning($"Guardian Architect Warning: Attempted to add progress to a non-existent achievement: '{achievementId}'.");
            return;
        }

        // If the achievement is already unlocked, do nothing.
        if (_achievementProgress.ContainsKey(achievementId) && _achievementProgress[achievementId] >= _achievements[achievementId].requiredValue)
        {
            return;
        }

        int newProgress = _achievementProgress.ContainsKey(achievementId) ? _achievementProgress[achievementId] + amount : amount;
        _achievementProgress[achievementId] = newProgress;

        Debug.Log($"Guardian Architect: Progress added to '{achievementId}'. New progress: {newProgress}/{_achievements[achievementId].requiredValue}");

        // Check for unlock condition
        if (newProgress >= _achievements[achievementId].requiredValue)
        {
            UnlockAchievement(achievementId);
        }

        SaveProgress(achievementId);
    }

    /// <summary>
    /// Unlocks a specific achievement, triggers the event, and distributes rewards.
    /// </summary>
    private void UnlockAchievement(string achievementId)
    {
        if (!_achievements.ContainsKey(achievementId))
        {
            return;
        }

        AchievementData achievement = _achievements[achievementId];

        // Ensure it'''s not already considered fully progressed (handles edge cases)
        _achievementProgress[achievementId] = achievement.requiredValue;
        SaveProgress(achievementId);

        Debug.Log($"<color=yellow><b>Guardian Architect: Achievement Unlocked! -> {achievement.achievementName}</b></color>");

        // --- A-TO-Z CONNECTIVITY: Broadcast the unlocked achievement to all listeners (like AchievementPopup) ---
        OnAchievementUnlocked?.Invoke(achievement);

        // --- DEPENDENCY_FIX: Grant the reward (e.g., call a currency manager) ---
        // Guardian Note: This is a forward-thinking integration point.
        // Replace with `CurrencyManager.Instance.AddCurrency(achievement.rewardAmount);` when available.
        Debug.Log($"Guardian Architect: Rewarding player with {achievement.rewardAmount} currency for unlocking '{achievement.achievementName}'.");

    }

    #region Data Persistence (PlayerPrefs)

    /// <summary>
    /// Saves the progress of a single achievement to PlayerPrefs.
    /// </summary>
    private void SaveProgress(string achievementId)
    {
        PlayerPrefs.SetInt(ProgressKeyPrefix + achievementId, _achievementProgress[achievementId]);
    }

    /// <summary>
    /// Loads the progress of a single achievement from PlayerPrefs.
    /// </summary>
    private void LoadProgress(string achievementId)
    {
        int savedProgress = PlayerPrefs.GetInt(ProgressKeyPrefix + achievementId, 0);
        _achievementProgress[achievementId] = savedProgress;
    }

    /// <summary>
    /// [DEBUG] Resets all achievement progress stored in PlayerPrefs.
    /// </summary>
    public void ResetAllAchievementProgress()
    {
        foreach (var achievement in _achievements.Values)
        {
            PlayerPrefs.DeleteKey(ProgressKeyPrefix + achievement.id);
            _achievementProgress[achievement.id] = 0;
        }
        PlayerPrefs.Save();
        Debug.Log("Guardian Architect: All achievement progress has been reset.");
    }

    #endregion

    #region Public Getters

    /// <summary>
    /// Gets the current progress for a specific achievement.
    /// </summary>
    public int GetProgress(string achievementId)
    {
        if (_achievementProgress.TryGetValue(achievementId, out int progress))
        {
            return progress;
        }
        return 0;
    }

    /// <summary>
    /// Checks if an achievement is unlocked.
    /// </summary>
    public bool IsAchievementUnlocked(string achievementId)
    {
        if (_achievements.TryGetValue(achievementId, out AchievementData data) && _achievementProgress.TryGetValue(achievementId, out int progress))
        {
            return progress >= data.requiredValue;
        }
        return false;
    }

    /// <summary>
    /// Gets all achievement data assets.
    /// </summary>
    public IEnumerable<AchievementData> GetAllAchievements()
    {
        return _achievements.Values.OrderBy(a => a.achievementName);
    }

    #endregion
}
