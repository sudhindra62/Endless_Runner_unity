
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;
using System.Collections;

namespace Managers
{
    /// <summary>
    /// The central hub for all UI-related operations, from in-game HUD to game-over screens and panels.
    /// It coordinates with other managers to display data and listens to game state changes.
    /// This script has been fortified by Supreme Guardian Architect v13 to properly delegate to specialized UI panels.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        public event Action OnRestartButtonPressed;

        // --- PANEL REFERENCES (Assigned in Inspector) ---
        [Header("UI Panels")]
        [Tooltip("The primary in-game display for score, coins, etc.")]
        [SerializeField] private UIPanel_InGame hudPanel;
        [Tooltip("The screen displayed upon game over.")]
        [SerializeField] private UIPanel_GameOver gameOverPanel;
        [Tooltip("The panel used to display tutorial instructions.")]
        [SerializeField] private UIPanel_Tutorial tutorialPanel; // <-- INTEGRATION POINT

        // --- UNITY LIFECYCLE ---
        protected override void Awake()
        {
            base.Awake();
            // Safely find panels if not assigned
            if (hudPanel == null) hudPanel = FindObjectOfType<UIPanel_InGame>();
            if (gameOverPanel == null) gameOverPanel = FindObjectOfType<UIPanel_GameOver>();
            if (tutorialPanel == null) tutorialPanel = FindObjectOfType<UIPanel_Tutorial>();
        }

        private void Start()
        {
            // Subscribe to critical game events
            SubscribeToEvents();

            // Initialize UI state based on current game state
            if (GameManager.Instance != null)
            {
                HandleGameStateChanged(GameManager.Instance.CurrentState);
            }
            else
            {
                // If no GameManager, assume we are in a menu or pre-game state
                hudPanel?.Hide();
                gameOverPanel?.Hide();
                tutorialPanel?.Hide();
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            UnsubscribeFromEvents();
        }

        // --- EVENT HANDLING ---

        private void SubscribeToEvents()
        {
            if (GameManager.Instance != null) GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged += UpdateScore;
                ScoreManager.Instance.OnCoinsChanged += UpdateCoins;
            }
            if (gameOverPanel != null) gameOverPanel.OnRestartClicked += HandleRestartClicked;
        }

        private void UnsubscribeFromEvents()
        {
            if (GameManager.Instance != null) GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged -= UpdateScore;
                ScoreManager.Instance.OnCoinsChanged -= UpdateCoins;
            }
            if (gameOverPanel != null) gameOverPanel.OnRestartClicked -= HandleRestartClicked;
        }

        /// <summary>
        /// Handles changes in the game state to show/hide the appropriate UI panels.
        /// </summary>
        private void HandleGameStateChanged(GameManager.GameState newState)
        { 
            hudPanel?.gameObject.SetActive(newState == GameManager.GameState.Playing);
            gameOverPanel?.gameObject.SetActive(newState == GameManager.GameState.GameOver);

            if (newState == GameManager.GameState.GameOver && ScoreManager.Instance != null)
            {
                gameOverPanel?.SetFinalScore(ScoreManager.Instance.Score);
            }
        }

        // --- DATA DISPLAY ---

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

        // --- PUBLIC API for TUTORIALS (Called by TutorialManager) ---

        /// <summary>
        /// Displays a tutorial message by delegating the call to the specialized tutorial panel.
        /// </summary>
        public void ShowTutorialMessage(string message, float duration)
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.Show(message, duration);
            }
            else
            {
                Debug.LogWarning("Guardian Architect Warning: UIManager cannot show tutorial message. UIPanel_Tutorial is not assigned.");
            }
        }

        /// <summary>
        /// Hides the tutorial message panel by delegating the call.
        /// </summary>
        public void HideTutorialMessage()
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.Hide();
            }
        }
    }
}
