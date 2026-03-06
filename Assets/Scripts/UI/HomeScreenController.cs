
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Manages the home screen UI and user interactions, including navigation and daily rewards.
/// It dynamically updates UI elements based on data from the PlayerDataManager and ScoreManager.
/// </summary>
public class HomeScreenController : MonoBehaviour
{
    #region UI REFERENCES

    [Header("Texts")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text gemsText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private TMP_Text challengeText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text dailyRewardTimerText;

    [Header("Progress")]
    [SerializeField] private Image progressBar;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button missionsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button statsButton;
    [SerializeField] private Button dailyRewardButton;
    [SerializeField] private Button achievementsButton;

    [Header("Visuals")]
    [SerializeField] private GameObject rewardNotification;
    [SerializeField] private GameObject characterPreview;

    #endregion

    [Header("Scene Settings")]
    [SerializeField] private string mainGameSceneName = "MainScene";

    [Header("Dependencies")]
    [SerializeField] private SceneLoader sceneLoader;

    // Daily Challenge Configuration
    private const int DAILY_CHALLENGE_TARGET = 100;
    private const int DAILY_CHALLENGE_REWARD = 200;

    // Daily Reward Configuration
    private const string LAST_REWARD_TIME_KEY = "HomeController_LastRewardTime";
    private readonly TimeSpan rewardCooldown = TimeSpan.FromHours(24);
    private bool isRewardReady = false;

    #region Unity Lifecycle & Event Subscriptions

    private void OnEnable()
    {
        // Subscribe to events
        PlayerDataManager.OnCoinsChanged += UpdateCoinsUI;
        PlayerDataManager.OnGemsChanged += UpdateGemsUI;
        ScoreManager.OnBestScoreChanged += UpdateBestScoreUI;

        // Add button listeners
        if (playButton) playButton.onClick.AddListener(PlayGame);
        if (shopButton) shopButton.onClick.AddListener(() => Debug.Log("Shop Button Clicked"));
        if (missionsButton) missionsButton.onClick.AddListener(() => Debug.Log("Missions Button Clicked"));
        if (settingsButton) settingsButton.onClick.AddListener(() => Debug.Log("Settings Button Clicked"));
        if (statsButton) statsButton.onClick.AddListener(() => Debug.Log("Stats Button Clicked"));
        if (dailyRewardButton) dailyRewardButton.onClick.AddListener(ClaimDailyReward);
        if (achievementsButton) achievementsButton.onClick.AddListener(() => UIManager.Instance.ShowTrophyGallery());

        // Initial UI setup
        InitialUIDisplay();
        SetupChallengeUI();
        CheckDailyRewardStatus();
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        PlayerDataManager.OnCoinsChanged -= UpdateCoinsUI;
        PlayerDataManager.OnGemsChanged -= UpdateGemsUI;
        ScoreManager.OnBestScoreChanged -= UpdateBestScoreUI;

        // Remove button listeners
        if (playButton) playButton.onClick.RemoveListener(PlayGame);
        if (shopButton) shopButton.onClick.RemoveListener(() => Debug.Log("Shop Button Clicked"));
        if (missionsButton) missionsButton.onClick.RemoveListener(() => Debug.Log("Missions Button Clicked"));
        if (settingsButton) settingsButton.onClick.RemoveListener(() => Debug.Log("Settings Button Clicked"));
        if (statsButton) statsButton.onClick.RemoveListener(() => Debug.Log("Stats Button Clicked"));
        if (dailyRewardButton) dailyRewardButton.onClick.RemoveListener(ClaimDailyReward);
        if (achievementsButton) achievementsButton.onClick.RemoveListener(() => UIManager.Instance.ShowTrophyGallery());
    }

    private void Update()
    {
        if (!isRewardReady)
        {
            UpdateRewardCooldownTimer();
        }
    }

    #endregion

    #region UI Update Handlers

    private void InitialUIDisplay()
    {
        // Set initial values from managers
        UpdateCoinsUI(PlayerDataManager.Instance.Coins);
        UpdateGemsUI(PlayerDataManager.Instance.Gems);
        UpdateBestScoreUI(ScoreManager.Instance.BestScore);
    }

    private void UpdateCoinsUI(int amount)
    {
        if (coinsText) coinsText.text = amount.ToString();
    }

    private void UpdateGemsUI(int amount)
    {
        if (gemsText) gemsText.text = amount.ToString();
    }

    private void UpdateBestScoreUI(int score)
    {
        if (bestScoreText) bestScoreText.text = "Best: " + score;
    }

    #endregion

    #region Navigation

    private void PlayGame()
    {
        sceneLoader.LoadScene(mainGameSceneName);
    }

    #endregion

    #region Daily Challenge

    private void SetupChallengeUI()
    {
        if (challengeText) challengeText.text = $"Collect {DAILY_CHALLENGE_TARGET} Coins";
        if (rewardText) rewardText.text = $"Reward: +{DAILY_CHALLENGE_REWARD} Coins";
        UpdateChallengeProgress(PlayerDataManager.Instance.Coins); // Use current coins for initial progress
    }

    public void UpdateChallengeProgress(int collected)
    {
        if (progressBar) progressBar.fillAmount = Mathf.Clamp01((float)collected / DAILY_CHALLENGE_TARGET);
    }

    #endregion

    #region Daily Reward

    private void CheckDailyRewardStatus()
    {
        string lastRewardTimeStr = PlayerPrefs.GetString(LAST_REWARD_TIME_KEY, "");

        if (string.IsNullOrEmpty(lastRewardTimeStr))
        {
            SetRewardAsReady();
            return;
        }

        if (DateTime.TryParse(lastRewardTimeStr, out DateTime lastRewardTime))
        {
            if (DateTime.UtcNow - lastRewardTime >= rewardCooldown)
            {
                SetRewardAsReady();
            }
            else
            {
                SetRewardAsOnCooldown();
            }
        }
        else
        {
            // Handle corrupted or invalid time string
            SetRewardAsReady();
        }
    }

    private void UpdateRewardCooldownTimer()
    {
        string lastRewardTimeStr = PlayerPrefs.GetString(LAST_REWARD_TIME_KEY, "");
        if (string.IsNullOrEmpty(lastRewardTimeStr) || !DateTime.TryParse(lastRewardTimeStr, out DateTime lastRewardTime)) return;

        TimeSpan remainingTime = (lastRewardTime + rewardCooldown) - DateTime.UtcNow;

        if (remainingTime.TotalSeconds > 0)
        {
            if (dailyRewardTimerText) dailyRewardTimerText.text = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
        }
        else
        {
            SetRewardAsReady();
        }
    }

    public void ClaimDailyReward()
    {
        if (!isRewardReady) return;

        // Use PlayerDataManager to add currency
        PlayerDataManager.Instance.AddCoins(250);
        PlayerDataManager.Instance.AddGems(5);

        PlayerPrefs.SetString(LAST_REWARD_TIME_KEY, DateTime.UtcNow.ToString());
        
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

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
