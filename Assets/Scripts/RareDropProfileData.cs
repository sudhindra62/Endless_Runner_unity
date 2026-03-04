
using UnityEngine;

[CreateAssetMenu(fileName = "RareDropProfileData", menuName = "Rare Drops/Drop Profile")]
public class RareDropProfileData : ScriptableObject
{
    [Header("Rarity Tier Definition")]
    public string rarityName;
    public Color rarityColor = Color.white;

    [Header("Core Drop Modifiers")]
    [Tooltip("Base probability for this rarity to be selected for a drop. Must be between 0 and 1.")]
    [Range(0f, 1f)]
    public float baseDropChance = 0.1f;

    [Tooltip("Minimum run score required before this rarity can even be considered.")]
    public int minRunRequirement = 0;

    [Header("Dynamic Boost Multipliers")]
    [Tooltip("Multiplier applied based on game difficulty. Higher difficulty = higher chance.")]
    [Range(1f, 2f)]
    public float difficultyModifierMultiplier = 1.0f;

    [Tooltip("Multiplier applied during active live events. Capped by RareDropEngine.")]
    [Range(1f, 3f)]
    public float eventBoostMultiplier = 1.0f;

    [Tooltip("Slight boost based on the player's competitive league tier.")]
    [Range(1f, 1.5f)]
    public float leagueBoostMultiplier = 1.0f;

    [Header("Pity System")]
    [Tooltip("If an item of this rarity or higher isn't dropped after this many runs, a special boost or guarantee is triggered.")]
    public int pityThreshold = 0; // 0 means no pity system for this tier
}
