
using UnityEngine;

/// <summary>
/// Provides a clean, safe API to pause and resume the game.
/// It contains guards to prevent state conflicts and only interacts with the GameStateManager.
/// </summary>
public class PauseResumeController : MonoBehaviour
{
    private GameState previousState;

    /// <summary>
    /// Pauses the game if it is currently in the Playing state.
    /// </summary>
    public void PauseGame()
    {
        // Guard: Only allow pausing if the game is currently being played.
        if (GameStateManager.Instance.CurrentState != GameState.Playing)
        {
            Debug.LogWarning("Cannot pause: Not in Playing state.");
            return;
        }

        // Store the state we were in before pausing, so we can return to it.
        previousState = GameStateManager.Instance.CurrentState;
        GameStateManager.Instance.SetState(GameState.Paused);
        
        // FUTURE HOOK: The Pause Menu UI would be shown here.
        // e.g., PauseMenu.Show();
    }

    /// <summary>
    /// Resumes the game if it is currently Paused.
    /// </summary>
    public void ResumeGame()
    {
        // Guard: Only allow resuming if the game is actually paused.
        if (GameStateManager.Instance.CurrentState != GameState.Paused)
        {
            Debug.LogWarning("Cannot resume: Not in Paused state.");
            return;
        }

        // Return to the state we were in before pausing (which should be Playing).
        GameStateManager.Instance.SetState(previousState);
        
        // FUTURE HOOK: The Pause Menu UI would be hidden here.
        // e.g., PauseMenu.Hide();
    }
}
