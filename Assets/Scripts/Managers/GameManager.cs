
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private void Start()
        {
            UpdateGameState(GameState.Playing);

            // Subscribe to the restart button event from the UIManager
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OnRestartButtonPressed += RestartGame;
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
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
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;
            }
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

        public void GameOver()
        {
            if (CurrentState != GameState.GameOver)
            {
                UpdateGameState(GameState.GameOver);
            }
        }

        public void RestartGame()
        {
            // Reload the current scene to restart the game
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
