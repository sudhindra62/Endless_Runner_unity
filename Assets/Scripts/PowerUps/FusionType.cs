
/// <summary>
/// Defines the different types of power-up fusions available in the game.
/// </summary>
public enum FusionType
{
    /// <summary>
    /// No fusion is currently active.
    /// </summary>
    None,

    /// <summary>
    /// The fusion of Magnet and Coin Doubler.
    /// Effects: Stronger magnet, 3x coin value, increased coin spawns.
    /// </summary>
    CoinStorm,

    /// <summary>
    /// The fusion of Shield and a speed boost.
    /// Effects: Full invincibility, forward acceleration, high score multiplier bonus.
    /// </summary>
    InvincibleDash,

    /// <summary>
    /// The fusion of a score multiplier and Fever Mode.
    /// Effects: Increased multiplier cap, doubled style bonus, extended combo timeout.
    /// </summary>
    UltraCombo
}
