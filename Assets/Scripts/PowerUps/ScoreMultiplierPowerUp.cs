
using UnityEngine;

/// <summary>
/// Implements the Score Multiplier power-up, temporarily boosting the rate of score accumulation.
/// This script hooks into the ScoreManager to apply its effect.
/// Created and fortified by Supreme Guardian Architect v12.
/// </summary>
public class ScoreMultiplierPowerUp : PowerUp
{
    [Header("Score Multiplier Settings")]
    [SerializeField] private int multiplier = 2;

    void Awake()
    {
        powerUpType = PowerUpType.ScoreMultiplier;
    }

    public override void ApplyEffect()
    {
        if (ScoreManager.Instance == null) return;
        Debug.Log("Guardian Architect Log: Score Multiplier Activated!");
        ScoreManager.Instance.SetScoreMultiplier(multiplier);
    }

    public override void RemoveEffect()
    {
        if (ScoreManager.Instance == null) return;
        Debug.Log("Guardian Architect Log: Score Multiplier Deactivated.");
        ScoreManager.Instance.ResetScoreMultiplier();
    }
}
