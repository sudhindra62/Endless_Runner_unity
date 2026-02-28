
using UnityEngine;

/// <summary>
/// Orchestrates the main game loop by coordinating between various game systems.
/// Handles starting, pausing, resuming, and ending a run.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    [Header("Required Managers")]
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ReviveManager reviveManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private NewCollectibleSpawner collectibleSpawner;

    #region Unity Lifecycle & Event Subscriptions

    private void OnEnable()
    {
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
        scoreManager.SaveBestScore();
        PlayerDataManager.Instance.AddXPFromRun(scoreManager.CurrentScore);
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

    #endregion
}
