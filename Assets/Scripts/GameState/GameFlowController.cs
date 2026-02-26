using UnityEngine;
using System;

/// <summary>
/// High-level coordinator for the game's run lifecycle.
/// Integrates with the GameStateManager to provide a simplified API for starting, pausing, and ending a run.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    // This event signals to UI elements (like the RevivePopup) that the player has died and the game is paused, waiting for a decision.
    public static event Action OnPauseForDeath;

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

    /// <summary>
    /// Starts a new run by setting the game state to Playing.
    /// </summary>
    public void StartRun()
    {
        if (GameStateManager.Instance.CurrentState == GameState.Playing) return;

        // Reset systems that need to be cleared for a new run.
        ReviveManager.Instance?.ResetForNewRun();

        GameStateManager.Instance.SetState(GameState.Playing);
        Debug.Log("Run started");
    }

    /// <summary>
    /// Pauses the game flow upon player death, triggers the OnPauseForDeath event for the UI.
    /// </summary>
    public void PauseForDeath()
    {
        if (GameStateManager.Instance.CurrentState != GameState.Playing) return;

        // Setting the state to GameOver will trigger the TimeControlManager to pause the game (Time.timeScale = 0).
        GameStateManager.Instance.SetState(GameState.GameOver);

        Debug.Log("Run paused for death decision.");
        OnPauseForDeath?.Invoke();
    }

    /// <summary>
    /// Resumes the run after a successful revive by setting the game state back to Playing.
    /// </summary>
    public void ResumeAfterRevive()
    {
        GameStateManager.Instance.SetState(GameState.Playing);
        Debug.Log("Run resumed after revive");
    }

    /// <summary>
    /// Finalizes the end of the run when the player does not revive.
    /// </summary>
    public void EndRunFinal()
    {
        // The state is already GameOver. This method's responsibility is to trigger the end-of-run UI sequence.
        Debug.Log("Run ended permanently (no revive). Transitioning to summary.");
        
        if (EndOfRunManager.Instance != null)
        {
            EndOfRunManager.Instance.ShowRunSummary();
        }
        else
        {
            // Fallback in case the EndOfRunManager isn't present.
            Debug.LogWarning("EndOfRunManager not found. Cannot show run summary.");
            // Directly go to home screen as a fallback.
            GameStateManager.Instance.SetState(GameState.Home);
        }
    }

    /// <summary>
    /// Checks if the game is currently in an active run.
    /// </summary>
    public bool IsRunActive() => GameStateManager.Instance.CurrentState == GameState.Playing;
}
