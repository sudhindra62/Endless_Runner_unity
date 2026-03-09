
using UnityEngine;
using UnityEngine.UI;

public class GameHUDController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text coinText;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        // Subscribe to events
        // if (ScoreManager.Instance != null)
        // {
        //     ScoreManager.OnScoreUpdated += UpdateScore;
        //     ScoreManager.OnCoinsUpdated += UpdateCoins;
        // }
        // if (PlayerHealth.Instance != null)
        // {
        //     PlayerHealth.OnHealthUpdated += UpdateHealth;
        // }
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        // if (ScoreManager.Instance != null)
        // {
        //     ScoreManager.OnScoreUpdated -= UpdateScore;
        //     ScoreManager.OnCoinsUpdated -= UpdateCoins;
        // }
        // if (PlayerHealth.Instance != null)
        // {
        //     PlayerHealth.OnHealthUpdated -= UpdateHealth;
        // }
    }

    private void UpdateScore(int newScore)
    {
        if (scoreText != null) scoreText.text = "Score: " + newScore;
    }

    private void UpdateCoins(int newCoinCount)
    {
        if (coinText != null) coinText.text = "Coins: " + newCoinCount;
    }

    private void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthSlider != null) healthSlider.value = currentHealth / maxHealth;
    }
}
