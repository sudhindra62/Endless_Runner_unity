
using System;
using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Paused,
    Dead,
    EndOfRun
}

public class GameManager : Singleton<GameManager>
{
    // --- EVENTS ---
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnRunStart;
    public static event Action OnRunEnd;

    // --- STATE ---
    public GameState CurrentState { get; private set; }

    private void Start()
    {
        // Set the initial state of the game
        UpdateState(GameState.Menu);
    }

    /// <summary>
    /// Updates the game to a new state, handling all state transition logic.
    /// </summary>
    public void UpdateState(GameState newState)
    {
        if (newState == CurrentState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        // Handle state-specific logic and events
        switch (newState)
        {
            case GameState.Menu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                OnRunStart?.Invoke(); // A new run begins when we enter the Playing state
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Dead:
                // Logic for the Dead state (e.g., waiting for revive)
                break;
            case GameState.EndOfRun:
                OnRunEnd?.Invoke(); // The run concludes when we enter the EndOfRun state
                break;
        }
    }

    // --- PUBLIC API for state changes ---

    /// <summary>
    /// Starts the game run, transitioning the state to Playing.
    /// </summary>
    public void StartGame()
    {
        UpdateState(GameState.Playing);
    }

    /// <summary>
    /// Called when the player dies, transitioning the state to Dead.
    /// </summary>
    public void PlayerDied()
    {
        UpdateState(GameState.Dead);
    }

    /// <summary>
    /// Ends the current run, transitioning the state to EndOfRun.
    /// </summary>
    public void EndRun()
    {
        UpdateState(GameState.EndOfRun);
    }
}
