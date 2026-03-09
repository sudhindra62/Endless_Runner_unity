
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
        LootLockerSDKManager.GetSingleAssetInstances(gachaContextID, (response) =>
        {
            if (response.success)
            {
                if (response.instances.Length > 0)
                {
                    var asset = response.instances[0];
                    Debug.Log($"GachaManager: Pull successful. Result: {asset.asset.name}");
                    OnGachaResult?.Invoke(asset);
                }
                else
                {
                    Debug.LogWarning("GachaManager: Gacha pull was successful but returned no instances.");
                }
            }
            else
            {
                Debug.LogError("GachaManager: Gacha pull failed: " + response.Error);
                OnGachaResult?.Invoke(null); // Signal a failure to listeners
            }
        });
    }
}
