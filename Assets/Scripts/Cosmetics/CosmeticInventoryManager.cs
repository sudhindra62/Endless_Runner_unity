
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's inventory of unlocked cosmetic effects.
/// Created by Supreme Guardian Architect v12 to fulfill the A-to-Z Connectivity mandate for CosmeticEffectManager.
/// </summary>
public class CosmeticInventoryManager : Singleton<CosmeticInventoryManager>
{
    // Using a HashSet for fast lookups
    private HashSet<string> _unlockedEffectIDs = new HashSet<string>();

    private const string UNLOCKED_EFFECTS_SAVE_KEY = "UnlockedCosmeticEffects";

    protected override void Awake()
    {
        base.Awake();
        LoadUnlockedEffects();
    }

    /// <summary>
    /// Checks if a cosmetic effect is unlocked.
    /// </summary>
    public bool IsEffectUnlocked(string effectID)
    {
        return _unlockedEffectIDs.Contains(effectID);
    }
    
    public bool IsUnlocked(string effectID) => IsEffectUnlocked(effectID);
    public void EquipCosmetic(string effectID) { /* Hook for equipment UI */ }
    public bool IsCosmeticUnlocked(string effectID) => IsEffectUnlocked(effectID);
    public void EquipCosmetic(string effectID, CosmeticEffectType effectType) => EquipCosmetic(effectID);

    /// <summary>
    /// Unlocks a cosmetic effect for the player.
    /// </summary>
    public void UnlockEffect(string effectID)
    {
        if (_unlockedEffectIDs.Add(effectID))
        {
            SaveUnlockedEffects();
            Debug.Log($"<color=green>Guardian Architect: Cosmetic Effect '{effectID}' unlocked.</color>");
        }
    }

    /// <summary>
    /// Gets a list of all unlocked effect IDs.
    /// </summary>
    public List<string> GetUnlockedEffectIDs()
    {
        return new List<string>(_unlockedEffectIDs);
    }

    private void SaveUnlockedEffects()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.Data.unlockedCosmeticEffects = new List<string>(_unlockedEffectIDs);
        SaveManager.Instance.SaveGame();
    }

    private void LoadUnlockedEffects()
    {
        if (SaveManager.Instance == null) return;
        _unlockedEffectIDs = new HashSet<string>(SaveManager.Instance.Data.unlockedCosmeticEffects);
    }
}
