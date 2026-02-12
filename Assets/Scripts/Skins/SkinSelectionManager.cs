
using UnityEngine;
using System;

/// <summary>
/// A singleton manager responsible for tracking the currently selected skin.
/// It persists the selection using PlayerPrefs and ensures that only unlocked skins can be selected.
/// </summary>
public class SkinSelectionManager : MonoBehaviour
{
    public static SkinSelectionManager Instance { get; private set; }

    private const string SelectedSkinKey = "SELECTED_SKIN_ID";
    private const string DefaultSkinId = "default_runner"; // Fallback skin

    public static event Action<string> OnSkinSelected; // Passes skinId

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
    /// Selects a skin to be used. The selection is validated against the unlock manager.
    /// If the skin is not unlocked, the selection is denied.
    /// </summary>
    /// <returns>True if the skin was successfully selected, false otherwise.</returns>
    public bool SelectSkin(string skinId)
    {
        if (!SkinUnlockManager.Instance.IsSkinUnlocked(skinId))
        {
            Debug.LogWarning($"Attempted to select locked skin: {skinId}. Selection denied.");
            return false;
        }

        PlayerPrefs.SetString(SelectedSkinKey, skinId);
        PlayerPrefs.Save();
        
        Debug.Log($"Skin Selected: {skinId}");

        // FUTURE HOOK: The character visual system will listen to this event to swap the model/texture.
        OnSkinSelected?.Invoke(skinId);
        
        return true;
    }

    /// <summary>
    /// Gets the ID of the currently selected skin.
    /// If no skin has been selected or the saved one is invalid, it returns the default skin ID.
    /// </summary>
    public string GetSelectedSkinId()
    {
        string savedSkinId = PlayerPrefs.GetString(SelectedSkinKey, DefaultSkinId);

        // Validate that the saved skin is still unlocked (in case of data corruption or changes).
        if (!SkinUnlockManager.Instance.IsSkinUnlocked(savedSkinId))
        {
            return DefaultSkinId;
        }

        return savedSkinId;
    }
    
    /// <summary>
    /// Gets the full definition of the currently selected skin.
    /// </summary>
    public SkinDefinition GetSelectedSkin()
    {
        string selectedId = GetSelectedSkinId();
        return SkinUnlockManager.Instance.skinCatalog.GetSkinById(selectedId);
    }
}
