
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.Managers
{
    /// <summary>
    /// The central nervous system of the game. Manages game state and the overall game loop.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public GameState CurrentState { get; private set; }

        // Managers are now accessed via ServiceLocator, so no direct references here.

        private void Start()
        {
            // Initial game state
            SetState(GameState.MainMenu);
        }

        public void StartGame()
        {
            if (CurrentState == GameState.MainMenu || CurrentState == GameState.GameOver)
            {
                SetState(GameState.Starting);
            }
        }

        public void GoToMainMenu()
        {
            if (CurrentState == GameState.GameOver)
            {
                SetState(GameState.MainMenu);
            }
        }

        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            ExitState(CurrentState);
            CurrentState = newState;
            EnterState(CurrentState);
        }

        private void EnterState(GameState state)
        {
            Logger.Log("GAME_MANAGER", $"Entering state: {state}");
            switch (state)
            {
                case GameState.MainMenu:
                    ServiceLocator.Get<TimeManager>().Resume();
                    // TODO: Show main menu UI
                    break;
                case GameState.Starting:
                    ServiceLocator.Get<TimeManager>().Resume();
                    GameEvents.TriggerGameStart();
                    SetState(GameState.Playing);
                    break;
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    ServiceLocator.Get<TimeManager>().Pause();
                    GameEvents.TriggerShowGameOverPanel();
                    break;
            }
        }

        private void ExitState(GameState state)
        {
            Logger.Log("GAME_MANAGER", $"Exiting state: {state}");
            switch (state)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Starting:
                    break;
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
    }
}
