
using System;
using UnityEngine;
using Core;

namespace Managers
{
    /// <summary>
    /// The central nervous system of the game, responsible for managing the overall game state.
    /// It listens to critical events from other managers to transition between states like Playing, GameOver, and Paused.
    /// This script has been architecturally rewritten by Supreme Guardian Architect v13 for full integration.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver
        }

        // --- EVENTS ---
        public event Action<GameState> OnGameStateChanged;

        // --- PROPERTIES ---
        public GameState CurrentState { get; private set; }

        // --- UNITY LIFECYCLE & EVENT WIRING ---

        protected override void Awake()
        {
            base.Awake();
            // Ensure this manager persists across scene loads if necessary, though HomeSceneSetup handles instantiation.
            // DontDestroyOnLoad(gameObject);
            SubscribeToEvents();
        }

        private void Start()
        {
            // The game starts in the main menu. Another script (e.g., MainMenuManager) will call ChangeState(Playing).
            // For now, we'll start in Playing for direct testing, but a full flow would start at MainMenu.
            ChangeState(GameState.Playing);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        // --- PUBLIC STATE MACHINE ---

        /// <summary>
        /// The authoritative method for changing the game's state. Triggers OnGameStateChanged event.
        /// </summary>
        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            Debug.Log($"Guardian Architect: GameState changed to -> {newState}");

            switch (newState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f; // Pause the physics and time-based updates
                    break;
                case GameState.GameOver:
                    Time.timeScale = 1f; // Keep time moving for death animations/UI
                    if (ScoreManager.Instance != null) ScoreManager.Instance.SaveState();
                    break;
            }

            OnGameStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// Restarts the game by resetting all relevant managers and setting the state back to Playing.
        /// </summary>
        public void RestartGame()
        {
            Debug.Log("Guardian Architect: RestartGame command received. Resetting all relevant managers.");

            // Command other managers to reset their states for a new run.
            if (PlayerManager.Instance != null) PlayerManager.Instance.ReviveAndReset();
            if (ScoreManager.Instance != null) ScoreManager.Instance.ResetRunScore();
            // if (LevelGenerator.Instance != null) LevelGenerator.Instance.Reset(); // Assumes LevelGenerator has this method

            // Finally, set the game state back to Playing.
            ChangeState(GameState.Playing);
        }
        
        // --- EVENT HANDLERS (A-TO-Z CONNECTIVITY) ---

        private void SubscribeToEvents()
        {
            // Listen for the UI restart button
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OnRestartButtonPressed += RestartGame;
            }
            
            // Listen for the player's death event from the authoritative PlayerManager
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.OnPlayerDeath += HandlePlayerDeath;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OnRestartButtonPressed -= RestartGame;
            }
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.OnPlayerDeath -= HandlePlayerDeath;
            }
        }

        /// <summary>
        /// Called when the PlayerManager announces the player has died.
        /// </summary>
        private void HandlePlayerDeath()
        {
            Debug.Log("Guardian Architect: GameManager received OnPlayerDeath event. Changing state to GameOver.");
            ChangeState(GameState.GameOver);
        }
    }
}
