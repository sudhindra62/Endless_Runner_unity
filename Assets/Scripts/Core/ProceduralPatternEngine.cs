
using UnityEngine;
using Managers;
using Systems;

namespace Core
{
    /// <summary>
    /// The central engine for generating procedural obstacle patterns.
    /// It uses a seed for determinism, consults a difficulty profile for scaling,
    /// and ensures all patterns are fair and contextually appropriate via validators.
    /// </summary>
    public class ProceduralPatternEngine : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("The difficulty profile that dictates scaling over time.")]
        public PatternDifficultyProfile difficultyProfile;

        [Header("Dependencies")]
        [Tooltip("The manager for providing a deterministic seed.")]
        public PatternSeedManager seedManager;
        [Tooltip("The validator for checking rules and fairness.")]
        public PatternRuleValidator ruleValidator;

        private const int MAX_GENERATION_ATTEMPTS = 50; // Safety break to prevent infinite loops

        private IPlayerSkillProvider _playerSkillProvider; // Interface to get player skill data
        private float _gameTime;

        private void Start()
        {
            // In a real project, this would be resolved via a service locator or dependency injection
            // For example: _playerSkillProvider = ServiceLocator.GetService<IPlayerSkillProvider>();
            if (_playerSkillProvider == null) _playerSkillProvider = new MockPlayerSkillProvider();
        }

        private void Update()
        {
            _gameTime += Time.deltaTime;
        }

        /// <summary>
        /// Generates the next valid obstacle pattern for the spawner to use.
        /// </summary>
        /// <param name="gameSpeed">The current forward speed of the player/world.</param>
        /// <returns>A validated procedural pattern instruction set.</returns>
        public ProceduralPattern GeneratePattern(float gameSpeed)
        {
            for (int i = 0; i < MAX_GENERATION_ATTEMPTS; i++)
            {
                // 1. Create a candidate pattern based on current difficulty
                ProceduralPattern candidatePattern = CreateCandidatePattern(gameSpeed);

                // 2. Validate the pattern against all game rules and fairness checks
                if (ruleValidator.ValidatePattern(candidatePattern, gameSpeed))
                {
                    return candidatePattern; // SUCCESS: Pattern is valid and approved
                }
                
                // 3. If validation fails, the loop continues and we try again.
            }
            
            Debug.LogWarning("ProceduralPatternEngine: Failed to generate a valid pattern after max attempts. Spawning a failsafe pattern.");
            return CreateFailsafePattern(); // Return a guaranteed-safe, empty pattern as a fallback.
        }

        /// <summary>
        /// Creates a pattern based on the current game state and difficulty profile.
        /// </summary>
        private ProceduralPattern CreateCandidatePattern(float gameSpeed)
        {
            ProceduralPattern pattern = new ProceduralPattern();
            
            // Get difficulty parameters from the profile
            float density = difficultyProfile.GetCurrentDensity(_gameTime);
            float playerSkill = _playerSkillProvider.GetSkillRating(); // from 0.0 to 1.0
            
            // Adjust density based on player skill (higher skill = slightly more density)
            density *= (1 + (playerSkill - 0.5f) * 0.2f); // Adjusts density by up to +/- 10%

            // This is where the core generation logic would go. It would use the seedManager
            // to place different types of obstacles in the 3 lanes based on the density value.
            // For example:
            for (int lane = 0; lane < 3; lane++)
            {
                if (seedManager.GetNextFloat() < density)
                {
                    // Add an obstacle to this lane in the pattern data structure
                }
            }

            return pattern;
        }

        /// <summary>
        /// Creates a guaranteed-safe pattern to be used when generation fails.
        /// </summary>
        private ProceduralPattern CreateFailsafePattern()
        {
            // Returns an empty pattern with no obstacles.
            return new ProceduralPattern();
        }
    }
    
    #region Mocks and Interfaces

    /// <summary>
    /// Mock interface for providing player skill data.
    /// </summary>
    public interface IPlayerSkillProvider
    {
        float GetSkillRating();
        int GetDeathsToPattern(int patternId);
    }

    /// <summary>
    /// Mock implementation of the skill provider.
    /// </summary>
    public class MockPlayerSkillProvider : IPlayerSkillProvider
    {
        public float GetSkillRating() { return 0.5f; } // Average skill
        public int GetDeathsToPattern(int patternId) { return 0; }
    }

    #endregion
}
