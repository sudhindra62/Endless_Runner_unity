/// <summary>
/// Defines unique, immutable identifiers for all achievements in the game.
/// Global scope.
/// </summary>
public enum AchievementID
{
    // --- RUN-BASED ---
    FirstRunComplete,
    TenRunsComplete,
    FiftyRunsComplete,
    TotalDistance,
    NoReviveRun,
    ThousandMeterClub,
    
    // --- SCORE-BASED ---
    Score10000Points,
    Score50000Points,
    Score250000Points,

    // --- COIN-BASED ---
    Collect100CoinsInRun,
    Collect5000CoinsTotal,
    TotalCoins,
    CoinCollector,

    // --- COMBO-BASED ---
    ComboPeak,

    // --- BOSS-BASED ---
    BossesDefeated,

    // --- ITEM-BASED ---
    LegendaryShards,
    PowerUpMaster,

    // --- LEAGUE-BASED ---
    DiamondLeague,

    // --- LOGIN-BASED ---
    LoginStreak,
    DailyPlayer,
    
    // --- MISC ---
    FirstPowerUpUsed,
    FirstNearMiss,
    FirstReviveUsed,
    ObstacleDodger,
    SocialButterfly
}
