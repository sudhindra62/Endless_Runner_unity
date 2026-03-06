
using System;

public static class AchievementProgressTracker
{
    public static void Initialize()
    {
        // Subscribe to all relevant game events here
        RunSessionManager.OnRunEnded += OnRunEnded;
        FlowComboManager.OnComboChanged += OnComboChanged;
        BossManager.OnBossDefeated += OnBossDefeated;
        RareDropManager.OnRareDropAwarded += OnRareDropAwarded;
        LeagueManager.OnPlayerLeagueChanged += OnPlayerLeagueChanged;
        DailyLoginManager.OnLoginStreakChanged += OnLoginStreakChanged;
        PlayerCoinManager.OnCoinsChanged += OnCoinsChanged;
    }

    public static void Uninitialize()
    {
        // Unsubscribe from all relevant game events here
        RunSessionManager.OnRunEnded -= OnRunEnded;
        FlowComboManager.OnComboChanged -= OnComboChanged;
        BossManager.OnBossDefeated -= OnBossDefeated;
        RareDropManager.OnRareDropAwarded -= OnRareDropAwarded;
        LeagueManager.OnPlayerLeagueChanged -= OnPlayerLeagueChanged;
        DailyLoginManager.OnLoginStreakChanged -= OnLoginStreakChanged;
        PlayerCoinManager.OnCoinsChanged -= OnCoinsChanged;
    }

    private static void OnRunEnded(RunSessionData data)
    {
        AchievementManager.Instance.UpdateAchievement(AchievementID.TotalDistance, (int)data.distance);
        if (data.reviveCount == 0)
        {
            AchievementManager.Instance.UpdateAchievement(AchievementID.NoReviveRun, 1);
        }
    }

    private static void OnComboChanged(int combo)
    {
        AchievementManager.Instance.UpdateAchievement(AchievementID.ComboPeak, combo);
    }

    private static void OnBossDefeated()
    {
        AchievementManager.Instance.UpdateAchievement(AchievementID.BossesDefeated, 1);
    }

    private static void OnRareDropAwarded(string itemID, string rarity)
    {
        if (rarity == "Legendary" && itemID.Contains("Shard"))
        {
            AchievementManager.Instance.UpdateAchievement(AchievementID.LegendaryShards, 1);
        }
    }

    private static void OnPlayerLeagueChanged(LeagueTier tier)
    {
        if (tier.LeagueName == "Diamond")
        {
            AchievementManager.Instance.UpdateAchievement(AchievementID.DiamondLeague, 1);
        }
    }

    private static void OnLoginStreakChanged(int streak)
    {
        AchievementManager.Instance.UpdateAchievement(AchievementID.LoginStreak, streak);
    }

    private static void OnCoinsChanged(int coins)
    {
        AchievementManager.Instance.UpdateAchievement(AchievementID.TotalCoins, coins);
    }
}
