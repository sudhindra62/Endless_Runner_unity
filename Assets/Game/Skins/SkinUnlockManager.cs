
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// A singleton manager that tracks the unlock state of all skins.
/// It uses PlayerPrefs to persist which skins the player owns between sessions.
/// </summary>
public class SkinUnlockManager : MonoBehaviour
{
    public static SkinUnlockManager Instance { get; private set; }

    [Tooltip("Assign the SkinCatalog asset here.")]
    public SkinCatalog skinCatalog;

    private const string UnlockKeyPrefix = "SKIN_UNLOCKED_";
    
    public static event Action<string> OnSkinUnlocked; // Passes skinId

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
    }

    /// <summary>
    /// Checks if a specific skin is unlocked.
    /// A skin is considered unlocked if it's a default skin or its key exists in PlayerPrefs.
    /// </summary>
    public bool IsSkinUnlocked(string skinId)
    {
        var skinDef = skinCatalog.GetSkinById(skinId);
        if (skinDef == null) return false;

        // Default skins are always considered unlocked.
        if (skinDef.isDefaultUnlocked) return true;

        return PlayerPrefs.GetInt(UnlockKeyPrefix + skinId, 0) == 1;
    }

    /// <summary>
    /// Marks a skin as unlocked and saves its state.
    /// </summary>
    public void UnlockSkin(string skinId)
    {
        if (IsSkinUnlocked(skinId)) return;

        PlayerPrefs.SetInt(UnlockKeyPrefix + skinId, 1);
        PlayerPrefs.Save();
        
        Debug.Log($"Skin Unlocked: {skinId}");

        // FUTURE HOOK: The Shop UI will listen to this to refresh the skin's state.
        OnSkinUnlocked?.Invoke(skinId);
        
        // FUTURE HOOK: Post analytics event for skin unlock.
    }

    /// <summary>
    /// Retrieves a list of all skin definitions that are currently unlocked by the player.
    /// </summary>
    public List<SkinDefinition> GetUnlockedSkins()
    {
        return skinCatalog.GetAllSkins()
                          .Where(skin => IsSkinUnlocked(skin.skinId))
                          .ToList();
    }
}
