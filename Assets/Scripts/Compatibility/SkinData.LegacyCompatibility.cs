using UnityEngine;

/// <summary>
/// LEGACY COMPATIBILITY LAYER for SkinData
/// READ-ONLY ALIASES ONLY
/// NO DATA, NO DUPLICATES, NO SETTERS
/// </summary>
public partial class SkinData
{
    // -------------------------------------------------
    // 🔹 IDENTIFIERS (aliases only)
    // -------------------------------------------------
    public string SkinID => Id;
    public string skinID => Id;

    // -------------------------------------------------
    // 🔹 DISPLAY (aliases only)
    // -------------------------------------------------
    public string DisplayName => skinName;
    public string displayName => DisplayName;
    public string SkinName => DisplayName;

    // -------------------------------------------------
    // 🔹 ICONS / ART (legacy expects these)
    // -------------------------------------------------
    public Sprite PreviewSprite => null;
    public Sprite shopPreviewSprite => PreviewSprite;
    public Sprite skinIcon => PreviewSprite;
    public Sprite ShopIcon => PreviewSprite;
    public Sprite characterArt => null;

    // -------------------------------------------------
    // 🔹 COSMETICS (placeholders)
    // -------------------------------------------------
    public GameObject runTrailPrefab => null;
    public GameObject petPrefab => null;
    public GameObject SkinPrefab => null;

    // -------------------------------------------------
    // 🔹 ECONOMY (ALIAS — NOT REDEFINED)
    // -------------------------------------------------
    public int cost => Cost;
    public int CoinPrice => Cost;

    // -------------------------------------------------
    // 🔹 UNLOCKING (ALIAS — NOT REDEFINED)
    // -------------------------------------------------
    public SkinUnlockType unlockType => UnlockType;

    // -------------------------------------------------
    // 🔹 RARITY (legacy-safe constant)
    // -------------------------------------------------
    public SkinRarity Rarity => SkinRarity.Common;

}
