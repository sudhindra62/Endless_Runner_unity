
using UnityEngine;
using System;

/// <summary>
/// A singleton that manages the global game state.
/// It is the single source of truth for the game's flow and raises events when the state changes.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.None;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentState = GameState.Home; // Set initial state
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the new game state and invokes the OnGameStateChanged event.
    /// Includes a guard to prevent redundant state changes.
    /// </summary>
    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        Debug.Log($"Game State changed to: {newState}");

        // FUTURE HOOK: Analytics events for state changes would be sent from here.
        OnGameStateChanged?.Invoke(newState);
    }

    /// <summary>
    /// A convenience method to check if the game is currently in the Playing state.
    /// </summary>
    public bool IsPlaying()
    {
        return CurrentState == GameState.Playing;
    }

    /// <summary>
    /// A convenience method to check if the game is currently in the Paused state.
    /// </summary>
    public bool IsPaused()
    {
        return CurrentState == GameState.Paused;
    }
}
