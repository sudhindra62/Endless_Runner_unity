using UnityEngine;

/// <summary>
/// A plain C# class that analyzes session data to detect player frustration.
/// </summary>
public class FrustrationDetector
{
    private int quickDeathCounter;      // Deaths within a short time window
    private float lastDeathTime;
    private int reviveCounter;

    private const int QUICK_DEATH_THRESHOLD = 3;
    private const float QUICK_DEATH_WINDOW_SECONDS = 45f;
    private const int REVIVE_THRESHOLD = 2;

    /// <summary>
    /// Resets the state for a new session.
    /// </summary>
    public void Reset()
    {
        quickDeathCounter = 0;
        reviveCounter = 0;
        lastDeathTime = -1;
    }

    /// <summary>
    /// Tracks a death event.
    /// </summary>
    public void TrackDeath()
    {
        if (Time.time - lastDeathTime < QUICK_DEATH_WINDOW_SECONDS)
        {
            quickDeathCounter++;
        }
        else
        {
            quickDeathCounter = 1; // Reset counter if window has passed
        }
        lastDeathTime = Time.time;
    }

    /// <summary>
    /// Tracks a revive event.
    /// </summary>
    public void TrackRevive()
    {
        reviveCounter++;
    }
    
    /// <summary>
    /// Processes the final session data.
    /// (Currently unused, GetFrustrationScore is the primary output)
    /// </summary>
    public void ProcessSession(SessionAnalyticsData sessionData)
    {
        // This method can be used for more complex, post-session analysis if needed.
    }

    /// <summary>
    /// Calculates a normalized frustration score from 0.0 to 1.0.
    /// </summary>
    /// <returns>A float representing the player's frustration level.</returns>
    public float GetFrustrationScore()
    {
        float score = 0f;

        // Weight quick deaths heavily
        if (quickDeathCounter >= QUICK_DEATH_THRESHOLD)
        {
            score += 0.8f;
        }

        // Add weight for revives
        if (reviveCounter >= REVIVE_THRESHOLD)
        {
            score += 0.5f;
        }

        return Mathf.Clamp01(score);
    }
}
