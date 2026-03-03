
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Monitors player performance and adjusts game difficulty in real-time.
/// This manager subscribes to various game events to track metrics like dodge success,
/// death causes, and combo streaks, then emits events to modify difficulty.
/// It now dynamically loads its tuning values from a remote configuration source.
/// </summary>
public class AdaptiveDifficultyManager : MonoBehaviour
{
    #region CONFIGURATION
    /// <summary>
    /// A data structure to hold the tuning values for the adaptive difficulty system.
    /// This will be populated from a remote source to allow for live tuning.
    /// </summary>
    private struct DifficultyConfig
    {
        public float MaxDifficultyModifier;
        public float MinDifficultyModifier;
        public float DifficultyAdjustmentRate;
        public float ObstacleDensityIncrease;
        public float PatternComplexityIncrease;
        public float ObstacleDensityDecrease;
        public float CoinDensityIncrease;
    }
    private DifficultyConfig config;
    #endregion

    #region PERFORMANCE_METRICS
    // --- Existing Performance Metrics (Preserved) ---
    private int successfulDodges = 0;
    private int failedDodges = 0;
    private int totalDeaths = 0;
    private float lastDodgeTime = 0f;
    private float lastObstacleTime = 0f;
    private List<float> reactionTimes = new List<float>();
    private Dictionary<string, int> obstacleHitPatterns = new Dictionary<string, int>();
    private Dictionary<string, int> deathCauseFrequency = new Dictionary<string, int>();
    #endregion

    #region STATE
    // --- Existing Difficulty State (Preserved) ---
    private float currentDifficultyModifier = 1.0f;
    // --- Hardcoded constants are now replaced by the 'config' struct ---
    // private const float MAX_DIFFICULTY_MODIFIER = 1.5f;
    // private const float MIN_DIFFICULTY_MODIFIER = 0.75f;
    // private const float DIFFICULTY_ADJUSTMENT_RATE = 0.05f;
    #endregion

    #region EVENTS
    // --- Existing Events (Preserved) ---
    public static event System.Action<float> OnDifficultyChanged;
    public static event System.Action<float> OnObstacleDensityChanged;
    public static event System.Action<float> OnPatternComplexityChanged;
    public static event System.Action<float> OnCoinDensityChanged;
    #endregion

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        // --- EVOLUTION: Load config from remote settings ---
        LoadConfiguration();
        
