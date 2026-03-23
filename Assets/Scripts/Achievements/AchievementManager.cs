
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class AchievementManager : Singleton<AchievementManager>
    {
        public static event System.Action<Achievement> OnAchievementUnlocked;

        public List<Achievement> achievements;
        private Dictionary<AchievementType, int> _progress;

        protected override void Awake()
        {
            base.Awake();
            LoadProgress();
        }

        private void LoadProgress()
        {
            _progress = new Dictionary<AchievementType, int>();
            if (SaveManager.Instance != null)
            {
                foreach (var kvp in SaveManager.Instance.Data.achievementProgress)
                {
                    if (System.Enum.TryParse(kvp.Key, out AchievementType type))
                    {
                        _progress[type] = kvp.Value;
                    }
                }
            }
            
            // Ensure defaults
            if (!_progress.ContainsKey(AchievementType.Jumps)) _progress[AchievementType.Jumps] = 0;
            if (!_progress.ContainsKey(AchievementType.Score)) _progress[AchievementType.Score] = 0;
            if (!_progress.ContainsKey(AchievementType.Coins)) _progress[AchievementType.Coins] = 0;
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

        public void AddProgress(AchievementType type, int amount) => UpdateProgress(type, amount);

        private void UpdateProgress(AchievementType type, int amount)
        {
            if (!_progress.ContainsKey(type)) _progress[type] = 0;
            
            _progress[type] += amount;
            
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.Data.achievementProgress[type.ToString()] = _progress[type];
                SaveManager.Instance.SaveGame();
            }

            CheckForUnlock(type);
        }

        private void CheckForUnlock(AchievementType type)
        {
            foreach (var achievement in achievements)
            {
                if (achievement.achievementType == type && !IsUnlocked(achievement))
                {
                    if (_progress[type] >= achievement.unlockThreshold)
                    {
                        UnlockAchievementInternal(achievement);
                    }
                }
            }
        }

        private void UnlockAchievementInternal(Achievement achievement)
        {
            if (SaveManager.Instance != null && !SaveManager.Instance.Data.unlockedAchievements.Contains(achievement.id))
            {
                SaveManager.Instance.Data.unlockedAchievements.Add(achievement.id);
                SaveManager.Instance.SaveGame();
            }
            
            GameEvents.TriggerAchievementUnlocked(achievement);
            OnAchievementUnlocked?.Invoke(achievement);
            Logger.Log("ACHIEVEMENT_MANAGER", $"Achievement unlocked: {achievement.title}");
        }

        public bool IsUnlocked(Achievement achievement)
        {
            return SaveManager.Instance != null && SaveManager.Instance.Data.unlockedAchievements.Contains(achievement.id);
        }

        public void ClaimReward(Achievement achievement)
        {
            if (achievement != null) achievement.ClaimReward();
        }

        public void ClaimReward(string achievementID)
        {
            var achievement = GetAchievementByID(achievementID);
            if (achievement != null)
            {
                ClaimReward(achievement);
            }
        }

        public int GetProgress(AchievementType type)
        {
            return _progress.ContainsKey(type) ? _progress[type] : 0;
        }

        public int GetProgress(string achievementID)
        {
            return GetAchievementProgress(achievementID);
        }

        public List<Achievement> GetAllAchievements() => achievements;

        public void UnlockAchievementByID(string achievementID)
        {
            var achievement = achievements.Find(a => a.id == achievementID);
            if (achievement != null) UnlockAchievement(achievement);
        }

        public void TrackProgress(string achievementID, int amount)
        {
            var achievement = achievements.Find(a => a.id == achievementID);
            if (achievement != null) UpdateProgress(achievement.achievementType, amount);
        }

        public int GetAchievementProgress(string achievementID)
        {
            var achievement = achievements.Find(a => a.id == achievementID);
            if (achievement != null) return GetProgress(achievement.achievementType);
            return 0;
        }

        public bool IsAchievementUnlocked(string achievementID)
        {
            return SaveManager.Instance != null && SaveManager.Instance.Data.unlockedAchievements.Contains(achievementID);
        }

        public Achievement[] GetUnlockedAchievements()
        {
            if (SaveManager.Instance == null) return new Achievement[0];
            return achievements.Where(a => SaveManager.Instance.Data.unlockedAchievements.Contains(a.id)).ToArray();
        }

        public Reward GetAchievementReward(string achievementID)
        {
            var achievement = achievements.Find(a => a.id == achievementID);
            if (achievement == null) return null;

            return new Reward(achievement.title, RewardType.Coins, achievement.rewardCoins, achievement.id);
        }

        public void ResetAchievement(string achievementID)
        {
            if (SaveManager.Instance == null) return;
            var achievement = achievements.Find(a => a.id == achievementID);
            if (achievement != null)
            {
                SaveManager.Instance.Data.unlockedAchievements.Remove(achievementID);
                _progress[achievement.achievementType] = 0;
                SaveManager.Instance.SaveGame();
            }
        }

        // --- Type Conversion Overloads (Phase 2A: Type Consistency) ---

        public void UnlockAchievement(Achievement achievement)
        {
            if (achievement != null) UnlockAchievementInternal(achievement);
        }

        public void TrackProgress(Achievement achievement, int amount)
        {
            if (achievement != null)
                TrackProgress(achievement.id, amount);
        }

        public int GetAchievementProgress(Achievement achievement)
        {
            return achievement != null ? GetAchievementProgress(achievement.id) : 0;
        }

        public bool IsAchievementUnlocked(Achievement achievement)
        {
            return achievement != null && IsAchievementUnlocked(achievement.id);
        }

        public Achievement GetAchievementByID(string achievementID)
        {
            return achievements?.Find(a => a.id == achievementID);
        }

        public AchievementData[] GetAchievementData()
        {
            if (achievements == null) return new AchievementData[0];

            return achievements
                .Select(a => new AchievementData
                {
                    id = a.id,
                    name = a.title,
                    achievementName = a.title,
                    description = a.description,
                    requiredValue = a.requiredValue,
                    icon = a.Badge,
                    iconReference = a.iconReference,
                    tier = a.tier
                })
                .ToArray();
        }
    }

