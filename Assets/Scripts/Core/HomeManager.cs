using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Unified HomeManager
/// Preserves:
/// - Currency display (Coins + Gems)
/// - Best Score display
/// - Daily challenge UI
/// - Progress bar
/// - Daily reward system with cooldown
/// - Navigation buttons
/// </summary>
public class HomeManager : MonoBehaviour
{
    #region UI REFERENCES

    [Header("Texts")]
    public TMP_Text coinsText;
    public TMP_Text gemsText;
    public TMP_Text bestScoreText;
    public TMP_Text challengeText;
    public TMP_Text rewardText;
    public TMP_Text dailyRewardTimerText;

    [Header("Progress")]
    public Image progressBar;

    [Header("Buttons")]
    public Button playButton;
    public Button shopButton;
    public Button missionsButton;
    public Button settingsButton;
    public Button statsButton;
    public Button dailyRewardButton;

    [Header("Visuals")]
    public GameObject rewardNotification;
    public GameObject characterPreview;

    #endregion

    [Header("Scene Settings")]
    public string mainSceneName = "MainScene";

    // Currency
    private int coins;
    private int gems;

    // Best score
    private int bestScore;

    // Daily Challenge
    private int dailyTarget = 100;
    private int rewardCoins = 200;

    // Daily Reward Cooldown
    private const string LastRewardTimeKey = "HomeManager_LastRewardTime";
    private readonly TimeSpan rewardCooldown = TimeSpan.FromHours(24);
    private bool isRewardReady;

    #region Unity Lifecycle

    private void Awake()
    {
        if (playButton) playButton.onClick.AddListener(PlayGame);
        if (shopButton) shopButton.onClick.AddListener(() => Debug.Log("Shop Clicked"));
        if (missionsButton) missionsButton.onClick.AddListener(() => Debug.Log("Missions Clicked"));
        if (settingsButton) settingsButton.onClick.AddListener(() => Debug.Log("Settings Clicked"));
        if (statsButton) statsButton.onClick.AddListener(() => Debug.Log("Stats Clicked"));
        if (dailyRewardButton) dailyRewardButton.onClick.AddListener(ClaimDailyReward);
    }

    private void Start()
    {
        LoadCurrency();
        LoadBestScore();
        SetupChallenge();
        CheckDailyRewardStatus();

        if (characterPreview == null)
            Debug.LogWarning("Character preview not assigned.");
    }

    private void Update()
    {
        if (!isRewardReady)
            UpdateRewardCooldownTimer();
    }

    #endregion

    #region Navigation

    public void PlayGame()
    {
        // Correctly loads the main game scene.
        // The GameFlowManager within that scene will handle the run initialization.
        SceneManager.LoadScene(mainSceneName, LoadSceneMode.Single);
    }

    #endregion

    #region Currency

    private void LoadCurrency()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        gems = PlayerPrefs.GetInt("Gems", 0);
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        if (coinsText) coinsText.text = "Coins: " + coins;
        if (gemsText) gemsText.text = "Gems: " + gems;
    }

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

    #region Best Score

    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (bestScoreText)
            bestScoreText.text = "Best: " + bestScore;
    }

    #endregion

    #region Daily Challenge

    private void SetupChallenge()
    {
        if (challengeText)
            challengeText.text = "Collect " + dailyTarget + " Coins";

        if (rewardText)
            rewardText.text = "Reward: +" + rewardCoins + " Coins";

        if (progressBar)
            progressBar.fillAmount = 0f;
    }

    public void UpdateChallengeProgress(int collected)
    {
        if (progressBar)
            progressBar.fillAmount = Mathf.Clamp01((float)collected / dailyTarget);
    }

    #endregion

    #region Daily Reward

    private void CheckDailyRewardStatus()
    {
        string lastRewardTimeStr = PlayerPrefs.GetString(LastRewardTimeKey, "");

        if (string.IsNullOrEmpty(lastRewardTimeStr))
        {
            SetRewardAsReady();
            return;
        }

        DateTime lastRewardTime = DateTime.Parse(lastRewardTimeStr);

        if (DateTime.UtcNow - lastRewardTime >= rewardCooldown)
            SetRewardAsReady();
        else
            SetRewardAsOnCooldown();
    }

    private void UpdateRewardCooldownTimer()
    {
        string lastRewardTimeStr = PlayerPrefs.GetString(LastRewardTimeKey, "");
        if (string.IsNullOrEmpty(lastRewardTimeStr)) return;

        DateTime lastRewardTime = DateTime.Parse(lastRewardTimeStr);
        TimeSpan remainingTime = (lastRewardTime + rewardCooldown) - DateTime.UtcNow;

        if (remainingTime.TotalSeconds > 0)
        {
            if (dailyRewardTimerText)
                dailyRewardTimerText.text =
                    $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
        }
        else
        {
            SetRewardAsReady();
        }
    }

    public void ClaimDailyReward()
    {
        if (!isRewardReady)
            return;

        int coinReward = 250;
        int gemReward = 5;

        AddCurrency(coinReward, gemReward);

        PlayerPrefs.SetString(LastRewardTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();

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
