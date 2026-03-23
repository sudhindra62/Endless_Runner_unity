
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
/// The SUPREME manager for daily login rewards. It handles streaks, anti-cheat, and reward granting.
/// This script has absorbed all functionality from the redundant DailyLoginRewardManager and RewardManager.
/// </summary>
public class DailyLoginManager : Singleton<DailyLoginManager>
{
    public static event Action<int> OnLoginStreakChanged;

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

        if (currentDate < lastLoginDate)
        {
            Debug.LogWarning("System time appears to have been wound back. Daily login reward will not be granted.");
            return;
        }

        if (IsRewardClaimedForDate(currentDate))
        {
            Debug.Log("Daily reward for today has already been claimed.");
            return;
        }

        if (currentDate > lastLoginDate)
        {
            if (SaveManager.Instance == null) return;
            int streak = SaveManager.Instance.Data.loginStreak;

            if (lastLoginDate != DateTime.MinValue && (currentDate - lastLoginDate).TotalDays == 1.0)
            {
                streak++;
            }
            else
            {
                streak = 1;
            }

            OnLoginStreakChanged?.Invoke(streak);
            GrantStreakReward(streak);

            if (SpinWheelManager.Instance != null)
            {
                SpinWheelManager.Instance.ResetDailySpins();
            }

            SaveManager.Instance.Data.lastLoginTimestamp = currentDate.ToBinary();
            SaveManager.Instance.Data.loginStreak = streak;
            MarkRewardAsClaimedForDate(currentDate);
            SaveManager.Instance.SaveGame();
        }
    }

    private void GrantStreakReward(int streak)
    {
        if (streakRewards.Length == 0)
        {
            Debug.LogWarning("No daily rewards have been configured.");
            return;
        }

        int rewardIndex = (streak - 1) % streakRewards.Length;
        DailyReward reward = streakRewards[rewardIndex];

        // ◈ MERGED: Directly grant rewards to the relevant managers ◈
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(reward.Coins);
            CurrencyManager.Instance.AddGems(reward.Gems);
        }

        if (PlayerProgression.Instance != null)
        {
            PlayerProgression.Instance.AddXP(reward.Xp, "DailyLogin");
        }

        Debug.Log($"Daily login reward granted for streak day {streak}: {reward.Coins} coins, {reward.Gems} gems, {reward.Xp} XP.");
    }

    private DateTime GetLastLoginDate()
    {
        if (SaveManager.Instance == null || SaveManager.Instance.Data.lastLoginTimestamp == 0)
        {
            return DateTime.MinValue;
        }
        return DateTime.FromBinary(SaveManager.Instance.Data.lastLoginTimestamp).Date;
    }
    
    private bool IsRewardClaimedForDate(DateTime date)
    {
        string dateKey = date.ToString("yyyy-MM-dd");
        return SaveManager.Instance != null && SaveManager.Instance.Data.claimedDailyRewards.ContainsKey(dateKey);
    }

    private void MarkRewardAsClaimedForDate(DateTime date)
    {
        if (SaveManager.Instance == null) return;
        string dateKey = date.ToString("yyyy-MM-dd");
        if (!SaveManager.Instance.Data.claimedDailyRewards.ContainsKey(dateKey))
        {
            SaveManager.Instance.Data.claimedDailyRewards.Add(dateKey, true);
        }
    }
    
    [ContextMenu("Reset Daily Login")]
    public void ResetDailyLogin()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Data.lastLoginTimestamp = 0;
            SaveManager.Instance.Data.loginStreak = 0;
            SaveManager.Instance.Data.claimedDailyRewards.Clear();
            SaveManager.Instance.SaveGame();
        }
        Debug.Log("Daily login data has been reset.");
    }
}
