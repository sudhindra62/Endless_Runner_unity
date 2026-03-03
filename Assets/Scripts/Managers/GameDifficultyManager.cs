
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Aggregates difficulty modifiers from various sources (like Adaptive Difficulty)
/// and also applies its own progressive difficulty scaling over the course of a run.
/// Its parameters are now dynamically loaded from a remote configuration.
/// </summary>
public class GameDifficultyManager : MonoBehaviour
{
    #region CONFIGURATION
    /// <summary>
    /// A new data structure to hold the tuning values for the progressive difficulty system.
    /// </summary>
    private struct ProgressionConfig
    {
        public float StartTime; // Time in seconds before difficulty starts to increase.
        public float EndTime;   // Time in seconds when difficulty reaches its max progressive multiplier.
        public float MaxProgressiveMultiplier; // The maximum multiplier added by time.
    }
    private ProgressionConfig config;
    #endregion

    #region STATE_AND_CONSTANTS
    // --- Existing dictionary is 100% preserved ---
    private readonly Dictionary<string, float> difficultyMultipliers = new Dictionary<string, float>();
    
    // --- Existing and New Source IDs ---
    private const string ADAPTIVE_DIFFICULTY_SOURCE_ID = "AdaptiveDifficulty";
    private const string PROGRESSIVE_DIFFICULTY_SOURCE_ID = "ProgressiveDifficulty"; // NEW
    #endregion

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        // --- EVOLUTION: Load config from remote settings ---
        LoadConfiguration();

        // --- Existing ServiceLocator registration (Preserved) ---
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        // --- Existing subscription is 100% preserved ---
        AdaptiveDifficultyManager.OnDifficultyChanged += HandleAdaptiveDifficultyChange;
        
        // --- EVOLUTION: Subscribe to config updates and start the progression coroutine ---
        RemoteConfig.OnConfigUpdated += HandleConfigUpdated;
        StartCoroutine(ProgressiveDifficultyRoutine());
    }

    private void OnDestroy()
    {
        // --- Existing unsubscriptions are 100% preserved ---
        ServiceLocator.Unregister<GameDifficultyManager>();
        AdaptiveDifficultyManager.OnDifficultyChanged -= HandleAdaptiveDifficultyChange;

        // --- EVOLUTION: Unsubscribe from config updates ---
        RemoteConfig.OnConfigUpdated -= HandleConfigUpdated;
    }
    #endregion

    #region CONFIGURATION_HANDLING
    /// <summary>
    /// Populates the progressive difficulty configuration from the RemoteConfig manager.
    /// </summary>
    private void LoadConfiguration()
    {
        config = new ProgressionConfig
        {
            StartTime = RemoteConfig.GetFloat("ProgressiveDifficultyStartTime", 60f),
            EndTime = RemoteConfig.GetFloat("ProgressiveDifficultyEndTime", 600f),
            MaxProgressiveMultiplier = RemoteConfig.GetFloat("ProgressiveDifficultyMaxMultiplier", 1.5f)
        };
        Debug.Log("Game Difficulty Manager configuration loaded.");
    }

    /// <summary>
    /// Handles live updates to the remote configuration, reloading settings.
    /// </summary>
    private void HandleConfigUpdated()
    {
        Debug.Log("Remote config updated. Reloading Game Difficulty Manager settings.");
        LoadConfiguration();
    }
    #endregion

    #region CORE_LOGIC
    /// <summary>
    /// NEW: This coroutine runs for the duration of the game, slowly increasing
    /// a difficulty multiplier based on the time elapsed.
    /// </summary>
    private IEnumerator ProgressiveDifficultyRoutine()
    {
        while (true)
        {
            float runTime = Time.timeSinceLevelLoad;
            float progressiveMultiplier = 1f;

            if (runTime > config.StartTime)
            {
                // Calculate how far we are into the difficulty scaling period.
                float progressionRatio = Mathf.Clamp01((runTime - config.StartTime) / (config.EndTime - config.StartTime));
                // Lerp from 1.0 to the max multiplier based on the ratio.
                progressiveMultiplier = Mathf.Lerp(1f, config.MaxProgressiveMultiplier, progressionRatio);
            }
            
            // Apply the new multiplier.
            ApplyDifficultyMultiplier(PROGRESSIVE_DIFFICULTY_SOURCE_ID, progressiveMultiplier);

            // Wait for a second before recalculating.
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Preserved Method: Handles changes from the adaptive difficulty system.
    /// </summary>
    private void HandleAdaptiveDifficultyChange(float adaptiveMultiplier)
    {
        ApplyDifficultyMultiplier(ADAPTIVE_DIFFICULTY_SOURCE_ID, adaptiveMultiplier);
    }
    #endregion

    #region PUBLIC_API
    /// <summary>
    /// Preserved Method: Applies a multiplier from an external source.
    /// </summary>
    public void ApplyDifficultyMultiplier(string sourceId, float multiplier)
    {
        difficultyMultipliers[sourceId] = multiplier;
    }

    /// <summary>
    /// Preserved Method: Removes a multiplier from an external source.
    /// </summary>
    public void RemoveDifficultyMultiplier(string sourceId)
    {
        difficultyMultipliers.Remove(sourceId);
    }

    /// <summary>
    /// Preserved Method: Calculates the final combined difficulty multiplier.
    /// The logic of this method is unchanged and now correctly includes the new progressive multiplier.
    /// </summary>
    public float GetDifficultyMultiplier()
    {
        float totalMultiplier = 1f;
        foreach (var multiplier in difficultyMultipliers.Values)
        {
            totalMultiplier *= multiplier;
        }
        return totalMultiplier;
    }

    /// <summary>
    /// Preserved Method: Resets the state. This correctly clears all multipliers,
    /// including the progressive one, preparing the manager for a new run.
    /// </summary>
    public void ResetState()
    {
        difficultyMultipliers.Clear();
    }
    #endregion
}
