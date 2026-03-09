
using UnityEngine;

/// <summary>
/// Concrete implementation for the Speed Boost power-up.
/// Increases the player's forward movement speed for a set duration.
/// Engineered by the Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(BoxCollider))] // For trigger detection
public class SpeedBoostPowerUp : PowerUp
{
    [Header("Speed Boost Specifics")]
    [SerializeField] private float speedMultiplier = 1.5f;
    
    public SpeedBoostPowerUp()
    {
        powerUpType = PowerUpType.SpeedBoost;
        duration = 7f;
    }

    protected override void Activate(PlayerController player)
    {
        if (player != null)
        {
            player.ApplySpeedBoost(speedMultiplier);
        }
    }

    protected override void Deactivate(PlayerController player)
    {
        if (player != null)
        {
            player.ApplySpeedBoost(1f); // Reset to default speed
        }
    }
}
