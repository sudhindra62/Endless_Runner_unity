
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public enum GameState
    {
        MainMenu,
        Starting,
        Playing,
        GameOver
    }

    /// <summary>
    /// The central nervous system of the game. Manages game state and the overall game loop.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public GameState CurrentState { get; private set; }

        public CurrencyManager Currency { get; private set; }
        public AudioManager Audio { get; private set; }
        public CloudLoggingManager CloudLogging { get; private set; }
        public AdManager Ads { get; private set; }
        public FirebaseManager Firebase { get; private set; }
        public ReviveManager Revives { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Currency = GetComponent<CurrencyManager>();
            Audio = GetComponent<AudioManager>();
            CloudLogging = GetComponent<CloudLoggingManager>();
            Ads = GetComponent<AdManager>();
            Firebase = GetComponent<FirebaseManager>();
            Revives = GetComponent<ReviveManager>();
        }

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

        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            Debug.Log($"GAME_MANAGER: State changed to {newState}");

            switch (newState)
            {
                case GameState.MainMenu:
                    // TODO: Show main menu UI
                    Time.timeScale = 1; // Unpause in case we came from a game over
                    break;
                case GameState.Starting:
                    // Start the game run
                    Time.timeScale = 1;
                    GameEvents.TriggerGameStart();
                    SetState(GameState.Playing);
                    break;
                case GameState.Playing:
                    // The main game loop is active
                    break;
                case GameState.GameOver:
                    // Handle game over logic
                    Time.timeScale = 0; // Pause the game
                    GameEvents.TriggerShowGameOverPanel();
                    break;
            }
        }
    }
}
