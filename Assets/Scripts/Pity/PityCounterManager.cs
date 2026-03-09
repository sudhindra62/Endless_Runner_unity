
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the player's pity counters for various drop categories.
/// Refactored by the Supreme Guardian Architect v12 to be data-driven and modular.
/// </summary>
public class PityCounterManager : Singleton<PityCounterManager>
{
    // --- STATE ---
    private PityCounterData _pityData;

    // This would be loaded from a central PlayerData/SaveManager
    public void LoadPityData(PityCounterData data)
    {
        _pityData = data ?? new PityCounterData();
        Debug.Log("Guardian Architect: PityCounterManager data loaded.");
    }

    // This would be called by a central PlayerData/SaveManager
    public PityCounterData GetPityDataForSaving()
    {
        return _pityData;
    }

    /// <summary>
    /// Increments the counters for all specified pity profiles.
    /// Typically called after a run or a failed drop attempt.
    /// </summary>
    /// <param name="profilesToIncrement">A list of profiles whose counters should be increased.</param>
    public void IncrementPityCounters(IEnumerable<PityProfile> profilesToIncrement)
    {
        if (_pityData == null) return;

        foreach (var profile in profilesToIncrement)
        {
            _pityData.IncrementCounter(profile.pityCategoryName);
        }
        // In a real scenario, you might mark the data as dirty and let a SaveManager handle it.
        // For example: SaveManager.Instance.MarkDirty(_pityData);
    }

    /// <summary>
    /// Checks if the pity counter for a given profile has met or exceeded its guarantee threshold.
    /// </summary>
    /// <param name="profile">The pity profile to check.</param>
    /// <returns>True if the guarantee is met, otherwise false.</returns>
    public bool IsPityGuaranteeMet(PityProfile profile)
    {
        if (_pityData == null || profile.pityGuaranteeThreshold <= 0) return false;

        int currentCount = _pityData.GetCounter(profile.pityCategoryName);
        return currentCount >= profile.pityGuaranteeThreshold;
    }

    /// <summary>
    /// Calculates a pity boost multiplier based on the current progress towards the guarantee.
    /// </summary>
    /// <param name="profile">The pity profile to calculate the boost for.</param>
    /// <returns>A float multiplier (e.g., 1.0 for no boost, 1.5 for a 50% boost).</returns>
    public float GetPityBoost(PityProfile profile)
    {
        if (_pityData == null || profile.pityGuaranteeThreshold <= 0) return 1.0f;

        int currentCount = _pityData.GetCounter(profile.pityCategoryName);
        float progress = Mathf.Clamp01((float)currentCount / profile.pityGuaranteeThreshold);

        return profile.pityBoostCurve.Evaluate(progress);
    }

    /// <summary>
    /// Resets the pity counter for a given profile and any other profiles it is configured to reset.
    /// </summary>
    /// <param name="triggeredProfile">The profile that was successfully triggered (e.g., from a drop).</param>
    public void ResetPityCounter(PityProfile triggeredProfile)
    {
        if (_pityData == null) return;

        // Reset the primary profile
        _pityData.ResetCounter(triggeredProfile.pityCategoryName);
        Debug.Log($"Guardian Architect: Pity counter for '{triggeredProfile.pityCategoryName}' was reset.");

        // Reset any dependent categories
        foreach (var profileToReset in triggeredProfile.resetsCategories)
        {
            _pityData.ResetCounter(profileToReset.pityCategoryName);
            Debug.Log($"Guardian Architect: Cascading reset for '{profileToReset.pityCategoryName}'.");
        }
    }
}
