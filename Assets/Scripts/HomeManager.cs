using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Manages the Home Screen UI and its core functionalities like navigation,
/// currency display, and daily rewards.
/// </summary>
public class HomeManager : MonoBehaviour
{
    [Header("UI References - Texts")]
    [Tooltip("Text to display player's total coins.")]
    public TMP_Text coinsText;
    [Tooltip("Text to display player's total gems.")]
    public TMP_Text gemsText;

    [Header("UI References - Buttons")]
    public Button playButton;
    public Button shopButton;
    public Button missionsButton;
    public Button settingsButton;
    public Button statsButton;

    [Header("UI References - Daily Reward")]
    [Tooltip("Button to claim the daily reward.")]
    public Button dailyRewardButton;
    [Tooltip("Text to show the cooldown timer or ready status for the reward.")]
    public TMP_Text dailyRewardTimerText;
    [Tooltip("A visual indicator that the daily reward is ready to be claimed.")]
    public GameObject rewardNotification;

    [Header("UI References - Placeholders")]
    [Tooltip("A placeholder GameObject for showing the selected character.")]
    public GameObject characterPreview;

    [Header("Game Settings")]
    [Tooltip("The name of the main gameplay scene to load.")]
    public string mainSceneName = "MainScene";

    // --- Private Fields ---

    // Currency
    private int coins;
    private int gems;

    // Daily Reward
    private const string LastRewardTimeKey = "HomeManager_LastRewardTime";
    private readonly TimeSpan rewardCooldown = TimeSpan.FromHours(24);
    private bool isRewardReady = false;

    #region Unity Lifecycle Methods

    void Awake()
    {
        // --- Button Listeners ---
        if (playButton) playButton.onClick.AddListener(PlayGame);
        if (shopButton) shopButton.onClick.AddListener(OnShopClicked);
        if (missionsButton) missionsButton.onClick.AddListener(OnMissionsClicked);
        if (settingsButton) settingsButton.onClick.AddListener(OnSettingsClicked);
        if (statsButton) statsButton.onClick.AddListener(OnStatsClicked);
        if (dailyRewardButton) dailyRewardButton.onClick.AddListener(ClaimDailyReward);
    }

    void Start()
    {
        // Initialize all UI elements and data
        LoadCurrency();
        CheckDailyRewardStatus();

        // Optional: Log warning if character preview is not set
        if (characterPreview == null)
        {
            Debug.LogWarning("Character Preview placeholder is not assigned in the HomeManager.");
        }
    }

    void Update()
    {
        // Continuously update the timer if the reward is not ready
        if (!isRewardReady)
        {
            UpdateRewardCooldownTimer();
        }
    }

    #endregion

    #region Navigation

    /// <summary>
    /// Loads the main gameplay scene.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    // Placeholder methods for other navigation buttons
    private void OnShopClicked() => Debug.Log("Shop button clicked - Navigation logic to be implemented.");
    private void OnMissionsClicked() => Debug.Log("Missions button clicked - Navigation logic to be implemented.");
    private void OnSettingsClicked() => Debug.Log("Settings button clicked - Navigation logic to be implemented.");
    private void OnStatsClicked() => Debug.Log("Stats button clicked - Navigation logic to be implemented.");

    #endregion

    #region Currency Management

    /// <summary>
    /// Loads currency from PlayerPrefs and updates the UI.
    /// </summary>
    private void LoadCurrency()
    {
        coins = PlayerPrefs.GetInt("Coins", 100);
        gems = PlayerPrefs.GetInt("Gems", 5);
        UpdateCurrencyUI();
    }

    /// <summary>
    /// Updates the currency text elements in the UI.
    /// </summary>
    private void UpdateCurrencyUI()
    {
        if (coinsText) coinsText.text = coins.ToString();
        if (gemsText) gemsText.text = gems.ToString();
    }

    /// <summary>
    /// Adds or removes currency and saves it to PlayerPrefs.
    /// </summary>
    /// <param name="coinAmount">The amount of coins to add (can be negative).</param>
    /// <param name="gemAmount">The amount of gems to add (can be negative).</param>
    private void AddCurrency(int coinAmount, int gemAmount)
    {
        coins += coinAmount;
        gems += gemAmount;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.Save();
        UpdateCurrencyUI();
    }

    #endregion

    #region Daily Reward

    /// <summary>
    /// Checks if the daily reward is available to be claimed.
    /// </summary>
    private void CheckDailyRewardStatus()
    {
        string lastRewardTimeStr = PlayerPrefs.GetString(LastRewardTimeKey, "");

        if (string.IsNullOrEmpty(lastRewardTimeStr))
        {
            // If the key doesn't exist, the reward is ready.
            SetRewardAsReady();
        }
        else
        {
            DateTime lastRewardTime = DateTime.Parse(lastRewardTimeStr);
            if (DateTime.UtcNow - lastRewardTime >= rewardCooldown)
            {
                // Cooldown has passed.
                SetRewardAsReady();
            }
            else
            {
                // Cooldown is still active.
                SetRewardAsOnCooldown();
            }
        }
    }

    /// <summary>
    /// Updates the timer text that shows the remaining cooldown for the daily reward.
    /// </summary>
    private void UpdateRewardCooldownTimer()
    {
        string lastRewardTimeStr = PlayerPrefs.GetString(LastRewardTimeKey, "");
        if (string.IsNullOrEmpty(lastRewardTimeStr)) return;

        DateTime lastRewardTime = DateTime.Parse(lastRewardTimeStr);
        TimeSpan remainingTime = (lastRewardTime + rewardCooldown) - DateTime.UtcNow;

        if (remainingTime.TotalSeconds > 0)
        {
            if (dailyRewardTimerText)
            {
                dailyRewardTimerText.text = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            }
        }
        else
        { 
            // Timer just finished.
            SetRewardAsReady();
        }
    }

    /// <summary>
    /// Handles the logic for claiming the daily reward.
    /// </summary>
    public void ClaimDailyReward()
    {
        if (!isRewardReady)
        {
            Debug.Log("Daily reward is not ready to be claimed yet.");
            return;
        }

        // --- Grant the reward ---
        int coinReward = 250;
        int gemReward = 5;
        AddCurrency(coinReward, gemReward);
        Debug.Log($"Daily reward claimed: +{coinReward} Coins, +{gemReward} Gems");

        // --- Save the claim time ---
        PlayerPrefs.SetString(LastRewardTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();

        // --- Update the UI to show cooldown ---
        SetRewardAsOnCooldown();
    }

    private void SetRewardAsReady()
    {
        isRewardReady = true;
        if (dailyRewardButton) dailyRewardButton.interactable = true;
        if (rewardNotification) rewardNotification.SetActive(true);
        if (dailyRewardTimerText) dailyRewardTimerText.text = "Claim Reward!";
    }

    private void SetRewardAsOnCooldown()
    {
        isRewardReady = false;
        if (dailyRewardButton) dailyRewardButton.interactable = false;
        if (rewardNotification) rewardNotification.SetActive(false);
    }

    #endregion
}
