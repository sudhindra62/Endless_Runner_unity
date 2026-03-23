using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the player's pity counters for various drop categories.
/// Provides bad luck protection by guaranteeing drops after a threshold.
/// Global scope.
/// </summary>
public class PityCounterManager : Singleton<PityCounterManager>
{
    private Dictionary<string, int> _pityCounters = new Dictionary<string, int>();

    public void IncrementPityCounter(string category = "Default")
    {
        if (!_pityCounters.ContainsKey(category)) _pityCounters[category] = 0;
        _pityCounters[category]++;
        Debug.Log($"[PitySystem] {category} incremented to {_pityCounters[category]}");
    }

    public void ResetPityCounter(string category = "Default")
    {
        _pityCounters[category] = 0;
        Debug.Log($"[PitySystem] {category} reset.");
    }

    public bool IsPityMet(string category = "Default", int threshold = 10)
    {
        if (!_pityCounters.ContainsKey(category)) return false;
        return _pityCounters[category] >= threshold;
    }

    public int GetCounter(string category = "Default")
    {
        return _pityCounters.ContainsKey(category) ? _pityCounters[category] : 0;
    }

    // --- API ALIAS for ShardDropEngine ---
    public bool HasPityThresholdBeenMet(string category = "Default") => IsPityMet(category);
}
