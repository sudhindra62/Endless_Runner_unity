
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the revive popup that appears upon player death.
/// Displays available revive options and handles button clicks.
/// </summary>
public class RevivePopupUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button adReviveButton;
    [SerializeField] private Button gemReviveButton;
    [SerializeField] private Button tokenReviveButton;
    [SerializeField] private Button endRunButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI nearRecordText;

    private void OnEnable()
    {
        // GameFlowController.OnPlayerDeath += ShowPopup;
        // ReviveEconomyManager.OnReviveStateChanged += UpdateButtonStates;
    }

    private void OnDisable()
    {
        // GameFlowController.OnPlayerDeath -= ShowPopup;
        // ReviveEconomyManager.OnReviveStateChanged -= UpdateButtonStates;
    }

    private void ShowPopup()
    {
        // scoreText.text = ScoreManager.Instance.GetCurrentScore().ToString();
        // distanceText.text = ScoreManager.Instance.GetCurrentDistance().ToString() + "m";
        
        // // Monetization Example
        // if (ScoreManager.Instance.IsCloseToBestScore())
        // {
        //     nearRecordText.gameObject.SetActive(true);
        // }
        // else
        // {
        //     nearRecordText.gameObject.SetActive(false);
        // }

        UpdateButtonStates();
        popupPanel.SetActive(true);
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    private void UpdateButtonStates()
    {
        int revives = ReviveEconomyManager.Instance.GetRevivesThisRun();

        adReviveButton.gameObject.SetActive(revives == 0);
        gemReviveButton.gameObject.SetActive(revives == 1 && CurrencyManager.Instance.HasEnoughGems(ReviveEconomyManager.Instance.gemCostTier2));
        tokenReviveButton.gameObject.SetActive(revives == 2 && ReviveTokenManager.Instance.HasEnoughTokens(ReviveEconomyManager.Instance.tokenCostTier3));
    }

    public void OnAdRevivePressed()
    {
        // This would initiate the ad view process.
        // AdMobManager.Instance.ShowRewardedVideoForRevive();
        HidePopup();
    }

    public void OnGemRevivePressed()
    {
        if (ReviveEconomyManager.Instance.TryRevive())
        {
            // GameFlowController.Instance.ResumeGameAfterRevive();
        }
        HidePopup();
    }

    public void OnTokenRevivePressed()
    {
        if (ReviveEconomyManager.Instance.TryRevive())
        {
            // GameFlowController.Instance.ResumeGameAfterRevive();
        }
        HidePopup();
    }

    public void OnEndRunPressed()
    {
        // GameFlowController.Instance.EndRun();
        HidePopup();
    }
}
