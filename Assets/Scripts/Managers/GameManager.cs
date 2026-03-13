
using UnityEngine;
using System;

/// <summary>
/// The central nervous system of the game. Manages game state, score, and the overall game loop.
/// This is a singleton that persists across all scenes.
/// Logic has been fully implemented by Supreme Guardian Architect v12.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver }
    private GameState _currentState;
    public static event Action<GameState> OnGameStateChanged;

    public int Score { get; private set; }
    public int Currency { get; private set; }

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
        // Initial state of the game
        UpdateGameState(GameState.MainMenu);
    }

    /// <summary>
    /// Updates the game state and notifies all subscribed systems.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    public void UpdateGameState(GameState newState)
    {
        _currentState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                // Reset score or other values at the start of a new game if needed
                Score = 0;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                // Here you might trigger a game over UI, save high scores, etc.
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return _currentState;
    }

    /// <summary>
    /// Adds a specified amount to the player's score.
    /// </summary>
    public void AddScore(int amount)
    {
        if (_currentState != GameState.Playing) return;
        Score += amount;
        // Optional: Add an event for score changes to update UI
        // OnScoreChanged?.Invoke(Score);
    }

    /// <summary>
    /// Adds a specified amount of currency.
    /// </summary>
    public void AddCurrency(int amount)
    {
        Currency += amount;
        // Optional: Add an event for currency changes
        // OnCurrencyChanged?.Invoke(Currency);
    }
}
