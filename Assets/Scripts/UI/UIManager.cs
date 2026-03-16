
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Core;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    [RequireComponent(typeof(TutorialUI))]
    [RequireComponent(typeof(DailyRewardUI))]
    [RequireComponent(typeof(AchievementUI))]
    [RequireComponent(typeof(LeaderboardUI))]
    [RequireComponent(typeof(SettingsUI))]
    [RequireComponent(typeof(CharacterCustomizationUI))]
    [RequireComponent(typeof(ThemeShopUI))]
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

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }
            GameEvents.OnScoreGained += UpdateScoreUI;
            if (ThemeManager.Instance != null)
            {
                ThemeManager.Instance.OnThemeChanged += HandleThemeChanged;
            }
        }

        private void OnDisable()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
            GameEvents.OnScoreGained -= UpdateScoreUI;
            if (ThemeManager.Instance != null)
            {
                ThemeManager.Instance.OnThemeChanged -= HandleThemeChanged;
            }
        }

        private void Start()
        {
            InitializeUI();
            achievementsButton.onClick.AddListener(ShowAchievementsPanel);
            leaderboardButton.onClick.AddListener(ShowLeaderboardPanel);
            settingsButton.onClick.AddListener(ShowSettingsPanel);
            characterCustomizationButton.onClick.AddListener(ShowCharacterCustomizationPanel);
            themeShopButton.onClick.AddListener(ShowThemeShopPanel);
        }

        private void InitializeUI()
        {
            UpdateScoreUI(0);
            SetPanelActive(mainMenuPanel, true);
            SetPanelActive(gameOverPanel, false);
            SetPanelActive(revivePopup.gameObject, false);
            SetPanelActive(tutorialUI.gameObject, false);
            SetPanelActive(achievementUI.gameObject, false);
            SetPanelActive(leaderboardUI.gameObject, false);
            SetPanelActive(settingsUI.gameObject, false);
            SetPanelActive(characterCustomizationUI.gameObject, false);
            SetPanelActive(themeShopUI.gameObject, false);
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            SetPanelActive(mainMenuPanel, newState == GameManager.GameState.MainMenu);
            SetPanelActive(gameOverPanel, newState == GameManager.GameState.GameOver);
            SetPanelActive(powerUpHUDController.gameObject, newState == GameManager.GameState.Playing);
            SetPanelActive(currencyUI.gameObject, newState == GameManager.GameState.Playing || newState == GameManager.GameState.MainMenu);
        }

        private void HandleThemeChanged(ThemeSO theme)
        {
            if (mainPanelImage != null)
            {
                mainPanelImage.sprite = theme.uiPanelSprite;
            }

            // Update the color of all buttons to match the theme's accent color
            foreach (Button button in FindObjectsOfType<Button>())
            {
                ColorBlock colors = button.colors;
                colors.normalColor = theme.uiAccentColor;
                button.colors = colors;
            }
        }

        private void SetPanelActive(GameObject panel, bool isActive)
        {
            if (panel != null)
            {
                panel.SetActive(isActive);
            }
        }

        public void UpdateScoreUI(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score}";
            }
        }

        public void ShowAchievementsPanel()
        {
            achievementUI.ShowPanel();
        }

        public void ShowLeaderboardPanel()
        {
            leaderboardUI.ShowPanel();
        }

        public void ShowSettingsPanel()
        {
            settingsUI.ShowPanel();
        }

        public void ShowCharacterCustomizationPanel()
        {
            characterCustomizationUI.ShowPanel();
        }

        public void ShowThemeShopPanel()
        {
            themeShopUI.ShowPanel();
        }

        public void OnRestartButtonPressed()
        {
            GameManager.Instance.StartGame();
        }
    }
}
