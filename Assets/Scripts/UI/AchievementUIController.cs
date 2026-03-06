
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the UI display for all achievements.
/// Populates a list with achievement entries and updates them based on manager data.
/// </summary>
public class AchievementUIController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private AchievementManager achievementManager;

    [Header("UI Prefabs & Containers")]
    [SerializeField] private GameObject achievementEntryPrefab; // A prefab for a single achievement UI element
    [SerializeField] private Transform contentParent; // The scroll view content area

    [SerializeField] private List<AchievementData> allAchievements; // Reference to all SOs

    private List<GameObject> uiEntries = new List<GameObject>();

    private void Start()
    {
        // Ideally, get achievements from the manager to ensure a single source of truth
        PopulateAchievementList();
    }

    private void OnEnable()
    {
        // Refresh the UI when the panel is opened
        RefreshUI();
    }

    /// <summary>
    /// Instantiates UI elements for each achievement.
    /// </summary>
    void PopulateAchievementList()
    {
        if (achievementEntryPrefab == null || contentParent == null) return;

        // Clear any existing test entries
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        uiEntries.Clear();

        foreach (var achievement in allAchievements)
        {
            GameObject entryGO = Instantiate(achievementEntryPrefab, contentParent);
            // The entryGO would have its own script, e.g., AchievementUIEntry.cs,
            // to set the icon, title, description, and progress bar.
            // Example:
            // AchievementUIEntry uiEntry = entryGO.GetComponent<AchievementUIEntry>();
            // uiEntry.Setup(achievement);
            uiEntries.Add(entryGO);
        }
        
        RefreshUI();
    }

    /// <summary>
    /// Updates the visual state of all achievement UI entries.
    /// </summary>
    public void RefreshUI()
    {
        if (achievementManager == null) return;

        for (int i = 0; i < allAchievements.Count; i++)
        {
            AchievementData achievement = allAchievements[i];
            GameObject uiEntryGO = uiEntries[i];

            bool isUnlocked = achievementManager.IsAchievementUnlocked(achievement.achievementId);
            float progress = achievementManager.GetAchievementProgress(achievement.achievementId);
            float goal = achievement.goalValue;

            // Again, this logic would live within the UI entry's own script.
            // Example:
            // AchievementUIEntry uiEntry = uiEntryGO.GetComponent<AchievementUIEntry>();
            // uiEntry.UpdateProgress(progress, goal, isUnlocked);
        }
    }
}
