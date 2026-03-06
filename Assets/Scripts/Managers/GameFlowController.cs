
using UnityEngine;
using System;

public enum GameState
{
    MainMenu,
    Starting,
    Playing,
    Paused,
    Revive, // ◈ ARCHITECT'S ADDITION: New state for the revive sequence.
    GameOver
}

public class GameFlowController : Singleton<GameFlowController>
{
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnRunStarted;
    public static event Action OnPlayerDeath;
    public static event Action OnRunEnded;

    public GameState currentState { get; private set; }

    // Existing Awake, OnEnable, OnDisable methods remain unchanged...

    public void StartGame()
    {
        if (currentState != GameState.MainMenu) return;
        ChangeState(GameState.Starting);
        // ... existing start game logic
        OnRunStarted?.Invoke(); // ◈ This is where the run officially begins.
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (currentState != GameState.Playing) return;
        ChangeState(GameState.Paused);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (currentState != GameState.Paused) return;
        Time.timeScale = 1f;
        ChangeState(GameState.Playing);
    }

    // ◈ ARCHITECT'S MODIFICATION: This is the critical integration point.
    public void HandlePlayerDeath()
    {
        // Instead of immediate game over, we check for revive potential.
        if (ReviveEconomyManager.Instance != null && ReviveEconomyManager.Instance.CanRevive())
        {
            ChangeState(GameState.Revive);
            Time.timeScale = 0f; // Pause the game for the decision
            OnPlayerDeath?.Invoke(); // Triggers RevivePopupUI to show
        }
        else
        {
            EndRun(); // No revives left, proceed to game over.
        }
    }

    // ◈ ARCHITECT'S ADDITION: Called by UI after a successful revive.
    public void ResumeGameAfterRevive()
    {
        if (currentState != GameState.Revive) return;
        
        Time.timeScale = 1f;
        
        // TODO: Implement player respawn logic here
        // - Move player character forward slightly
        // - Grant temporary shield/invincibility
        // ReviveManager.Instance.PlayerRevived(); // This is already called by the economy manager
        
        ChangeState(GameState.Playing);
    }

    // ◈ ARCHITECT'S ADDITION: Called by RevivePopupUI if the player declines.
    public void EndRunFromRevive()
    {
        if (currentState != GameState.Revive) return;
        EndRun();
    }

    private void EndRun()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        ChangeState(GameState.GameOver);
        OnRunEnded?.Invoke();
        // ... existing game over logic (showing leaderboard, etc.)
    }

    private void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        OnGameStateChanged?.Invoke(currentState);
    }
}
