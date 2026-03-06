
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authoritative, consolidated manager for all game achievements.
/// Merged from /Managers/AchievementManager and /Achievements/AchievementManager.
/// This version prioritizes the event-driven architecture from the /Achievements version for better decoupling,
/// and uses a specific SaveSystem for more robust data persistence.
/// </summary>
public class AchievementManager : Singleton<AchievementManager>
{
    // From /Achievements: Event-driven approach for UI decoupling.
    public static event Action<AchievementData> OnAchievementUnlocked;

    // From /Achievements: This list contains the master data for all achievements.
    public List<AchievementData> achievements;

    protected override void Awake()
    {
        base.Awake(); // Call the base Singleton Awake.
        InitializeAchievements();
        LoadAchievements(); // Load progress from the specific save system.
    }

    private void OnEnable()
    {
        // Logic from /Achievements version to manage the tracker lifecycle
        AchievementProgressTracker.Initialize();
    }

    private void OnDisable()
    {
        // Logic from /Achievements version to manage the tracker lifecycle
        AchievementProgressTracker.Uninitialize();
    }

    /// <summary>
    /// From /Achievements: Preserves the hardcoded, essential achievement definitions.
    /// This is the master list of all possible achievements in the game.
    /// </summary>
    private void InitializeAchievements()
    {
        achievements = new List<AchievementData>
        {
            new AchievementData { id = AchievementID.TotalDistance, name = "Road Warrior", description = "Run 100km total", requiredValue = 100000, rewardId = "REWARD_COINS_LARGE" },
            new AchievementData { id = AchievementID.ComboPeak, name = "Combo Master", description = "Reach 50 combo", requiredValue = 50, rewardId = "REWARD_GEMS_SMALL" },
            new AchievementData { id = AchievementID.BossesDefeated, name = "Boss Hunter", description = "Defeat 20 bosses", requiredValue = 20, rewardId = "REWARD_CHEST_RARE" },
            new AchievementData { id = AchievementID.LegendaryShards, name = "Shard Hoarder", description = "Collect 5 legendary shards", requiredValue = 5, rewardId = "REWARD_SHARD_LEGENDARY_RANDOM" },
            new AchievementData { id = AchievementID.DiamondLeague, name = "Diamond Status", description = "Reach Diamond league", requiredValue = 1, rewardId = "REWARD_SKIN_EXCLUSIVE" },
            new AchievementData { id = AchievementID.LoginStreak, name = "Loyal Player", description = "7-day login streak", requiredValue = 7, rewardId = "REWARD_COINS_MEDIUM" },
            new AchievementData { id = AchievementID.NoReviveRun, name = "Untouchable", description = "Complete a run with no revives", requiredValue = 1, rewardId = "REWARD_GEMS_MEDIUM" },
            new AchievementData { id = AchievementID.TotalCoins, name = "Millionaire", description = "Collect 1 million total coins", requiredValue = 1000000, rewardId = "REWARD_GEMS_LARGE" }
        };
    }

    /// <summary>
    /// From /Achievements: More robust incremental progress updating.
    /// </summary>
    public void UpdateAchievement(AchievementID id, int progressAmount)
    {
        AchievementData achievement = achievements.Find(a => a.id == id);

        if (achievement != null && !achievement.isCompleted)
        {
            achievement.IncrementProgress(progressAmount);

            if (achievement.currentValue >= achievement.requiredValue)
            {
                UnlockAchievement(achievement);
            }
            SaveAchievements(); // Persist progress
        }
    }

    /// <summary>
    /// From /Achievements: Handles unlocking with validation and fires event for UI.
    /// </summary>
    private void UnlockAchievement(AchievementData achievement)
    {
        if (AchievementUnlockValidator.CanUnlock(achievement.id))
        {
            achievement.isCompleted = true;
            AchievementUnlockValidator.MarkAsUnlocked(achievement.id);

            // Fire event for UI systems to listen to. This is superior to direct UI calls.
            OnAchievementUnlocked?.Invoke(achievement);

            // From both versions: Reward logic is essential.
            if (!string.IsNullOrEmpty(achievement.rewardId))
            {
                // The quantity is determined by the RewardManager, which holds the master reward data.
                RewardManager.Instance.Award(achievement.rewardId, 1);
            }

            SaveAchievements(); // Persist the unlocked state
        }
    }

    /// <summary>
    /// From /Achievements: Specific and robust saving mechanism.
    /// </summary>
    private void SaveAchievements()
    {
        PlayerAchievementSaveData data = new PlayerAchievementSaveData(achievements, AchievementUnlockValidator.UnlockedAchievements);
        SaveSystem.SaveAchievements(data);
    }

    /// <summary>
    /// From /Achievements: Specific and robust loading mechanism.
    /// This logic safely restores progress to the master list of achievements.
    /// </summary>
    private void LoadAchievements()
    {
        PlayerAchievementSaveData data = SaveSystem.LoadAchievements();
        if (data != null)
        {
            var savedProgress = new Dictionary<AchievementID, AchievementData>();
            foreach(var savedAch in data.achievements)
            {
                savedProgress[savedAch.id] = savedAch;
            }
        
            foreach(var masterAch in this.achievements)
            {
                if(savedProgress.TryGetValue(masterAch.id, out var progress))
                {
                    masterAch.currentValue = progress.currentValue;
                    masterAch.isCompleted = progress.isCompleted;
                }
            }
            
            AchievementUnlockValidator.LoadUnlockedAchievements(data.unlockedAchievements);
        }
        else
        {
            // If no save data, initialize validator with an empty set.
            AchievementUnlockValidator.LoadUnlockedAchievements(new HashSet<AchievementID>());
        }
    }

    /// <summary>
    /// Checks if a given achievement has been completed.
    /// </summary>
    public bool IsAchievementUnlocked(AchievementID achievementId)
    {
        AchievementData achievement = achievements.Find(a => a.id == achievementId);
        return achievement != null && achievement.isCompleted;
    }
}
