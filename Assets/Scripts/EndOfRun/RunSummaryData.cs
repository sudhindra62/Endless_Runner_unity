
using System;

/// <summary>
/// A serializable data class that stores the finalized results of a single gameplay run.
/// This is a plain C# object, making it easy to pass between systems or prepare for UI display.
/// It does not contain any logic, only data.
/// </summary>
[Serializable]
public class RunSummaryData
{
    // --- Base Stats from the Run ---
    public int coinsEarned;         // Coins collected during this specific run.
    public float distanceRun;       // Final distance achieved.
    public float timeSurvived;      // Total seconds the run lasted.

    // --- Calculated Rewards & Bonuses ---
    public int xpEarned;            // Total XP calculated from run performance.
    public float bonusMultiplier;   // The multiplier applied to rewards (e.g., from an ad).

    // --- Metadata ---
    public DateTime runCompletedAt; // Timestamp for when the run ended.

    public RunSummaryData()
    {
        runCompletedAt = DateTime.UtcNow;
        bonusMultiplier = 1.0f;
    }
}
