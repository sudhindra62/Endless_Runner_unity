
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the revive popup, offering the player a chance to watch an ad to continue the run.
/// </summary>
public class RevivePopupUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button reviveWithAdButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private TMP_Text countdownText;

    [Header("Configuration")]
    [SerializeField] private float countdownDuration = 5f;

    private float countdownTimer;
    private bool isCountingDown;

    private ReviveManager reviveManager;
    private AdMobManager adMobManager;

    private void Awake()
    {
        // Initialize with the panel hidden
        if (popupPanel != null) popupPanel.SetActive(false);

        // Set up button listeners
        reviveWithAdButton.onClick.AddListener(OnReviveButtonPressed);
        declineButton.onClick.AddListener(OnDeclineButtonPressed);
    }

    private void Start()
    {
        reviveManager = ServiceLocator.Get<ReviveManager>();
        adMobManager = ServiceLocator.Get<AdMobManager>();
    }

    private void Update()
    {
        if (isCountingDown)
        {
            HandleCountdown();
        }
    }

    /// <summary>
    /// Shows the revive popup and starts the countdown timer.
    /// </summary>
    public void ShowPopup()
    {
        if (popupPanel == null) return;

        popupPanel.SetActive(true);
        countdownTimer = countdownDuration;
        isCountingDown = true;
    }

    private void HandleCountdown()
    {
        countdownTimer -= Time.deltaTime;
        countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();

        if (countdownTimer <= 0)
        {
            isCountingDown = false;
            OnDeclineButtonPressed();
        }
    }

    private void OnReviveButtonPressed()
    {
        isCountingDown = false;
        reviveWithAdButton.interactable = false; // Prevent multiple clicks

        adMobManager.ShowRewardedAd(() =>
        {
            reviveManager.GrantRevive();
            HidePopup();
        });
    }

    private void OnDeclineButtonPressed()
    {
        isCountingDown = false;
        reviveManager.DeclineRevive();
        HidePopup();
    }

    private void HidePopup()
    {
        if (popupPanel != null) popupPanel.SetActive(false);
        reviveWithAdButton.interactable = true; // Reset for next time
    }
}
