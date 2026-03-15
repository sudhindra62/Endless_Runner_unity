
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Managers;

/// <summary>
/// Manages the revive popup UI. Listens for the 'Dead' game state.
/// It communicates with the ReviveManager to request a revive or end the run.
/// </summary>
public class RevivePopupUI : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button gemReviveButton;
    [SerializeField] private Button adReviveButton; 
    [SerializeField] private Button declineButton;

    private void Start()
    {
        gemReviveButton.onClick.AddListener(OnGemRevive);
        adReviveButton.onClick.AddListener(OnAdRevive);
        declineButton.onClick.AddListener(OnDecline);
    }

    private void OnDestroy()
    {
        gemReviveButton.onClick.RemoveListener(OnGemRevive);
        adReviveButton.onClick.RemoveListener(OnAdRevive);
        declineButton.onClick.RemoveListener(OnDecline);
    }

    private void OnGemRevive()
    {
        DisableAllButtons();
        GameManager.Instance.Revives.AttemptRevive();
        Hide();
    }

    private void OnAdRevive()
    {
        DisableAllButtons();
        GameManager.Instance.Revives.AttemptReviveWithAd();
        Hide();
    }

    private void OnDecline()
    {
        DisableAllButtons();
        GameManager.Instance.Revives.DeclineRevive();
        Hide();
    }

    /// <summary>
    /// Prevents the user from clicking multiple options.
    /// </summary>
    private void DisableAllButtons()
    {
        gemReviveButton.interactable = false;
        adReviveButton.interactable = false;
        declineButton.interactable = false;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateButtons();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Updates the interactable state of the revive buttons based on revive eligibility.
    /// </summary>
    private void UpdateButtons()
    {
        bool canRevive = GameManager.Instance.Revives.CanRevive();
        
        gemReviveButton.interactable = canRevive && GameManager.Instance.Currency.HasEnoughGems(GameManager.Instance.Revives.GetCurrentReviveCost());
        adReviveButton.interactable = canRevive; // Assuming ads are always available
        declineButton.interactable = true; // The player can always decline.
    }
}
