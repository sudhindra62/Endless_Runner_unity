
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages all UI elements, including HUD, menus, and pop-ups.
/// Now fully equipped to handle tutorial messages and power-up indicators.
/// Healed and fortified by Supreme Guardian Architect v12.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("HUD Elements")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text coinText;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text finalCoinText;

    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;

    [Header("Power-Up Indicators")]
    [SerializeField] private GameObject powerUpPanel;
    [SerializeField] private Image powerUpIcon;
    [SerializeField] private Text powerUpTimerText;

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to relevant game events ---
        GameManager.OnGameStateChanged += HandleGameStateChange;
        ScoreManager.OnScoreUpdated += UpdateScoreUI;
        ScoreManager.OnCoinsUpdated += UpdateCoinUI;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
        ScoreManager.OnScoreUpdated -= UpdateScoreUI;
        ScoreManager.OnCoinsUpdated -= UpdateCoinUI;
    }

    private void HandleGameStateChange(GameManager.GameState newState)
    {
        gameOverPanel.SetActive(newState == GameManager.GameState.GameOver);
        if(newState == GameManager.GameState.GameOver)
        {
            finalScoreText.text = "Score: " + ScoreManager.Instance.GetCurrentScore();
            finalCoinText.text = "Coins: " + ScoreManager.Instance.GetCurrentCoins();
        }
    }

    private void UpdateScoreUI(int newScore)
    {
        if (scoreText != null) scoreText.text = "Score: " + newScore;
    }

    private void UpdateCoinUI(int newCoinCount)
    {
        if (coinText != null) coinText.text = "Coins: " + newCoinCount;
    }

    // --- TUTORIAL INTEGRATION ---

    public void ShowTutorialMessage(string message, float duration)
    {
        if (tutorialPanel == null || tutorialText == null) return;
        
        tutorialText.text = message;
        tutorialPanel.SetActive(true);

        if (duration > 0)
        {
            StartCoroutine(HideTutorialMessageAfterDelay(duration));
        }
    }

    public void HideTutorialMessage()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    private IEnumerator HideTutorialMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideTutorialMessage();
    }

    // --- POWER-UP UI INTEGRATION ---

    public void ShowPowerUpIcon(PowerUpType type, float duration)
    {
        if (powerUpPanel == null || powerUpIcon == null) return;

        // Here you would set the sprite of the powerUpIcon based on the PowerUpType
        // Example: powerUpIcon.sprite = GetSpriteForPowerUp(type);

        powerUpPanel.SetActive(true);
        StartCoroutine(PowerUpTimer(duration));
    }

    private IEnumerator PowerUpTimer(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            if(powerUpTimerText != null) powerUpTimerText.text = timer.ToString("F1");
            timer -= Time.deltaTime;
            yield return null;
        }
        powerUpPanel.SetActive(false);
    }
}