        // --- Existing ServiceLocator registration (Preserved) ---
        if (ServiceLocator.Instance != null)
        {
            ServiceLocator.Register(this);
        }
    }

    private void Start()
    {
        // --- Existing Event Subscriptions (Preserved) ---
        PlayerDeathHandler.OnPlayerDeath += HandlePlayerDeath;
        PerfectDodgeDetector.OnPerfectDodge += HandlePerfectDodge;
        FlowComboManager.OnComboChanged += HandleComboChange;
        PlayerCollisionHandler.OnObstacleHit += HandleObstacleHit;
        
        // --- EVOLUTION: Subscribe to config updates ---
        RemoteConfig.OnConfigUpdated += HandleConfigUpdated;
    }

    private void OnDestroy()
    {
        // --- Existing ServiceLocator Unregistration (Preserved) ---
        if (ServiceLocator.Instance != null)
        {
            ServiceLocator.Unregister<AdaptiveDifficultyManager>();
        }
        
        // --- Existing Event Unsubscriptions (Preserved) ---
        PlayerDeathHandler.OnPlayerDeath -= HandlePlayerDeath;
        PerfectDodgeDetector.OnPerfectDodge -= HandlePerfectDodge;
        FlowComboManager.OnComboChanged -= HandleComboChange;
        PlayerCollisionHandler.OnObstacleHit -= HandleObstacleHit;

        // --- EVOLUTION: Unsubscribe from config updates ---
        RemoteConfig.OnConfigUpdated -= HandleConfigUpdated;
    }
    #endregion

    #region CONFIGURATION_HANDLING
    /// <summary>
    /// Populates the difficulty configuration from the RemoteConfig manager.
    /// Provides default fallback values to ensure stability if the config is not available.
    /// </summary>
    private void LoadConfiguration()
    {
        // Fetch values from RemoteConfig, with paranoid fallbacks to the original hardcoded values.
        config = new DifficultyConfig
        {
            MaxDifficultyModifier = RemoteConfig.GetFloat("AdaptiveMaxDifficulty", 1.5f),
            MinDifficultyModifier = RemoteConfig.GetFloat("AdaptiveMinDifficulty", 0.75f),
            DifficultyAdjustmentRate = RemoteConfig.GetFloat("AdaptiveAdjustmentRate", 0.05f),
            ObstacleDensityIncrease = RemoteConfig.GetFloat("AdaptiveObstacleDensityUp", 1.1f),
            PatternComplexityIncrease = RemoteConfig.GetFloat("AdaptivePatternComplexityUp", 1.1f),
            ObstacleDensityDecrease = RemoteConfig.GetFloat("AdaptiveObstacleDensityDown", 0.9f),
            CoinDensityIncrease = RemoteConfig.GetFloat("AdaptiveCoinDensityUp", 1.1f)
        };
        Debug.Log("Adaptive Difficulty configuration loaded.");
    }

    /// <summary>
    /// Handles live updates to the remote configuration, reloading the settings.
    /// </summary>
    private void HandleConfigUpdated()
    {
        Debug.Log("Remote config updated. Reloading Adaptive Difficulty settings.");
        LoadConfiguration();
    }
    #endregion



    #region EVENT_HANDLERS
    /// <summary>
    /// Handles obstacle collisions, which are considered failed dodges.
    /// (Original summary and functionality preserved)
    /// </summary>
    private void HandleObstacleHit(string obstacleTag)
    {
        // --- Existing Logic (Preserved) ---
        if (obstacleHitPatterns.ContainsKey(obstacleTag))
        {
            obstacleHitPatterns[obstacleTag]++;
        }
        else
        {
            obstacleHitPatterns.Add(obstacleTag, 1);
        }
        failedDodges++;
        AdjustDifficulty(false); 
    }

    /// <summary>
    /// Handles the player's death by tracking the cause and adjusting difficulty.
    /// (Original summary and functionality preserved)
    /// </summary>
    private void HandlePlayerDeath(string cause)
    {
        // --- Existing Logic (Preserved) ---
        totalDeaths++;
        failedDodges++;
        if (deathCauseFrequency.ContainsKey(cause))
        {
            deathCauseFrequency[cause]++;
        }
        else
        {
            deathCauseFrequency.Add(cause, 1);
        }
        AdjustDifficulty(false);
    }

    /// <summary>
    /// Handles successful dodges, tracking reaction time and adjusting difficulty upwards.
    /// (Original summary and functionality preserved)
    /// </summary>
    private void HandlePerfectDodge()
    {
        // --- Existing Logic (Preserved) ---
        successfulDodges++;
        lastDodgeTime = Time.time;
        if (lastObstacleTime > 0)
        {
            reactionTimes.Add(lastDodgeTime - lastObstacleTime);
        }
        AdjustDifficulty(true);
    }

    /// <summary>
    /// Adjusts difficulty based on combo status.
    /// (Original summary and functionality preserved)
    /// </summary>
    private void HandleComboChange(int combo)
    {
        // --- Existing Logic (Preserved) ---
        if (combo > 5)
        {
            AdjustDifficulty(true);
        }
        else if (combo == 0)
        {
            AdjustDifficulty(false);
        }
    }
    #endregion

    #region CORE_LOGIC
    /// <summary>
    /// The core logic for adjusting difficulty based on player performance.
    /// Emits events that other managers can subscribe to.
    /// --- EVOLUTION: Uses values from the 'config' struct instead of hardcoded constants. ---
    /// </summary>
    private void AdjustDifficulty(bool increase)
    {
        if (increase) // Player performance is high
        {
            currentDifficultyModifier += config.DifficultyAdjustmentRate;
            // --- EVOLUTION: Use config values ---
            OnObstacleDensityChanged?.Invoke(config.ObstacleDensityIncrease); 
            OnPatternComplexityChanged?.Invoke(config.PatternComplexityIncrease);
        }
        else // Player is struggling
        {
            currentDifficultyModifier -= config.DifficultyAdjustmentRate;
            // --- EVOLUTION: Use config values ---
            OnObstacleDensityChanged?.Invoke(config.ObstacleDensityDecrease); 
            OnCoinDensityChanged?.Invoke(config.CoinDensityIncrease);
        }

        // Clamp the difficulty modifier using the new config values.
        currentDifficultyModifier = Mathf.Clamp(currentDifficultyModifier, config.MinDifficultyModifier, config.MaxDifficultyModifier);

        // Notify other systems of the difficulty change (Preserved).
        OnDifficultyChanged?.Invoke(currentDifficultyModifier);
    }
    #endregion

    #region PUBLIC_API
    /// <summary>
    /// This original public function is preserved.
    /// </summary>
    public float GetCurrentDifficultyModifier()
    {
        return currentDifficultyModifier;
    }

    /// <summary>
    /// This original public function is preserved.
    /// </summary>
    public void SetLastObstacleTime()
    {
        lastObstacleTime = Time.time;
    }
    #endregion
}
