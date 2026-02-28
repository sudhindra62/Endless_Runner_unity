using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI for the daily reward system.
/// This script handles button interactions and updates the UI to reflect the reward status.
/// </summary>
public class DailyRewardView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button claimButton;
    [SerializeField] private Text rewardStatusText;

    private void OnEnable()
    {
        UpdateUI();
        claimButton.onClick.AddListener(OnClaimButtonPressed);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(OnClaimButtonPressed);
    }

    /// <summary>
    /// Updates the UI to reflect the current reward status.
    /// </summary>
    private void UpdateUI()
    {
        if (DailyRewardManager.Instance.IsRewardAvailable())
        {
            claimButton.interactable = true;
            if (rewardStatusText != null) rewardStatusText.text = "Your daily reward is ready!";
        }
        else
        {
            claimButton.interactable = false;
            if (rewardStatusText != null) rewardStatusText.text = "Come back tomorrow for your next reward.";
        }
    }

    /// <summary>
    /// Handles the button press event for claiming the daily reward.
    /// </summary>
    private void OnClaimButtonPressed()
    {
        DailyRewardManager.Instance.ClaimReward();
        UpdateUI();
    }
}
