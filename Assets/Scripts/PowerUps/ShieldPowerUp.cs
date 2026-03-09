
using UnityEngine;

/// <summary>
/// Concrete implementation for the Shield power-up.
/// Protects the player from one obstacle collision.
/// Crafted by the Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(BoxCollider))] // For trigger detection
public class ShieldPowerUp : PowerUp
{
    public ShieldPowerUp()
    {
        powerUpType = PowerUpType.Shield;
        duration = 0; // Shield is a one-time use effect, so duration is not applicable.
    }

    protected override void Activate(PlayerController player)
    {
        // The PowerupManager now handles the shield's active state.
        // This script is primarily for defining the power-up's type and properties.
        Debug.Log("Guardian Architect Log: Shield PowerUp collected. The PowerupManager will handle the logic.");
    }

    protected override void Deactivate(PlayerController player)
    {
        // Deactivation logic is managed by the Obstacle script upon collision.
    }
}
