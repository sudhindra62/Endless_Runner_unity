using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Central Manager for all UI systems.
/// Global scope Singleton for project-wide accessibility.
/// Aligned with AEIS protocols for zero-leak system connectivity.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private CurrencyUI currencyUI;
    [SerializeField] private PowerUpHUDController powerUpHUDController;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private RevivePopupUI revivePopup;
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private DailyRewardUI dailyRewardUI;
    [SerializeField] private AchievementUI achievementUI;
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private CharacterCustomizationUI characterCustomizationUI;
    [SerializeField] private ThemeShopUI themeShopUI;
    [SerializeField] private Image mainPanelImage;

    [Header("Buttons")]
    [SerializeField] private Button achievementsButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button characterCustomizationButton;
    [SerializeField] private Button themeShopButton;

    // Legacy Screen References (for compatibility)
    public HomeScreenUI homeScreen;
    public GameScreenUI gameScreen;
    public PauseScreenUI pauseScreen;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.OnGameStateChanged += HandleGameStateChanged;
        }
        GameEvents.OnScoreGained += UpdateScoreUI;
        ThemeManager.OnThemeChanged += HandleThemeChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
        }
        GameEvents.OnScoreGained -= UpdateScoreUI;
        ThemeManager.OnThemeChanged -= HandleThemeChanged;
    }

    private void Start()
    {
        InitializeUI();
        if (achievementsButton) achievementsButton.onClick.AddListener(ShowAchievementsPanel);
        if (leaderboardButton) leaderboardButton.onClick.AddListener(ShowLeaderboardPanel);
        if (settingsButton) settingsButton.onClick.AddListener(ShowSettingsPanel);
        if (characterCustomizationButton) characterCustomizationButton.onClick.AddListener(ShowCharacterCustomizationPanel);
        if (themeShopButton) themeShopButton.onClick.AddListener(ShowThemeShopPanel);
    }

    private void InitializeUI()
    {
        UpdateScoreUI(0);
        SetPanelActive(mainMenuPanel, true);
        SetPanelActive(gameOverPanel, false);
        if (revivePopup) SetPanelActive(revivePopup.gameObject, false);
        if (tutorialUI) SetPanelActive(tutorialUI.gameObject, false);
        if (achievementUI) SetPanelActive(achievementUI.gameObject, false);
        if (leaderboardUI) SetPanelActive(leaderboardUI.gameObject, false);
        if (settingsUI) SetPanelActive(settingsUI.gameObject, false);
        if (characterCustomizationUI) SetPanelActive(characterCustomizationUI.gameObject, false);
        if (themeShopUI) SetPanelActive(themeShopUI.gameObject, false);
    }

    private void HandleGameStateChanged(GameState newState)
    {
        SetPanelActive(mainMenuPanel, newState == GameState.MainMenu);
        SetPanelActive(gameOverPanel, newState == GameState.GameOver);
        if (powerUpHUDController) SetPanelActive(powerUpHUDController.gameObject, newState == GameState.Playing);
        if (currencyUI) SetPanelActive(currencyUI.gameObject, newState == GameState.Playing || newState == GameState.MainMenu);
    }

    private void HandleThemeChanged(ThemeSO theme)
    {
        if (mainPanelImage != null)
        {
            mainPanelImage.sprite = theme.uiPanelSprite;
        }

        foreach (Button button in FindObjectsByType<Button>(FindObjectsSortMode.None))
        {
            ColorBlock colors = button.colors;
            colors.normalColor = theme.uiAccentColor;
            button.colors = colors;
        }
    }

    private void SetPanelActive(GameObject panel, bool isActive)
    {
        if (panel != null) panel.SetActive(isActive);
    }

    public void UpdateScoreUI(int score)
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
    }

    public void UpdateCurrency(int coins, int gems)
    {
        if (currencyUI != null) currencyUI.UpdateDisplay(coins, gems);
    }

    public void ShowHomeScreen() => SetPanelActive(mainMenuPanel, true);
    public void ShowGameScreen() => SetPanelActive(mainMenuPanel, false);
    public void ShowPauseScreen() => SetPanelActive(pauseScreen != null ? pauseScreen.gameObject : null, true);
    public void HidePauseScreen() => SetPanelActive(pauseScreen != null ? pauseScreen.gameObject : null, false);
    public void ShowGameOverScreen() => SetPanelActive(gameOverPanel, true);

    public void ShowAchievementsPanel() => achievementUI?.ShowPanel();
    public void ShowLeaderboardPanel() => leaderboardUI?.ShowPanel();
    public void ShowSettingsPanel() => settingsUI?.ShowPanel();
    public void ShowCharacterCustomizationPanel() => characterCustomizationUI?.ShowPanel();
    public void ShowThemeShopPanel() => themeShopUI?.ShowPanel();

    public void OnRestartButtonPressed() => GameManager.Instance.StartGame();

    public void UpdateCoinCount(int coins) => UpdateCurrency(coins, 0);
    public void UpdateShardCountUI(int shards) { /* Optional hook */ }
    
    public void ShowSystemMessage(string msg) { Debug.Log($"System -> {msg}"); }
    public void GoToMainMenu() => HandleGameStateChanged(GameState.MainMenu);
    public void ShowTrophyGallery() { Debug.Log("Trophy Gallery requested."); }
    public void OnPauseButtonPressed() { if (GameManager.Instance != null) GameManager.Instance.TogglePause(); }
    public void OnMainMenuButtonPressed() => GoToMainMenu();
    
    // SaveManager.GameData alias compatibility bridge
    public GameData GetGameData() => SaveManager.Instance?.Data;
}
