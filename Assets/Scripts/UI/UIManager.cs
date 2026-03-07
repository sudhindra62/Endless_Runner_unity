
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the game's entire UI, showing and hiding panels based on game state.
/// Authored by OMNI_ARCHITECT_v31 to provide a centralized UI control system.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    // --- Inspector References for UI Panels ---
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;

    // --- Inspector References for UI Text ---
    [Header("In-Game HUD Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Game Over Elements")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalHighScoreText;

    // --- UNITY LIFECYCLE ---

    private void OnEnable()
    {
        // Subscribe to Game and Score Manager events
        GameManager.OnGameStateChanged += OnGameStateChanged;
        ScoreManager.OnScoreChanged += UpdateScoreText;
        ScoreManager.OnHighScoreChanged += UpdateHighScoreText;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        ScoreManager.OnScoreChanged -= UpdateScoreText;
        ScoreManager.OnHighScoreChanged -= UpdateHighScoreText;
    }

    void Start()
    {
        // Ensure all text elements are updated on start
        if (ScoreManager.Instance != null)
        {
            UpdateScoreText(ScoreManager.Instance.CurrentScore);
            UpdateHighScoreText(ScoreManager.Instance.HighScore);
        }
    }

    // --- PRIVATE METHODS ---

    private void UpdateScoreText(int newScore)
    {
        if (scoreText != null) scoreText.text = "Score: " + newScore.ToString();
    }

    private void UpdateHighScoreText(int newHighScore)
    {
        if (highScoreText != null) highScoreText.text = "High Score: " + newHighScore.ToString();
    }

    private void UpdateGameOverText()
    {
        if (finalScoreText != null && ScoreManager.Instance != null)
        {
            finalScoreText.text = "Your Score: " + ScoreManager.Instance.CurrentScore.ToString();
        }
        if (finalHighScoreText != null && ScoreManager.Instance != null)
        {
            finalHighScoreText.text = "High Score: " + ScoreManager.Instance.HighScore.ToString();
        }
    }
    
    private void ShowPanel(GameObject panelToShow)
    {
        // Disable all panels first
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (inGameHUD != null) inGameHUD.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Enable the requested panel
        if (panelToShow != null) panelToShow.SetActive(true);
    }

    // --- Event Handlers ---

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                ShowPanel(mainMenuPanel);
                break;
            case GameState.Playing:
                ShowPanel(inGameHUD);
                break;
            case GameState.Paused:
                ShowPanel(pauseMenuPanel);
                break;
            case GameState.GameOver:
                UpdateGameOverText(); // Update the text before showing the panel
                ShowPanel(gameOverPanel);
                break;
        }
    }
}
