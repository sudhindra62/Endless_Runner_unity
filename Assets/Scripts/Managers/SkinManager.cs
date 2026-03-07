
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    [Header("Skin Database")]
    [SerializeField] private List<SkinData> allSkins = new List<SkinData>();

    private List<string> unlockedSkinIDs = new List<string>();
    private string equippedSkinID;

    private const string UNLOCKED_SKINS_KEY = "UnlockedSkins";
    private const string EQUIPPED_SKIN_KEY = "EquippedSkin";

    public static event Action<SkinData> OnSkinEquipped;

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
        LoadState();
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
            
            SkinData skinToApply = GetSkinData(skinID);
            if (skinToApply != null)
            {
                OnSkinEquipped?.Invoke(skinToApply);
            }
        }
    }

    public bool IsSkinUnlocked(string skinID) => unlockedSkinIDs.Contains(skinID);
    public string GetEquippedSkinID() => equippedSkinID;
    public SkinData GetEquippedSkinData() => GetSkinData(equippedSkinID);
    public List<SkinData> GetAllSkins() => allSkins;
    public SkinData GetSkinData(string skinID) => allSkins.FirstOrDefault(s => s.skinID == skinID);

    private void SaveState()
    {
        PlayerPrefs.SetString(UNLOCKED_SKINS_KEY, string.Join(",", unlockedSkinIDs));
        PlayerPrefs.SetString(EQUIPPED_SKIN_KEY, equippedSkinID);
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        string unlocked = PlayerPrefs.GetString(UNLOCKED_SKINS_KEY, "");
        if (!string.IsNullOrEmpty(unlocked))
        {
            unlockedSkinIDs = unlocked.Split(',').ToList();
        }

        // Ensure default skins are always unlocked
        foreach (var skin in allSkins.Where(s => s.isDefault && !unlockedSkinIDs.Contains(s.skinID)))
        {
            unlockedSkinIDs.Add(skin.skinID);
        }

        equippedSkinID = PlayerPrefs.GetString(EQUIPPED_SKIN_KEY, allSkins.FirstOrDefault(s => s.isDefault)?.skinID ?? "");

        // Equip the skin at startup
        SkinData skinToApply = GetSkinData(equippedSkinID);
        if (skinToApply != null)
        {
            OnSkinEquipped?.Invoke(skinToApply);
        }
    }
}
