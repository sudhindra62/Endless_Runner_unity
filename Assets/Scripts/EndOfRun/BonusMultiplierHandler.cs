
using UnityEngine;
using System;

/// <summary>
/// A component that manages the reward multiplier for the end of a run.
/// This acts as a central point for features like rewarded ads or special events to modify rewards.
/// </summary>
public class BonusMultiplierHandler : MonoBehaviour
{
    private float currentMultiplier = 1.0f;

    public static event Action<float> OnMultiplierChanged;

    /// <summary>
    /// Gets the current reward multiplier.
    /// </summary>
    public float GetCurrentMultiplier()
    {
        return currentMultiplier;
    }

    /// <summary>
    /// Sets the multiplier for the next reward calculation. This should be called *before* rewards are calculated.
    /// </summary>
    /// <param name="multiplier">The multiplier to apply (e.g., 2.0 for double rewards).</param>
    public void SetMultiplier(float multiplier)
    {
        currentMultiplier = Mathf.Max(1.0f, multiplier); // Ensure multiplier is at least 1.
        OnMultiplierChanged?.Invoke(currentMultiplier);
        
        // FUTURE HOOK: The End-Run UI will listen to this to update the displayed multiplier.
        // e.g., UpdateRewardMultiplierText(multiplier);
    }

    /// <summary>
    /// Resets the multiplier back to its default value. Should be called after rewards are applied.
    /// </summary>
    public void ResetMultiplier()
    {
        currentMultiplier = 1.0f;
        OnMultiplierChanged?.Invoke(currentMultiplier);
    }
    
    // --- FUTURE INTEGRATION EXAMPLES ---
    
    /// <summary>
    /// Placeholder method for when a rewarded ad is successfully completed.
    /// </summary>
    public void OnRewardedAdCompleted()
    {
        Debug.Log("Rewarded ad completed. Setting multiplier to 2.0x!");
        SetMultiplier(2.0f);
        
        // FUTURE HOOK: This would be called by an AdManager after the ad callback succeeds.
        // After this, the EndRunRewardCalculator would be re-run to get the doubled rewards.
    }
}
