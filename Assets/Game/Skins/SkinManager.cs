using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the player's skins, including unlocking, selecting, and equipping them.
/// This is the single authority for all skin-related operations.
/// </summary>
public partial class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    // UI compatibility events
    public static event Action OnSkinChanged;
    public static event Action OnSkinUnlocked;
    public static event Action OnSkinSelected;
    public static event Action<GameObject> OnSkinEquipped;

    [SerializeField]
    private SkinDatabase skinDatabase;

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
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSkins()
    {
        if (skinDatabase.GetAllSkins().Count == 0) return;

        // Unlock the first skin by default
        UnlockSkin(skinDatabase.GetAllSkins()[0].Id);

        // If the selected skin is not unlocked, select the first skin
        if (!IsSkinUnlocked(GetSelectedSkinID()))
        {
            SelectSkin(skinDatabase.GetAllSkins()[0].Id);
        }
    }

    /// <summary>
    /// Gets the SkinData for the currently equipped skin.
    /// </summary>
    public SkinData GetEquippedSkinData()
    {
        return GetSkinData(GetSelectedSkinID());
    }

    /// <summary>
    /// Gets the ID of the currently equipped skin.
    /// </summary>
    public string GetEquippedSkinID()
    {
        return GetSelectedSkinID();
    }

    /// <summary>
    /// Gets the SkinData for a specific skin ID.
    /// </summary>
    public SkinData GetSkinData(string skinID)
    {
        return skinDatabase.GetSkinByID(skinID);
    }

    /// <summary>
    /// Equips a skin. This is an alias for SelectSkin.
    /// </summary>
    public void EquipSkin(string skinID)
    {
        SelectSkin(skinID);
        OnSkinSelected?.Invoke();
        OnSkinChanged?.Invoke();
    }

    /// <summary>
    /// Attempts to unlock a skin. Returns true if the skin was successfully unlocked.
    /// </summary>
    public bool TryUnlockSkin(string skinID)
    {
        if (IsSkinUnlocked(skinID)) return false;
        UnlockSkin(skinID);
        OnSkinUnlocked?.Invoke();
        return true;
    }

    /// <summary>
    /// Unlocks a skin.
    /// </summary>
    public void UnlockSkin(string skinID)
    {
        var list = GetUnlockedSkinIDs();
        if (!list.Contains(skinID))
        {
            list.Add(skinID);
            PlayerPrefs.SetString(UnlockedSkinsKey, string.Join(",", list));
        }
    }

    /// <summary>
    /// Selects a skin. The skin must be unlocked.
    /// </summary>
    public void SelectSkin(string skinID)
    {
        if (!IsSkinUnlocked(skinID)) return;
        PlayerPrefs.SetString(SelectedSkinKey, skinID);
        OnSkinEquipped?.Invoke(GetSelectedSkinPrefab());
    }

    /// <summary>
    /// Gets the prefab for the currently selected skin.
    /// </summary>
    public GameObject GetSelectedSkinPrefab()
    {
        return GetEquippedSkinData()?.SkinPrefab;
    }

    /// <summary>
    /// Checks if a skin is unlocked.
    /// </summary>
    public bool IsSkinUnlocked(string skinID)
    {
        return GetUnlockedSkinIDs().Contains(skinID);
    }

    /// <summary>
    /// Gets the ID of the selected skin.
    /// </summary>
    public string GetSelectedSkinID()
    {
        return PlayerPrefs.GetString(
            SelectedSkinKey,
            skinDatabase.GetAllSkins().Count > 0 ? skinDatabase.GetAllSkins()[0].Id : string.Empty
        );
    }

    /// <summary>
    /// Gets a list of all unlocked skin IDs.
    /// </summary>
    private List<string> GetUnlockedSkinIDs()
    {
        var raw = PlayerPrefs.GetString(UnlockedSkinsKey, "");
        return string.IsNullOrEmpty(raw) ? new() : raw.Split(',').ToList();
    }

    /// <summary>
    /// Compatibility API for UI code expecting a boolean return.
    /// </summary>
    public bool SelectSkinBool(string skinID)
    {
        SelectSkin(skinID);
        return true;
    }
    
    /// <summary>
    /// Required by UI/Shop. Returns a copy of the list for safety.
    /// </summary>
    public List<SkinData> GetAllSkins()
    {
        return skinDatabase != null
            ? new List<SkinData>(skinDatabase.GetAllSkins()) // return copy for safety
            : new List<SkinData>();
    }

    /// <summary>
    /// Unlocks a skin and returns true.
    /// </summary>
    public bool UnlockSkinBool(string skinID)
    {
        UnlockSkin(skinID);
        return true;
    }
}
