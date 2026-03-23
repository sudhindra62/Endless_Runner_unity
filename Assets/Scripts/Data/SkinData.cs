
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Endless Runner/Skin Data")]
public class SkinData : ScriptableObject
{
    [Tooltip("Unique identifier for this skin.")]
    public string skinID;

    [Tooltip("The player prefab with the visual representation of this skin.")]
    public GameObject playerPrefab;

    [Tooltip("The name of the skin to be displayed in the UI.")]
    public string skinName;

    [Tooltip("The description of the skin to be displayed in the UI.")]
    public string skinDescription;

    [Tooltip("The cost of the skin in coins.")]
    public int coinCost;

    [Tooltip("The cost of the skin in gems.")]
    public int gemCost;

    [Tooltip("The rarity of the skin.")]
    public CosmeticRarity rarity;

    [Tooltip("The method used to unlock this skin.")]
    public SkinUnlockType unlockType;

    [Tooltip("Is this the default skin? There should only be one.")]
    public bool isDefault = false;
    
    [Tooltip("Sprite for UI display and preview.")]
    public Sprite sprite; // ADDED: Missing sheet/icon
    
    [Tooltip("Character art for full-body preview.")]
    public Sprite characterArt; // ADDED: Missing character visual
    
    [Tooltip("Price value (alias for gemCost).")]
    public int price; // ADDED: Price alias field
}
