using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RunModifierData", menuName = "Gameplay/Run Modifier Data")]
public class RunModifierData : ScriptableObject
{
    public string modifierId;
    public string displayName;
    [TextArea] public string description;
    public ModifierType modifierType;
    public float modifierValue;
    public int difficultyScore; // Replaces riskLevel for more granular control
    public bool isStackable;

    [Tooltip("List of modifier types that cannot be active at the same time as this one.")]
    public List<ModifierType> incompatibleWith = new List<ModifierType>();
}

public enum ModifierType
{
    // PlayerMovement Modifiers
    SpeedMultiplier,
    GravityModifier,
    JumpDisabled,
    ReverseInput,

    // Spawner/Difficulty Modifiers
    CoinDensityIncrease,
    ObstacleDensityIncrease,

    // Scoring/Combo Modifiers
    StyleBonusMultiplier,
    ComboTimeoutReduction,

    // FeverMode Modifiers
    FeverChargeBoost,

    // PowerUp Modifiers
    PowerUpDurationMultiplier,

    // Visual Modifiers
    VisionFog,

    // Skill Tree and LiveOps Merged
    None,
    MagnetRadiusBoost,
    ShieldDurationBoost,
    CoinDoublerDurationBoost,
    CoinValueBoost,
    StyleBonusBoost,
    ComboTimeoutBoost,
    FeverDurationBoost,
    FusionDurationBoost,
    ReviveCostReduction,
    SpeedCapIncrease,
    DifficultyReduction,
    BaseSpeedIncrease,
    Difficulty,
    PowerUpDuration,
    DropRate,
    AdFrequency,
    ReviveCost,
    BossInterval,
    LeagueThreshold,
    RiskReward
}
