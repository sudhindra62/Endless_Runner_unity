
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Achievements
{
    public class AchievementProgressTracker : MonoBehaviour
    {
        [SerializeField] private AchievementDatabase achievementDatabase;
        private Dictionary<AchievementID, AchievementProgressData> achievementProgress;

        private void Awake()
        {
            achievementProgress = new Dictionary<AchievementID, AchievementProgressData>();
            foreach (var achievement in achievementDatabase.achievements)
            {
                achievementProgress[achievement.ID] = new AchievementProgressData(achievement);
            }
        }

        private void OnEnable()
        {
            GameEvents.OnRunComplete += OnRunComplete;
            GameEvents.OnScoreIncreased += OnScoreIncreased;
            GameEvents.OnCoinCollected += OnCoinCollected;
            GameEvents.OnBossDefeated += OnBossDefeated;
            GameEvents.OnPowerUpUsed += OnPowerUpUsed;
            GameEvents.OnNearMiss += OnNearMiss;
            GameEvents.OnReviveUsed += OnReviveUsed;
            GameEvents.OnLogin += OnLogin;
        }

        private void OnDisable()
        {
            GameEvents.OnRunComplete -= OnRunComplete;
            GameEvents.OnScoreIncreased -= OnScoreIncreased;
            GameEvents.OnCoinCollected -= OnCoinCollected;
            GameEvents.OnBossDefeated -= OnBossDefeated;
            GameEvents.OnPowerUpUsed -= OnPowerUpUsed;
            GameEvents.OnNearMiss -= OnNearMiss;
            GameEvents.OnReviveUsed -= OnReviveUsed;
            GameEvents.OnLogin -= OnLogin;
        }

        public AchievementProgressData GetProgress(AchievementID id)
        {
            return achievementProgress.ContainsKey(id) ? achievementProgress[id] : null;
        }

        public List<AchievementProgressData> GetAllProgress()
        {
            return achievementProgress.Values.ToList();
        }

        private void OnRunComplete(bool noRevive)
        {
            AddProgress(AchievementID.FirstRunComplete, 1);
            AddProgress(AchievementID.TenRunsComplete, 1);
            AddProgress(AchievementID.FiftyRunsComplete, 1);
            if(noRevive)
            {
                AddProgress(AchievementID.NoReviveRun, 1);
            }
        }
        
        private void OnScoreIncreased(int amount)
        {
            AddProgress(AchievementID.Score10000Points, amount);
            AddProgress(AchievementID.Score50000Points, amount);
            AddProgress(AchievementID.Score250000Points, amount);
        }
        
        private void OnCoinCollected(int amount)
        {
            AddProgress(AchievementID.Collect100CoinsInRun, amount);
            AddProgress(AchievementID.Collect5000CoinsTotal, amount);
            AddProgress(AchievementID.TotalCoins, amount);
        }

        private void OnBossDefeated()
        {
            AddProgress(AchievementID.BossesDefeated, 1);
        }

        private void OnPowerUpUsed()
        {
            AddProgress(AchievementID.FirstPowerUpUsed, 1);
        }

        private void OnNearMiss()
        {
            AddProgress(AchievementID.FirstNearMiss, 1);
        }

        private void OnReviveUsed()
        {
            AddProgress(AchievementID.FirstReviveUsed, 1);
        }

        private void OnLogin()
        {
            AddProgress(AchievementID.LoginStreak, 1);
        }

        public void AddProgress(AchievementID id, int amount)
        {
            if (achievementProgress.TryGetValue(id, out var progress))
            {
                progress.AddProgress(amount);
            }
        }
    }
}
