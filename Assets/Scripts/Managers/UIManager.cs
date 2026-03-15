
using System;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.UI;
using EndlessRunner.Missions;

namespace EndlessRunner.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        #region Events
        public event Action OnPlayButtonPressed;
        public event Action OnRestartButtonPressed;
        public event Action OnMainMenuButtonPressed;
        public event Action OnPauseButtonPressed;
        #endregion

        #region Serialized Fields
        [Header("UI Panels")]
        [SerializeField] private MainMenuPanel mainMenuPanel;
        [SerializeField] private GameplayPanel gameplayPanel;
        [SerializeField] private PausePanel pausePanel;
        [SerializeField] private GameOverPanel gameOverPanel;

        [Header("UI Elements")]
        [SerializeField] private MissionUI missionUI;
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            SetupPanels();
            SubscribeToGameManager();

            // Initial state
            mainMenuPanel.Show();
            gameplayPanel.Hide();
            pausePanel.Hide();
            gameOverPanel.Hide();
            missionUI.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UnsubscribeFromGameManager();
        }
        
        private void Update()
        {
            if(GameManager.Instance != null && GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
            {
                if (MissionManager.Instance != null && MissionManager.Instance.GetCurrentMission() != null)
                {
                    missionUI.UpdateMission(MissionManager.Instance.GetCurrentMission());
                }
            }
        }
        #endregion

        #region Setup
        private void SetupPanels()
        {
            mainMenuPanel.Setup(this);
            gameplayPanel.Setup(this);
            pausePanel.Setup(this);
            gameOverPanel.Setup(this);
        }

        private void SubscribeToGameManager()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }
        }

        private void UnsubscribeFromGameManager()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
        }
        #endregion

        #region Game State Handling
        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            mainMenuPanel.Hide();
            gameplayPanel.Hide();
            pausePanel.Hide();
            gameOverPanel.Hide();
            missionUI.gameObject.SetActive(false);

            switch (newState)
            {
                case GameManager.GameState.MainMenu:
                    mainMenuPanel.Show();
                    break;
                case GameManager.GameState.Playing:
                    gameplayPanel.Show();
                    missionUI.gameObject.SetActive(true);
                    break;
                case GameManager.GameState.Paused:
                    gameplayPanel.Show(); // Keep gameplay UI visible
                    pausePanel.Show();
                    missionUI.gameObject.SetActive(true);
                    break;
                case GameManager.GameState.GameOver:
                    gameplayPanel.Show(); // Keep final score visible
                    gameOverPanel.Show();
                    break;
            }
        }
        #endregion
    }
}
