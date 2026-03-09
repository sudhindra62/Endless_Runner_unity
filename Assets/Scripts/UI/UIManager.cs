
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
    
    [Header("Power-Up Sprites")]
    [SerializeField] private Sprite coinMagnetIcon;
    [SerializeField] private Sprite scoreMultiplierIcon;
    [SerializeField] private Sprite invincibilityIcon;
    [SerializeField] private Sprite speedBoostIcon;
    [SerializeField] private Sprite doubleJumpIcon;

    private Dictionary<PowerUpType, Coroutine> _powerUpTimers = new Dictionary<PowerUpType, Coroutine>();

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
        ScoreManager.OnScoreUpdated += UpdateScoreUI;
        ScoreManager.OnCoinsUpdated += UpdateCoinUI;
        PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivated;
        PowerUpManager.Instance.OnPowerUpDeactivated += HandlePowerUpDeactivated;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
        ScoreManager.OnScoreUpdated -= UpdateScoreUI;
        ScoreManager.OnCoinsUpdated -= UpdateCoinUI;
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivated;
            PowerUpManager.Instance.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
        }
    }

    private void HandleGameStateChange(GameManager.GameState newState)
    {
        gameOverPanel.SetActive(newState == GameManager.GameState.GameOver);
        if (newState == GameManager.GameState.GameOver)
        {
            UpdateGameOverUI();
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

    private void UpdateGameOverUI()
    {
        finalScoreText.text = "Score: " + ScoreManager.Instance.GetCurrentScore();
        finalCoinText.text = "Coins: " + ScoreManager.Instance.GetCurrentCoins();
    }

    public void ShowTutorialMessage(string message, float duration)
    {
        if (tutorialPanel == null || tutorialText == null) return;
        tutorialText.text = message;
        tutorialPanel.SetActive(true);
        if (duration > 0) Invoke(nameof(HideTutorialMessage), duration);
    }

    public void HideTutorialMessage()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    private void HandlePowerUpActivated(PowerUp powerUp)
    {
        if (_powerUpTimers.TryGetValue(powerUp.type, out Coroutine existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
        }
        Coroutine timerCoroutine = StartCoroutine(PowerUpTimerRoutine(powerUp));
        _powerUpTimers[powerUp.type] = timerCoroutine;
    }

    private void HandlePowerUpDeactivated(PowerUp powerUp)
    {
        if (_powerUpTimers.ContainsKey(powerUp.type))
        {
            StopCoroutine(_powerUpTimers[powerUp.type]);
            _powerUpTimers.Remove(powerUp.type);
        }
        HidePowerUpUI();
    }

    private IEnumerator PowerUpTimerRoutine(PowerUp powerUp)
    {
        ShowPowerUpIcon(powerUp.type);
        float remainingTime = powerUp.duration;
        while (remainingTime > 0)
        {
            UpdatePowerUpTimer(remainingTime);
            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }
        HidePowerUpUI();
    }

    private void ShowPowerUpIcon(PowerUpType type)
    {
        if (powerUpPanel == null || powerUpIcon == null) return;
        powerUpIcon.sprite = GetSpriteForPowerUp(type);
        powerUpPanel.SetActive(true);
    }

    private void UpdatePowerUpTimer(float timeRemaining)
    {
        if (powerUpTimerText != null) powerUpTimerText.text = timeRemaining.ToString("F1");
    }

    private void HidePowerUpUI()
    {
        if (powerUpPanel != null) powerUpPanel.SetActive(false);
    }

    private Sprite GetSpriteForPowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.CoinMagnet: return coinMagnetIcon;
            case PowerUpType.ScoreMultiplier: return scoreMultiplierIcon;
            case PowerUpType.Invincibility: return invincibilityIcon;
            case PowerUpType.SpeedBoost: return speedBoostIcon;
            case PowerUpType.DoubleJump: return doubleJumpIcon;
            default: return null;
        }
    }
}
