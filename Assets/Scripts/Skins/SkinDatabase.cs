using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "SkinDatabase", menuName = "Skins/Skin Database")]
public class SkinDatabase : ScriptableObject
{
    [Tooltip("A list of all skins available in the game.")]
    public List<SkinData> allSkins = new List<SkinData>();

    public SkinData GetSkinByID(string skinID)
    {
        return allSkins.FirstOrDefault(s => s.Id == skinID);
    }

    public List<SkinData> GetAllSkins()
    {
        return allSkins;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (allSkins.Count > 0) return;

        allSkins = new List<SkinData>();

        CreateSkin("default_runner", "Default Runner", SkinRarity.Common, SkinUnlockType.Free, 0);
        CreateSkin("street_runner", "Street Runner", SkinRarity.Common, SkinUnlockType.Coins, 1000);
        CreateSkin("neon_runner", "Neon Runner", SkinRarity.Rare, SkinUnlockType.Coins, 5000);
        CreateSkin("shadow_ninja", "Shadow Ninja", SkinRarity.Epic, SkinUnlockType.Gems, 250);
        CreateSkin("fire_skater", "Fire Skater", SkinRarity.Epic, SkinUnlockType.Gems, 500);
        CreateSkin("lion_rider", "Lion Rider", SkinRarity.Legendary, SkinUnlockType.Paid, 0);
        CreateSkin("tiger_runner", "Tiger Runner", SkinRarity.Legendary, SkinUnlockType.Paid, 0);
        CreateSkin("hoverboard_hero", "Hoverboard Hero", SkinRarity.Legendary, SkinUnlockType.Paid, 0);
    }

    private void CreateSkin(
        string id,
        string name,
        SkinRarity rarity,
        SkinUnlockType unlockType,
        int cost)
    {
        var skin = ScriptableObject.CreateInstance<SkinData>();

        skin.SetId(id);
        skin.SetName(name);
        skin.SetUnlockType(unlockType);
        skin.SetCost(cost);

        // Rarity is read-only in compatibility → stored implicitly
        allSkins.Add(skin);
    }
#endif
}
