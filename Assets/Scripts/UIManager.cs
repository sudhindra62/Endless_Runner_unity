
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the primary in-game UI, including the score display and game state panels (Pause, Game Over).
/// Subscribes to GameManager events to automatically show/hide relevant UI screens.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The main gameplay HUD that is visible during play.")]
    [SerializeField] private GameObject gameplayUIPanel;
    [Tooltip("The panel that appears when the game is paused.")]
    [SerializeField] private GameObject pauseUIPanel;
    [Tooltip("The panel that appears when the game is over.")]
    [SerializeField] private GameObject gameOverUIPanel;

    [Header("Score Display")]
    [Tooltip("Text element to display the current score during gameplay.")]
    [SerializeField] private TMP_Text scoreText;
    [Tooltip("Text element on the game over screen to show the final score.")]
    [SerializeField] private TMP_Text finalScoreText;
    [Tooltip("Text element on the game over screen to show the best score.")]
    [SerializeField] private TMP_Text bestScoreText;

    #region Unity Lifecycle Methods

    private void OnEnable()
    {
        // Subscribe to GameManager events to automatically manage UI states
        GameManager.OnGamePaused += ShowPauseUI;
        GameManager.OnGameResumed += HidePauseUI;
        GameManager.OnGameOver += ShowGameOverUI;
    }

    private void OnDisable()
    {
        // Always unsubscribe from events when the object is disabled
        GameManager.OnGamePaused -= ShowPauseUI;
        GameManager.OnGameResumed -= HidePauseUI;
        GameManager.OnGameOver -= ShowGameOverUI;
    }

    private void Start()
    {
        // Ensure a clean UI state at the start of the game
        if (gameplayUIPanel) gameplayUIPanel.SetActive(true);
        if (pauseUIPanel) pauseUIPanel.SetActive(false);
        if (gameOverUIPanel) gameOverUIPanel.SetActive(false);
    }

    private void Update()
    {
        // Continuously update the score display during gameplay
        UpdateScoreDisplay();
    }

    #endregion

    #region UI Event Handlers

    private void ShowPauseUI()
    {
        if (pauseUIPanel) pauseUIPanel.SetActive(true);
        if (gameplayUIPanel) gameplayUIPanel.SetActive(false); // Hide gameplay UI on pause
    }

    private void HidePauseUI()
    {
        if (pauseUIPanel) pauseUIPanel.SetActive(false);
        if (gameplayUIPanel) gameplayUIPanel.SetActive(true); // Restore gameplay UI
    }

    private void ShowGameOverUI()
    {
        if (gameOverUIPanel) gameOverUIPanel.SetActive(true);
        if (gameplayUIPanel) gameplayUIPanel.SetActive(false); // Hide gameplay UI on game over

        // Populate the final scores on the game over screen
        if (finalScoreText && GameManager.Instance)
        {
            finalScoreText.text = "Score: " + GameManager.Instance.GetCurrentScore().ToString();
        }
        if (bestScoreText && GameManager.Instance)
        { 
            bestScoreText.text = "Best: " + GameManager.Instance.GetBestScore().ToString();
        }
    }

    #endregion

    #region Public UI Methods

    // These methods can be linked to UI buttons in the Inspector

    public void OnPauseButtonPressed()
    {
        if (GameManager.Instance) GameManager.Instance.PauseGame();
    }

    public void OnResumeButtonPressed()
    {
        if (GameManager.Instance) GameManager.Instance.ResumeGame();
    }

    public void OnRestartButtonPressed()
    {
        if (GameManager.Instance) GameManager.Instance.RestartGame();
    }

    #endregion

    #region Private Helpers

    private void UpdateScoreDisplay()
    {
        // Update the score text, but only if the gameplay UI is active
        if (scoreText && gameplayUIPanel.activeSelf && GameManager.Instance)
        {
            // scoreText.text = "Score: " + GameManager.Instance.GetCurrentScore().ToString();
        }
    }

    #endregion
}
