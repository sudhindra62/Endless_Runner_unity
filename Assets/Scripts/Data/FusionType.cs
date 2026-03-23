/// <summary>
/// Defines the different types of power-up fusions available in the game.
/// Global scope.
/// </summary>
public enum FusionType
{
    None,

    /// <summary>
    /// Magnet + Coin Doubler.
    /// Stronger magnet, 3x coin value, increased coin spawns.
    /// </summary>
    CoinStorm,

    /// <summary>
    /// Shield + Speed Boost.
    /// Full invincibility, forward acceleration, high score multiplier.
    /// </summary>
    InvincibleDash,

    /// <summary>
    /// Score Multiplier + Fever Mode.
    /// Increased multiplier cap, doubled style bonus.
    /// </summary>
    UltraCombo,

    FeverMode,
    FeverFrenzy,
    DoubleXP,
    TripleCoins
}
