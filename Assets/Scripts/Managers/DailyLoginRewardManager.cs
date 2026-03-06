
using System;
using UnityEngine;

/// <summary>
/// Manages the daily login reward system.
/// Tracks consecutive login days and grants rewards to improve retention.
/// </summary>
public class DailyLoginRewardManager : Singleton<DailyLoginRewardManager>
{
    [Serializable]
    public struct DailyReward
    {
        public int coins;
        public int gems;
        public int xp; // Added XP
    }

    [Header("Configuration")]
    [SerializeField] private DailyReward[] rewards; // A cycle of rewards (e.g., 7 days)
    [SerializeField] private bool resetOnCycleEnd = true; // Does the cycle reset after the last reward?

    private int consecutiveDays;
    private DateTime lastLoginDate;

    public bool IsRewardAvailable { get; private set; }

    private const string LAST_LOGIN_KEY = "LastLoginDate";
    private const string CONSECUTIVE_DAYS_KEY = "ConsecutiveDays";

    protected override void Awake()
    {
        base.Awake();
        LoadState();
        CheckRewardStatus();
    }

    private void CheckRewardStatus()
    {
        DateTime today = DateTime.UtcNow.Date;

        if (lastLoginDate == today)
        {
            // Already logged in today, no new reward.
            IsRewardAvailable = false;
            return;
        }

        if ((today - lastLoginDate).Days == 1)
        {
            // Consecutive day login.
            consecutiveDays++;
        }
        else
        {
            // The streak is broken.
            consecutiveDays = 1;
        }

        IsRewardAvailable = true;
    }

    /// <summary>
    /// Claims the reward for the current day.
    /// </summary>
    public void ClaimReward()
    {
        if (!IsRewardAvailable)
        {
            Debug.LogWarning("No daily reward available to be claimed.");
            return;
        }

        int rewardIndex = (consecutiveDays - 1) % rewards.Length;
        if (consecutiveDays > rewards.Length && !resetOnCycleEnd)
        {
            // If the cycle doesn't reset, give the last reward repeatedly.
            rewardIndex = rewards.Length - 1;
        }
        
        DailyReward reward = rewards[rewardIndex];
        
        // Grant reward through a centralized system
        CurrencyManager.Instance.AddCoins(reward.coins);
        CurrencyManager.Instance.AddGems(reward.gems);
        PlayerProgression.Instance.AddXP(reward.xp); // Grant XP
        
        Debug.Log($"Claimed Day {consecutiveDays} reward: {reward.coins} coins, {reward.gems} gems, {reward.xp} XP.");

        // Update state
        IsRewardAvailable = false;
        lastLoginDate = DateTime.UtcNow.Date;
        SaveState();
        
        // You would typically show a UI panel with the reward details here.
    }

    public int GetCurrentDayInStreak() => consecutiveDays;
    public DailyReward GetTodayRewardPreview() => rewards[(consecutiveDays - 1) % rewards.Length];

    private void SaveState()
    {
        PlayerPrefs.SetString(LAST_LOGIN_KEY, lastLoginDate.ToBinary().ToString());
        PlayerPrefs.SetInt(CONSECUTIVE_DAYS_KEY, consecutiveDays);
    }

    private void LoadState()
    {
        if (PlayerPrefs.HasKey(LAST_LOGIN_KEY))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString(LAST_LOGIN_KEY));
            lastLoginDate = DateTime.FromBinary(temp);
        }
        else
        {
            // First time playing, set to yesterday to grant reward today.
            lastLoginDate = DateTime.UtcNow.Date.AddDays(-1);
        }

        consecutiveDays = PlayerPrefs.GetInt(CONSECUTIVE_DAYS_KEY, 0);
    }
}
