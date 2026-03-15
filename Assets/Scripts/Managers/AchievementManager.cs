
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Data;
using EndlessRunner.Achievements;

namespace EndlessRunner.Managers
{
    /// <summary>
    /// Manages player achievements by listening to game events and tracking progress.
    /// </summary>
    public class AchievementManager : Singleton<AchievementManager>
    {
        private List<Achievement> allAchievements;
        private Dictionary<string, AchievementData> playerProgress;

        protected override void Awake()
        {
            base.Awake();
            LoadAchievements();
        }

        private void OnEnable()
        {
            GameEvents.OnPlayerJump += HandlePlayerJump;
            GameEvents.OnScoreGained += HandleScoreGained;
            GameEvents.OnCoinsGained += HandleCoinsGained;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerJump -= HandlePlayerJump;
            GameEvents.OnScoreGained -= HandleScoreGained;
            GameEvents.OnCoinsGained -= HandleCoinsGained;
        }

        private void Start()
        {
            LoadProgress();
        }

        private void LoadAchievements()
        {
            allAchievements = Resources.LoadAll<Achievement>("Achievements").ToList();
            Debug.Log($"ACHIEVEMENT_MANAGER: Loaded {allAchievements.Count} achievements from Resources.");
        }

        private void LoadProgress()
        {
            if (DataManager.Instance != null)
            {
                playerProgress = DataManager.Instance.GameData.achievementData;
                // Ensure all achievements have a corresponding progress entry
                foreach (var achievement in allAchievements)
                {
                    if (!playerProgress.ContainsKey(achievement.id))
                    {
                        playerProgress[achievement.id] = new AchievementData(achievement.id);
                    }
                }
            }
            else
            {
                playerProgress = new Dictionary<string, AchievementData>();
            }
        }

        private void SaveProgress()
        {
            if (DataManager.Instance != null)
            {
                DataManager.Instance.GameData.achievementData = playerProgress;
                DataManager.Instance.SaveData();
            }
        }

        private void HandlePlayerJump()
        {
            IncrementAchievementProgress(AchievementType.Jumps, 1);
        }

        private void HandleScoreGained(int score)
        {
            IncrementAchievementProgress(AchievementType.Score, score);
        }

        private void HandleCoinsGained(int coins)
        {
            IncrementAchievementProgress(AchievementType.Coins, coins);
        }

        private void IncrementAchievementProgress(AchievementType type, int amount)
        {
            var achievements = allAchievements.Where(a => a.achievementType == type);
            foreach (var achievement in achievements)
            {
                if (playerProgress.TryGetValue(achievement.id, out AchievementData progress) && !progress.isUnlocked)
                {
                    progress.currentProgress += amount;
                    CheckForUnlock(achievement, progress);
                }
            }
            SaveProgress();
        }

        private void CheckForUnlock(Achievement achievement, AchievementData progress)
        {
            if (progress.currentProgress >= achievement.unlockThreshold)
            {
                progress.isUnlocked = true;
                GameEvents.TriggerAchievementUnlocked(achievement);
                Debug.Log($"ACHIEVEMENT_MANAGER: Unlocked '{achievement.title}'!");
            }
        }
    }
}
