
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Manages long-term player achievements.
/// Tracks progress, saves it to PlayerPrefs, and grants rewards upon completion.
/// This is a persistent singleton with no direct UI dependencies.
/// 
/// --- Inspector Setup ---
/// 1. Attach this to a persistent GameObject in your starting scene.
/// 2. Create and assign a list of all possible AchievementData objects to the 'Achievements' list.
/// </summary>
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [Header("Achievement Definitions")]
    [SerializeField] private List<AchievementData> allAchievements;

    // --- PlayerPrefs Keys ---
    private const string AchievementProgressKey = "Achievement_Progress_"; // + ID
    private const string AchievementClaimedKey = "Achievement_Claimed_";   // + ID

    private Dictionary<string, long> achievementProgress;
    private Dictionary<string, bool> achievementClaimed;

    public static event Action<AchievementData, long> OnProgressUpdated;
    public static event Action<AchievementData> OnAchievementUnlocked;

    #region Unity Lifecycle & Initialization

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public API for Progress Updates

    /// <summary>
    /// Updates achievement progress for stats that are cumulative over time.
    /// This should be called by other game systems (e.g., end of a run).
    /// </summary>
    public void UpdateTotalRunStats(int coinsCollectedThisRun, float runTime)
    {
        AddProgress("TOTAL_COINS_COLLECTED", coinsCollectedThisRun);
        AddProgress("TOTAL_RUNS_COMPLETED", 1);

        // Update longest run if this one was longer
        long currentLongest = GetProgress("LONGEST_RUN_TIME");
        if (runTime > currentLongest)
        {
            SetProgress("LONGEST_RUN_TIME", (long)runTime);
        }
    }

    #endregion

    #region Progress & Unlocking Logic

    private void AddProgress(string achievementID, long amount)
    {
        SetProgress(achievementID, GetProgress(achievementID) + amount);
    }

    private void SetProgress(string achievementID, long value)
    {
        if (IsClaimed(achievementID)) return;

        achievementProgress[achievementID] = value;
        PlayerPrefs.SetString(AchievementProgressKey + achievementID, value.ToString()); // Use string for long

        var achievement = GetAchievementData(achievementID);
        if (achievement != null)
        {
            OnProgressUpdated?.Invoke(achievement, value);
            CheckForUnlock(achievement);
        }
        
        PlayerPrefs.Save();
    }

    private void CheckForUnlock(AchievementData achievement)
    {
        if (IsClaimed(achievement.AchievementID)) return;

        long progress = GetProgress(achievement.AchievementID);
        if (progress >= achievement.TargetValue)
        {
            UnlockAchievement(achievement);
        }
    }

    private void UnlockAchievement(AchievementData achievement)
    {
        if (IsClaimed(achievement.AchievementID)) return;

        achievementClaimed[achievement.AchievementID] = true;
        PlayerPrefs.SetInt(AchievementClaimedKey + achievement.AchievementID, 1);
        PlayerPrefs.Save();

        GrantReward(achievement);
        OnAchievementUnlocked?.Invoke(achievement);
        Debug.Log($"Achievement Unlocked: {achievement.Title}");
    }

    private void GrantReward(AchievementData achievement)
    {
        // This relies on other managers being present, but does not modify them.
        switch (achievement.RewardType)
        {
            case EngagementRewardType.Coins:
                if (CurrencyManager.Instance != null) CurrencyManager.Instance.AddCoins(achievement.RewardAmount);
                break;
            case EngagementRewardType.Gems:
                if (CurrencyManager.Instance != null) CurrencyManager.Instance.AddGems(achievement.RewardAmount);
                break;
            case EngagementRewardType.Chest:
                 if (ChestManager.Instance != null) ChestManager.Instance.AddChest((ChestType)achievement.RewardAmount);
                break;
        }
    }

    #endregion

    #region Data Access

    public long GetProgress(string achievementID)
    {
        return achievementProgress.ContainsKey(achievementID) ? achievementProgress[achievementID] : 0;
    }

    public bool IsClaimed(string achievementID)
    {
        return achievementClaimed.ContainsKey(achievementID) && achievementClaimed[achievementID];
    }

    public AchievementData GetAchievementData(string id) => allAchievements.FirstOrDefault(a => a.AchievementID == id);
    public List<AchievementData> GetAllAchievements() => allAchievements;

    #endregion

    #region Persistence

    private void LoadAchievements()
    {
        achievementProgress = new Dictionary<string, long>();
        achievementClaimed = new Dictionary<string, bool>();

        foreach (var achievement in allAchievements)
        {
            string progressStr = PlayerPrefs.GetString(AchievementProgressKey + achievement.AchievementID, "0");
            long.TryParse(progressStr, out long progress);
            achievementProgress[achievement.AchievementID] = progress;

            bool claimed = PlayerPrefs.GetInt(AchievementClaimedKey + achievement.AchievementID, 0) == 1;
            achievementClaimed[achievement.AchievementID] = claimed;
        }
    }

    #endregion
}
