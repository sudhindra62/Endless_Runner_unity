
using System;
using System.Collections.Generic;

using UnityEngine;

    public enum DailyRewardType { Gems, Coins, TemporaryTheme }

    [System.Serializable]
    public struct DailyRewardItem
    {
        public DailyRewardType rewardType;
        public int amount;
        public ThemeSO temporaryTheme; // Only used for TemporaryTheme rewards
        public int temporaryThemeDurationHours; // Duration for which the theme is unlocked
    }

    public class DailyRewardManager : MonoBehaviour
    {
        public static DailyRewardManager Instance;

        public List<DailyRewardItem> possibleRewards;

        private const string LastRewardTimeKey = "LastRewardTime";
        private const string TempThemeUnlockTimeKey = "TempThemeUnlockTime_";
        private const string TempThemeNameKey = "TempThemeName_";
        private const int RewardIntervalHours = 24;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsRewardAvailable()
        {
            if (SaveManager.Instance == null) return false;

            if (SaveManager.Instance.Data.lastDailyRewardTimestamp == 0)
            {
                return true;
            }

            DateTime lastRewardTime = DateTime.FromBinary(SaveManager.Instance.Data.lastDailyRewardTimestamp);
            return (DateTime.Now - lastRewardTime).TotalHours >= RewardIntervalHours;
        }

        public DailyRewardItem GetRandomReward()
        {
            if (possibleRewards.Count == 0)
            {
                // Default reward if none are set up
                return new DailyRewardItem { rewardType = DailyRewardType.Gems, amount = 100 };
            }

            return possibleRewards[UnityEngine.Random.Range(0, possibleRewards.Count)];
        }

        public Reward GetRandomRewardAsReward()
        {
            DailyRewardItem reward = GetRandomReward();
            return new Reward("Daily Reward", ConvertRewardType(reward.rewardType), reward.amount, reward.rewardType.ToString())
            {
                temporaryTheme = reward.temporaryTheme,
                temporaryThemeDurationHours = reward.temporaryThemeDurationHours
            };
        }

        public void ClaimReward(DailyRewardItem reward)
        {
            if (!IsRewardAvailable() || SaveManager.Instance == null) return;

            GiveReward(reward);

            SaveManager.Instance.Data.lastDailyRewardTimestamp = DateTime.Now.ToBinary();
            SaveManager.Instance.SaveGame();
        }

        public void ClaimReward()
        {
            ClaimReward(GetRandomReward());
        }

        private void GiveReward(DailyRewardItem reward)
        {
            switch (reward.rewardType)
            {
                case DailyRewardType.Gems:
                    CurrencyManager.Instance.AddGems(reward.amount);
                    Debug.Log($"Daily reward claimed! {reward.amount} gems awarded.");
                    break;
                case DailyRewardType.Coins:
                    // Assuming you have a method to add coins in CurrencyManager
                    // CurrencyManager.Instance.AddCoins(reward.amount);
                    Debug.Log($"Daily reward claimed! {reward.amount} coins awarded.");
                    break;
                case DailyRewardType.TemporaryTheme:
                    if (reward.temporaryTheme != null && SaveManager.Instance != null)
                    {
                        SaveManager.Instance.Data.tempThemeName = reward.temporaryTheme.themeName;
                        SaveManager.Instance.Data.tempThemeUnlockTimestamp = DateTime.Now.ToBinary();
                        SaveManager.Instance.Data.tempThemeDurationHours = reward.temporaryThemeDurationHours;
                        SaveManager.Instance.SaveGame();
                        Debug.Log($"Daily reward claimed! Temporary unlock of {reward.temporaryTheme.themeName} for {reward.temporaryThemeDurationHours} hours.");
                    }
                    break;
            }
        }

        public bool IsThemeTemporarilyUnlocked(ThemeSO theme)
        {
            if (SaveManager.Instance == null || string.IsNullOrEmpty(SaveManager.Instance.Data.tempThemeName)) return false;

            if (SaveManager.Instance.Data.tempThemeName == theme.themeName)
            {
                DateTime unlockTime = DateTime.FromBinary(SaveManager.Instance.Data.tempThemeUnlockTimestamp);
                int durationHours = SaveManager.Instance.Data.tempThemeDurationHours;

                if ((DateTime.Now - unlockTime).TotalHours < durationHours)
                {
                    return true;
                }
                else
                {
                    // Clean up expired temp theme
                    SaveManager.Instance.Data.tempThemeName = "";
                    SaveManager.Instance.Data.tempThemeUnlockTimestamp = 0;
                    SaveManager.Instance.Data.tempThemeDurationHours = 0;
                    SaveManager.Instance.SaveGame();
                    return false;
                }
            }
            return false;
        }

        // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
        
        public void ClaimReward(DailyRewardItem reward, long bonusAmount = 0)
        {
            ClaimReward(reward);
            if (bonusAmount > 0 && RewardManager.Instance != null)
            {
                RewardManager.Instance.Award("COINS", bonusAmount);
            }
        }

        public DailyRewardItem CreateReward(DailyRewardType rewardType, long amount)
        {
            return new DailyRewardItem
            {
                rewardType = rewardType,
                amount = (int)System.Math.Min(amount, int.MaxValue),
                temporaryTheme = null,
                temporaryThemeDurationHours = 0
            };
        }

        public DateTime GetLastRewardTime()
        {
            if (SaveManager.Instance == null || SaveManager.Instance.Data.lastDailyRewardTimestamp == 0)
                return DateTime.MinValue;

            return DateTime.FromBinary(SaveManager.Instance.Data.lastDailyRewardTimestamp);
        }

        public double GetHoursSinceLastReward()
        {
            return (DateTime.Now - GetLastRewardTime()).TotalHours;
        }

        private static RewardType ConvertRewardType(DailyRewardType rewardType)
        {
            return rewardType switch
            {
                DailyRewardType.Gems => RewardType.Gems,
                DailyRewardType.Coins => RewardType.Coins,
                _ => RewardType.DailyBonus
            };
        }
    }

