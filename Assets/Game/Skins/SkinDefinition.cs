
using System;

/// <summary>
/// A serializable data class that holds the blueprint for a single character skin.
/// This is a plain C# object, making it easy to serialize for catalogs or save games.
/// It does not contain any logic, only data.
/// </summary>
[Serializable]
public class SkinDefinition
{
    public string skinId;          // Unique identifier, e.g., "player_knight"
    public string displayName;     // User-facing name, e.g., "Sir Reginald"
    public SkinRarity rarity;      // Common, Rare, Epic, Legendary
    public SkinUnlockType unlockType; // How the skin is unlocked (Coins, Gems, etc.)
    public int unlockCost;         // The amount of currency required, if any.
    public bool isDefaultUnlocked; // True if the skin is available from the start.

    // FUTURE HOOK: A reference to the actual character prefab would go here.
    // public GameObject characterPrefab;

    public SkinDefinition(string id, string name, SkinRarity rarity, SkinUnlockType type, int cost, bool defaultUnlock = false)
    {
        this.skinId = id;
        this.displayName = name;
        this.rarity = rarity;
        this.unlockType = type;
        this.unlockCost = cost;
        this.isDefaultUnlocked = defaultUnlock;
    }
}
