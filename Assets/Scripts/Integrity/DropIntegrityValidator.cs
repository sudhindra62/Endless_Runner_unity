
using UnityEngine;

/// <summary>
/// Validates the integrity of a player's run data to prevent cheating for rare drops.
/// Refactored to use a ScriptableObject profile for designer-configurable values.
/// Architected by the Supreme Guardian Architect v12.
/// </summary>
public class DropIntegrityValidator : Singleton<DropIntegrityValidator>
{
    /// <summary>
    /// Validates a run against a set of integrity rules defined in a profile.
    /// </summary>
    /// <param name="runData">The data from the completed run.</param>
    /// <param name="bossDefeated">Whether the run ended by defeating a boss.</param>
    /// <param name="profile">The integrity profile to validate against.</param>
    /// <returns>True if the run is valid, false otherwise.</returns>
    public bool IsRunValid(RunSessionData runData, bool bossDefeated, DropIntegrityProfile profile)
    {
        if (profile == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: No DropIntegrityProfile provided. Run cannot be validated.");
            return false;
        }

        // 1. Validate Run Duration and Score
        if (runData.duration > 1f && (runData.score / runData.duration) > profile.MaxScorePerSecond)
        {
            Debug.LogWarning($"INTEGRITY FAIL: Score per second ({runData.score / runData.duration}) exceeds profile maximum of {profile.MaxScorePerSecond}.");
            return false;
        }

        // 2. Time Scale Verification (dependency on a future TimeControlManager)
        // This check remains a placeholder until the TimeControlManager is implemented to provide max timescale.
        if (Time.timeScale > profile.MaxTimeScale) // Placeholder
        {
            Debug.LogWarning($"INTEGRITY FAIL: Time.timeScale ({Time.timeScale}) is abnormally high. Potential speed hack.");
            return false;
        }

        // 3. Revive Abuse Check
        if (runData.reviveCount > profile.MaxReviveCount)
        {
            Debug.LogWarning($"INTEGRITY FAIL: Revive count ({runData.reviveCount}) exceeds profile maximum of {profile.MaxReviveCount}.");
            return false;
        }

        // All checks passed
        Debug.Log("<color=green>Guardian Architect: Run integrity validated successfully.</color>");
        return true;
    }
}
