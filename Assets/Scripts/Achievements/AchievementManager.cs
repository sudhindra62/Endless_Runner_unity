
using System.Collections.Generic;
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.Achievements
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public List<Achievement> achievements;
        private Dictionary<AchievementType, int> _progress;

        protected override void Awake()
        {
            base.Awake();
            _progress = new Dictionary<AchievementType, int>
            {
                { AchievementType.Jumps, 0 },
                { AchievementType.Score, 0 },
                { AchievementType.Coins, 0 }
            };
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

        private void HandlePlayerJump()
        {
            UpdateProgress(AchievementType.Jumps, 1);
        }

        private void HandleScoreGained(int amount)
        {
            UpdateProgress(AchievementType.Score, amount);
        }

        private void HandleCoinsGained(int amount)
        {
            UpdateProgress(AchievementType.Coins, amount);
        }

        private void UpdateProgress(AchievementType type, int amount)
        {
            if (_progress.ContainsKey(type))
            {
                _progress[type] += amount;
                CheckForUnlock(type);
            }
        }

        private void CheckForUnlock(AchievementType type)
        {
            foreach (var achievement in achievements)
            {
                if (achievement.achievementType == type && !IsUnlocked(achievement))
                {
                    if (_progress[type] >= achievement.unlockThreshold)
                    {
                        UnlockAchievement(achievement);
                    }
                }
            }
        }

        private void UnlockAchievement(Achievement achievement)
        {
            PlayerPrefs.SetInt(achievement.id, 1);
            PlayerPrefs.Save();
            
            GameEvents.TriggerAchievementUnlocked(achievement);
            Logger.Log("ACHIEVEMENT_MANAGER", $"Achievement unlocked: {achievement.title}");
        }

        public bool IsUnlocked(Achievement achievement)
        {
            return PlayerPrefs.GetInt(achievement.id, 0) == 1;
        }
    }
}
