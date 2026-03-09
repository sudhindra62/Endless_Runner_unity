namespace Achievements
{
    /// <summary>
    /// Defines unique, immutable identifiers for all achievements in the game.
    /// This enum is the single source of truth for identifying achievements, preventing errors from string-based lookups.
    /// </summary>
    public enum AchievementID
    {
        // --- RUN-BASED ACHIEVEMENTS ---
        FirstRunComplete,
        TenRunsComplete,
        FiftyRunsComplete,
        
        // --- SCORE-BASED ACHIEVEMENTS ---
        Score10000Points,
        Score50000Points,
        Score250000Points,
        
        // --- COIN-BASED ACHIEVEMENTS ---
        Collect100CoinsInRun,
        Collect5000CoinsTotal,
        
        // --- MISC. ACHIEVEMENTS ---
        FirstPowerUpUsed,
        FirstNearMiss,
        FirstReviveUsed
    }
}
