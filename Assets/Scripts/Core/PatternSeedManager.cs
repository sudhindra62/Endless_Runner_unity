using UnityEngine;

namespace Core
{
    /// <summary>
    /// Authoritative manager for the procedural pattern generation seed.
    /// Ensures deterministic and replicable runs, which is critical for features like
    /// replays, ghost runs, and daily challenges.
    /// Fortified by Supreme Guardian Architect v12 into a robust, globally accessible singleton.
    /// </summary>
    public class PatternSeedManager
    {
        // --- SINGLETON IMPLEMENTATION ---
        private static PatternSeedManager _instance;
        public static PatternSeedManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PatternSeedManager();
                }
                return _instance;
            }
        }

        // --- STATE ---
        public int CurrentSeed { get; private set; }
        private System.Random _masterRandomGenerator;

        /// <summary>
        /// Private constructor to enforce the singleton pattern.
        /// </summary>
        private PatternSeedManager()
        {
            // Initialize with a default, non-deterministic seed upon creation.
            // A specific seed should be set by the game state manager before a run begins.
            SetNewRandomSeed();
        }

        /// <summary>
        /// Sets a specific seed to begin a deterministic run.
        /// This should be called by a GameManager or RunManager when starting a seeded run (e.g., Daily Challenge).
        /// </summary>
        /// <param name="seed">The integer seed to use.</param>
        public void SetDeterministicSeed(int seed)
        {
            CurrentSeed = seed;
            _masterRandomGenerator = new System.Random(CurrentSeed);
            Debug.Log($"<color=cyan>[PatternSeedManager]</color> Deterministic seed set: <b>{CurrentSeed}</b>");
        }

        /// <summary>
        /// Generates a new, non-deterministic seed based on the system clock.
        /// This is used for standard, non-seeded runs.
        /// </summary>
        public void SetNewRandomSeed()
        {
            CurrentSeed = (int)System.DateTime.Now.Ticks;
            _masterRandomGenerator = new System.Random(CurrentSeed);
            Debug.Log($"<color=cyan>[PatternSeedManager]</color> New random seed generated: <b>{CurrentSeed}</b>");
        }

        /// <summary>
        /// Returns the next random integer within a specified range from the master generator.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The exclusive upper bound.</param>
        /// <returns>A random integer.</returns>
        public int GetNext(int min, int max)
        {
            if (_masterRandomGenerator == null)
            {
                Debug.LogWarning("[PatternSeedManager] Generator not initialized! Setting new random seed.");
                SetNewRandomSeed();
            }
            return _masterRandomGenerator.Next(min, max);
        }

        /// <summary>
        /// Returns the next random float between 0.0 (inclusive) and 1.0 (exclusive) from the master generator.
        /// </summary>
        /// <returns>A random float.</returns>
        public float GetNextFloat()
        {
            if (_masterRandomGenerator == null)
            {
                Debug.LogWarning("[PatternSeedManager] Generator not initialized! Setting new random seed.");
                SetNewRandomSeed();
            }
            return (float)_masterRandomGenerator.NextDouble();
        }
    }
}
