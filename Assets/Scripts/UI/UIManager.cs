
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI Text")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameManager.RegisterUIManager(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        mainMenuPanel.SetActive(newState == GameManager.GameState.MainMenu);
        gameUIPanel.SetActive(newState == GameManager.GameState.Playing);
        pauseMenuPanel.SetActive(newState == GameManager.GameState.Paused);
        gameOverPanel.SetActive(newState == GameManager.GameState.GameOver);

        if (newState == GameManager.GameState.GameOver)
        {
            UpdateGameOverScreen();
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateGameOverScreen()
    {
        if (GameManager.ScoreManager != null && highScoreText != null)
        {
            highScoreText.text = "High Score: " + GameManager.ScoreManager.HighScore;
        }
    }
    
    // The public Show... methods are now obsolete but can be kept for legacy calls or removed.
    public void ShowMainMenu() => HandleGameStateChanged(GameManager.GameState.MainMenu);
    public void ShowGameUI() => HandleGameStateChanged(GameManager.GameState.Playing);
    public void ShowPauseMenu() => HandleGameStateChanged(GameManager.GameState.Paused);
    public void ShowGameOverScreen() => HandleGameStateChanged(GameManager.GameState.GameOver);
}
