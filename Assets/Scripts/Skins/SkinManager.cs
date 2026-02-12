using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    // 🔹 UI compatibility events
    public static event Action OnSkinChanged;
    public static event Action OnSkinUnlocked;
    public static event Action OnSkinSelected;
    public static event Action<GameObject> OnSkinEquipped;

    [SerializeField] private List<SkinData> allSkins = new();

    private const string UnlockedSkinsKey = "UnlockedSkins";
    private const string SelectedSkinKey = "SelectedSkinID";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSkins();
        }
        else Destroy(gameObject);
    }

    private void InitializeSkins()
    {
        if (allSkins.Count == 0) return;

        UnlockSkin(allSkins[0].SkinID);

        if (!IsSkinUnlocked(GetSelectedSkinID()))
            SelectSkin(allSkins[0].SkinID);
    }

    // 🔹 REQUIRED BY UI / COSMETICS
    public SkinData GetEquippedSkinData()
    {
        return GetSkinData(GetSelectedSkinID());
    }

    public string GetEquippedSkinID()
    {
        return GetSelectedSkinID();
    }

    public SkinData GetSkinData(string skinID)
    {
        return allSkins.FirstOrDefault(s => s.SkinID == skinID);
    }

    public void EquipSkin(string skinID)
    {
        SelectSkin(skinID);
        OnSkinSelected?.Invoke();
        OnSkinChanged?.Invoke();
    }

    public bool TryUnlockSkin(string skinID)
    {
        if (IsSkinUnlocked(skinID)) return false;
        UnlockSkin(skinID);
        OnSkinUnlocked?.Invoke();
        return true;
    }

    // =========================
    // 🔹 ORIGINAL CORE LOGIC (UNCHANGED)
    // =========================

    public void UnlockSkin(string skinID)
    {
        var list = GetUnlockedSkinIDs();
        if (!list.Contains(skinID))
        {
            list.Add(skinID);
            PlayerPrefs.SetString(UnlockedSkinsKey, string.Join(",", list));
        }
    }

    public void SelectSkin(string skinID)
    {
        if (!IsSkinUnlocked(skinID)) return;
        PlayerPrefs.SetString(SelectedSkinKey, skinID);
        OnSkinEquipped?.Invoke(GetSelectedSkinPrefab());
    }

    public GameObject GetSelectedSkinPrefab()
    {
        return GetEquippedSkinData()?.SkinPrefab;
    }

    public bool IsSkinUnlocked(string skinID)
    {
        return GetUnlockedSkinIDs().Contains(skinID);
    }

    public string GetSelectedSkinID()
    {
        return PlayerPrefs.GetString(
            SelectedSkinKey,
            allSkins.Count > 0 ? allSkins[0].SkinID : string.Empty
        );
    }

    private List<string> GetUnlockedSkinIDs()
    {
        var raw = PlayerPrefs.GetString(UnlockedSkinsKey, "");
        return string.IsNullOrEmpty(raw) ? new() : raw.Split(',').ToList();
    }

    // =========================
    // 🔹 SAFE COMPATIBILITY API (NO DUPLICATES)
    // =========================

    // Used by UI code expecting bool return
    public bool SelectSkinBool(string skinID)
    {
        SelectSkin(skinID);
        return true;
    }
    // =========================
// 🔹 REQUIRED BY UI / SHOP
// =========================
public List<SkinData> GetAllSkins()
{
    return allSkins != null
        ? new List<SkinData>(allSkins) // return copy for safety
        : new List<SkinData>();
}


    public bool UnlockSkinBool(string skinID)
    {
        UnlockSkin(skinID);
        return true;
    }
}
