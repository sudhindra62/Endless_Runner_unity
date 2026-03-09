
using UnityEngine;

public class AchievementProgressTracker : MonoBehaviour
{
    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.OnGameOver += UpdateRunAchievements;
        CurrencyManager.OnPrimaryCurrencyChanged += UpdateCoinAchievements;
        DailyLoginManager.OnLoginStreakChanged += UpdateStreakAchievements;
        RareDropManager.OnRareDropAwarded += UpdateRareDropAchievements;
    }

    private void UnsubscribeFromEvents()
    {
        GameManager.OnGameOver -= UpdateRunAchievements;
        CurrencyManager.OnPrimaryCurrencyChanged -= UpdateCoinAchievements;
        DailyLoginManager.OnLoginStreakChanged -= UpdateStreakAchievements;
        RareDropManager.OnRareDropAwarded -= UpdateRareDropAchievements;
    }

    private void UpdateRunAchievements(RunSessionData runData)
    {
        AchievementManager.Instance.UpdateProgress(AchievementID.HighScorer, runData.Score);
        AchievementManager.Instance.UpdateProgress(AchievementID.CoinCollector, runData.Coins);
        AchievementManager.Instance.UpdateProgress(AchievementID.DistanceRunner, (int)runData.Distance);
    }

    private void UpdateCoinAchievements(int totalCoins)
    {
        AchievementManager.Instance.UpdateProgress(AchievementID.CoinHoarder, totalCoins);
    }

    private void UpdateStreakAchievements(int streak)
    {
        AchievementManager.Instance.UpdateProgress(AchievementID.StreakEnthusiast, streak);
    }

    private void UpdateRareDropAchievements(RareDropData drop)
    {
        if (drop.rarity == "Legendary")
        {
            AchievementManager.Instance.UpdateProgress(AchievementID.LegendaryHunter, 1);
        }
    }
}
