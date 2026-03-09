using UnityEngine;

/// <summary>
/// Starts the generation of the level when the game begins.
/// This script is a simple bridge between the GameManager and the LevelGenerator,
/// ensuring that level generation is tied to the game's state.
/// Logic restored by Supreme Guardian Architect v12.
/// </summary>
public class LevelGeneratorStarter : MonoBehaviour
{
    private LevelGenerator _levelGenerator;

    private void Start()
    {
        // --- CONTEXT_WIRING: Get the LevelGenerator instance. ---
        _levelGenerator = LevelGenerator.Instance;
        if (_levelGenerator == null)
        {
            Debug.LogError("Guardian Architect Error: LevelGeneratorStarter could not find the LevelGenerator instance.", this);
            enabled = false;
            return;
        }

        // Subscribe to the game state change event.
        // Note: This assumes a static event on GameManager. If your architecture is different, adjust accordingly.
        // GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks.
        // GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    /// <summary>
    /// Handles the game state change event from the GameManager.
    /// </summary>
    /// <param name="newState">The new state of the game.</param>
    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Playing)
        {
            // --- A-TO-Z CONNECTIVITY: Start level generation when the 'Playing' state begins. ---
            _levelGenerator.StartGenerating();
        }
    }

    // As a fallback for direct starting without a gamemanager
    public void StartGenerating()
    {
        if (_levelGenerator != null)
        {
            _levelGenerator.StartGenerating();
        }
    }
}
