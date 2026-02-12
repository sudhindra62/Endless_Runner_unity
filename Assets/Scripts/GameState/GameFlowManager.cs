
using UnityEngine;

/// <summary>
/// The master orchestrator for game flow, state, and lifecycle events.
/// This singleton provides a single, clean entry point for the existing GameManager to interact with
/// the new state management systems. It houses all the controller components.
///
/// --- INTEGRATION ---
/// Attach this script to your main GameManager object. It will automatically add all required controller
/// components. The existing GameManager can then call the public methods on this singleton instance.
/// e.g., GameFlowManager.Instance.StartRun();
/// e.g., GameFlowManager.Instance.Pause();
/// </summary>
[RequireComponent(typeof(GameStateManager), typeof(TimeControlManager))]
[RequireComponent(typeof(PauseResumeController), typeof(RestartController), typeof(RunLifecycleController))]
public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    // --- Child Controllers ---
    private RunLifecycleController runLifecycleController;
    private PauseResumeController pauseResumeController;
    private RestartController restartController;

    private void Awake()
    {
        if (Instance == null)
        {            
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Get references to the controller components that were added by RequireComponent.
            runLifecycleController = GetComponent<RunLifecycleController>();
            pauseResumeController = GetComponent<PauseResumeController>();
            restartController = GetComponent<RestartController>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // --- PUBLIC API for GameManager ---

    /// <summary>
    /// Starts a new run.
    /// </summary>
    public void StartRun()
    {
        runLifecycleController.OnRunStart();
    }

    /// <summary>
    /// Ends the current run.
    /// </summary>
    public void EndRun()
    {
        runLifecycleController.OnRunEnd();
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void Pause()
    {
        pauseResumeController.PauseGame();
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void Resume()
    {
        pauseResumeController.ResumeGame();
    }

    /// <summary>
    /// Restarts the game by reloading the scene.
    /// </summary>
    public void Restart()
    {
        restartController.RestartGame();
    }
}
