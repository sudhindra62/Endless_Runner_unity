
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI for displaying player level, XP progress, and the next reward.
/// It listens to events from XPManager and LevelRewardManager to keep the UI synchronized.
/// 
/// --- Inspector Setup ---
/// 1. Attach this to a root UI panel for player progression.
/// 2. Assign the TMP_Text for the current level display.
/// 3. Assign an Image with its type set to 'Filled' for the XP progress bar.
/// 4. Assign UI elements for the next reward preview (text, icon).
/// </summary>
public class LevelProgressUI : MonoBehaviour
{
    [Header("Core UI References")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image xpProgressBar;

    [Header("Next Reward Preview")]
    [SerializeField] private GameObject nextRewardGroup; // Parent group for the preview
    [SerializeField] private TMP_Text nextRewardLevelText;
    [SerializeField] private TMP_Text nextRewardDescriptionText;

    #region Unity Lifecycle Methods

    private void OnEnable()
    {
        // Subscribe to events to automatically update the UI
        XPManager.OnXPChanged += UpdateXPUI;
        XPManager.OnLeveledUp += HandleLevelUp;
        // Also listen for reward grants to update the 'next reward' preview
        LevelRewardManager.OnRewardGranted += (reward) => UpdateRewardPreview();
        
        // Initial full UI update
        UpdateXPUI(XPManager.Instance?.GetProgressionData());
        UpdateRewardPreview();
    }

    private void OnDisable()
    {
        XPManager.OnXPChanged -= UpdateXPUI;
        XPManager.OnLeveledUp -= HandleLevelUp;
        LevelRewardManager.OnRewardGranted -= (reward) => UpdateRewardPreview();
    }

    #endregion

    /// <summary>
    /// Updates the level text and XP progress bar.
    /// </summary>
    private void UpdateXPUI(PlayerProgressionData data)
    {
        if (data == null) return; // Don't update if data is not available

        if (levelText != null)
        {
            levelText.text = $"Level {data.CurrentLevel}";
        }

        if (xpProgressBar != null)
        { 
            xpProgressBar.fillAmount = (float)data.CurrentXP / data.XPToNextLevel;
        }
    }

    /// <summary>
    /// Handles the level-up event to ensure the reward preview is refreshed.
    /// </summary>
    private void HandleLevelUp(PlayerProgressionData data)
    {
        UpdateXPUI(data);
        UpdateRewardPreview();
    }

    /// <summary>
    /// Updates the UI element that shows the next available level-up reward.
    /// </summary>
    private void UpdateRewardPreview()
    {
        if (nextRewardGroup == null || LevelRewardManager.Instance == null || XPManager.Instance == null) return;

        PlayerProgressionData progData = XPManager.Instance.GetProgressionData();
        LevelRewardData nextReward = LevelRewardManager.Instance.GetNextUnclaimedReward(progData.CurrentLevel);

        if (nextReward != null)
        {
            nextRewardGroup.SetActive(true);
            if (nextRewardLevelText != null) nextRewardLevelText.text = $"Next Reward: Lvl {nextReward.Level}";
            if (nextRewardDescriptionText != null) nextRewardDescriptionText.text = GetRewardDescription(nextReward);
        }
        else
        {
            // No more rewards defined, hide the preview
            nextRewardGroup.SetActive(false);
        }
    }

    private string GetRewardDescription(LevelRewardData reward)
    {
        switch (reward.RewardType)
        {
            case LevelRewardType.Coins: return $"{reward.Amount} Coins";
            case LevelRewardType.Gems: return $"{reward.Amount} Gems";
            case LevelRewardType.Chest: return $"{(ChestType)reward.Amount} Chest";
            case LevelRewardType.SkinUnlock: return "New Skin";
            default: return "";
        }
    }
}
