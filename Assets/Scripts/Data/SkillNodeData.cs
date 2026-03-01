using UnityEngine;

[CreateAssetMenu(fileName = "SkillNodeData", menuName = "Endless Runner/Skill Node Data")]
public class SkillNodeData : ScriptableObject
{
    public string nodeId;
    public string displayName;
    public int maxLevel;
    
    [HideInInspector]
    public int currentLevel;

    public float baseModifierValue;
    public float incrementPerLevel;
    public ModifierType modifierType;

    public float GetCurrentModifierValue()
    {
        if (currentLevel == 0) return 0; // No bonus if not upgraded
        return baseModifierValue + (incrementPerLevel * (currentLevel - 1));
    }
}

public enum ModifierType
{
    None,
    // PowerUpManager
    MagnetRadiusBoost,
    ShieldDurationBoost,
    CoinDoublerDurationBoost, // Example for another power-up
    
    // ScoreManager
    CoinValueBoost, // Stacks with character passive
    StyleBonusBoost, // For a potential Style system

    // FlowComboManager
    ComboTimeoutBoost,

    // FeverModeManager
    FeverDurationBoost,

    // PowerUpFusionManager
    FusionDurationBoost,

    // ReviveManager
    ReviveCostReduction,

    // GameDifficultyManager
    SpeedCapIncrease,
    DifficultyReduction, // Stacks with character passive

    // PlayerMovement
    BaseSpeedIncrease
}
