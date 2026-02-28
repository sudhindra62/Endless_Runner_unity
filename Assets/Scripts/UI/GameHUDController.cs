
using TMPro;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

/// <summary>
/// Manages the Heads-Up Display for in-game stats. It subscribes to manager events to update its values,
/// ensuring it acts as a passive view with high performance and no garbage allocation in its update loops.
/// </summary>
public class GameHUDController : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI gemText;

    [Header("Buttons")]
    [SerializeField] private Button pauseButton;

    [Header("Sub-Controllers")]
    [SerializeField] private PowerUpHUDController powerUpHUDController;

    // Caching for performance to avoid allocations.
    private readonly StringBuilder scoreBuilder = new StringBuilder(16);
    private readonly StringBuilder timeBuilder = new StringBuilder(8);
    private readonly StringBuilder multiplierBuilder = new StringBuilder(4);
    private readonly StringBuilder currencyBuilder = new StringBuilder(10);
    
    private ScoreManager scoreManager;
    private CurrencyManager currencyManager;
    private GameFlowController gameFlowController;

    private bool isPaused = false;
    private float runTime = 0f;

    private void Start()
    {
        // Resolve dependencies
        scoreManager = ServiceLocator.Get<ScoreManager>();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        gameFlowController = ServiceLocator.Get<GameFlowController>();

        // Initialize display with current values
        UpdateScoreText(scoreManager.CurrentScore);
        UpdateMultiplierText(scoreManager.CurrentMultiplier);
        UpdateCoinText(currencyManager.Coins);
        UpdateGemText(currencyManager.Gems);
        
        ResetHUD();
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScoreText;
        ScoreManager.OnMultiplierChanged += UpdateMultiplierText;
        CurrencyManager.OnCoinsChanged += UpdateCoinText;
        CurrencyManager.OnGemsChanged += UpdateGemText;
        if (pauseButton != null) pauseButton.onClick.AddListener(OnPauseClicked);

        GameFlowController.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreText;
        ScoreManager.OnMultiplierChanged -= UpdateMultiplierText;
        CurrencyManager.OnCoinsChanged -= UpdateCoinText;
        CurrencyManager.OnGemsChanged -= UpdateGemText;
        if (pauseButton != null) pauseButton.onClick.RemoveListener(OnPauseClicked);

        GameFlowController.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Update()
    {
        if (!isPaused)
        {
            runTime += Time.deltaTime;
            UpdateTimerText(runTime);
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        isPaused = (newState == GameState.Paused);

        if (newState == GameState.Menu)
        {
            ResetHUD();
        }
    }

    private void OnPauseClicked()
    {
        // Use the centralized GameFlowController to pause the game.
        gameFlowController?.PauseGame();
    }

    private void UpdateScoreText(long newScore)
    {
        scoreBuilder.Clear();
        scoreBuilder.Append(newScore);
        scoreText.SetText(scoreBuilder);
    }

    private void UpdateMultiplierText(int newMultiplier)
    {
        multiplierBuilder.Clear();
        multiplierBuilder.Append("x").Append(newMultiplier);
        multiplierText.SetText(multiplierBuilder);
    }

    private void UpdateCoinText(int amount)
    {
        currencyBuilder.Clear();
        currencyBuilder.Append(amount);
        coinText.SetText(currencyBuilder);
    }

    private void UpdateGemText(int amount)
    {
        currencyBuilder.Clear();
        currencyBuilder.Append(amount);
        gemText.SetText(currencyBuilder);
    }

    private void UpdateTimerText(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);

        timeBuilder.Clear();
        timeBuilder.Append(minutes.ToString("D2"));
        timeBuilder.Append(":");
        timeBuilder.Append(seconds.ToString("D2"));

        timerText.SetText(timeBuilder);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (powerUpHUDController != null) powerUpHUDController.ShowAll();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (powerUpHUDController != null) powerUpHUDController.HideAll();
    }
    
    public void ResetHUD()
    {
        runTime = 0f;
        UpdateTimerText(0);
        if (scoreManager != null)
        {
            UpdateScoreText(0);
            UpdateMultiplierText(1);
        }
        powerUpHUDController?.ResetIcons();
    }
}
