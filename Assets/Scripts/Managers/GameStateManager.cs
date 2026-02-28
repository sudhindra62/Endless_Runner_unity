
using System;
using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Paused,
    Dead,
    Reviving,
    EndOfRun
}

/// <summary>
/// The single authority for game state transitions and time scale management.
/// No other script should modify the game state or time scale directly.
/// </summary>
public static class GameStateManager
{
    public static event Action<GameState> OnGameStateChanged;

    private static GameState currentState;
    public static GameState CurrentState
    {
        get => currentState;
        set
        {
            if (currentState != value)
            {
                currentState = value;
                HandleTimeScale(currentState);
                OnGameStateChanged?.Invoke(currentState);
            }
        }
    }

    private static void HandleTimeScale(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
            case GameState.Menu:
            case GameState.Reviving: // Time runs during revive animation/transition
                Time.timeScale = 1f;
                break;
            
            case GameState.Paused:
            case GameState.EndOfRun:
                Time.timeScale = 0f;
                break;
            
            case GameState.Dead:
                Time.timeScale = 0.5f; // Slow-motion effect on death
                break;
        }
    }
}
