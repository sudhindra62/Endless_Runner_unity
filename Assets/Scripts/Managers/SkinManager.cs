
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    [Header("Skin Database")]
    [SerializeField] private List<SkinData> allSkins = new List<SkinData>();

    public static event Action<SkinData> OnSkinEquipped;
    public static event Action<string> OnSkinChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ShopManager.OnSkinPurchased += UnlockSkin;
    }

    private void OnDestroy()
    {
        ShopManager.OnSkinPurchased -= UnlockSkin;
    }

    public void UnlockSkin(string skinID)
    {
        if (SaveManager.Instance == null) return;
        
        if (!SaveManager.Instance.Data.unlockedSkins.Contains(skinID))
        {
            SaveManager.Instance.Data.unlockedSkins.Add(skinID);
            SaveManager.Instance.SaveGame();
            Debug.Log($"Unlocked skin: {skinID}");
        }
    }

    public void EquipSkin(string skinID)
    {
        if (SaveManager.Instance == null) return;

        if (IsSkinUnlocked(skinID))
        {
            SaveManager.Instance.Data.activeTheme = skinID; // Using activeTheme as skin ID carrier
            SaveManager.Instance.SaveGame();
            Debug.Log($"Equipped skin: {skinID}");
            
            SkinData skinToApply = GetSkinData(skinID);
            if (skinToApply != null)
            {
                OnSkinEquipped?.Invoke(skinToApply);
            }
            OnSkinChanged?.Invoke(skinID);
        }
    }

    public bool IsSkinUnlocked(string skinID) => SaveManager.Instance != null && SaveManager.Instance.Data.unlockedSkins.Contains(skinID);
    public string GetEquippedSkinID() => SaveManager.Instance != null ? SaveManager.Instance.Data.activeTheme : "";
    public SkinData GetEquippedSkinData() => GetSkinData(GetEquippedSkinID());
    public List<SkinData> GetAllSkins() => allSkins;
    public SkinData GetSkinData(string skinID) => allSkins.FirstOrDefault(s => s.skinID == skinID);

    public void SetActiveSkin(SkinData skin)
    {
        if (skin != null) EquipSkin(skin.skinID);
    }

    public SkinData GetActiveSkin() => GetEquippedSkinData();

    public int GetSkinPrice(SkinData skin)
    {
        return skin != null ? skin.price : 0;
    }

    public void SelectSkin(string skinID)
    {
        if (IsSkinUnlocked(skinID)) EquipSkin(skinID);
    }

    // --- Type Conversion Overloads (Phase 2A: Type Consistency) ---

    public void UnlockSkin(SkinData skin)
    {
        if (skin != null) UnlockSkin(skin.skinID);
    }

    public void EquipSkin(SkinData skin)
    {
        if (skin != null) EquipSkin(skin.skinID);
    }

    public bool IsSkinUnlocked(SkinData skin)
    {
        return skin != null && IsSkinUnlocked(skin.skinID);
    }

    public int GetSkinPrice(string skinID)
    {
        var skin = GetSkinData(skinID);
        return GetSkinPrice(skin);
    }

    public string GetSkinName(SkinData skin)
    {
        return skin != null ? skin.skinID : "";
    }

    public string GetSelectedSkinID() => GetEquippedSkinID();

    // Note: State is managed by SaveManager. LoadState/SaveState removed to enforce Single Source of Truth [R32].
}
