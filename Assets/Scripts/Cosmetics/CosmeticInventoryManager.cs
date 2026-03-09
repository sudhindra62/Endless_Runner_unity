
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
        string data = string.Join(",", _unlockedEffectIDs);
        PlayerPrefs.SetString(UNLOCKED_EFFECTS_SAVE_KEY, data);
        PlayerPrefs.Save();
    }

    private void LoadUnlockedEffects()
    {
        string data = PlayerPrefs.GetString(UNLOCKED_EFFECTS_SAVE_KEY, string.Empty);
        if (!string.IsNullOrEmpty(data))
        {
            _unlockedEffectIDs = new HashSet<string>(data.Split(','));
        }
    }
}
