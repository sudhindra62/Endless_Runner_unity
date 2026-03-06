
using System;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Gameplay,
    Paused,
    GameOver
}

public class GameStateManager : Singleton<GameStateManager>
{
    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Gameplay:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }
}
