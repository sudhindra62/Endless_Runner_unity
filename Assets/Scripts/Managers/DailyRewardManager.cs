
using System;
using System.Collections.Generic;
using EndlessRunner.Themes;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public enum DailyRewardType { Gems, Coins, TemporaryTheme }

    [System.Serializable]
    public struct Reward
    {
        public DailyRewardType rewardType;
        public int amount;
        public ThemeSO temporaryTheme; // Only used for TemporaryTheme rewards
        public int temporaryThemeDurationHours; // Duration for which the theme is unlocked
    }

    public class DailyRewardManager : MonoBehaviour
    {
        public static DailyRewardManager Instance;

        public List<Reward> possibleRewards;

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
            if (!PlayerPrefs.HasKey(LastRewardTimeKey))
            {
                return true;
            }

            string lastRewardTimeString = PlayerPrefs.GetString(LastRewardTimeKey);
            DateTime lastRewardTime = DateTime.Parse(lastRewardTimeString);

            return (DateTime.Now - lastRewardTime).TotalHours >= RewardIntervalHours;
        }

        public Reward GetRandomReward()
        {
            if (possibleRewards.Count == 0)
            {
                // Default reward if none are set up
                return new Reward { rewardType = DailyRewardType.Gems, amount = 100 };
            }

            return possibleRewards[UnityEngine.Random.Range(0, possibleRewards.Count)];
        }

        public void ClaimReward(Reward reward)
        {
            if (!IsRewardAvailable()) return;

            GiveReward(reward);

            PlayerPrefs.SetString(LastRewardTimeKey, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }

        private void GiveReward(Reward reward)
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
                    if (reward.temporaryTheme != null)
                    {
                        string unlockTimeKey = TempThemeUnlockTimeKey + reward.temporaryTheme.themeName;
                        string themeNameKey = TempThemeNameKey + reward.temporaryTheme.themeName;
                        PlayerPrefs.SetString(unlockTimeKey, DateTime.Now.ToString());
                        PlayerPrefs.SetString(themeNameKey, reward.temporaryTheme.themeName);
                        PlayerPrefs.SetInt(reward.temporaryTheme.themeName + "_temp_duration", reward.temporaryThemeDurationHours);
                        PlayerPrefs.Save();
                        Debug.Log($"Daily reward claimed! Temporary unlock of {reward.temporaryTheme.themeName} for {reward.temporaryThemeDurationHours} hours.");
                    }
                    break;
            }
        }

        public bool IsThemeTemporarilyUnlocked(ThemeSO theme)
        {
            string unlockTimeKey = TempThemeUnlockTimeKey + theme.themeName;
            if (PlayerPrefs.HasKey(unlockTimeKey))
            {
                string unlockTimeString = PlayerPrefs.GetString(unlockTimeKey);
                DateTime unlockTime = DateTime.Parse(unlockTimeString);
                int durationHours = PlayerPrefs.GetInt(theme.themeName + "_temp_duration", 0);

                if ((DateTime.Now - unlockTime).TotalHours < durationHours)
                {
                    return true;
                }
                else
                {
                    // Clean up expired keys
                    PlayerPrefs.DeleteKey(unlockTimeKey);
                    PlayerPrefs.DeleteKey(TempThemeNameKey + theme.themeName);
                    PlayerPrefs.DeleteKey(theme.themeName + "_temp_duration");
                    return false;
                }
            }
            return false;
        }
    }
}
