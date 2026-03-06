using UnityEngine;

/// <summary>
/// Data structure for passing fever mode modifier information through events.
/// This ensures that all effects are centralized and consistently applied.
/// </summary>
public class FeverModeData
{
    public float ScoreMultiplier { get; }
    public float SpeedBoost { get; }
    public bool IsInvincible { get; }

    public FeverModeData(float scoreMultiplier, float speedBoost, bool isInvincible)
    {
        ScoreMultiplier = scoreMultiplier;
        SpeedBoost = speedBoost;
        IsInvincible = isInvincible;
    }
}
