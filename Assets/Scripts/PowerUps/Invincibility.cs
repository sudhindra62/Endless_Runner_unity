
using UnityEngine;

/// <summary>
/// Concrete implementation for the Invincibility power-up.
/// Makes the player invulnerable to all obstacles for a set duration.
/// Forged by the Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(BoxCollider))] // For trigger detection
public class Invincibility : PowerUp
{
    public Invincibility()
    {
        powerUpType = PowerUpType.Invincibility;
        duration = 8f;
    }

    protected override void Activate(PlayerController player)
    {
        if (player != null)
        {
            player.SetInvincible(true);
            // You could also activate a visual effect on the player here.
            // Example: player.GetComponent<PlayerVFXController>().ActivateInvincibilityEffect();
        }
    }

    protected override void Deactivate(PlayerController player)
    {
        if (player != null)
        {
            player.SetInvincible(false);
            // And deactivate the visual effect.
            // Example: player.GetComponent<PlayerVFXController>().DeactivateInvincibilityEffect();
        }
    }
}
