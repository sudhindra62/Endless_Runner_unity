
using UnityEngine;

/// <summary>
/// The base class for all power-up collectibles.
/// Defines the type, duration, and the activation/deactivation logic.
/// Synthesized by Supreme Guardian Architect v12.
/// </summary>
public abstract class PowerUp : MonoBehaviour
{
    [Header("Power-Up Configuration")]
    [SerializeField] protected PowerUpType powerUpType;
    [SerializeField] protected float duration = 10f;

    public PowerUpType GetPowerUpType() => powerUpType;
    public float GetDuration() => duration;

    /// <summary>
    /// Abstract method to be implemented by each specific power-up type.
    /// This is called by the PowerupManager when the power-up is collected.
    /// </summary>
    /// <param name="player">The player who collected the power-up.</param>
    public abstract void TriggerActivation(PlayerController player);

    /// <summary>
    /// Abstract method to be implemented by each specific power-up type.
    /// This is called by the PowerupManager when the power-up duration expires.
    /// </summary>
    /// <param name="player">The player who had the power-up.</param>
    public abstract void TriggerDeactivation(PlayerController player);
}

// Enum to define all possible power-up types
public enum PowerUpType
{
    Shield,
    SpeedBoost,
    ScoreMultiplier,
    CoinMagnet
}
