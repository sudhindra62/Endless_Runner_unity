
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
    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    private void Start()
    {
        // Initial state
        UpdateState(GameState.Menu);
    }

    public void UpdateState(GameState newState)
    {
        if (newState == CurrentState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        // Handle state-specific logic
        switch (newState)
        {
            case GameState.Menu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Dead:
                // Specific logic for Dead state can be added here
                break;
            case GameState.EndOfRun:
                // Logic for what happens at the end of a run
                break;
        }
    }

    public void StartGame()
    {
        UpdateState(GameState.Playing);
    }

    public void PlayerDied()
    {
        UpdateState(GameState.Dead);
    }

    public void EndRun()
    {
        UpdateState(GameState.EndOfRun);
    }
}
