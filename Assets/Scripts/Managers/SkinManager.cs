
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the player's owned skins and which skin is currently equipped.
/// </summary>
public class SkinManager : Singleton<SkinManager>
{
    [Header("Skin Database")]
    [SerializeField] private List<SkinData> allSkins = new List<SkinData>();

    private List<string> unlockedSkinIDs = new List<string>();
    private string equippedSkinID;

    private const string UNLOCKED_SKINS_KEY = "UnlockedSkins";
    private const string EQUIPPED_SKIN_KEY = "EquippedSkin";

    protected override void Awake()
    {
        base.Awake();
        LoadState();
    }

    public void UnlockSkin(string skinID)
    {
        if (!unlockedSkinIDs.Contains(skinID))
        {
            unlockedSkinIDs.Add(skinID);
            SaveState();
            Debug.Log($"Unlocked skin: {skinID}");
        }
    }

    public void EquipSkin(string skinID)
    {
        if (unlockedSkinIDs.Contains(skinID))
        {
            equippedSkinID = skinID;
            SaveState();
            Debug.Log($"Equipped skin: {skinID}");
            // ApplySkin(skinID); // This would call a method on the Player to change their appearance
        }
    }

    public bool IsSkinUnlocked(string skinID) => unlockedSkinIDs.Contains(skinID);
    public string GetEquippedSkinID() => equippedSkinID;
    public SkinData GetEquippedSkinData() => allSkins.FirstOrDefault(s => s.skinID == equippedSkinID);
    public List<SkinData> GetAllSkins() => allSkins;

    private void SaveState()
    {
        PlayerPrefs.SetString(UNLOCKED_SKINS_KEY, string.Join(",", unlockedSkinIDs));
        PlayerPrefs.SetString(EQUIPPED_SKIN_KEY, equippedSkinID);
    }

    private void LoadState()
    {
        string unlocked = PlayerPrefs.GetString(UNLOCKED_SKINS_KEY, "");
        if (!string.IsNullOrEmpty(unlocked))
        {
            unlockedSkinIDs = unlocked.Split(',').ToList();
        }

        // Ensure all default skins are unlocked
        foreach (var skin in allSkins.Where(s => s.isDefault))
        {
            if (!unlockedSkinIDs.Contains(skin.skinID))
            {
                unlockedSkinIDs.Add(skin.skinID);
            }
        }

        equippedSkinID = PlayerPrefs.GetString(EQUIPPED_SKIN_KEY, allSkins.FirstOrDefault(s => s.isDefault)?.skinID);
    }

    /*
    private void ApplySkin(string skinID)
    {
        SkinData skinToApply = allSkins.FirstOrDefault(s => s.skinID == skinID);
        if (skinToApply != null)
        {
            // Example: Find the player and swap its model prefab
            // PlayerController player = FindObjectOfType<PlayerController>();
            // if (player != null) { player.SetSkin(skinToApply.playerPrefab); }
        }
    }
    */
}
