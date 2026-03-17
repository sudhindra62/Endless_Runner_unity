using UnityEngine;
using System;
using EndlessRunner.Utils;

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance;

    private const string LAST_REWARD_TIME_KEY = "LastRewardTime";
    private const int REWARD_COOLDOWN_HOURS = 24;

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

    public bool CanClaimReward()
    {
        if (PlayerPrefs.HasKey(LAST_REWARD_TIME_KEY))
        {
            long lastRewardTicks = Convert.ToInt64(PlayerPrefs.GetString(LAST_REWARD_TIME_KEY));
            DateTime lastRewardTime = new DateTime(lastRewardTicks);
            return (DateTime.UtcNow - lastRewardTime).TotalHours >= REWARD_COOLDOWN_HOURS;
        }
        return true; // First time playing
    }

    public void ClaimReward()
    {
        if (CanClaimReward())
        {
            // Grant the reward
            int rewardType = UnityEngine.Random.Range(0, 3);
            switch (rewardType)
            {
                case 0: // Coins
                    CurrencyManager.Instance.AddCoins(500);
                    Debug.Log("Daily Reward: 500 Coins!");
                    break;
                case 1: // Gems
                    CurrencyManager.Instance.AddGems(50);
                    Debug.Log("Daily Reward: 50 Gems!");
                    break;
                case 2: // Temporary Theme Unlock
                    UnlockRandomThemeTemporarily(24); // Unlock for 24 hours
                    Debug.Log("Daily Reward: Temporary Theme Unlock!");
                    break;
            }

            // Save the reward time
            PlayerPrefs.SetString(LAST_REWARD_TIME_KEY, DateTime.UtcNow.Ticks.ToString());
            PlayerPrefs.Save();
        }
    }

    private void UnlockRandomThemeTemporarily(int hours)
    {
        ThemeConfig[] allThemes = Resources.LoadAll<ThemeConfig>("Themes");
        ThemeConfig randomTheme = allThemes[UnityEngine.Random.Range(0, allThemes.Length)];

        // In a real game, you would need to manage temporary unlocks separately.
        // For now, we will just unlock it permanently as a demonstration.
        ThemeUnlockManager.Instance.UnlockThemeWithGems(randomTheme); 
    }
}
