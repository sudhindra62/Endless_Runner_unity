
using System;
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Core
{
    /// <summary>
    /// Manages the overall game state, responding to key game events.
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

        public event Action<GameState> OnGameStateChanged;
        public GameState CurrentState { get; private set; }

        private void OnEnable()
        {
            GameEvents.OnPlayerDeath += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        }

        private void Start()
        {
            // Set the initial game state
            ChangeState(GameState.MainMenu);
        }

        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 1f;
                    break;
            }

            OnGameStateChanged?.Invoke(newState);
        }

        public void StartGame()
        {
            ChangeState(GameState.Playing);
        }

        public void PauseGame(bool isPaused)
        {
            ChangeState(isPaused ? GameState.Paused : GameState.Playing);
        }

        private void HandlePlayerDeath()
        {
            ChangeState(GameState.GameOver);
        }
    }
}
