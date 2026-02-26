
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the primary in-game UI, including the score display and game state panels (Pause, Game Over).
/// Subscribes to GameStateManager events to automatically show/hide relevant UI screens.
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
        // Subscribe to the new, centralized GameStateManager event
        GameStateManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        // Always unsubscribe from events when the object is disabled
        GameStateManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        // Initial UI setup based on the starting game state
        if (GameStateManager.Instance != null)
        {
            HandleGameStateChanged(GameStateManager.Instance.CurrentState);
        }
    }

    private void Update()
    {
        // Continuously update the score display during gameplay
        UpdateScoreDisplay();
    }

    #endregion

    #region State Machine Driven UI

    private void HandleGameStateChanged(GameState newState)
    {
        gameplayUIPanel.SetActive(newState == GameState.Playing);
        pauseUIPanel.SetActive(newState == GameState.Paused);
        gameOverUIPanel.SetActive(newState == GameState.GameOver);

        if (newState == GameState.GameOver)
        {
            UpdateGameOverScores();
        }
    }

    private void UpdateGameOverScores()
    {
        // Populate the final scores on the game over screen
        if (finalScoreText && ScoreManager.Instance)
        {
            finalScoreText.text = "Score: " + ScoreManager.Instance.CurrentScore.ToString();
        }
        if (bestScoreText && ScoreManager.Instance)
        { 
            bestScoreText.text = "Best: " + ScoreManager.Instance.GetBestScore().ToString();
        }
    }

    #endregion

    #region Public UI Methods

    // These methods can be linked to UI buttons in the Inspector

    public void OnPauseButtonPressed()
    {
        if (GameManager.Instance != null) GameManager.Instance.Flow.Pause();
    }

    public void OnResumeButtonPressed()
    {
        if (GameManager.Instance != null) GameManager.Instance.Flow.Resume();
    }

    public void OnRestartButtonPressed()
    {
        if (GameManager.Instance != null) GameManager.Instance.Flow.StartRun();
    }

    #endregion

    #region Private Helpers

    private void UpdateScoreDisplay()
    {
        // Update the score text, but only if the gameplay UI is active
        if (scoreText && gameplayUIPanel.activeSelf && ScoreManager.Instance)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore.ToString();
        }
    }

    #endregion
}
