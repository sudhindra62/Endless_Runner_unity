
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Data;

namespace EndlessRunner.Economy
{
    /// <summary>
    /// Manages the "pity" counter for legendary shard drops.
    /// This provides bad luck protection, guaranteeing a legendary drop after a certain number of failures.
    /// This is a core component of the AAA Rare Drop & Legendary Shard Engine.
    /// </summary>
    public class PityCounterManager : Singleton<PityCounterManager>
    {
        // After this many non-legendary drops, the next drop is guaranteed to be legendary.
        private const int LEGENDARY_PITY_THRESHOLD = 200;

        /// <summary>
        /// Increments the counter each time a kill does NOT result in a legendary drop.
        /// </summary>
        public void IncrementPityCounter()
        {
            if (DataManager.Instance == null) return;

            DataManager.Instance.GameData.pityCounter++;
            DataManager.Instance.SaveData(); // Persist the counter state immediately.
            Debug.Log($"PITY_COUNTER: Counter incremented to {DataManager.Instance.GameData.pityCounter}");
        }

        /// <summary>
        /// Resets the counter to zero. This should be called whenever a legendary shard is awarded.
        /// </summary>
        public void ResetPityCounter()
        {
            if (DataManager.Instance == null) return;

            if (DataManager.Instance.GameData.pityCounter > 0)
            {
                Debug.Log($"PITY_COUNTER: Counter reset from {DataManager.Instance.GameData.pityCounter}. Legendary was awarded.");
                DataManager.Instance.GameData.pityCounter = 0;
                DataManager.Instance.SaveData();
            }
        }

        /// <summary>
        /// Checks if the pity threshold has been met or exceeded.
        /// </summary>
        /// <returns>True if a pity drop should be forced.</returns>
        public bool HasPityThresholdBeenMet()
        {
            if (DataManager.Instance == null) return false;
            
            bool met = DataManager.Instance.GameData.pityCounter >= LEGENDARY_PITY_THRESHOLD;
            if (met)
            {
                Debug.LogWarning($"PITY_COUNTER: Threshold of {LEGENDARY_PITY_THRESHOLD} has been met!");
            }
            return met;
        }
    }
}
