
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EndlessRunner.Generation.Patterns;
using EndlessRunner.Generation.Rules;
using EndlessRunner.Core;

namespace EndlessRunner.Generation
{
    /// <summary>
    /// The core engine for procedural level generation.
    /// Manages a library of level patterns and selects the next pattern to be placed
    /// based on a set of rules, difficulty, and a deterministic seed.
    /// </summary>
    public class ProceduralPatternEngine : Singleton<ProceduralPatternEngine>
    {
        [Header("Pattern Library")]
        [Tooltip("The complete list of all LevelPattern assets available to the generator.")]
        [SerializeField] private List<LevelPattern> patternLibrary = new List<LevelPattern>();

        [Header("Engine Configuration")]
        [Tooltip("A special pattern to use for the very start of the level.")]
        [SerializeField] private LevelPattern startingPattern;

        private List<LevelPattern> safePatternLibrary = new List<LevelPattern>();
        private System.Random randomGenerator;
        private int currentDifficulty = 1;

        protected override void Awake()
        {
            base.Awake();
            ValidateAndCachePatterns();
        }

        /// <summary>
        /// Initializes the engine with a specific seed for deterministic generation.
        /// </summary>
        public void Initialize(int seed)
        {
            Debug.Log($"PROCEDURAL_ENGINE: Initializing with seed: {seed}");
            randomGenerator = new System.Random(seed);
        }

        /// <summary>
        /// Sets the current difficulty, affecting which patterns are chosen.
        /// </summary>
        public void SetDifficulty(int difficulty)
        {
            currentDifficulty = Mathf.Clamp(difficulty, 1, 10);
        }

        /// <summary>
        /// Pre-validates all patterns in the library to ensure they are internally solvable.
        /// </summary>
        private void ValidateAndCachePatterns()
        {
            safePatternLibrary = patternLibrary
                .Where(pattern => SafePathValidator.IsPathSafe(pattern))
                .ToList();
            
            Debug.Log($"PROCEDURAL_ENGINE: Validated library. {safePatternLibrary.Count} of {patternLibrary.Count} patterns are safe to use.");
        }

        public LevelPattern GetStartingPattern()
        {
            if (startingPattern == null || !SafePathValidator.IsPathSafe(startingPattern))
            {
                Debug.LogError("PROCEDURAL_ENGINE: Assigned starting pattern is null or has no safe path! Attempting to find a fallback.");
                return safePatternLibrary.FirstOrDefault(p => p.difficultyRating == 1);
            }
            return startingPattern;
        }

        public LevelPattern SelectNextPattern(LevelPattern previousPattern)
        {
            if (randomGenerator == null)
            {
                Debug.LogWarning("PROCEDURAL_ENGINE: Engine not initialized with a seed. Using a random seed.");
                Initialize(Random.Range(int.MinValue, int.MaxValue));
            }

            var validNextPatterns = safePatternLibrary
                .Where(pattern => PatternRuleValidator.CanConnect(previousPattern, pattern))
                .ToList();

            if (validNextPatterns.Count == 0)
            {
                Debug.LogWarning($"PROCEDURAL_ENGINE: No valid, safe pattern found to connect after '{previousPattern.patternID}'. Re-using starting pattern as a fallback.");
                return GetStartingPattern();
            }

            var difficultyFilteredPatterns = validNextPatterns
                .Where(p => p.difficultyRating <= currentDifficulty)
                .ToList();

            if (difficultyFilteredPatterns.Count > 0)
            {
                int randomIndex = randomGenerator.Next(0, difficultyFilteredPatterns.Count);
                return difficultyFilteredPatterns[randomIndex];
            }
            else
            {
                int randomIndex = randomGenerator.Next(0, validNextPatterns.Count);
                return validNextPatterns[randomIndex];
            }
        }
    }
}
