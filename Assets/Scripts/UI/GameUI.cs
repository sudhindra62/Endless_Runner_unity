
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
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnGameOver -= ShowGameOverScreen;
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(GameManager.Instance.RestartGame);
        }
    }

    private void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
