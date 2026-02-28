
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authoritative singleton for managing player skins.
/// Handles unlocking, equipping, and applying skins to the player model.
/// Persists all skin data to PlayerPrefs.
/// </summary>
public class SkinManager : Singleton<SkinManager>
{
    public static event Action<SkinData> OnSkinEquipped;

    [SerializeField] private SkinDatabase skinDatabase;
    [SerializeField] private SkinData defaultSkin;

    public SkinData EquippedSkin { get; private set; }
    public List<string> UnlockedSkinIds { get; } = new List<string>();

    private const string UnlockedSkinsKey = "UnlockedSkins";
    private const string EquippedSkinKey = "EquippedSkin";

    protected override void Awake()
    {
        base.Awake();
        LoadSkinData();
    }

    private void LoadSkinData()
    {
        // Load unlocked skins
        string unlockedSkinsString = PlayerPrefs.GetString(UnlockedSkinsKey, string.Empty);
        if (!string.IsNullOrEmpty(unlockedSkinsString))
        {
            UnlockedSkinIds.AddRange(unlockedSkinsString.Split(','));
        }

        // Ensure default skin is always unlocked
        if (!IsSkinUnlocked(defaultSkin.SkinId))
        {
            UnlockSkin(defaultSkin.SkinId);
        }

        // Load equipped skin
        string equippedSkinId = PlayerPrefs.GetString(EquippedSkinKey, defaultSkin.SkinId);
        EquipSkin(GetSkinById(equippedSkinId));
    }

    private void SaveSkinData()
    {
        PlayerPrefs.SetString(UnlockedSkinsKey, string.Join(",", UnlockedSkinIds));
        PlayerPrefs.SetString(EquippedSkinKey, EquippedSkin.SkinId);
        PlayerPrefs.Save();
    }

    public bool IsSkinUnlocked(string skinId)
    {
        return UnlockedSkinIds.Contains(skinId);
    }

    public void UnlockSkin(string skinId)
    {
        if (IsSkinUnlocked(skinId)) return;

        UnlockedSkinIds.Add(skinId);
        SaveSkinData();
        Debug.Log($"Unlocked skin: {skinId}");
    }

    public void EquipSkin(SkinData skin)
    {
        if (skin == null || !IsSkinUnlocked(skin.SkinId)) return;

        EquippedSkin = skin;
        OnSkinEquipped?.Invoke(skin);
        SaveSkinData();
        Debug.Log($"Equipped skin: {skin.SkinName}");
    }

    public SkinData GetSkinById(string skinId)
    {
        return skinDatabase.Skins.Find(s => s.SkinId == skinId);
    }
}
