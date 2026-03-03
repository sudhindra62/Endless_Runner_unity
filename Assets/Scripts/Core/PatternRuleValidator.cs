
using UnityEngine;
using Managers;
using Systems;

namespace Core
{
    /// <summary>
    /// Validates a pattern against a complex set of game state rules from various managers.
    /// This ensures patterns are not only fair but also contextually appropriate for the current gameplay moment.
    /// </summary>
    public class PatternRuleValidator : MonoBehaviour
    {
        // Dependencies from the Service Locator
        private AdaptiveDifficultyManager _adaptiveDifficultyManager;
        private BossChaseManager _bossChaseManager;
        private WorldEventManager _worldEventManager;
        private DecisionPathManager _decisionPathManager;
        private SafePathValidator _safePathValidator;

        private void Start()
        {
            // Resolve dependencies using a service locator pattern
            _adaptiveDifficultyManager = ServiceLocator.GetService<AdaptiveDifficultyManager>();
            _bossChaseManager = ServiceLocator.GetService<BossChaseManager>();
            _worldEventManager = ServiceLocator.GetService<WorldEventManager>();
            _decisionPathManager = ServiceLocator.GetService<DecisionPathManager>();
            _safePathValidator = ServiceLocator.GetService<SafePathValidator>();
        }

        /// <summary>
        /// Primary validation function. Checks a generated pattern against all current game state rules.
        /// </summary>
        /// <param name="pattern">The procedural pattern to validate.</param>
        /// <param name="gameSpeed">Current speed of the player/game.</param>
        /// <returns>True if the pattern is valid and fair, false otherwise.</returns>
        public bool ValidatePattern(ProceduralPattern pattern, float gameSpeed)
        {
            // 1. Basic Fairness Check (is there a physical path?)
            // This is the most crucial step. If there's no safe path, nothing else matters.
            if (!_safePathValidator.IsPathSafe(pattern, gameSpeed))
            {
                return false; // REJECT: Impossible geometry or timing
            }

            // 2. Boss State Validation
            if (_bossChaseManager.IsBossActive)
            {
                if (!IsPatternBossCompatible(pattern))
                {
                    return false; // REJECT: Pattern conflicts with boss attack zones
                }
            }

            // 3. Adaptive Difficulty Validation
            // Does the pattern respect the current player skill level?
            if (!_adaptiveDifficultyManager.IsPatternAppropriate(pattern))
            {
                return false; // REJECT: Too hard or too easy for the current skill rating
            }
            
            // 4. World Event Modifier Validation
            if (_worldEventManager.HasActiveEventModifier("PatternSuppression"))
            {
                if (pattern.IsHighDensity) // Example modifier
                {
                    return false; // REJECT: Event requires low-density patterns
                }
            }
            
            // 5. Decision Path/Risk Lane Validation
            if (_decisionPathManager.IsSplitActive)
            {
                // Ensure the pattern doesn't block the entrance to a decision path
                if (!IsCompatibleWithDecisionPath(pattern))
                {
                     return false; // REJECT: Pattern illegally obstructs a choice
                }
            }

            // If all checks pass, the pattern is approved.
            return true;
        }

        private bool IsPatternBossCompatible(ProceduralPattern pattern)
        {
            // Example: Check if the pattern places obstacles in a lane the boss is about to attack.
            // This logic would be highly specific to the boss's attack patterns.
            // For now, we'll assume a simple validation.
            // e.g., if boss will attack lane 1, don't place a blocking obstacle there.
            return true; // Placeholder for more complex boss-aware logic
        }

        private bool IsCompatibleWithDecisionPath(ProceduralPattern pattern)
        {
            // Example: If a split is coming up, ensure the pattern doesn't force the player
            // into a lane that would make taking the desired path impossible.
            return true; // Placeholder for more complex pathing logic
        }
    }

    /// <summary>
    /// A placeholder for the actual pattern data structure.
    /// </summary>
    public class ProceduralPattern
    {
        public bool IsHighDensity { get; set; }
        // This would contain detailed information about obstacle placement, types, timings, etc.
    }
}
