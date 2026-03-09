
using System;
using UnityEngine;
using Core;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            Playing,
            Paused,
            GameOver
        }

        public event Action<GameState> OnGameStateChanged;

        public GameState CurrentState { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            GameEvents.OnPlayerDied += GameOver;
        }

        private void Start()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OnRestartButtonPressed += RestartGame;
            }
            UpdateGameState(GameState.Playing);
        }

        private void OnDestroy()
        {
            GameEvents.OnPlayerDied -= GameOver;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OnRestartButtonPressed -= RestartGame;
            }
        }

        private void UpdateGameState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);

            switch (newState)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 1f; // Keep time running for animations
                    ScoreManager.Instance.SaveScore();
                    break;
            }
        }

        public GameState GetCurrentState()
        {
            return CurrentState;
        }

        public void PauseGame()
        {
            if (CurrentState == GameState.Playing)
            {
                UpdateGameState(GameState.Paused);
            }
        }

        public void ResumeGame()
        {
            if (CurrentState == GameState.Paused)
            {
                UpdateGameState(GameState.Playing);
            }
        }

        private void GameOver()
        {
            if (CurrentState != GameState.GameOver)
            {
                UpdateGameState(GameState.GameOver);
            }
        }

        public void RestartGame()
        {
            // Reset all necessary systems
            PlayerController.Instance.Reset();
            ScoreManager.Instance.Reset();
            LevelGenerator.Instance.Reset();

            // Set the state back to playing
            UpdateGameState(GameState.Playing);
        }
    }
}
