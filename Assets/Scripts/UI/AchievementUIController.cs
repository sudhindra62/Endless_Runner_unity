
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the UI display for all achievements.
/// Populates a list with achievement entries and updates them based on manager data.
/// Logic fully restored and fortified by Supreme Guardian Architect v12.
/// </summary>
[AddComponentMenu("UI/Achievements/Achievement UI Controller")]
public class AchievementUIController : MonoBehaviour
{
    [Header("UI Prefabs & Containers")]
    [Tooltip("The prefab for a single achievement UI element. Must have an AchievementUIEntry component.")]
    [SerializeField] private AchievementUIEntry achievementEntryPrefab;

    [Tooltip("The Transform parent where the achievement entry prefabs will be instantiated.")]
    [SerializeField] private Transform contentParent;

    // --- PRIVATE STATE ---
    private List<AchievementUIEntry> _uiEntries = new List<AchievementUIEntry>();
    private bool _isPopulated = false;

    #region Unity Lifecycle

    private void Start()
    {
        // --- ERROR_HANDLING_POLICY: Validate all essential references ---
        if (achievementEntryPrefab == null || contentParent == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: UI Prefab or Content Parent not assigned in AchievementUIController Inspector. Disabling component.", this);
            enabled = false;
            return;
        }

        // The list is populated once, and then refreshed on enable.
        PopulateAchievementList();
    }

    private void OnEnable()
    {
        // Refresh the UI'''s progress state every time the panel becomes visible.
        RefreshUI();
    }

    #endregion

    /// <summary>
    /// Instantiates UI elements for each achievement defined in the AchievementManager.
    /// This method adheres to the SINGLE_SOURCE_OF_TRUTH principle.
    /// </summary>
    private void PopulateAchievementList()
    {
        if (_isPopulated) return;

        // --- SINGLE_SOURCE_OF_TRUTH: Get all achievement data directly from the manager ---
        var allAchievements = AchievementManager.Instance.GetAllAchievements();

        if (allAchievements == null || !allAchievements.Any())
        {
            Debug.LogWarning("Guardian Architect Warning: No achievements found in AchievementManager. The UI will be empty.", this);
            return;
        }

        // Clear any placeholder children from the editor
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        _uiEntries.Clear();

        // --- DYNAMIC INSTANTIATION & SETUP ---
        foreach (var achievement in allAchievements)
        {
            AchievementUIEntry newEntry = Instantiate(achievementEntryPrefab, contentParent);
            newEntry.Setup(achievement); // Setup the static data
            _uiEntries.Add(newEntry);
        }

        _isPopulated = true;
        Debug.Log($"Guardian Architect: Achievement UI populated with {_uiEntries.Count} entries.");
    }

    /// <summary>
    /// Updates the visual state (progress, unlocked status) of all achievement UI entries.
    /// </summary>
    public void RefreshUI()
    {
        if (!_isPopulated)
        {
            // If the list wasn'''t populated (e.g., due to manager not being ready in Start),
            // try to populate it now.
            PopulateAchievementList();
        }

        // If still not populated, exit.
        if (!_isPopulated) return;

        // Refresh each entry with the latest progress from the manager
        foreach (var entry in _uiEntries)
        {
            entry.UpdateVisuals();
        }

        Debug.Log("Guardian Architect: Achievement UI refreshed.");
    }
}
