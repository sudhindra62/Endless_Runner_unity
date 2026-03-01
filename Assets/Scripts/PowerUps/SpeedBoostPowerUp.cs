
using UnityEngine;

/// <summary>
/// This power-up increases the player's forward speed for a short duration.
/// It interacts with the GameDifficultyManager to apply a speed boost.
/// </summary>
public class SpeedBoostPowerUp : PowerUpEffect
{
    private readonly float speedMultiplier;
    private GameDifficultyManager difficultyManager;

    public SpeedBoostPowerUp(float duration, float multiplier) : base(duration)
    {
        this.speedMultiplier = multiplier;
        this.difficultyManager = ServiceLocator.Get<GameDifficultyManager>();
    }

    public override void Activate()
    {
        base.Activate();
        if (difficultyManager != null)
        {
            difficultyManager.SetSpeedBoostMultiplier(speedMultiplier);
            Debug.Log($"Speed Boost Activated! Multiplier: {speedMultiplier}");
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (difficultyManager != null)
        {
            difficultyManager.SetSpeedBoostMultiplier(1.0f); // Reset to default
            Debug.Log("Speed Boost Deactivated!");
        }
    }
}
