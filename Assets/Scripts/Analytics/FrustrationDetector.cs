
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This component analyzes a player's session data to determine if they are likely
/// becoming frustrated. It uses a set of simple, lightweight rules to make this
/// determination. This detector is a key part of the adaptive difficulty loop.
/// </summary>
public class FrustrationDetector : MonoBehaviour
{
    [Header("Frustration Thresholds")]
    [Tooltip("Number of deaths in a short time to be considered 'quick deaths'")]
    [SerializeField] private int quickDeathThreshold = 3;
    [Tooltip("The time window (in seconds) to check for quick deaths.")]
    [SerializeField] private float quickDeathTimeWindow = 60f; // 1 minute
    [Tooltip("The maximum combo streak to be considered 'low'")]
    [SerializeField] private int lowComboStreakThreshold = 5;
    [Tooltip("A session shorter than this (in seconds) after a death is a frustration signal")]
    [SerializeField] private float shortSessionAfterDeathThreshold = 30f;

    public bool IsPlayerFrustrated { get; private set; }

    private List<float> deathTimestamps = new List<float>();

    /// <summary>
    /// The main analysis function. It checks the provided session data against the frustration rules.
    /// This should be called periodically or at the end of a session.
    /// </summary>
    public void AnalyzeSession(SessionAnalyticsData sessionData)
    {
        if (sessionData == null) return;

        IsPlayerFrustrated = false; // Reset before analysis

        // RULE 1: Check for quick, successive deaths using the timestamped list.
        if (CheckForQuickDeaths())
        {
            IsPlayerFrustrated = true;
            Debug.Log("Frustration Detected: Quick, successive deaths.");
            return; 
        }

        // RULE 2: Check for a consistently low combo streak in a reasonably long session.
        if (sessionData.ComboPeak <= lowComboStreakThreshold && sessionData.SessionDuration > 120f)
        {
            IsPlayerFrustrated = true;
            Debug.Log("Frustration Detected: Low combo peak despite long playtime.");
            return;
        }
    }

    /// <summary>
    /// Called by the PlayerAnalyticsManager whenever a player dies.
    /// </summary>
    /// <param name="timestamp">The Time.time of the death event.</param>
    public void OnPlayerDeath(float timestamp)
    {
        deathTimestamps.Add(timestamp);
        // Remove old timestamps that are outside the frustration window
        deathTimestamps.RemoveAll(t => timestamp - t > quickDeathTimeWindow);

        // After adding the new death, analyze the current state
        CheckForQuickDeaths();
    }

    private bool CheckForQuickDeaths()
    {
        // If we have enough deaths within the time window, the player is likely frustrated.
        if (deathTimestamps.Count >= quickDeathThreshold)
        {
            IsPlayerFrustrated = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Called at the end of a session to perform a final frustration check.
    /// </summary>
    public void OnSessionEnd(SessionAnalyticsData sessionData)
    {
        if (sessionData.WasAbruptlyEnded && sessionData.DeathCount > 0 && sessionData.SessionDuration < shortSessionAfterDeathThreshold)
        {
            // If the player died and quit within a short time, it's a strong frustration signal.
            IsPlayerFrustrated = true;
            Debug.Log("Frustration Detected: Player quit shortly after dying.");
        }
    }

    /// <summary>
    /// Resets the detector's state for the next session.
    /// </summary>
    public void ResetFrustrationState()
    {
        IsPlayerFrustrated = false;
        deathTimestamps.Clear();
    }
}
