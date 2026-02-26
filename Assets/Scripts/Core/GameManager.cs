using UnityEngine;

/// <summary>
/// The primary bootstrap for the game's core systems.
/// This script's main responsibility is to initialize and hold the reference to the 
/// master GameFlowManager, which orchestrates the entire game state and lifecycle.
///
/// This class has been refactored to delegate all state management to the new, modular
/// system managed by GameFlowManager. It no longer contains any direct state-changing logic.
/// </summary>
[RequireComponent(typeof(GameFlowManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Public reference to the master game flow controller.
    /// All game state operations (start, pause, end) should be called through this instance.
    /// e.g., GameManager.Instance.Flow.StartRun();
    /// </summary>
    public GameFlowManager Flow { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // The GameFlowManager and its dependencies are now the single source of truth for game state.
            Flow = GetComponent<GameFlowManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
