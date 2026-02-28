using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        ServiceLocator.Register<GameStateManager>(this);
        SetState(GameState.MainMenu); // Default state
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<GameStateManager>();
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        Time.timeScale = CurrentState == GameState.Paused ? 0f : 1f;
    }
}
