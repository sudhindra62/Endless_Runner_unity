
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use TextMeshPro

/// <summary>
/// Displays a notification for a new challenge and allows the player to accept or decline it.
/// Fully refactored and fortified by Supreme Guardian Architect v12.
/// </summary>
[AddComponentMenu("UI/Challenge Notification UI")]
public class ChallengeNotificationUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private GameObject notificationPanel; // The parent panel for the whole UI

    private Challenge _currentChallenge;

    #region Unity Lifecycle & Event Subscription

    private void OnEnable()
    {
        // Subscribe to the event that fires when a new challenge is received
        ChallengeManager.OnNewChallengeReceived += DisplayChallengeNotification;

        // Add button listeners
        acceptButton.onClick.AddListener(HandleAccept);
        declineButton.onClick.AddListener(HandleDecline);

        // Start with the panel hidden
        if(notificationPanel != null) notificationPanel.SetActive(false);
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        ChallengeManager.OnNewChallengeReceived -= DisplayChallengeNotification;
        
        acceptButton.onClick.RemoveListener(HandleAccept);
        declineButton.onClick.RemoveListener(HandleDecline);
    }

    #endregion

    #region UI Logic

    /// <summary>
    /// Displays the notification UI when a new challenge is available.
    /// This is the event handler for ChallengeManager.OnNewChallengeReceived.
    /// </summary>
    private void DisplayChallengeNotification(Challenge challenge)
    {
        // --- ERROR_HANDLING_POLICY: Prevent stacking notifications ---
        if (notificationPanel.activeSelf)
        {
            Debug.LogWarning("Guardian Architect Warning: A challenge notification is already being displayed. Ignoring new challenge.");
            return;
        }

        _currentChallenge = challenge;
        
        // --- DATA_BINDING: Format the notification text ---
        string challengeDescription = GetChallengeDescription(challenge.type);
        notificationText.text = $"<b>{challenge.challengerID}</b> has challenged you!\nBeat their {challengeDescription} of <b>{challenge.valueToBeat}</b>.";
        
        if(notificationPanel != null) notificationPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the notification panel.
    /// </summary>
    private void HideNotification()
    {
        _currentChallenge = null;
        if(notificationPanel != null) notificationPanel.SetActive(false);
    }

    #endregion

    #region Button Handlers

    /// <summary>
    /// Called when the player clicks the 'Accept' button.
    /// </summary>
    private void HandleAccept()
    {
        if (_currentChallenge == null) return;
        
        // --- DELEGATION_MANDATE: Delegate logic to the manager ---
        ChallengeManager.Instance.AcceptChallenge(_currentChallenge);
        HideNotification();
    }

    /// <summary>
    /// Called when the player clicks the 'Decline' button.
    /// </summary>
    private void HandleDecline()
    {
        if (_currentChallenge == null) return;

        // --- DELEGATION_MANDATE: Delegate logic to the manager ---
        ChallengeManager.Instance.DeclineChallenge(_currentChallenge);
        HideNotification();
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Returns a user-friendly string for a given ChallengeType.
    /// </summary>
    private string GetChallengeDescription(ChallengeType type)
    {
        switch (type)
        {
            case ChallengeType.Distance: return "distance";
            case ChallengeType.Score: return "score";
            case ChallengeType.CoinsCollected: return "coin collection";
            default: return "record";
        }
    }

    #endregion
}
