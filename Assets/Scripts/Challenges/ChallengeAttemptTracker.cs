
using System;
using UnityEngine;

/// <summary>
/// Tracks the number of attempts a player has made for the current Daily Challenge.
/// Handles the daily reset of attempt counts.
/// </summary>
public class ChallengeAttemptTracker : Singleton<ChallengeAttemptTracker>
{
    private const int MAX_ATTEMPTS = 3;

    private int attemptsMade;
    private string lastAttemptDateId;

    private void Start()
    {
        // In a real game, this would be loaded from a persistent save file.
        // LoadState();
    }

    /// <summary>
    /// Checks if the player has any attempts remaining for today's challenge.
    /// Also handles the automatic reset if the day has changed.
    /// </summary>
    public bool HasAttemptsRemaining()
    {
        // Check if the challenge day has changed since the last attempt.
        if (lastAttemptDateId != DailyChallengeSeedGenerator.GetCurrentDayChallengeId())
        {
            // The day has changed, so reset the attempts.
            attemptsMade = 0;
            lastAttemptDateId = DailyChallengeSeedGenerator.GetCurrentDayChallengeId();
            // SaveState(); // Persist the reset.
        }

        return attemptsMade < MAX_ATTEMPTS;
    }

    /// <summary>
    /// Records that an attempt has been made.
    /// </summary>
    public void RecordAttempt()
    {
        if (!HasAttemptsRemaining()) return; // Should not happen if checked before run starts

        attemptsMade++;
        // SaveState(); // Persist the change
        Debug.Log($"Daily Challenge attempt {attemptsMade}/{MAX_ATTEMPTS} recorded for {lastAttemptDateId}.");
    }

    /// <summary>
    /// Adds an extra attempt, typically from watching a rewarded ad.
    /// </summary>
    public void AddExtraAttempt()
    {
        // This allows one extra attempt beyond the max.
        if (attemptsMade > 0) // Cannot get an extra attempt if you haven't used one yet.
        {
            attemptsMade--;
            // SaveState();
            Debug.Log("Extra attempt granted.");
        }
    }

    public int GetAttemptsMade()
    {
        // Ensure the state is up-to-date with the current day.
        HasAttemptsRemaining(); 
        return attemptsMade;
    }
    
    public int GetMaxAttempts()
    {
        return MAX_ATTEMPTS;
    }

    // --- Persistence (Example) ---
    // private void SaveState()
    // {
    //     PlayerPrefs.SetInt("DailyChallenge_Attempts", attemptsMade);
    //     PlayerPrefs.SetString("DailyChallenge_DateId", lastAttemptDateId);
    // }

    // private void LoadState()
    // {
    //     attemptsMade = PlayerPrefs.GetInt("DailyChallenge_Attempts", 0);
    //     lastAttemptDateId = PlayerPrefs.GetString("DailyChallenge_DateId", string.Empty);
    // }
}
