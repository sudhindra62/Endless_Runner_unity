
using UnityEngine;

/// <summary>
/// A non-MonoBehaviour class that performs deterministic reward calculations.
/// It takes a RunSummaryData object and computes the final rewards based on defined rules.
/// This keeps the calculation logic clean, testable, and separate from Unity's lifecycle.
/// </summary>
public class EndRunRewardCalculator
{
    private readonly RunSummaryData summary;

    // --- Reward Calculation Parameters ---
    private const float COINS_PER_METER = 0.1f;    // Grant 1 coin for every 10 meters.
    private const float XP_PER_SECOND = 2.5f;      // Grant 2.5 XP for every second survived.
    private const float XP_PER_COIN = 1.5f;      // Grant 1.5 XP for every coin collected.

    public EndRunRewardCalculator(RunSummaryData summaryData)
    {
        this.summary = summaryData;
    }

    /// <summary>
    /// Calculates the total number of coins to be awarded, including bonuses.
    /// </summary>
    /// <returns>The final coin total, multiplied by the bonus multiplier.</returns>
    public int CalculateTotalCoins()
    {
        if (summary == null) return 0;

        // Base coins are what was collected in the run.
        int baseCoins = summary.coinsEarned;
        
        // Add bonus coins from distance.
        int distanceBonusCoins = Mathf.FloorToInt(summary.distanceRun * COINS_PER_METER);
        
        int totalCoins = baseCoins + distanceBonusCoins;

        // Apply the final bonus multiplier (e.g., from a rewarded ad).
        return Mathf.RoundToInt(totalCoins * summary.bonusMultiplier);
    }

    /// <summary>
    /// Calculates the total amount of XP to be awarded.
    /// </summary>
    /// <returns>The final XP total, multiplied by the bonus multiplier.</returns>
    public int CalculateTotalXP()
    {
        if (summary == null) return 0;

        // Calculate XP from time survived and coins collected.
        float timeXP = summary.timeSurvived * XP_PER_SECOND;
        float coinXP = summary.coinsEarned * XP_PER_COIN;
        
        int totalXP = Mathf.FloorToInt(timeXP + coinXP);
        
        // Apply the final bonus multiplier.
        return Mathf.RoundToInt(totalXP * summary.bonusMultiplier);
    }

    /// <summary>
    /// A convenience method to get both final rewards at once.
    /// </summary>
    /// <returns>A tuple containing the final coin and XP amounts.</returns>
    public (int finalCoins, int finalXP) CalculateFinalRewards()
    {
        int finalCoins = CalculateTotalCoins();
        int finalXP = CalculateTotalXP();

        // Update the summary object with the calculated XP.
        summary.xpEarned = finalXP;
        
        // FUTURE HOOK: The End-Run UI will read these values from the updated summary object.

        return (finalCoins, finalXP);
    }
}
