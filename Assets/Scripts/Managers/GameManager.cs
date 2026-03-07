
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// The central nervous system of the game. Manages game state, scene transitions, and coordinates all other managers.
/// Authored by OMNI_ARCHITECT_v31 to establish a singleton-based, event-driven game core.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    // --- EVENTS ---
    public static event Action<GameState> OnGameStateChanged;

    // --- STATE ---
    public GameState CurrentState { get; private set; }

    // --- UNITY LIFECYCLE ---
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // The game should start in the Main Menu state.
        ChangeState(GameState.MainMenu);
    }

    // --- PUBLIC API ---

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        // Broadcast the state change to all listeners (UI, Score, etc.)
        OnGameStateChanged?.Invoke(newState);
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            if (CurrentState == GameState.Playing)
            {
                ChangeState(GameState.Paused);
            }
        }
        else
        {
            if (CurrentState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
            }
        }
    }

    public void ReloadScene()
    {
        // Reloads the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- PRIVATE STATE HANDLERS ---

    private void HandleMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is running for menu animations
        // Potentially load a main menu scene if the game is architected that way
    }

    private void HandlePlaying()
    {
        Time.timeScale = 1f; // Resume game time
    }

    private void HandlePaused()
    {
        Time.timeScale = 0f; // Freeze game time
    }

    private void HandleGameOver()
    {
        Time.timeScale = 0.5f; // Optional: slow-motion effect on game over
        // After a delay, you might want to fully stop time or show a menu
    }
}

// --- ENUM DEFINITION ---

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
