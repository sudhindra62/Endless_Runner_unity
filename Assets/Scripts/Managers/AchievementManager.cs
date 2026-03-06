
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Tracks, unlocks, and stores the state of all achievements in the game.
/// </summary>
public class AchievementManager : Singleton<AchievementManager>
{
    [SerializeField] private List<AchievementData> allAchievements; // All possible achievements, loaded from Resources

    // Runtime dictionary to track progress and unlock status
    private Dictionary<string, float> achievementProgress = new Dictionary<string, float>();
    private HashSet<string> unlockedAchievements = new HashSet<string>();

    private const string PROGRESS_KEY_PREFIX = "AchievementProgress_";
    private const string UNLOCKED_KEY_PREFIX = "AchievementUnlocked_";

    protected override void Awake()
    {
        base.Awake();
        LoadAchievements();
    }

    private void Start()
    {
        // Subscribe to game events to update progress
        // Example: ScoreManager.OnScoreUpdated += UpdateDistanceStat;
        // Example: CurrencyManager.OnCoinsUpdated += UpdateCoinStat;
    }

    /// <summary>
    /// Updates the progress for a specific statistic.
    /// Checks all relevant achievements to see if they are unlocked.
    /// </summary>
    /// <param name="stat">The stat to update.</param>
    /// <param name="value">The value to add to the progress.</param>
    public void UpdateStat(AchievementStat stat, float value)
    {
        // Find all achievements that track this stat
        foreach (var achievement in allAchievements.Where(a => a.statToTrack == stat && !unlockedAchievements.Contains(a.achievementId)))
        {
            float currentProgress = achievementProgress.ContainsKey(achievement.achievementId) ? achievementProgress[achievement.achievementId] : 0;
            currentProgress += value;
            achievementProgress[achievement.achievementId] = currentProgress;

            Debug.Log($"Updating stat {stat} for achievement '{achievement.displayName}'. New progress: {currentProgress}/{achievement.goalValue}");

            if (currentProgress >= achievement.goalValue)
            {
                UnlockAchievement(achievement);
            }
        }

        // SaveProgress();
    }

    private void UnlockAchievement(AchievementData achievement)
    {
        if (unlockedAchievements.Contains(achievement.achievementId)) return;

        unlockedAchievements.Add(achievement.achievementId);
        achievement.isUnlocked = true; // Update runtime data

        Debug.Log($"<color=yellow>Achievement Unlocked: {achievement.displayName}!</color>");

        // Grant rewards via a central manager
        // RewardManager.Instance.GrantReward(new Reward(achievement.rewardCoins, achievement.rewardGems));

        // Show a UI notification
        // UIManager.Instance.ShowAchievementNotification(achievement);
        
        // Save that this specific achievement is unlocked
        PlayerPrefs.SetInt(UNLOCKED_KEY_PREFIX + achievement.achievementId, 1);
    }

    public bool IsAchievementUnlocked(string achievementId)
    {
        return unlockedAchievements.Contains(achievementId);
    }

    public float GetAchievementProgress(string achievementId)
    {
        return achievementProgress.ContainsKey(achievementId) ? achievementProgress[achievementId] : 0;
    }

    // --- Persistence ---
    private void LoadAchievements()
    {
        // In a real game, you'd load from a save file (e.g., JSON, binary)
        // For this example, we'll use PlayerPrefs.
        foreach (var achievement in allAchievements)
        {
            if (PlayerPrefs.GetInt(UNLOCKED_KEY_PREFIX + achievement.achievementId, 0) == 1)
            {
                unlockedAchievements.Add(achievement.achievementId);
                achievement.isUnlocked = true;
                achievementProgress[achievement.achievementId] = achievement.goalValue;
            }
            else
            {
                achievementProgress[achievement.achievementId] = PlayerPrefs.GetFloat(PROGRESS_KEY_PREFIX + achievement.achievementId, 0);
            }
        }
    }

    private void SaveProgress()
    {
        foreach (var progress in achievementProgress)
        {
            PlayerPrefs.SetFloat(PROGRESS_KEY_PREFIX + progress.Key, progress.Value);
        }
        PlayerPrefs.Save();
    }
}
