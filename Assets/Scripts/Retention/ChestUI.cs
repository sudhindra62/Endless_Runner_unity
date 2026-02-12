using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Controls the UI for a single chest slot on the home screen.
/// It updates its visual state based on data from the ChestManager.
/// 
/// --- Prefab Setup ---
/// 1. A root GameObject with this script.
/// 2. A Button component to handle clicks.
/// 3. An Image component for the chest icon (`chestIcon`).
/// 4. A TextMeshProUGUI for the timer/status text (`timerText`).
/// 5. A GameObject to act as a "Ready" overlay/glow (`readyOverlay`).
/// </summary>
public class ChestUI : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The type of chest this UI element represents.")]
    [SerializeField] private ChestType chestType;

    [Header("UI References")]
    [SerializeField] private Image chestIcon;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject readyOverlay;
    [SerializeField] private Button claimButton;

    private ChestData chestData;

    void Start()
    {
        chestData = ChestManager.Instance.GetChestData(chestType);
        if (chestData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        chestIcon.sprite = chestData.chestIcon;
        claimButton.onClick.AddListener(OnClaimButtonClicked);

        UpdateVisuals();
    }

    void OnEnable()
    {
        ChestManager.OnChestStateChanged += HandleChestStateChange;
    }

    void OnDisable()
    {
        ChestManager.OnChestStateChanged -= HandleChestStateChange;
    }

    private void HandleChestStateChange(ChestType type)
    {
        if (type == this.chestType)
        {
            UpdateVisuals();
        }
    }

    void Update()
    {
        // Only update the timer text if the chest is not ready
        if (!ChestManager.Instance.IsChestReady(chestType))
        {
            DateTime nextAvailableTime = ChestManager.Instance.GetNextAvailableTime(chestType);
            TimeSpan timeRemaining = nextAvailableTime - DateTime.UtcNow;

            if (timeRemaining.TotalSeconds > 0)
            {
                timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
            }
            else
            { 
                // This case is handled by the manager checking state in its own Update loop,
                // which will fire an event to trigger UpdateVisuals().
                timerText.text = "Ready!";
            }
        }
    }

    private void UpdateVisuals()
    {
        bool isReady = ChestManager.Instance.IsChestReady(chestType);
        
        readyOverlay.SetActive(isReady);
        claimButton.interactable = isReady;
        timerText.gameObject.SetActive(!isReady);

        if (isReady)
        {
            timerText.text = "READY";
        }
    }

    private void OnClaimButtonClicked()
    {
        ChestManager.Instance.ClaimChest(chestType);

        // Optional: Trigger a UI animation here (e.g., coin burst)
    }
}
