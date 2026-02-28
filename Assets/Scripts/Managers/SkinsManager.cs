
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines the rarity of a skin, affecting its price and unlock conditions.
/// </summary>
public enum SkinRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

/// <summary>
/// Represents a single unlockable skin, including its visual representation and properties.
/// </summary>
[System.Serializable]
public class SkinData
{
    public string skinName;
    public Sprite skinIcon;
    public GameObject skinPrefab;
    public SkinRarity rarity;
    public int price;
    public bool isUnlocked;
}

/// <summary>
/// Manages unlockable skins, including purchasing, applying, and previewing.
/// </summary>
public class SkinsManager : MonoBehaviour
{
    [Header("Skin Configuration")]
    [SerializeField] private List<SkinData> skins = new List<SkinData>();

    private string selectedSkinName;

    /// <summary>
    /// Unlocks a skin for the player.
    /// </summary>
    public bool UnlockSkin(string skinName)
    {
        SkinData skin = skins.Find(s => s.skinName == skinName);
        if (skin != null && !skin.isUnlocked)
        {
            skin.isUnlocked = true;
            return true; // Unlock successful
        }
        return false; // Skin not found or already unlocked
    }

    /// <summary>
    /// Applies the selected skin to the player.
    /// </summary>
    public void ApplySkin(string skinName)
    {
        selectedSkinName = skinName;
        // Additional logic to apply the skin to the player model
    }

    /// <summary>
    /// Previews a skin in the UI.
    /// </summary>
    public GameObject GetSkinPreview(string skinName)
    {
        SkinData skin = skins.Find(s => s.skinName == skinName);
        return skin?.skinPrefab;
    }
}
