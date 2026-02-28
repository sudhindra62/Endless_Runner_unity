using UnityEngine;

/// <summary>
/// Orchestrates the main game loop by coordinating between various game systems.
/// Handles starting, pausing, resuming, and ending a run.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    public PlayerController playerController;
    public NewCollectibleSpawner collectibleSpawner;

    private GameStateManager gameStateManager;
    private ScoreManager scoreManager;
    private ReviveManager reviveManager;
    private PlayerDataManager playerDataManager;

    #region Unity Lifecycle & Event Subscriptions

    private void Start()
    {
        // Using ServiceLocator to get manager instances
        gameStateManager = ServiceLocator.Current.Get<GameStateManager>();
        scoreManager = ServiceLocator.Current.Get<ScoreManager>();
        reviveManager = ServiceLocator.Current.Get<ReviveManager>();
        playerDataManager = ServiceLocator.Current.Get<PlayerDataManager>();

        if (reviveManager != null)
        {
            reviveManager.OnPlayerRevived += OnPlayerRevived;
            reviveManager.OnReviveDeclined += OnReviveDeclined;
        }
    }

    private void OnDisable()
    {
        if (reviveManager != null)
        {
            reviveManager.OnPlayerRevived -= OnPlayerRevived;
            reviveManager.OnReviveDeclined -= OnReviveDeclined;
        }
    }

    #endregion

    #region Game Flow Control

    public void StartRun()
    {
        gameStateManager.SetState(GameState.Playing);
        scoreManager.ResetScore();
        reviveManager.ResetReviveState();
        playerController.ResetPlayerToStart();
        collectibleSpawner.ResetSpawner();
    }

    public void TogglePause()
    {
        if (gameStateManager.CurrentState == GameState.Playing)
        {
            Pause();
        }
        else if (gameStateManager.CurrentState == GameState.Paused)
        {
            Resume();
        }
    }

    public void Pause()
    {
        gameStateManager.SetState(GameState.Paused);
    }

    public void Resume()
    {
        gameStateManager.SetState(GameState.Playing);
    }

    public void EndRun()
    {
        gameStateManager.SetState(GameState.GameOver);
        scoreManager.SaveHighScore(); // Corrected method name
        if (playerDataManager != null)
        {
            playerDataManager.AddXPFromRun(scoreManager.CurrentScore); // Direct call
        }
        reviveManager.InitiateReviveFlow();
    }

    #endregion

    #region Event Handlers

    private void OnPlayerRevived()
    {
        playerController.Revive();
        gameStateManager.SetState(GameState.Playing);
    }

    private void OnReviveDeclined()
    {
        // Game is over, no action needed. The UI will handle the transition.
    }

    public void GoToMainMenu()
    {
        gameStateManager.SetState(GameState.MainMenu);
    }

    #endregion
}
