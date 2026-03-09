
using UnityEngine;

/// <summary>
/// Defines the minimum and maximum safety bounds for all LiveOps configuration values.
/// This ScriptableObject is used by the LiveOpsSafetyValidator to ensure that even
/// corrupted remote configs cannot break the game's economy or balance.
/// Authored by the Supreme Guardian Architect v12.
/// </summary>
[CreateAssetMenu(fileName = "LiveOpsSafetyBounds", menuName = "LiveOps/Safety Bounds")]
public class LiveOpsSafetyBounds : ScriptableObject
{
    [System.Serializable]
    public struct FloatRange
    {
        public float min;
        public float max;
    }

    [System.Serializable]
    public struct IntRange
    {
        public int min;
        public int max;
    }

    [Header("Gameplay Modifier Bounds")]
    [Tooltip("Min/Max bounds for the difficulty multiplier.")]
    public FloatRange difficultyMultiplier = new FloatRange { min = 0.5f, max = 2.0f };
    
    [Tooltip("Min/Max bounds for the power-up duration multiplier.")]
    public FloatRange powerUpDurationMultiplier = new FloatRange { min = 0.5f, max = 3.0f };
    
    [Tooltip("Min/Max bounds for the rare drop chance multiplier. This is critical for the economy.")]
    public FloatRange dropRateMultiplier = new FloatRange { min = 1.0f, max = 1.5f };
    
    [Tooltip("Min/Max bounds for the risk lane reward multiplier.")]
    public FloatRange riskLaneRewardMultiplier = new FloatRange { min = 1.0f, max = 2.5f };

    [Header("Economy Bounds")]
    [Tooltip("Min/Max bounds for the gem cost of a revive.")]
    public IntRange reviveGemCost = new IntRange { min = 0, max = 1000 };

    [Tooltip("Min/Max bounds for the ad frequency modifier.")]
    public FloatRange adFrequencyModifier = new FloatRange { min = 0.1f, max = 5.0f };

    [Header("Content & Event Bounds")]
    [Tooltip("Min/Max bounds for the boss spawn interval in minutes.")]
    public FloatRange bossSpawnIntervalMinutes = new FloatRange { min = 5.0f, max = 120.0f };
    
    [Tooltip("Min/Max percentage adjustment for league point thresholds.")]
    public FloatRange leagueThresholdAdjustment = new FloatRange { min = -0.25f, max = 0.25f };
}
