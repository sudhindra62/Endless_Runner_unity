
using System;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.UI;

namespace EndlessRunner.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public event Action OnRestartButtonPressed;
        public event Action OnMainMenuButtonPressed;

        [Header("UI Panels")]
        [SerializeField] private UIPanel_InGame hudPanel;
        [SerializeField] private UIPanel_GameOver gameOverPanel;
        [SerializeField] private UIPanel_MainMenu mainMenuPanel;
        [SerializeField] private UIPanel_Pause pausePanel;
        [SerializeField] private UIPanel_Tutorial tutorialPanel;

        protected override void Awake()
        {
            base.Awake();
            // Ensure all panels are found if not wired in the inspector
            hudPanel = hudPanel ?? FindObjectOfType<UIPanel_InGame>(true);
            gameOverPanel = gameOverPanel ?? FindObjectOfType<UIPanel_GameOver>(true);
            mainMenuPanel = mainMenuPanel ?? FindObjectOfType<UIPanel_MainMenu>(true);
            pausePanel = pausePanel ?? FindObjectOfType<UIPanel_Pause>(true);
            tutorialPanel = tutorialPanel ?? FindObjectOfType<UIPanel_Tutorial>(true);
        }

        private void Start()
        {
            SubscribeToEvents();

            if (GameManager.Instance != null)
            {
                HandleGameStateChanged(GameManager.Instance.CurrentState);
            }
            else
            {
                Debug.LogError("Guardian Architect CRITICAL ERROR: GameManager not found!");
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
                GameManager.Instance.OnScoreChanged += UpdateScore;
                GameManager.Instance.OnCoinsChanged += UpdateCoins;
            }

            if (gameOverPanel != null) gameOverPanel.OnRestartClicked += HandleRestartClicked;
            if (gameOverPanel != null) gameOverPanel.OnMainMenuClicked += HandleMainMenuClicked;
            if (pausePanel != null) pausePanel.OnMainMenuClicked += HandleMainMenuClicked;
            if (mainMenuPanel != null) mainMenuPanel.OnPlayClicked += HandlePlayClicked;
        }

        private void UnsubscribeFromEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
                GameManager.Instance.OnScoreChanged -= UpdateScore;
                GameManager.Instance.OnCoinsChanged -= UpdateCoins;
            }
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            hudPanel?.gameObject.SetActive(newState == GameManager.GameState.Playing);
            gameOverPanel?.gameObject.SetActive(newState == GameManager.GameState.GameOver);
            mainMenuPanel?.gameObject.SetActive(newState == GameManager.GameState.MainMenu);
            pausePanel?.gameObject.SetActive(newState == GameManager.GameState.Paused);

            if (newState == GameManager.GameState.GameOver && GameManager.Instance != null)
            {
                gameOverPanel?.SetFinalScore(GameManager.Instance.Score);
            }
        }

        private void UpdateScore(int newScore)
        {
            hudPanel?.UpdateScore(newScore);
        }

        private void UpdateCoins(int newCoins)
        {
            hudPanel?.UpdateCoins(newCoins);
        }

        private void HandleRestartClicked()
        {
            OnRestartButtonPressed?.Invoke();
        }
        
        private void HandleMainMenuClicked()
        {
            OnMainMenuButtonPressed?.Invoke();
        }

        private void HandlePlayClicked()
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
        }

        public void ShowTutorialMessage(string message, float duration)
        {
            tutorialPanel?.Show(message, duration);
        }

        public void HideTutorialMessage()
        {
            tutorialPanel?.Hide();
        }
    }
}
