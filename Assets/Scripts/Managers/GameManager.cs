
using System;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Player;
using EndlessRunner.Generation;

namespace EndlessRunner.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState { MainMenu, Playing, Paused, GameOver }

        public event Action<GameState> OnGameStateChanged;
        public event Action<int> OnScoreChanged;
        public event Action<int> OnCoinsChanged;

        public GameState CurrentState { get; private set; }
        public int Score { get; private set; }
        public int Coins { get; private set; }

        // --- System References ---
        private UIManager _uiManager;
        private PlayerController _playerController;
        private LevelGenerator _levelGenerator;

        protected override void Awake()
        {
            base.Awake();
            // Set a high target frame rate for smooth gameplay
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            // Locate and cache core system references
            _uiManager = UIManager.Instance;
            _playerController = PlayerController.Instance;
            _levelGenerator = LevelGenerator.Instance;
            
            // Error handling for missing critical components
            if (_uiManager == null) Debug.LogError("Guardian Architect CRITICAL ERROR: UIManager not found!");
            if (_playerController == null) Debug.LogError("Guardian Architect CRITICAL ERROR: PlayerController not found!");
            if (_levelGenerator == null) Debug.LogError("Guardian Architect CRITICAL ERROR: LevelGenerator not found!");

            // Subscribe to UI events
            _uiManager.OnRestartButtonPressed += RestartGame;
            _uiManager.OnMainMenuButtonPressed += GoToMainMenu;

            // Load persistent data
            // TODO: Replace with a dedicated SaveManager call
            // Coins = LoadCoins(); 

            // Begin the game in the Main Menu state
            UpdateGameState(GameState.MainMenu);
        }

        public void UpdateGameState(GameState newState)
        {
            if (CurrentState == newState) return;
            CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    HandleMainMenuState();
                    break;
                case GameState.Playing:
                    HandlePlayingState();
                    break;
                case GameState.Paused:
                    HandlePausedState();
                    break;
                case GameState.GameOver:
                    HandleGameOverState();
                    break;
            }

            OnGameStateChanged?.Invoke(newState);
        }

        private void HandleMainMenuState()
        {
            Time.timeScale = 1f;
            _levelGenerator.StopGenerating();
        }

        private void HandlePlayingState()
        {
            Time.timeScale = 1f;
            Score = 0;
            OnScoreChanged?.Invoke(Score);
            _levelGenerator.StartGenerating();
        }

        private void HandlePausedState()
        {
            Time.timeScale = 0f;
        }

        private void HandleGameOverState()
        {
            Time.timeScale = 0f;
             _levelGenerator.StopGenerating();
            // TODO: Save score and coins
        }

        private void RestartGame()
        {
            UpdateGameState(GameState.Playing);
        }

        private void GoToMainMenu()
        {
            UpdateGameState(GameState.MainMenu);
        }

        public void AddScore(int amount)
        {
            if (CurrentState != GameState.Playing) return;
            Score += amount;
            OnScoreChanged?.Invoke(Score);
        }

        public void AddCoins(int amount)
        { 
            Coins += amount;
            OnCoinsChanged?.Invoke(Coins);
            // TODO: Trigger UI feedback for coin collection
        }

        private void OnDestroy()
        {
            // Unsubscribe from events to prevent memory leaks
            if (_uiManager != null)
            {
                _uiManager.OnRestartButtonPressed -= RestartGame;
                _uiManager.OnMainMenuButtonPressed -= GoToMainMenu;
            }
        }
    }
}
