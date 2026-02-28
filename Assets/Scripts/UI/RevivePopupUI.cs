
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the revive popup UI. Listens for the 'Dead' game state.
/// It communicates with the GameFlowController through the ServiceLocator to request a revive or end the run.
/// </summary>
public class RevivePopupUI : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button gemReviveButton;
    [SerializeField] private Button adReviveButton; // Assuming ad logic is handled elsewhere
    [SerializeField] private Button declineButton;

    // Dependencies (resolved via ServiceLocator)
    private GameFlowController gameFlowController;
    private ReviveManager reviveManager;
    private CurrencyManager currencyManager;

    private void Start()
    {
        // Resolve dependencies from the ServiceLocator.
        // This is a cleaner, more robust pattern than FindObjectOfType.
        gameFlowController = ServiceLocator.Get<GameFlowController>();
        reviveManager = ServiceLocator.Get<ReviveManager>();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        
        // Add listeners once and remove them on destroy.
        gemReviveButton.onClick.AddListener(OnGemRevive);
        adReviveButton.onClick.AddListener(OnAdRevive);
        declineButton.onClick.AddListener(OnDecline);
    }

    private void OnDestroy()
    {
        // This is crucial to prevent memory leaks and errors when scenes are reloaded.
        gemReviveButton.onClick.RemoveListener(OnGemRevive);
        adReviveButton.onClick.RemoveListener(OnAdRevive);
        declineButton.onClick.RemoveListener(OnDecline);
    }

    private void OnGemRevive()
    {
        DisableAllButtons();
        // The popup UI does not decide if the revive is successful.
        // It only requests the action. The GameFlowController handles the outcome.
        gameFlowController.AttemptRevive();
        Hide();
    }

    private void OnAdRevive()
    {
        DisableAllButtons();
        // In a real implementation, you would trigger an ad here.
        // On ad completion, you would then call gameFlowController.AttemptRevive().
        // For now, we will just attempt the revive directly.
        Debug.Log("Ad Revive button clicked. Triggering ad flow...");
        gameFlowController.AttemptRevive();
        Hide();
    }

    private void OnDecline()
    {
        DisableAllButtons();
        // If the player declines, we tell the GameFlowController to officially end the run.
        gameFlowController.EndRun();
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
        // Check with the authoritative ReviveManager if a revive is possible.
        bool canRevive = reviveManager.CanRevive();
        
        // A more complex implementation would check currency and ad availability separately.
        gemReviveButton.interactable = canRevive && currencyManager.HasEnoughGems(reviveManager.NextReviveCost);
        adReviveButton.interactable = canRevive; // Assuming ads are always available
        declineButton.interactable = true; // The player can always decline.
    }
}
