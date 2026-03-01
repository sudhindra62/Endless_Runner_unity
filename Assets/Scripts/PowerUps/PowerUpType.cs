/// <summary>
/// Defines the distinct types of power-ups available in the game.
/// This enum is used for managing power-up activation, deactivation, and fusion logic.
/// </summary>
public enum PowerUpType
{
    None,
    Shield,
    Magnet,
    CoinDoubler,
    ScoreMultiplier,
    SpeedBoost,
    FeverMode, // Added for UltraCombo fusion

    // Fused Power-Ups
    CoinStorm,
    SurgeProtector,
    Overdrive,
    InvincibleDash,
    UltraCombo
}
