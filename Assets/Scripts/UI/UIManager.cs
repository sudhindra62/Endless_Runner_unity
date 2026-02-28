
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameplayUIPanel;
    [SerializeField] private GameObject pauseUIPanel;
    [SerializeField] private GameObject gameOverUIPanel;
    [SerializeField] private GameObject mainMenuUIPanel;
    [SerializeField] private GameObject settingsUIPanel;
    [SerializeField] private GameObject shopUIPanel;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private TMP_Text levelText;

    [Header("UI Controls")]
    [SerializeField] private Slider xpSlider;

    private GameStateManager gameStateManager;
    private ScoreManager scoreManager;
    private CurrencyManager currencyManager;
    private PlayerDataManager playerDataManager;
    private GameFlowController gameFlowController;

    private void Awake()
    {
        gameStateManager = ServiceLocator.Current.Get<GameStateManager>();
        scoreManager = ServiceLocator.Current.Get<ScoreManager>();
        currencyManager = ServiceLocator.Current.Get<CurrencyManager>();
        playerDataManager = ServiceLocator.Current.Get<PlayerDataManager>();
        gameFlowController = ServiceLocator.Current.Get<GameFlowController>();
    }

    private void OnEnable()
    {
        if (gameStateManager != null) gameStateManager.OnGameStateChanged += HandleGameStateChanged;
        if (scoreManager != null) scoreManager.OnScoreChanged += UpdateScoreDisplay;
        if (currencyManager != null) 
        {
            CurrencyManager.OnCoinsChanged += UpdateCoinDisplay;
            CurrencyManager.OnGemsChanged += UpdateGemDisplay;
        }
        if (playerDataManager != null)
        {
            PlayerDataManager.OnLevelChanged += UpdateLevelDisplay;
            PlayerDataManager.OnXPChanged += UpdateXPDisplay;
        }
    }

    private void OnDisable()
    {
        if (gameStateManager != null) gameStateManager.OnGameStateChanged -= HandleGameStateChanged;
        if (scoreManager != null) scoreManager.OnScoreChanged -= UpdateScoreDisplay;
        if (currencyManager != null)
        {
            CurrencyManager.OnCoinsChanged -= UpdateCoinDisplay;
            CurrencyManager.OnGemsChanged -= UpdateGemDisplay;
        }
        if (playerDataManager != null)
        {
            PlayerDataManager.OnLevelChanged -= UpdateLevelDisplay;
            PlayerDataManager.OnXPChanged -= UpdateXPDisplay;
        }
    }

    private void Start()
    {
        if (gameStateManager != null) HandleGameStateChanged(gameStateManager.CurrentState);
        if (scoreManager != null) UpdateScoreDisplay(scoreManager.CurrentScore);
        if (currencyManager != null) 
        {
            UpdateCoinDisplay(currencyManager.Coins);
            UpdateGemDisplay(currencyManager.Gems);
        }
        if (playerDataManager != null)
        {
            UpdateLevelDisplay(playerDataManager.Level);
            UpdateXPDisplay(playerDataManager.XP, playerDataManager.XPForNextLevel);
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        gameplayUIPanel.SetActive(newState == GameState.Playing);
        pauseUIPanel.SetActive(newState == GameState.Paused);
        gameOverUIPanel.SetActive(newState == GameState.GameOver);
        mainMenuUIPanel.SetActive(newState == GameState.MainMenu);

        if (newState == GameState.GameOver)
        {
            UpdateGameOverScores();
        }
    }

    private void UpdateScoreDisplay(int newScore)
    {
        if (scoreText != null) scoreText.text = "Score: " + newScore.ToString();
    }

    private void UpdateGameOverScores()
    {
        if (finalScoreText != null) finalScoreText.text = "Score: " + scoreManager.CurrentScore.ToString();
        if (bestScoreText != null) bestScoreText.text = "Best: " + scoreManager.HighScore.ToString();
    }

    private void UpdateCoinDisplay(int newBalance)
    {
        if (coinText != null) coinText.text = newBalance.ToString();
    }

    private void UpdateGemDisplay(int newBalance)
    {
        if (gemText != null) gemText.text = newBalance.ToString();
    }

    private void UpdateLevelDisplay(int newLevel)
    {
        if (levelText != null) levelText.text = "Level " + newLevel.ToString();
    }

    private void UpdateXPDisplay(int currentXP, int xpForNextLevel)
    {
        if (xpSlider != null) xpSlider.value = (float)currentXP / xpForNextLevel;
    }

    public void OnPauseButtonPressed() => gameFlowController.Pause();
    public void OnResumeButtonPressed() => gameFlowController.Resume();
    public void OnRestartButtonPressed() => gameFlowController.StartRun();
    public void OnMainMenuButtonPressed() => gameFlowController.GoToMainMenu();
    public void OnSettingsButtonPressed() => settingsUIPanel.SetActive(true); // Or navigate to a settings scene
    public void OnShopButtonPressed() => shopUIPanel.SetActive(true); // Or navigate to a shop scene
}
