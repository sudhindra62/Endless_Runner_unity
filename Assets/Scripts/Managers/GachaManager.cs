
using UnityEngine;
using LootLocker.Requests;
using System;

public class GachaManager : Singleton<GachaManager>
{
    public static event Action<LootLockerCommonAsset> OnGachaResult;

    // --- CONFIGURATION --
    [Header("Gacha Settings")]
    [Tooltip("The ID of the LootLocker context for the gacha pool.")]
    [SerializeField] private int gachaContextID = 31; // Defaulted to the hardcoded value found in MainMenu.

    public void PerformGachaPull()
    {
        LootLockerCommonAsset fallbackAsset = new LootLockerCommonAsset
        {
            name = $"Gacha Reward {gachaContextID}",
            id = gachaContextID
        };

        Debug.Log($"GachaManager: Pull simulated. Result: {fallbackAsset.name}");
        OnGachaResult?.Invoke(fallbackAsset);
    }
}
