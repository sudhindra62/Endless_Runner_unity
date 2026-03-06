
using UnityEngine;
using System;

/// <summary>
/// Defines the different states of the game.
/// </summary>
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

/// <summary>
/// The master Singleton for managing the core game state and flow.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    public bool IsGameActive => CurrentState == GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Set the initial state of the game
        UpdateGameState(GameState.MainMenu);
    }

    public void UpdateGameState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                // Logic for main menu
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                // Logic for starting the game
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                // Logic for pausing the game
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                // Logic for game over
                Time.timeScale = 0f; // Or a slow-mo effect
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        // Notify all listeners that the game state has changed.
        OnGameStateChanged?.Invoke(newState);
        Debug.Log($"Game state changed to: {newState}");
    }

    // Example methods to be called by UI buttons or other managers
    public void StartGame()
    {
        UpdateGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        UpdateGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        UpdateGameState(GameState.Playing);
    }

    public void EndGame()
    {
        UpdateGameState(GameState.GameOver);
    }

    public void ReturnToMenu()
    {
        UpdateGameState(GameState.MainMenu);
    }
}
