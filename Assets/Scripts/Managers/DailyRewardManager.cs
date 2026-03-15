
using UnityEngine;
using System;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class DailyRewardManager : Singleton<DailyRewardManager>
    {
        public static event Action<bool> OnDailyRewardStatus;

        [SerializeField] private int dailyRewardAmount = 100; // Example: 100 coins
        [SerializeField] private int streakBonusMultiplier = 50; // Example: 50 extra coins per streak day

        private void Start()
        {
            CheckDailyRewardStatus();
        }

        public void CheckDailyRewardStatus()
        {
            bool isAvailable = IsRewardAvailable();
            OnDailyRewardStatus?.Invoke(isAvailable);
        }

        public bool IsRewardAvailable()
        {
            DateTime now = DateTime.UtcNow;
            long lastRewardTimestamp = SaveManager.Instance.Data.lastDailyRewardTimestamp;

            if (lastRewardTimestamp == 0)
            {
                return true; // First time playing
            }

            DateTime lastRewardDate = DateTime.FromBinary(lastRewardTimestamp);
            return (now - lastRewardDate).TotalDays >= 1;
        }

        public void ClaimReward()
        {
            if (!IsRewardAvailable()) return;

            DateTime now = DateTime.UtcNow;
            long lastRewardTimestamp = SaveManager.Instance.Data.lastDailyRewardTimestamp;
            DateTime lastRewardDate = DateTime.FromBinary(lastRewardTimestamp);

            // Check and update streak
            if ((now - lastRewardDate).TotalDays < 2) // Less than 48 hours passed
            {
                SaveManager.Instance.Data.dailyRewardStreak++;
            }
            else
            {
                SaveManager.Instance.Data.dailyRewardStreak = 1; // Reset streak
            }

            int streak = SaveManager.Instance.Data.dailyRewardStreak;
            int rewardAmount = dailyRewardAmount + (streak * streakBonusMultiplier);

            // Add reward to player's currency
            CurrencyManager.Instance.AddCoins(rewardAmount);

            // Update save data
            SaveManager.Instance.Data.lastDailyRewardTimestamp = now.ToBinary();
            SaveManager.Instance.SaveGame();

            // Notify UI to update
            OnDailyRewardStatus?.Invoke(false); // Reward is no longer available today
            Debug.Log($"DAILY_REWARD: Player claimed {rewardAmount} coins. Current streak: {streak}");
        }
    }
}
