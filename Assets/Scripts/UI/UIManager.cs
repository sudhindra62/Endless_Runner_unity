
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

    private GameStateManager gameStateManager;
    private ScoreManager scoreManager;
    private GameFlowController gameFlowController;

    #region Unity Lifecycle Methods

    private void Awake()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameFlowController = FindObjectOfType<GameFlowController>();
    }

    private void OnEnable()
    {
        gameStateManager.OnGameStateChanged += HandleGameStateChanged;
        scoreManager.OnScoreChanged += UpdateScoreDisplay;
    }

    private void OnDisable()
    {
        gameStateManager.OnGameStateChanged -= HandleGameStateChanged;
        scoreManager.OnScoreChanged -= UpdateScoreDisplay;
    }

    private void Start()
    {
        HandleGameStateChanged(gameStateManager.CurrentState);
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
        if (finalScoreText)
        {
            finalScoreText.text = "Score: " + scoreManager.CurrentScore.ToString();
        }
        if (bestScoreText) 
        { 
            bestScoreText.text = "Best: " + scoreManager.GetBestScore().ToString();
        }
    }

    #endregion

    #region Public UI Methods

    public void OnPauseButtonPressed()
    {
        gameFlowController.Pause();
    }

    public void OnResumeButtonPressed()
    {
        gameFlowController.Resume();
    }

    public void OnRestartButtonPressed()
    {
        gameFlowController.StartRun();
    }

    #endregion

    #region Private Helpers

    private void UpdateScoreDisplay(int newScore)
    {
        if (scoreText)
        {
            scoreText.text = "Score: " + newScore.ToString();
        }
    }

    #endregion
}
