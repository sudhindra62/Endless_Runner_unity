
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Manages the UI for daily and timed reward chests.
/// This is a UI-only system and does not grant real items, only placeholder currency.
/// </summary>
public class RewardChestUI : MonoBehaviour
{
    [Header("Timed Chest UI References")]
    [Tooltip("Button to claim the timed reward.")]
    public Button timedChestButton;
    [Tooltip("Text to display the cooldown timer or ready status.")]
    public TMP_Text timedChestTimerText;
    [Tooltip("Visual indicator that the timed chest is ready.")]
    public GameObject timedChestNotification;

    [Header("Free Chest UI References")]
    [Tooltip("Button to claim the free, ad-based reward.")]
    public Button freeChestButton; // Placeholder for an ad-reward button

    [Header("Settings")]
    [Tooltip("Cooldown time for the timed chest in hours.")]
    public float timedChestCooldownHours = 4;

    // --- PlayerPrefs Keys ---
    private const string LastTimedChestClaimKey = "RewardChest_LastClaimTime";

    private TimeSpan timedChestCooldown; 
    private bool isTimedChestReady = false;

    #region Unity Lifecycle Methods

    void Awake()
    {
        // Initialize cooldown from settings
        timedChestCooldown = TimeSpan.FromHours(timedChestCooldownHours);

        // --- Button Listeners ---
        if (timedChestButton) timedChestButton.onClick.AddListener(ClaimTimedChest);
        if (freeChestButton) freeChestButton.onClick.AddListener(ClaimFreeChest);
    }

    void Start()
    {
        CheckTimedChestStatus();
    }

    void Update()
    {
        // Update the cooldown timer if the chest is not ready
        if (!isTimedChestReady)
        {
            UpdateCooldownTimer();
        }
    }

    #endregion

    #region Timed Chest Logic

    /// <summary>
    /// Checks the status of the timed chest (ready or on cooldown).
    /// </summary>
    private void CheckTimedChestStatus()
    {
        string lastClaimStr = PlayerPrefs.GetString(LastTimedChestClaimKey, "");

        if (string.IsNullOrEmpty(lastClaimStr))
        {
            // First time playing, the chest is ready.
            SetTimedChestAsReady();
        }
        else
        {
            DateTime lastClaimTime = DateTime.Parse(lastClaimStr);
            if (DateTime.UtcNow - lastClaimTime >= timedChestCooldown)
            {
                // Cooldown has passed.
                SetTimedChestAsReady();
            }
            else
            {
                // Still on cooldown.
                SetTimedChestAsOnCooldown();
            }
        }
    }

    /// <summary>
    /// Handles the claim button logic for the timed chest.
    /// </summary>
    private void ClaimTimedChest()
    {
        if (!isTimedChestReady) 
        {
            Debug.Log("Timed chest is not ready yet.");
            return;
        }

        // --- Grant Placeholder Reward ---
        int coinReward = 100; // Example reward
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", currentCoins + coinReward);
        Debug.Log($"Timed chest claimed! +{coinReward} coins.");

        // --- Save Claim Time & Update UI ---
        PlayerPrefs.SetString(LastTimedChestClaimKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
        SetTimedChestAsOnCooldown();
    }

    /// <summary>
    /// Updates the UI text to show the remaining cooldown time.
    /// </summary>
    private void UpdateCooldownTimer()
    {
        string lastClaimStr = PlayerPrefs.GetString(LastTimedChestClaimKey, "");
        if (string.IsNullOrEmpty(lastClaimStr)) return;

        DateTime lastClaimTime = DateTime.Parse(lastClaimStr);
        TimeSpan remainingTime = (lastClaimTime + timedChestCooldown) - DateTime.UtcNow;

        if (remainingTime.TotalSeconds > 0)
        {
            if (timedChestTimerText)
            {
                timedChestTimerText.text = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            }
        }
        else
        {
            // Timer just finished.
            SetTimedChestAsReady();
        }
    }

    private void SetTimedChestAsReady()
    {
        isTimedChestReady = true;
        if (timedChestButton) timedChestButton.interactable = true;
        if (timedChestNotification) timedChestNotification.SetActive(true);
        if (timedChestTimerText) timedChestTimerText.text = "Ready!";
    }

    private void SetTimedChestAsOnCooldown()
    {
        isTimedChestReady = false;
        if (timedChestButton) timedChestButton.interactable = false;
        if (timedChestNotification) timedChestNotification.SetActive(false);
    }

    #endregion

    #region Free Chest Logic

    /// <summary>
    /// Placeholder for claiming a free (ad-based) reward.
    /// </summary>
    private void ClaimFreeChest()
    {
        // In a real game, this would trigger an ad.
        // Here, we just give a placeholder reward.
        int coinReward = 50;
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", currentCoins + coinReward);
        PlayerPrefs.Save();
        Debug.Log($"Free chest claimed! +{coinReward} coins. (This would normally show an ad)");
    }

    #endregion
}
