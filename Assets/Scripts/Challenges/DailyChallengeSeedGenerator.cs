
using System;

/// <summary>
/// Generates a unique, deterministic seed for the Daily Challenge based on the current UTC date.
/// Ensures all players worldwide receive the exact same seed for the same day.
/// </summary>
public static class DailyChallengeSeedGenerator
{
    private const string SEED_PREFIX = "DC-";

    /// <summary>
    /// Generates the integer seed value for the procedural engine based on the current UTC date.
    /// </summary>
    /// <returns>A deterministic integer seed.</returns>
    public static int GetCurrentDaySeed()
    {
        DateTime today = DateTime.UtcNow.Date;
        // Use the date's ticks as a simple, unique, and deterministic integer.
        // More complex hashing could be used, but this is sufficient and performant.
        return (int)(today.Ticks % int.MaxValue);
    }

    /// <summary>
    /// Gets the string identifier for the current day's challenge, used for leaderboards.
    /// </summary>
    /// <returns>A string like "DC-2026-04-01".</returns>
    public static string GetCurrentDayChallengeId()
    {
        return SEED_PREFIX + DateTime.UtcNow.ToString("yyyy-MM-dd");
    }
}
