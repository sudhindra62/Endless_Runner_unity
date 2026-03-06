
using UnityEngine;
using System.Collections.Generic;

public class CosmeticInventoryManager : Singleton<CosmeticInventoryManager>
{
    private List<string> unlockedCosmetics = new List<string>();
    private Dictionary<CosmeticEffectType, string> equippedCosmetics = new Dictionary<CosmeticEffectType, string>();

    protected override void Awake()
    {
        base.Awake();
        LoadInventory();
    }

    public void UnlockCosmetic(string effectID)
    {
        if (!unlockedCosmetics.Contains(effectID))
        {
            unlockedCosmetics.Add(effectID);
            Debug.Log($"Unlocked cosmetic: {effectID}");
            SaveInventory();
        }
    }

    public bool IsCosmeticUnlocked(string effectID)
    {
        return unlockedCosmetics.Contains(effectID);
    }

    public void EquipCosmetic(string effectID, CosmeticEffectType effectType)
    {
        if (IsCosmeticUnlocked(effectID))
        {
            equippedCosmetics[effectType] = effectID;
            Debug.Log($"Equipped {effectID} for {effectType}");
            SaveInventory();

            // Notify the CosmeticEffectManager to update the visual effect
            CosmeticEffectManager.Instance.OnCosmeticEquipped(effectID, effectType);
        }
    }

    public string GetEquippedCosmetic(CosmeticEffectType effectType)
    {
        if (equippedCosmetics.TryGetValue(effectType, out string effectID))
        {
            return effectID;
        }
        return null;
    }

    private void SaveInventory()
    {
        // In a real game, this would save to PlayerPrefs or a backend service
    }

    private void LoadInventory()
    {
        // In a real game, this would load from PlayerPrefs or a backend service
    }

    public List<string> GetUnlockedCosmetics() => unlockedCosmetics;
}
