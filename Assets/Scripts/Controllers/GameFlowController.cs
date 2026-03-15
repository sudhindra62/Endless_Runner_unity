
using EndlessRunner.Core;
using EndlessRunner.Managers;
using UnityEngine;

namespace EndlessRunner.Controllers
{
    public class GameFlowController : Singleton<GameFlowController>
    {
        private GameManager gameManager;

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Late-bind to GameManager to avoid race conditions
            gameManager = ServiceLocator.Get<GameManager>();

            // Subscribe to game state changes
            gameManager.OnGameStateChanged += OnGameStateChanged;
            
            // Optional: Subscribe to revive events if this controller needs to react
            ServiceLocator.Get<ReviveManager>().OnReviveSuccess += OnPlayerRevived;
            ServiceLocator.Get<ReviveManager>().OnReviveDecline += OnReviveDeclined;

            // Start the game in the main menu
            if (gameManager.CurrentGameState == GameManager.GameState.PreGame)
            {
                gameManager.SetState(GameManager.GameState.MainMenu);
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= OnGameStateChanged;
            }
            
            var reviveManager = ServiceLocator.Get<ReviveManager>();
            if (reviveManager != null)
            {
                reviveManager.OnReviveSuccess -= OnPlayerRevived;
                reviveManager.OnReviveDecline -= OnReviveDeclined;
            }
        }

        private void OnGameStateChanged(GameManager.GameState newState)
        {
            switch (newState)
            {
                case GameManager.GameState.MainMenu:
                    // Logic to show the main menu
                    // For example, load the main menu scene or enable a UI panel
                    Debug.Log("GameFlow: MainMenu state");
                    break;
                case GameManager.GameState.Playing:
                    // Logic to start or resume the game
                    Debug.Log("GameFlow: Playing state");
                    Time.timeScale = 1;
                    break;
                case GameManager.GameState.Paused:
                    // Logic to pause the game
                    Debug.Log("GameFlow: Paused state");
                    Time.timeScale = 0;
                    break;
                case GameManager.GameState.GameOver:
                    // Logic for when the game is over
                    Debug.Log("GameFlow: GameOver state");
                    Time.timeScale = 0; // Or a slow-motion effect
                    // Here you might trigger a UI for game over
                    break;
                case GameManager.GameState.Reviving:
                    // This state is entered when the player dies and can revive
                    Debug.Log("GameFlow: Reviving state
");
                    Time.timeScale = 0; // Pause the game for the revive choice
                    ServiceLocator.Get<ReviveManager>().PromptRevive();
                    break;
            }
        }

        // Called from a UI button or input to start the game
        public void StartGame()
        {
            gameManager.SetState(GameManager.GameState.Playing);
        }

        // Called from a UI button or input to pause the game
        public void PauseGame()
        {
            if (gameManager.CurrentGameState == GameManager.GameState.Playing)
            {
                gameManager.SetState(GameManager.GameState.Paused);
            }
        }

        // Called from a UI button to resume the game
        public void ResumeGame()
        {
            if (gameManager.CurrentGameState == GameManager.GameState.Paused)
            {
                gameManager.SetState(GameManager.GameState.Playing);
            }
        }

        public void PlayerDied()
        {
            if (ServiceLocator.Get<ReviveManager>().CanRevive())
            {
                gameManager.SetState(GameManager.GameState.Reviving);
            }
            else
            {
                gameManager.SetState(GameManager.GameState.GameOver);
            }
        }
        
        public void ReviveAccepted()
        {
            gameManager.SetState(GameManager.GameState.Playing);
        }

        private void OnPlayerRevived()
        {
            // This logic will execute after a successful revive
            Debug.Log("GameFlow: Player revived, returning to game.");
            gameManager.SetState(GameManager.GameState.Playing);
        }
        
        private void OnReviveDeclined()
        {
            // If revive is declined, it's game over for sure
            Debug.Log("GameFlow: Revive declined, proceeding to game over.");
            gameManager.SetState(GameManager.GameState.GameOver);
        }
    }
}
