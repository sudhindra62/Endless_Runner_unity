using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    MainMenu,
    Playing,
    GameOver
}

/// <summary>
/// Manages the core game loop, state transitions, and scene loading.
/// This is the authoritative source for the game's overall state.
/// It also serves as the primary integration point for starting and ending analytics sessions.
/// </summary>
public class GameFlowController : Singleton<GameFlowController>
{
    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    private PlayerAnalyticsManager analyticsManager;

    protected override void Awake()
    {
        base.Awake();
        // Ensure the analytics manager singleton is initialized and ready.
        analyticsManager = PlayerAnalyticsManager.Instance;
    }

    private void Start()
    {
        // Ensure the game starts in the main menu state if we are in the main menu scene.
        if (SceneManager.GetActiveScene().buildIndex == 0 && CurrentState != GameState.MainMenu)
        {
            ChangeState(GameState.MainMenu);
        }
    }

    public void StartGame()
    {
        if (CurrentState == GameState.MainMenu)
        {
            // In a real project, this would load the game scene.
            // SceneManager.LoadSceneAsync(1);
            
            ChangeState(GameState.Playing);

            // --- ANALYTICS INTEGRATION ---
            analyticsManager.StartSession();
            Debug.Log("Game Started and new Analytics Session created.");
        }
    }

    public void PlayerDied(string cause)
    {
        if (CurrentState != GameState.Playing) return;

        // --- ANALYTICS INTEGRATION ---
        analyticsManager.TrackDeath(cause);
        Debug.Log($"Player death tracked. Cause: {cause}");
        
        EndGame(false); // A death ends the run.
    }

    public void EndGame(bool playerWon)
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.GameOver);

            // --- ANALYTICS INTEGRATION ---
            // A normal end-of-run is not an abrupt quit.
            analyticsManager.EndSession(false);
            Debug.Log("Game Ended and Analytics Session concluded.");
        }
    }
    
    // --- ANALYTICS INTEGRATION: RAGE QUIT TRACKING ---
    private void OnApplicationQuit()
    {
        // If the game is quit while a session is active, it's considered abrupt.
        if (CurrentState == GameState.Playing && analyticsManager != null)
        {
            analyticsManager.EndSession(true);
            Debug.LogWarning("Application quit during active play. Analytics Session marked as abrupt.");
        }
    }

    public void ReturnToMenu()
    {
        if (CurrentState == GameState.GameOver)
        {
            // In a real project, this would load the main menu scene.
            // SceneManager.LoadSceneAsync(0);
            ChangeState(GameState.MainMenu);
        }
    }

    private void ChangeState(GameState newState)
    {
        if (newState == CurrentState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
