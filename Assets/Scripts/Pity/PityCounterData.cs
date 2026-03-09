
using System.Collections.Generic;

/// <summary>
/// A data-centric class that encapsulates the state of all pity counters for the player.
/// This class is designed to be serializable and managed by a dedicated data manager.
/// Authored by the Supreme Guardian Architect v12.
/// </summary>
[System.Serializable]
public class PityCounterData
{
    // Using a dictionary to store pity counts allows for easy extension.
    // The key is a unique identifier for the pity category (e.g., a rarity name or a specific drop type).
    public Dictionary<string, int> PityCounters { get; private set; } = new Dictionary<string, int>();

    /// <summary>
    /// Increments the pity counter for a specific category.
    /// </summary>
    /// <param name="pityCategory">The category to increment (e.g., "Legendary").</param>
    public void IncrementCounter(string pityCategory)
    {
        if (!PityCounters.ContainsKey(pityCategory))
        {
            PityCounters[pityCategory] = 0;
        }
        PityCounters[pityCategory]++;
    }

    /// <summary>
    /// Resets the pity counter for a specific category, typically after a successful drop.
    /// </summary>
    /// <param name="pityCategory">The category to reset.</param>
    public void ResetCounter(string pityCategory)
    {
        if (PityCounters.ContainsKey(pityCategory))
        {
            PityCounters[pityCategory] = 0;
        }
    }

    /// <summary>
    /// Gets the current pity count for a specific category.
    /// </summary>
    /// <param name="pityCategory">The category to query.</param>
    /// <returns>The current pity count, or 0 if the category doesn't exist.</returns>
    public int GetCounter(string pityCategory)
    {
        PityCounters.TryGetValue(pityCategory, out int count);
        return count;
    }
}
