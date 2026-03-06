
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game UI, such as the Game Over screen.
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    
    [Header("Currency Display")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI gemText;

    private void Start()
    {
        // Ensure the game over panel is hidden at the start
        if (gameOverPanel != null) 
        {
            gameOverPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        }

        // Subscribe to game events
        GameManager.OnGameOver += ShowGameOverScreen;
        
        // Subscribe to currency events
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoinText;
            CurrencyManager.Instance.OnGemsChanged += UpdateGemText;
            // Initial UI update
            UpdateCoinText(CurrencyManager.Instance.GetCoins());
            UpdateGemText(CurrencyManager.Instance.GetGems());
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnGameOver -= ShowGameOverScreen;
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(GameManager.Instance.RestartGame);
        }
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= UpdateCoinText;
            CurrencyManager.Instance.OnGemsChanged -= UpdateGemText;
        }
    }

    private void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    
    private void UpdateCoinText(int newBalance)
    {
        if (coinText != null)
        {
            coinText.text = newBalance.ToString();
        }
    }

    private void UpdateGemText(int newBalance)
    {
        if (gemText != null)
        {
            gemText.text = newBalance.ToString();
        }
    }
}
