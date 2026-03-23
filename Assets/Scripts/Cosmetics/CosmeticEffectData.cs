
using UnityEngine;

/// <summary>
/// Defines the data for a single cosmetic effect.
/// Created by Supreme Guardian Architect v12 to fulfill the A-to-Z Connectivity mandate for CosmeticEffectManager.
/// </summary>
[System.Serializable]
public class CosmeticEffectData
{
    [Tooltip("The unique identifier for this effect.")]
    public string EffectID;

    [Tooltip("The user-friendly name of the effect.")]
    public string DisplayName;

    [Tooltip("The prefab to instantiate when this effect is equipped.")]
    public GameObject EffectPrefab;
    public GameObject effectPrefab => EffectPrefab;

    public string effectID => EffectID;
    public CosmeticRarity rarity;
    public string unlockMethod;
    public CosmeticEffectType effectType;
    public string effectIDAlias => EffectID;
}
