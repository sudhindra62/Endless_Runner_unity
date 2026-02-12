
using UnityEngine;

/// <summary>
/// A component responsible for constructing a RunSummaryData object at the end of a run.
/// It gathers raw data from various sources like RunSessionData and prepares it for calculation and display.
/// </summary>
public class RunSummaryBuilder : MonoBehaviour
{
    [SerializeField] private RunSessionData runSessionData;

    // Note: xpManager was previously found but not used. It's removed for now to clean up the code.
    // If it's needed later, it should also be a serialized field.

    /// <summary>
    /// Creates and returns a RunSummaryData object populated with the latest run stats.
    /// </summary>
    public RunSummaryData BuildSummary()
    {
        if (runSessionData == null)
        {
            Debug.LogError("RunSessionData not found. Cannot build run summary.");
            return new RunSummaryData(); // Return an empty summary to avoid nulls.
        }

        var summary = new RunSummaryData
        {
            coinsEarned = runSessionData.CoinsCollectedThisRun,
            distanceRun = runSessionData.DistanceThisRun,
            timeSurvived = runSessionData.TimeThisRun,
            // XP is calculated later, so we initialize it to 0 for now.
            xpEarned = 0, 
            // The default multiplier is set in the constructor.
        };

        // FUTURE HOOK: Analytics can capture this raw summary data right here.
        // e.g., AnalyticsService.RecordRunSummary(summary);
        
        return summary;
    }
}
