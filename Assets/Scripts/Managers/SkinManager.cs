using UnityEngine;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    // This would be loaded from a SaveManager
    private HashSet<string> unlockedSkins = new HashSet<string>();

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

    public void UnlockSkin(string skinID)
    {
        if (unlockedSkins.Add(skinID))
        {
            Debug.Log($"Skin Unlocked: {skinID}");
            // Here you would likely save the updated set of unlocked skins
            // e.g., SaveManager.Instance.SavePlayerData();
        }
    }

    public bool IsSkinUnlocked(string skinID)
    {
        return unlockedSkins.Contains(skinID);
    }
}
