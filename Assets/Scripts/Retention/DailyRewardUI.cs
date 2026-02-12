
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Manages the UI for the Daily Reward system.
/// It displays the current reward, shows a countdown timer, and handles the claim button state.
/// This script depends on a DailyRewardManager being present in the scene.
/// 
/// --- Inspector Setup ---
/// 1. Attach to the root panel of the Daily Rewards UI.
/// 2. Assign the TMP_Text for the countdown timer.
/// 3. Assign the main claim button.
/// 4. Assign UI elements for reward display (icon, text).
/// 5. Assign sprites for Coins, Gems, and Chests for visual feedback.
/// </summary>
public class DailyRewardUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Text to display the cooldown timer.")]
    [SerializeField] private TMP_Text timerText;
    [Tooltip("The button to claim the reward.")]
    [SerializeField] private Button claimButton;
    [Tooltip("Text on the claim button.")]
    [SerializeField] private TMP_Text claimButtonText;
    [Tooltip("Image to show the reward icon.")]
    [SerializeField] private Image rewardIcon;
    [Tooltip("Text to describe the reward.")]
    [SerializeField] private TMP_Text rewardText;

    [Header("Reward Sprites")]
    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Sprite gemSprite;
    [SerializeField] private Sprite chestSprite;
    [SerializeField] private Sprite megaChestSprite;

    private bool isClaimable;

    #region Unity Lifecycle Methods

    private void OnEnable()
    {
        // Subscribe to manager events to auto-update UI
        if (DailyRewardManager.Instance != null)
        {
            DailyRewardManager.OnRewardClaimed += UpdateUI;
        }
        claimButton.onClick.AddListener(OnClaimButtonPressed);
        UpdateUI();
    }

    private void OnDisable()
    {
        if (DailyRewardManager.Instance != null)
        {
            DailyRewardManager.OnRewardClaimed -= UpdateUI;
        }
        claimButton.onClick.RemoveListener(OnClaimButtonPressed);
    }

    private void Update()
    {
        // Only update the timer text if the reward is not yet claimable
        if (!isClaimable && DailyRewardManager.Instance != null)
        {
            UpdateTimer();
        }
    }

    #endregion

    /// <summary>
    /// Main method to refresh the entire UI state.
    /// </summary>
    public void UpdateUI()
    {
        if (DailyRewardManager.Instance == null)
        {
            Debug.LogError("DailyRewardUI: DailyRewardManager instance not found!");
            gameObject.SetActive(false); // Disable self to prevent errors
            return;
        }

        isClaimable = DailyRewardManager.Instance.CanClaimReward();

        if (isClaimable)
        {
            // State: Reward is ready to be claimed
            claimButton.interactable = true;
            if (claimButtonText) claimButtonText.text = "Claim";
            if (timerText) timerText.text = "Your reward is ready!";
        }
        else
        {
            // State: Reward is on cooldown
            claimButton.interactable = false;
            if (claimButtonText) claimButtonText.text = "Claimed";
            UpdateTimer();
        }

        // Update the reward display for the current day
        DisplayReward(DailyRewardManager.Instance.GetCurrentReward());
    }

    /// <summary>
    /// Updates the countdown timer text.
    /// </summary>
    private void UpdateTimer()
    {
        if (timerText == null) return;

        TimeSpan timeUntilNextClaim = DailyRewardManager.Instance.GetNextClaimTime() - DateTime.UtcNow;
        if (timeUntilNextClaim.TotalSeconds > 0)
        {
            timerText.text = $"Next reward in: {timeUntilNextClaim.Hours:D2}:{timeUntilNextClaim.Minutes:D2}:{timeUntilNextClaim.Seconds:D2}";
        }
        else
        {
            // Time is up, refresh the whole UI to switch to claimable state
            UpdateUI();
        }
    }

    /// <summary>
    /// Displays the icon and text for the current reward.
    /// </summary>
    private void DisplayReward(DailyReward reward)
    {
        if (rewardText != null) rewardText.text = GetRewardDescription(reward);
        if (rewardIcon != null) rewardIcon.sprite = GetRewardSprite(reward.Type);
    }

    private void OnClaimButtonPressed()
    {
        if (DailyRewardManager.Instance != null)
        {
            bool success = DailyRewardManager.Instance.ClaimReward();
            if (!success)
            {
                // This can happen if the button is pressed on the exact frame the cooldown ends
                // but before the UI has had a chance to update. A simple refresh fixes it.
                UpdateUI();
            }
        }
    }

    private string GetRewardDescription(DailyReward reward)
    {
        switch (reward.Type)
        {
            case RewardType.Coins: return $"{reward.Amount} Coins";
            case RewardType.Gems: return $"{reward.Amount} Gems";
            case RewardType.Chest: return "A Basic Chest";
            case RewardType.MegaChest: return "A Mega Chest!";
            default: return "";
        }
    }

    private Sprite GetRewardSprite(RewardType type)
    {
        switch (type)
        {
            case RewardType.Coins: return coinSprite;
            case RewardType.Gems: return gemSprite;
            case RewardType.Chest: return chestSprite;
            case RewardType.MegaChest: return megaChestSprite;
            default: return null;
        }
    }
}
