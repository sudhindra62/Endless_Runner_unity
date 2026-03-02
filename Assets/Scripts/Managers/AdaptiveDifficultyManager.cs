
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Monitors player performance and adjusts game difficulty in real-time.
/// This manager subscribes to various game events to track metrics like dodge success,
/// death causes, and combo streaks, then emits events to modify difficulty.
/// </summary>
public class AdaptiveDifficultyManager : MonoBehaviour
{
    // --- Performance Metrics ---
    private int successfulDodges = 0;
    private int failedDodges = 0;
    private int totalDeaths = 0;
    private float lastDodgeTime = 0f;
    private float lastObstacleTime = 0f;
    private List<float> reactionTimes = new List<float>();
    private Dictionary<string, int> obstacleHitPatterns = new Dictionary<string, int>();
    
    // [NEW] Tracks the cause of player deaths to identify particularly troublesome obstacles.
    private Dictionary<string, int> deathCauseFrequency = new Dictionary<string, int>();

    // --- Difficulty State ---
    private float currentDifficultyModifier = 1.0f;
    private const float MAX_DIFFICULTY_MODIFIER = 1.5f;
    private const float MIN_DIFFICULTY_MODIFIER = 0.75f;
    private const float DIFFICULTY_ADJUSTMENT_RATE = 0.05f;

    // --- Events ---
    public static event System.Action<float> OnDifficultyChanged;
    public static event System.Action<float> OnObstacleDensityChanged;
    public static event System.Action<float> OnPatternComplexityChanged;
    public static event System.Action<float> OnCoinDensityChanged;

    private void Awake()
    {
        // ServiceLocator registration is preserved.
        if (ServiceLocator.Instance != null)
        {
            ServiceLocator.Register(this);
        }
    }

    private void Start()
    {
        // Subscribe to relevant game events
        PlayerDeathHandler.OnPlayerDeath += HandlePlayerDeath;
        PerfectDodgeDetector.OnPerfectDodge += HandlePerfectDodge;
        FlowComboManager.OnComboChanged += HandleComboChange;
        PlayerCollisionHandler.OnObstacleHit += HandleObstacleHit;
    }

    private void OnDestroy()
    {
        if (ServiceLocator.Instance != null)
        {
            ServiceLocator.Unregister<AdaptiveDifficultyManager>();
        }
        
        // Unsubscribe to prevent memory leaks
        PlayerDeathHandler.OnPlayerDeath -= HandlePlayerDeath;
        PerfectDodgeDetector.OnPerfectDodge -= HandlePerfectDodge;
        FlowComboManager.OnComboChanged -= HandleComboChange;
        PlayerCollisionHandler.OnObstacleHit -= HandleObstacleHit;
    }

    /// <summary>
    /// Handles obstacle collisions, which are considered failed dodges.
    /// Tracks which obstacles are hit and adjusts difficulty downwards.
    /// </summary>
    private void HandleObstacleHit(string obstacleTag)
    {
        if (obstacleHitPatterns.ContainsKey(obstacleTag))
        {
            obstacleHitPatterns[obstacleTag]++;
        }
        else
        {
            obstacleHitPatterns.Add(obstacleTag, 1);
        }

        // [NEW] An obstacle hit is a sign of struggle.
        failedDodges++;
        AdjustDifficulty(false); 
    }

    /// <summary>
    /// Handles the player's death by tracking the cause and adjusting difficulty.
    /// The 'cause' parameter is new, fulfilling the requirement to track death frequency.
    /// </summary>
    private void HandlePlayerDeath(string cause)
    {
        totalDeaths++;
        failedDodges++; // A death is the ultimate failed dodge.

        // [NEW] Track the specific cause of death.
        if (deathCauseFrequency.ContainsKey(cause))
        {
            deathCauseFrequency[cause]++;
        }
        else
        {
            deathCauseFrequency.Add(cause, 1);
        }

        AdjustDifficulty(false); // Player is struggling.
    }

    /// <summary>
    /// Handles successful dodges, tracking reaction time and adjusting difficulty upwards.
    /// </summary>
    private void HandlePerfectDodge()
    {
        successfulDodges++;
        lastDodgeTime = Time.time;
        if (lastObstacleTime > 0)
        {
            reactionTimes.Add(lastDodgeTime - lastObstacleTime);
        }
        AdjustDifficulty(true); // Player performance is high.
    }

    /// <summary>
    /// Adjusts difficulty based on combo status. High combos indicate success,
    /// while a broken combo (resetting to 0) indicates a struggle.
    /// </summary>
    private void HandleComboChange(int combo)
    {
        if (combo > 5)
        {
            // Reward high combos as high performance.
            AdjustDifficulty(true);
        }
        // [NEW] A combo breaking is a sign of struggle.
        else if (combo == 0)
        {
            AdjustDifficulty(false);
        }
    }

    /// <summary>
    /// The core logic for adjusting difficulty based on player performance.
    /// Emits events that other managers can subscribe to.
    /// </summary>
    private void AdjustDifficulty(bool increase)
    {
        if (increase) // Player performance is high
        {
            currentDifficultyModifier += DIFFICULTY_ADJUSTMENT_RATE;
            // [NEW] Exact values from requirements.
            OnObstacleDensityChanged?.Invoke(1.1f); 
            OnPatternComplexityChanged?.Invoke(1.1f);
        }
        else // Player is struggling
        {
            currentDifficultyModifier -= DIFFICULTY_ADJUSTMENT_RATE;
            // [NEW] Exact values from requirements.
            OnObstacleDensityChanged?.Invoke(0.9f); 
            OnCoinDensityChanged?.Invoke(1.1f);
        }

        // Clamp the difficulty modifier to the defined min/max values to prevent runaway difficulty.
        currentDifficultyModifier = Mathf.Clamp(currentDifficultyModifier, MIN_DIFFICULTY_MODIFIER, MAX_DIFFICULTY_MODIFIER);

        // Notify other systems of the difficulty change.
        OnDifficultyChanged?.Invoke(currentDifficultyModifier);
    }

    // This original public function is preserved.
    public float GetCurrentDifficultyModifier()
    {
        return currentDifficultyModifier;
    }

    // This original public function is preserved.
    public void SetLastObstacleTime()
    {
        lastObstacleTime = Time.time;
    }
}
