
using UnityEngine;
using Core;

namespace Systems
{
    /// <summary>
    /// The gatekeeper for pattern validation. It aggregates multiple validation checks.
    /// This script consults world state managers (e.g., BossChaseManager) and the SafePathValidator
    /// to give a final approval or rejection of a generated pattern.
    /// </summary>
    public class PatternRuleValidator : MonoBehaviour
    {
        [Header("Validators")]
        [Tooltip("The geometric and timing validator for ensuring a safe path exists.")]
        [SerializeField] private SafePathValidator _safePathValidator;

        // --- REFERENCES TO WORLD STATE MANAGERS ---
        // In a real project, these would be found via a Service Locator or DI container.
        private BossChaseManager _bossChaseManager;
        private RiskLaneManager _riskLaneManager;
        private AdaptiveDifficultyManager _adaptiveDifficultyManager;
        
        private void Start()
        {
            // Attempt to find optional manager dependencies.
            _bossChaseManager = FindObjectOfType<BossChaseManager>();
            _riskLaneManager = FindObjectOfType<RiskLaneManager>();
            _adaptiveDifficultyManager = FindObjectOfType<AdaptiveDifficultyManager>();
        }

        /// <summary>
        /// The master validation method. Runs a pattern through all required checks.
        /// </summary>
        /// <param name="pattern">The candidate pattern to validate.</param>
        /// <param name="gameSpeed">The current forward speed of the world.</param>
        /// <returns>True if the pattern is approved, false otherwise.</returns>
        public bool ValidatePattern(ProceduralPattern pattern, float gameSpeed)
        {
            // 1. Fundamental Safety Check: Is there a physically possible path?
            if (!_safePathValidator.IsPathSafe(pattern, gameSpeed))
            {
                Debug.Log("Pattern INVALID: Failed Safe Path Validation.");
                return false;
            }

            // 2. Contextual Rule Check: Does this pattern violate any active game state rules?
            if (IsViolatingBossRules(pattern))
            {
                Debug.Log("Pattern INVALID: Violated Boss Mode rules.");
                return false;
            }
            
            // ... more checks for other managers like RiskLaneManager, etc.

            // 3. Player-centric Check: Is this pattern considered "unfair" to the player?
            if (_adaptiveDifficultyManager != null && _adaptiveDifficultyManager.IsPatternUnfair(pattern))
            {
                Debug.Log("Pattern INVALID: Flagged as unfair by Adaptive Difficulty Manager.");
                return false;
            }
            
            // If all checks pass, the pattern is approved.
            return true;
        }

        /// <summary>
        /// Checks if the pattern is valid during a boss chase.
        /// For example, it might prevent spawning a tall wall if the boss is about to fire a laser.
        /// </summary>
        private bool IsViolatingBossRules(ProceduralPattern pattern)
        {
            if (_bossChaseManager == null || !_bossChaseManager.IsBossActive)
            {
                return false; // No boss, no boss rules.
            }

            // This is where you would add specific rules, e.g.:
            // if (_bossChaseManager.IsPreparingLaserAttack && pattern.HasTallObstacles)
            // {
            //     return true; // Invalid: can't have a tall wall during the laser attack
            // }

            return false; // Pattern is compatible with the current boss state.
        }
    }

    #region MOCK_DEPENDENCIES
    // Mock classes for world state managers to allow this script to compile and run in isolation.

    public class BossChaseManager : MonoBehaviour 
    {
        public bool IsBossActive { get; private set; }
    }

    public class RiskLaneManager : MonoBehaviour { }

    public class AdaptiveDifficultyManager : MonoBehaviour 
    { 
        public bool IsPatternUnfair(ProceduralPattern p) { return false; } // Placeholder
    }
    
    #endregion
}
