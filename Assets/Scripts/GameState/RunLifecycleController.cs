
using UnityEngine;

/// <summary>
/// Orchestrates the high-level flow of starting and ending a run.
/// This helps keep the main GameManager clean by centralizing the run lifecycle logic.
/// </summary>
public class RunLifecycleController : MonoBehaviour
{
    [SerializeField] private RunSessionData runSessionData;
    [SerializeField] private EndOfRunManager endOfRunManager;

    /// <summary>
    /// Called by the GameManager when a new run should begin.
    /// </summary>
    public void OnRunStart()
    {
        // Guard against starting a run when one is already active.
        if (GameStateManager.Instance.IsPlaying()) 
        {
            Debug.LogWarning("A run is already in progress. Ignoring OnRunStart call.");
            return;
        }

        // Reset all data relevant to a single run.
        if (runSessionData != null)
        {
            runSessionData.Reset();
        }
        else
        {
            Debug.LogError("RunSessionData not found. Cannot start run.");
            return;
        }

        GameStateManager.Instance.SetState(GameState.Playing);
    }

    /// <summary>
    /// Called by the GameManager when the player dies or the run otherwise ends.
    /// </summary>
    public void OnRunEnd()
    {
        // Guard to ensure this can only be called while playing.
        if (!GameStateManager.Instance.IsPlaying())
        {
            Debug.LogWarning("Not in a run. Ignoring OnRunEnd call.");
            return;
        }

        GameStateManager.Instance.SetState(GameState.GameOver);

        // Trigger the end-of-run reward calculation pipeline.
        if (endOfRunManager != null)
        {
            endOfRunManager.ProcessEndOfRun();
        }
        else
        {
            Debug.LogError("EndOfRunManager not found. Cannot process rewards.");
        }
        
        // FUTURE HOOK: The Game Over UI screen would be triggered from here.
        // FUTURE HOOK: An interstitial ad could be shown here, after a short delay.
    }
}
