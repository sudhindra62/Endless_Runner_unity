
using System;
using UnityEngine;

[Serializable]
public struct DailyReward
{
    public int Coins;
    public int Gems;
    public int Xp;
}

/// <summary>
/// Manages daily login rewards and streaks. It is the authority for determining when a daily reward should be granted.
/// It includes protection against time manipulation by checking against the last saved login time.
/// </summary>
public class DailyLoginManager : Singleton<DailyLoginManager>
{
    [Tooltip("A list of rewards for each consecutive day of logging in. The list size determines the streak cycle.")]
    [SerializeField] private DailyReward[] streakRewards;

    private const string LastLoginKey = "DailyLogin_LastLoginTime";
    private const string LoginStreakKey = "DailyLogin_LoginStreak";
    private const string LastRewardClaimedKeyPrefix = "DailyLogin_LastRewardClaimed_";

    private void Start()
    {
        CheckDailyLogin();
    }

    private void CheckDailyLogin()
    {
        DateTime lastLoginDate = GetLastLoginDate();
        DateTime currentDate = DateTime.UtcNow.Date;

        // Anti-cheat: If the current time is earlier than the last recorded login, the user might be manipulating the system time.
        if (currentDate < lastLoginDate)
        {
            Debug.LogWarning("System time appears to have been wound back. Daily login reward will not be granted.");
            return;
        }

        // Check if a reward has already been claimed for the current date.
        if (IsRewardClaimedForDate(currentDate))
        {
            Debug.Log("Daily reward for today has already been claimed.");
            return;
        }

        if (currentDate > lastLoginDate)
        {
            int streak = PlayerPrefs.GetInt(LoginStreakKey, 0);

            // If the last login was yesterday, continue the streak. Otherwise, reset it.
            if ((currentDate - lastLoginDate).TotalDays == 1.0)
            {
                streak++;
            }
            else
            {
                streak = 1;
            }

            // Grant reward
            GrantStreakReward(streak);

            // Save state
            PlayerPrefs.SetString(LastLoginKey, currentDate.ToString("o"));
            PlayerPrefs.SetInt(LoginStreakKey, streak);
            MarkRewardAsClaimedForDate(currentDate);
            PlayerPrefs.Save();
        }
    }

    private void GrantStreakReward(int streak)
    {
        if (streakRewards.Length == 0)
        {
            Debug.LogWarning("No daily rewards have been configured.");
            return;
        }

        // The streak cycles through the available rewards.
        int rewardIndex = (streak - 1) % streakRewards.Length;
        DailyReward reward = streakRewards[rewardIndex];

        RewardManager.Instance.GrantDailyLoginReward(streak, reward.Coins, reward.Gems, reward.Xp);
        Debug.Log($"Daily login reward granted for streak day {streak}.");
    }

    private DateTime GetLastLoginDate()
    {
        string dateString = PlayerPrefs.GetString(LastLoginKey, null);
        if (string.IsNullOrEmpty(dateString))
        {
            return DateTime.MinValue;
        }
        return DateTime.Parse(dateString).Date;
    }
    
    private bool IsRewardClaimedForDate(DateTime date)
    {
        return PlayerPrefs.GetInt(LastRewardClaimedKeyPrefix + date.ToString("yyyy-MM-dd"), 0) == 1;
    }

    private void MarkRewardAsClaimedForDate(DateTime date)
    {
        PlayerPrefs.SetInt(LastRewardClaimedKeyPrefix + date.ToString("yyyy-MM-dd"), 1);
    }
    
    [ContextMenu("Reset Daily Login")]
    public void ResetDailyLogin()
    {
        PlayerPrefs.DeleteKey(LastLoginKey);
        PlayerPrefs.DeleteKey(LoginStreakKey);
        Debug.Log("Daily login data has been reset.");
    }
}
