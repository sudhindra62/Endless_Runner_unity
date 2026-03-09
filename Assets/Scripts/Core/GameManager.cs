using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// The central nervous system of the game. Manages game state, scene transitions, and coordinates all other managers.
/// Logic fully restored and integrated by Supreme Guardian Architect v12.
/// This system establishes a singleton-based, event-driven game core, ensuring all managers operate in sync.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    // --- EVENTS ---
    // This event is the primary way other systems are notified of game state changes.
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
        // The game always starts in the Main Menu.
        // On first launch, this ensures the player sees the main menu screen.
        ChangeState(GameState.MainMenu);
    }

    // --- PUBLIC API for STATE CHANGES ---

    /// <summary>
    /// The primary method for changing the game's state. It ensures all necessary logic for a state transition is executed.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        // --- A-TO-Z CONNECTIVITY: Execute state-specific logic ---
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
                throw new ArgumentOutOfRangeException(nameof(newState), newState, "Invalid game state specified.");
        }

        // --- DEPENDENCY_FIX: Broadcast the state change to all listeners (e.g., UIManager, ScoreManager, etc.) ---
        OnGameStateChanged?.Invoke(newState);

        // --- CONTEXT_WIRING: Directly notify key managers. UIManager needs immediate updates. ---
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HandleGameStateChanged(newState);
        }
    }

    // --- PUBLIC API for UI/Button interactions ---

    /// <summary>
    /// Called by the UI to start the gameplay session.
    /// </summary>
    public void StartGame()
    {
        // For a new game, we reload the main scene to ensure a fresh start.
        // In more complex projects, this might involve loading a specific 'game' scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// Pauses the game. Called by the pause button in the UI.
    /// </summary>
    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
    }

    /// <summary>
    /// Resumes the game from a paused state. Called by the resume button in the UI.
    /// </summary>
    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    /// <summary>
    /// Restarts the game. Can be called from the Pause or Game Over screens.
    /// </summary>
    public void RestartGame()
    {
        // Reload the scene and immediately go into the 'Playing' state.
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ChangeState(GameState.Playing);
    }


    // --- PRIVATE STATE HANDLERS ---

    private void HandleMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is running for any menu animations.
    }

    private void HandlePlaying()
    {
        Time.timeScale = 1f; // Set time scale to normal for gameplay.
    }

    private void HandlePaused()
    {
        Time.timeScale = 0f; // Freeze all physics and time-based updates.
    }

    private void HandleGameOver()
    {
        Time.timeScale = 1f; // Keep time moving for game over animations and effects.

        // --- MONETIZATION HOOK: Trigger interstitial ad based on play frequency. ---
        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowInterstitialAdAfterRun();
        }
    }
}

// --- ENUM DEFINITION ---
// Defines the possible states of the game, ensuring a clear and manageable game flow.
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
