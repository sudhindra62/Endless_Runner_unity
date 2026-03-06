using UnityEngine;

/// <summary>
/// Validates the integrity of a single game run session.
/// Checks for score anomalies, excessive revives, and time manipulation.
/// </summary>
public class SessionValidator
{
    // A reasonable upper bound for score per second.
    // This should be tuned based on game balancing and can be fetched from Remote Config.
    private const float THEORETICAL_MAX_SCORE_PER_SECOND = 150f;

    /// <summary>
    /// Validates the final run data against established rules.
    /// </summary>
    /// <param name="runSessionData">The data from the completed run.</param>
    /// <param name="maxRevivesAllowed">The maximum number of revives allowed for this run.</param>
    /// <returns>True if the session data is valid, false otherwise.</returns>
    public bool ValidateRun(RunSessionData runSessionData, int maxRevivesAllowed)
    {
        // 1. Score Validation: Score must be plausible for the run duration and multiplier.
        // We add a small buffer to the max theoretical score to avoid false positives from legitimate, exceptional gameplay.
        float buffer = 1.2f;
        float maxTheoreticalScore = runSessionData.distance * THEORETICAL_MAX_SCORE_PER_SECOND * runSessionData.highestMultiplier * buffer;

        if (runSessionData.score > maxTheoreticalScore && runSessionData.score > 1000) // Don't flag very low scores
        {
            IntegrityManager.Instance.ReportError($"Score validation failed. Score: {runSessionData.score}, Max Theoretical: {maxTheoreticalScore}");
            return false;
        }

        // 2. Revive Validation: Player cannot use more revives than the configured limit.
        if (runSessionData.reviveCount > maxRevivesAllowed)
        {
            IntegrityManager.Instance.ReportError($"Revive validation failed. Revives used: {runSessionData.reviveCount}, Max allowed: {maxRevivesAllowed}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the current time scale is valid. Detects speed hacks.
    /// </summary>
    /// <returns>True if Time.timeScale is at or below the normal value (1.0f).</returns>
    public bool IsTimeScaleValid()
    {
        // Allow for minor floating point inaccuracies and deliberate slow-motion effects.
        if (Time.timeScale > 1.05f)
        {
             return false;
        }
        return true;
    }
}
