
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EndlessRunner.Data;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class AchievementManager : Singleton<AchievementManager>
    {
        public static event System.Action<Achievement> OnAchievementUnlocked;
        public static event System.Action<Achievement> OnAchievementProgress;

        [SerializeField] private List<Achievement> achievements;

        private Dictionary<string, Achievement> achievementDictionary;

        protected override void Awake()
        {
            base.Awake();
            InitializeAchievements();
            LoadAchievements();
        }

        private void OnEnable()
        {
            GameEvents.OnScoreGained += HandleScoreGained;
            GameEvents.OnCoinsGained += HandleCoinsGained;
            GameEvents.OnDistanceUpdated += HandleDistanceUpdated;
            GameEvents.OnPowerUpUsed += HandlePowerUpUsed;
        }

        private void OnDisable()
        {
            GameEvents.OnScoreGained -= HandleScoreGained;
            GameEvents.OnCoinsGained -= HandleCoinsGained;
            GameEvents.OnDistanceUpdated -= HandleDistanceUpdated;
            GameEvents.OnPowerUpUsed -= HandlePowerUpUsed;
        }

        private void InitializeAchievements()
        {
            achievementDictionary = new Dictionary<string, Achievement>();
            foreach (var achievement in achievements)
            {
                achievement.isUnlocked = false;
                achievement.isRewardClaimed = false;
                achievementDictionary[achievement.id] = achievement;
            }
        }

        public void CheckProgress(AchievementType type, int value)
        {
            foreach (var achievement in achievements.Where(a => a.type == type && !a.isUnlocked))
            {
                if (value >= achievement.requiredValue)
                {
                    UnlockAchievement(achievement.id);
                }
                else
                {
                    OnAchievementProgress?.Invoke(achievement);
                }
            }
        }

        public void UnlockAchievement(string id)
        {
            if (achievementDictionary.TryGetValue(id, out Achievement achievement) && !achievement.isUnlocked)
            {
                achievement.isUnlocked = true;
                OnAchievementUnlocked?.Invoke(achievement);
                SaveAchievements();
                Debug.Log($"ACHIEVEMENT_UNLOCKED: {achievement.title}");
            }
        }

        public void ClaimReward(string id)
        {
            if (achievementDictionary.TryGetValue(id, out Achievement achievement) && achievement.isUnlocked && !achievement.isRewardClaimed)
            {
                achievement.isRewardClaimed = true;
                CurrencyManager.Instance.AddCoins(achievement.rewardCoins);
                SaveAchievements();
                Debug.Log($"ACHIEVEMENT_REWARD_CLAIMED: {achievement.title}");
            }
        }

        private void SaveAchievements()
        {
            // In a real project, this would use a robust saving system.
            // For this example, we'll use PlayerPrefs for simplicity.
            string achievementData = JsonUtility.ToJson(new AchievementSaveData { Achievements = achievements });
            PlayerPrefs.SetString("AchievementData", achievementData);
            PlayerPrefs.Save();
        }

        private void LoadAchievements()
        {
            if (PlayerPrefs.HasKey("AchievementData"))
            {
                string achievementData = PlayerPrefs.GetString("AchievementData");
                AchievementSaveData saveData = JsonUtility.FromJson<AchievementSaveData>(achievementData);

                foreach (var savedAchievement in saveData.Achievements)
                {
                    if (achievementDictionary.TryGetValue(savedAchievement.id, out Achievement achievement))
                    {
                        achievement.isUnlocked = savedAchievement.isUnlocked;
                        achievement.isRewardClaimed = savedAchievement.isRewardClaimed;
                    }
                }
            }
        }

        // --- Event Handlers ---
        private void HandleScoreGained(int score) => CheckProgress(AchievementType.Score, score);
        private void HandleCoinsGained(int coins) => CheckProgress(AchievementType.CoinsCollected, coins);
        private void HandleDistanceUpdated(float distance) => CheckProgress(AchievementType.Distance, (int)distance);
        private void HandlePowerUpUsed() => CheckProgress(AchievementType.PowerUpsUsed, 1); // Incremental

        public List<Achievement> GetAchievements() => achievements;
    }

    [System.Serializable]
    public class AchievementSaveData
    {
        public List<Achievement> Achievements;
    }
}
