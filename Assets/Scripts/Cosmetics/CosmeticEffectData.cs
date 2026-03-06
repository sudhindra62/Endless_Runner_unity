
using UnityEngine;

public enum CosmeticEffectType
{
    Trail,
    CoinPickup,
    Footstep,
    CharacterAura
}

public enum CosmeticRarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mythic
}

[System.Serializable]
public class CosmeticEffectData
{
    public string effectID;
    public string effectName;
    public CosmeticEffectType effectType;
    public CosmeticRarity rarity;
    public GameObject effectPrefab;
    public string unlockMethod; // e.g., "Shop", "BattlePass", "Event"
    public int price; // In coins or gems
    public string currencyType; // "coins" or "gems"
}
