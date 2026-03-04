
using UnityEngine;
using System.Collections.Generic;

public class CosmeticEffectManager : MonoBehaviour
{
    public static CosmeticEffectManager Instance { get; private set; }

    // This would be loaded from a SaveManager
    private HashSet<string> unlockedEffects = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockEffect(string effectID)
    {
        if (unlockedEffects.Add(effectID))
        {
            Debug.Log($"Cosmetic Effect Unlocked: {effectID}");
            // Here you would likely save the updated set of unlocked effects
            // e.g., SaveManager.Instance.SavePlayerData();
        }
    }

    public bool IsEffectUnlocked(string effectID)
    {
        return unlockedEffects.Contains(effectID);
    }
}
