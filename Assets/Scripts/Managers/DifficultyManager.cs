using UnityEngine;

/// <summary>
/// Manages the game's difficulty, scaling it over time and applying modifiers from the LiveOps engine.
/// </summary>
public class DifficultyManager : Singleton<DifficultyManager>
{
    [Header("Base Difficulty Settings")]
    [Tooltip("The starting difficulty modifier at the beginning of a run.")]
    [SerializeField] private float _baseDifficulty = 1.0f;

    [Tooltip("The amount difficulty increases per second.")]
    [SerializeField] private float _difficultyIncreaseRate = 0.01f;

    private float _timeElapsed;

    private void Start()
    {
        // In a full implementation, this would be tied to a GameManager state
        ResetDifficulty();
    }

    private void Update()
    {
        // Increase difficulty over time during the run
        _timeElapsed += Time.deltaTime;
    }

    /// <summary>
    /// Calculates the current difficulty based on time and the LiveOps multiplier.
    /// </summary>
    /// <returns>The final, combined difficulty multiplier.</returns>
    public float GetCurrentDifficultyMultiplier()
    {
        // 1. Calculate the base difficulty progression over time
        float timeBasedDifficulty = _baseDifficulty + (_timeElapsed * _difficultyIncreaseRate);

        // 2. *** LIVE OPS INTEGRATION POINT ***
        // PULL the multiplier from the LiveOpsManager.
        // This manager retains authority over its base calculation.
        float liveOpsMultiplier = 1.0f;
        if (LiveOpsManager.Instance != null)
        {
            liveOpsMultiplier = LiveOpsManager.Instance.DifficultyMultiplier;
        }

        // 3. Return the combined, final value
        float finalDifficulty = timeBasedDifficulty * liveOpsMultiplier;
        
        // Optional: Log for debugging
        // Debug.Log($"Final Difficulty: {finalDifficulty} (Base: {timeBasedDifficulty}, LiveOps: {liveOpsMultiplier})");

        return finalDifficulty;
    }

    /// <summary>
    /// Resets the difficulty timer, called at the start of a new run.
    /// </summary>
    public void ResetDifficulty()
    { 
        _timeElapsed = 0f;
    }
}
