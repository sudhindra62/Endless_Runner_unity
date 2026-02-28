
using UnityEngine;

/// <summary>
/// This power-up increases the player's forward speed for a short duration.
/// It directly modifies a value on the PlayerMotor, showcasing a direct stat manipulation.
/// </summary>
public class SpeedBoostPowerUp : PowerUpEffect
{
    private readonly float speedMultiplier;

    public SpeedBoostPowerUp(float duration, float multiplier) : base(duration)
    {
        this.speedMultiplier = multiplier;
    }

    public override void Activate()
    {
        base.Activate();

        PlayerController player = ServiceLocator.Get<PlayerController>();
        if (player != null)
        {
            player.PlayerMotor.SetSpeedModifier(speedMultiplier);
            Debug.Log($"Speed Boost Activated! Multiplier: {speedMultiplier}");
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        PlayerController player = ServiceLocator.Get<PlayerController>();
        if (player != null)
        {
            // Revert the speed modifier back to its default value.
            player.PlayerMotor.SetSpeedModifier(1.0f);
            Debug.Log("Speed Boost Deactivated!");
        }
    }
}
