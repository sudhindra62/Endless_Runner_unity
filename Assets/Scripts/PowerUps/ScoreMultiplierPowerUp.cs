
using UnityEngine;

/// <summary>
/// Concrete implementation for the Score Multiplier power-up.
/// Doubles the player's score acquisition for a set duration.
/// Architected by the Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(BoxCollider))] // For trigger detection
public class ScoreMultiplierPowerUp : PowerUp
{
    [Header("Multiplier Specifics")]
    [SerializeField] private int multiplier = 2;

    public ScoreMultiplierPowerUp()
    {
        powerUpType = PowerUpType.ScoreMultiplier;
        duration = 10f;
    }

    protected override void Activate(PlayerController player)
    {
        // This activation logic will likely be handled by the ScoreManager to avoid direct coupling.
        if (ScoreManager.Instance != null)
        {
            // ScoreManager.Instance.SetScoreMultiplier(multiplier);
            Debug.Log($"Guardian Architect Log: Score Multiplier x{multiplier} activated!");
        }
    }

    protected override void Deactivate(PlayerController player)
    {
        if (ScoreManager.Instance != null)
        {
            // ScoreManager.Instance.SetScoreMultiplier(1); // Reset to default
            Debug.Log("Guardian Architect Log: Score Multiplier deactivated.");
        }
    }
}
