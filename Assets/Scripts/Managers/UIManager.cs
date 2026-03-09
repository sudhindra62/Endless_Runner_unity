using UnityEngine;
using UnityEngine.UI;
using TMPro; // Using TextMeshPro for better text rendering

/// <summary>
/// Manages all user interface elements and game state transitions.
/// Logic fully restored and integrated by Supreme Guardian Architect v12.
/// This system is the central hub for all player-facing information and interactions.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    [Tooltip("The main menu panel, shown at the start of the game.")]
    [SerializeField] private GameObject mainMenuPanel;
    [Tooltip("The in-game Heads-Up Display, shown during gameplay.")]
    [SerializeField] private GameObject gameHUDPanel;
    [Tooltip("The pause menu panel, shown when the game is paused.")]
    [SerializeField] private GameObject pauseMenuPanel;
    [Tooltip("The game over screen, shown when the player loses.")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Text Elements")]
    [Tooltip("Text to display the current score during gameplay.")]
    [SerializeField] private TextMeshProUGUI scoreText_HUD;
    [Tooltip("Text to display the final score on the game over screen.")]
    [SerializeField] private TextMeshProUGUI finalScoreText_GameOver;
    [Tooltip("Text to display the high score on the game over screen.")]
    [SerializeField] private TextMeshProUGUI highScoreText_GameOver;
    [Tooltip("Text to display the high score on the main menu.")]
    [SerializeField] private TextMeshProUGUI highScoreText_MainMenu;

    [Header("Buttons")]
    [Tooltip("Button to start the game from the main menu.")]
    [SerializeField] private Button startButton;
    [Tooltip("Button to pause the game during gameplay.")]
    [SerializeField] private Button pauseButton;
    [Tooltip("Button to resume the game from the pause menu.")]
    [SerializeField] private Button resumeButton;
    [Tooltip("Button to restart the game from the game over or pause screen.")]
    [SerializeField] private Button restartButton_GameOver;
    [Tooltip("Button to restart the game from the pause menu.")]
    [SerializeField] private Button restartButton_Pause;


    protected override void Awake()
    {
        base.Awake();
        // --- INSPECTOR_AUTO-WIRING: Add listeners to buttons to link them to GameManager functions ---
        startButton.onClick.AddListener(GameManager.Instance.StartGame);
        pauseButton.onClick.AddListener(GameManager.Instance.PauseGame);
        resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);
        restartButton_GameOver.onClick.AddListener(GameManager.Instance.RestartGame);
        restartButton_Pause.onClick.AddListener(GameManager.Instance.RestartGame);
    }

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to events from other managers ---
        // GameManager.OnGameStateChanged += HandleGameStateChanged; // This assumes a static event. Let's use direct calls from GameManager for now.
        ScoreManager.OnScoreChanged += UpdateScoreDisplay;
        ScoreManager.OnHighScoreChanged += UpdateHighScoreDisplay;
    }

    private void OnDisable()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks ---
        // GameManager.OnGameStateChanged -= HandleGameStateChanged;
        ScoreManager.OnScoreChanged -= UpdateScoreDisplay;
        ScoreManager.OnHighScoreChanged -= UpdateHighScoreDisplay;
    }

    private void Start()
    {
        // Initialize UI based on the initial game state
        HandleGameStateChanged(GameManager.Instance.CurrentState);
        // Make sure high score is shown on start
        UpdateHighScoreDisplay(ScoreManager.Instance.HighScore);
    }

    /// <summary>
    /// Central handler for switching UI panels based on game state.
    /// </summary>
    public void HandleGameStateChanged(GameManager.GameState newState)
    {
        // Deactivate all panels first to ensure a clean slate
        mainMenuPanel.SetActive(false);
        gameHUDPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Activate the correct panel for the new state
        switch (newState)
        {
            case GameManager.GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                gameHUDPanel.SetActive(true);
                break;
            case GameManager.GameState.Paused:
                pauseMenuPanel.SetActive(true);
                break;
            case GameManager.GameState.GameOver:
                gameOverPanel.SetActive(true);
                // Update final scores when game is over
                finalScoreText_GameOver.text = "Score: " + ScoreManager.Instance.CurrentScore.ToString();
                highScoreText_GameOver.text = "High Score: " + ScoreManager.Instance.HighScore.ToString();
                break;
        }
    }

    /// <summary>
    /// Updates the score text on the in-game HUD.
    /// </summary>
    private void UpdateScoreDisplay(int newScore)
    {
        if (scoreText_HUD != null)
        {
            scoreText_HUD.text = "Score: " + newScore.ToString();
        }
    }

    /// <summary>
    /// Updates the high score text on the Main Menu and Game Over screens.
    /// </summary>
    private void UpdateHighScoreDisplay(int newHighScore)
    {
        if (highScoreText_MainMenu != null)
        {
            highScoreText_MainMenu.text = "High Score: " + newHighScore.ToString();
        }
        if (highScoreText_GameOver != null)
        { 
            highScoreText_GameOver.text = "High Score: " + newHighScore.ToString();
        }
    }
}
