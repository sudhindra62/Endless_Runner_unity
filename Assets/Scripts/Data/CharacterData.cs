using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Endless Runner/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterId;
    public string displayName;
    public PassiveType passiveType;
    
    // The base value is now the first entry in the list.
    public List<float> passiveValuePerLevel = new List<float>(); 

    public int maxUpgradeLevel;
    
    [HideInInspector] // Current level should be managed by the save system.
    public int currentUpgradeLevel;

    /// <summary>
    /// Gets the passive value for the current upgrade level.
    /// </summary>
    public float GetCurrentPassiveValue()
    {
        int index = Mathf.Clamp(currentUpgradeLevel, 0, passiveValuePerLevel.Count - 1);
        return passiveValuePerLevel[index];
    }
}

public enum PassiveType
{
    None,
    MagnetDurationBoost, // PowerUpManager
    CoinValueBoost, // ScoreManager
    ExtraRevive, // ReviveManager
    DifficultyReduction, // GameDifficultyManager
    ComboTimeoutIncrease, // FlowComboManager
    XPBoost, // XPManager
    StyleBonusBoost // StyleManager
}
