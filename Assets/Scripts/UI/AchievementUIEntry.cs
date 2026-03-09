
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single achievement entry in the UI list.
/// This component is responsible for displaying the achievement'''s data and its current progress.
/// Created by Supreme Guardian Architect v12 as a dependency for AchievementUIController.
/// </summary>
[AddComponentMenu("UI/Achievements/Achievement UI Entry")]
public class AchievementUIEntry : MonoBehaviour
{
    [Header("UI Element References")]
    [Tooltip("Text component for the achievement'''s name.")]
    [SerializeField] private Text achievementNameText;

    [Tooltip("Text component for the achievement'''s description.")]
    [SerializeField] private Text achievementDescriptionText;

    [Tooltip("Image component for the achievement'''s icon.")]
    [SerializeField] private Image iconImage;

    [Tooltip("Slider component to visualize the progress towards unlocking.")]
    [SerializeField] private Slider progressBar;
    
    [Tooltip("Text to display progress, e.g., '10/100'.")]
    [SerializeField] private Text progressText;

    [Tooltip("GameObject that is activated when the achievement is unlocked.")]
    [SerializeField] private GameObject unlockedOverlay;
    
    [Tooltip("The AchievementBadge component to show the tier.")]
    [SerializeField] private AchievementBadge badge;

    // --- PRIVATE STATE ---
    private AchievementData _achievementData;

    private void OnValidate()
    {
        // --- ERROR_HANDLING_POLICY: Basic validation in editor ---
        if (progressBar != null) progressBar.interactable = false;
    }

    /// <summary>
    /// Sets up the static data for this UI entry based on an AchievementData asset.
    /// This should be called once upon instantiation.
    /// </summary>
    /// <param name="data">The achievement data to display.</param>
    public void Setup(AchievementData data)
    {
        _achievementData = data;

        if (_achievementData == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: Setup called with null AchievementData.", this);
            gameObject.SetActive(false);
            return;
        }

        // --- DATA BINDING: Populate UI with static data ---
        achievementNameText.text = _achievementData.achievementName;
        achievementDescriptionText.text = _achievementData.description;
        iconImage.sprite = _achievementData.icon;
        if (badge != null) badge.SetTier(_achievementData.tier);

        // Initial visual update based on loaded progress
        UpdateVisuals();
    }

    /// <summary>
    /// Updates the dynamic elements of the UI, such as the progress bar and unlocked state.
    /// This should be called whenever the UI needs to be refreshed.
    /// </summary>
    public void UpdateVisuals()
    {
        if (_achievementData == null) return; // Not yet setup

        bool isUnlocked = AchievementManager.Instance.IsAchievementUnlocked(_achievementData.id);
        int currentProgress = AchievementManager.Instance.GetProgress(_achievementData.id);
        int requiredValue = _achievementData.requiredValue;

        // --- VISUAL LOGIC: Update progress bar and text ---
        if (progressBar != null)
        {
            // Avoid division by zero if an achievement requires 0
            progressBar.value = (requiredValue > 0) ? (float)currentProgress / requiredValue : (isUnlocked ? 1f : 0f);
        }

        if (progressText != null)
        {
            progressText.text = isUnlocked ? "Completed" : $"{currentProgress} / {requiredValue}";
        }

        // --- STATE VISUALIZATION: Toggle unlocked overlay ---
        if (unlockedOverlay != null)
        {
            unlockedOverlay.SetActive(isUnlocked);
        }
    }
}
