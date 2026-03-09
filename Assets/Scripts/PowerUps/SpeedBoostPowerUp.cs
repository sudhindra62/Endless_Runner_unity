using UnityEngine;

/// <summary>
/// This power-up increases the player's forward speed for a short duration.
/// It interacts with the GameDifficultyManager to apply a speed boost.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/SpeedBoost")]
public class SpeedBoostPowerUp : PowerUp
{
    public float speedMultiplier = 1.5f;
    
    public override void Activate(GameObject player)
    {
        GameDifficultyManager difficultyManager = ServiceLocator.Get<GameDifficultyManager>();
        if (difficultyManager != null)
        {
            difficultyManager.SetSpeedBoostMultiplier(speedMultiplier);
            Debug.Log($"Speed Boost Activated! Multiplier: {speedMultiplier}");
        }
    }

    public override void Deactivate(GameObject player)
    {
        GameDifficultyManager difficultyManager = ServiceLocator.Get<GameDifficultyManager>();
        if (difficultyManager != null)
        {
            difficultyManager.SetSpeedBoostMultiplier(1.0f); // Reset to default
            Debug.Log("Speed Boost Deactivated!");
        }
    }
}
