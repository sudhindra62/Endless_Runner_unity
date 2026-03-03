using UnityEngine;

/// <summary>
/// Stores all critical data for a single gameplay run, such as score, coins collected, and distance.
/// It is managed by the GameStateManager to ensure proper setup and teardown.
/// --- EVOLUTION: Now tracks Time Warp availability ---
/// </summary>
public class RunSessionData : MonoBehaviour
{
    [Header("Core Run Metrics")]
    public int currentScore;
    public int coinsCollected;
    public float distanceTraveled;

    // --- NEW: Time Warp State Tracking ---
    // This flag ensures the "once per run" rule is strictly enforced.
    private bool hasUsedTimeWarp = false;

    /// <summary>
    /// Called by GameStateManager when a new run begins.
    /// --- PRESERVED & ADAPTED: Resets Time Warp state for the new run ---
    /// </summary>
    public void InitializeRun()
    {
        currentScore = 0;
        coinsCollected = 0;
        distanceTraveled = 0f;
        
        // Reset the flag at the start of every new run.
        hasUsedTimeWarp = false;

        Debug.Log("New run initialized. Time Warp is available.");
    }

    #region TIME_WARP_INTEGRATION
    /// <summary>
    /// Checks if the Time Warp mechanic is available to be used in the current run.
    /// </summary>
    /// <returns>True if the Time Warp has not yet been used; otherwise, false.</returns>
    public bool CanUseTimeWarp()
    {
        return !hasUsedTimeWarp;
    }

    /// <summary>
    /// Consumes the Time Warp for the current run. This action is irreversible.
    /// </summary>
    public void UseTimeWarp()
    {
        if (CanUseTimeWarp())
        {
            hasUsedTimeWarp = true;
            Debug.Log("Time Warp has been consumed for this run.");
        }
    }
    #endregion

    // --- PRESERVED: All original metric update logic is 100% intact ---
    public void AddScore(int amount)
    {
        if (amount > 0)
        {
            currentScore += amount;
        }
    }

    public void CollectCoin()
    {
        coinsCollected++;
    }

    public void UpdateDistance(float distance)
    {
        distanceTraveled = distance;
    }
}
