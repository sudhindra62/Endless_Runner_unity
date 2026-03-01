
using UnityEngine;

/// <summary>
/// Manages player leagues, leaderboards, and promotions.
/// </summary>
public class LeagueManager : Singleton<LeagueManager>
{
    // --- Public State ---

    /// <summary>
    /// If true, submissions to the leaderboard are blocked for the current session.
    /// This is controlled by the IntegrityManager.
    /// </summary>
    public bool SubmissionsDisabled { get; private set; } = false;

    /// <summary>
    /// Called by the IntegrityManager when a suspicious session is detected.
    /// </summary>
    public void DisableSubmissionsForSession()
    {
        if (SubmissionsDisabled) return;
        
        SubmissionsDisabled = true;
        Debug.LogWarning("Leaderboard submissions have been disabled for this session.");
    }

    /// <summary>
    /// Attempts to submit a score to the leaderboard.
    /// </summary>
    public void SubmitScore(int score)
    {
        if (SubmissionsDisabled)
        {
            Debug.Log("Score submission blocked due to suspicious activity.");
            return;
        }

        // --- Placeholder for actual submission logic ---
        Debug.Log($"Submitted score {score} to the leaderboard.");
    }
}
