using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// Merged GameState enum with all states
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

/// <summary>
/// Manages the core game loop, state transitions, and scene loading.
/// This is the authoritative source for the game's overall state.
/// It also serves as the primary integration point for starting and ending analytics sessions.
/// </summary>
public class GameFlowController : Singleton<GameFlowController>
{
    // From Core/GameFlowController
    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        // The PlayerAnalyticsManager is referenced via its Singleton instance directly,
        // so no need to cache it here.
    }

    private void Start()
    {
        // Logic from Core/GameFlowController: Ensure correct starting state based on scene.
        if (SceneManager.GetActiveScene().buildIndex == 0 && CurrentState != GameState.MainMenu)
        {
            ChangeState(GameState.MainMenu);
        }
    }

    private void Update()
    {
        // From Managers/GameFlowController: Integrity check during gameplay.
        if (CurrentState == GameState.Playing)
        {
            if (IntegrityManager.Instance != null && !IntegrityManager.Instance.sessionValidator.IsTimeScaleValid())
            {
                IntegrityManager.Instance.ReportError("Time scale manipulation detected!");
                // FAILSAFE: As per requirements, reset the time scale to its normal value.
                Time.timeScale = 1.0f;
            }
        }
    }

    public void StartGame()
    {
        // Combined logic
        if (CurrentState == GameState.MainMenu || CurrentState == GameState.GameOver)
        {
            // In a real project, this would handle scene loading.
            // SceneManager.LoadSceneAsync(1);
            
            ChangeState(GameState.Playing);

            // --- ANALYTICS INTEGRATION ---
            if (PlayerAnalyticsManager.Instance != null)
            {
                PlayerAnalyticsManager.Instance.StartSession();
                Debug.Log("Game Started and new Analytics Session created.");
            }
        }
    }
    
    // From Core/GameFlowController
    public void PlayerDied(string cause)
    {
        if (CurrentState != GameState.Playing) return;

        // --- ANALYTICS INTEGRATION ---
        if (PlayerAnalyticsManager.Instance != null)
        {
            PlayerAnalyticsManager.Instance.TrackDeath(cause);
            Debug.Log($"Player death tracked. Cause: {cause}");
        }
        
        EndGame(false); // A death ends the run.
    }

    // Merged EndGame logic
    public void EndGame(bool playerWon)
    {
        if (CurrentState != GameState.Playing) return;

        ChangeState(GameState.GameOver);

        // --- ANALYTICS INTEGRATION ---
        if (PlayerAnalyticsManager.Instance != null)
        {
            PlayerAnalyticsManager.Instance.EndSession(false); // Normal end
            Debug.Log("Game Ended and Analytics Session concluded.");
        }
        
        // --- ADAPTIVE DIFFICULTY INTEGRATION ---
        if(AdaptiveDifficultyManager.Instance != null)
        {
            AdaptiveDifficultyManager.Instance.OnSessionEnd();
        }
    }

    // From Core/GameFlowController
    public void ReturnToMenu()
    {
        if (CurrentState == GameState.GameOver)
        {
            // In a real project, this would load the main menu scene.
            // SceneManager.LoadSceneAsync(0);
            ChangeState(GameState.MainMenu);
        }
    }

    private void OnApplicationQuit()
    {
        // If the game is quit while a session is active, it's considered abrupt.
        if (CurrentState == GameState.Playing && PlayerAnalyticsManager.Instance != null)
        {
            PlayerAnalyticsManager.Instance.EndSession(true); // Abrupt end (Rage Quit)
            Debug.LogWarning("Application quit during active play. Analytics Session marked as abrupt.");
        }
    }
    
    private void ChangeState(GameState newState)
    {
        if (newState == CurrentState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState); // Invoke event from Core/GameFlowController
    }
}
