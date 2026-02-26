
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A ScriptableObject that serves as a central database for all available skins in the game.
/// This allows for easy management and access to skin definitions without hardcoding them in managers.
/// 
/// --- Integration ---
/// Create an instance of this via the Asset Menu and assign it to managers that need skin data.
/// </summary>
[CreateAssetMenu(fileName = "SkinCatalog", menuName = "EndlessRunner/Skin Catalog", order = 1)]
public class SkinCatalog : ScriptableObject
{
    // This list would be populated in the Unity Inspector.
    [SerializeField]
    private List<SkinDefinition> allSkins = new List<SkinDefinition>
    {
        new SkinDefinition("default_runner", "Default Runner", SkinRarity.Common, SkinUnlockType.Free, 0, true),
        new SkinDefinition("blue_trackstar", "Blue Trackstar", SkinRarity.Common, SkinUnlockType.Coins, 500),
        new SkinDefinition("red_ninja", "Red Ninja", SkinRarity.Rare, SkinUnlockType.Coins, 2500),
        new SkinDefinition("golden_god", "Golden God", SkinRarity.Epic, SkinUnlockType.Gems, 100),
        new SkinDefinition("cyber_samurai", "Cyber Samurai", SkinRarity.Epic, SkinUnlockType.Gems, 250),
        new SkinDefinition("cosmic_wanderer", "Cosmic Wanderer", SkinRarity.Legendary, SkinUnlockType.Paid, 0), // Placeholder for IAP
        new SkinDefinition("street_punk", "Street Punk", SkinRarity.Rare, SkinUnlockType.Coins, 3000),
        new SkinDefinition("forest_guardian", "Forest Guardian", SkinRarity.Epic, SkinUnlockType.Gems, 150)
    };

    /// <summary>
    /// Returns a read-only collection of all skins available in the game.
    /// </summary>
    public IEnumerable<SkinDefinition> GetAllSkins()
    {
        return allSkins.AsReadOnly();
    }

    /// <summary>
    /// Finds and returns a specific skin definition by its unique ID.
    /// </summary>
    /// <returns>The found SkinDefinition, or null if not found.</returns>
    public SkinDefinition GetSkinById(string id)
    {
        return allSkins.FirstOrDefault(skin => skin.skinId == id);
    }
}
