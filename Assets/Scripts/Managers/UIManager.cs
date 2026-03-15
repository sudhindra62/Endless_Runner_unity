
using EndlessRunner.Core;
using EndlessRunner.Events;
using EndlessRunner.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Main Menu Panel")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button themeSwitcherButton;

        [Header("Gameplay Panel")]
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        [Header("Game Over Panel")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button restartButton;

        private ScoreManager scoreManager;
        private ThemeSwitcher themeSwitcher;
        private ThemeColorApplicator themeColorApplicator;

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
            themeSwitcher = gameObject.AddComponent<ThemeSwitcher>();
            themeColorApplicator = gameObject.AddComponent<ThemeColorApplicator>();
        }

        private void Start()
        {
            scoreManager = ServiceLocator.Get<ScoreManager>();
            
            // Subscribe to events
            ServiceLocator.Get<GameManager>().OnGameStateChanged += OnGameStateChanged;
            scoreManager.OnScoreChanged += UpdateScoreText;
            scoreManager.OnHighScoreChanged += UpdateHighScoreText;

            // Add button listeners
            startButton.onClick.AddListener(OnStartButtonPressed);
            restartButton.onClick.AddListener(OnRestartButtonPressed);
            themeSwitcherButton.onClick.AddListener(OnThemeSwitcherButtonPressed);

            // Initial UI state
            ShowMainMenu();
            UpdateScoreText(0);
            UpdateHighScoreText(scoreManager.HighScore);
        }

        private void OnDestroy()
        {
            ServiceLocator.Get<GameManager>().OnGameStateChanged -= OnGameStateChanged;
            scoreManager.OnScoreChanged -= UpdateScoreText;
            scoreManager.OnHighScoreChanged -= UpdateHighScoreText;
        }

        private void OnGameStateChanged(GameManager.GameState newState)
        {
            switch (newState)
            {
                case GameManager.GameState.MainMenu:
                    ShowMainMenu();
                    break;
                case GameManager.GameState.Playing:
                    ShowGameplayPanel();
                    break;
                case GameManager.GameState.GameOver:
                    ShowGameOverPanel();
                    break;
            }
        }

        private void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
            gameplayPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }

        private void ShowGameplayPanel()
        {
            mainMenuPanel.SetActive(false);
            gameplayPanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }

        private void ShowGameOverPanel()
        {
            mainMenuPanel.SetActive(false);
            gameplayPanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        private void UpdateScoreText(int newScore)
        {
            scoreText.text = $"Score: {newScore}";
        }

        private void UpdateHighScoreText(int newHighScore)
        {
            highScoreText.text = $"High Score: {newHighScore}";
        }

        private void OnStartButtonPressed()
        {
            ServiceLocator.Get<GameFlowController>().StartGame();
        }

        private void OnRestartButtonPressed()
        {
            ServiceLocator.Get<GameFlowController>().StartGame();
        }

        private void OnThemeSwitcherButtonPressed()
        {
            themeSwitcher.SwitchTheme();
        }
    }
}
