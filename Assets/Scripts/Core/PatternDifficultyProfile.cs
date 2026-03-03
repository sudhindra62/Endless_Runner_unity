
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Defines the difficulty parameters for pattern generation at different stages of a run.
    /// This allows for a smooth difficulty curve that can be easily tweaked.
    /// </summary>
    [CreateAssetMenu(fileName = "PatternDifficultyProfile", menuName = "EndlessRunner/Pattern Difficulty Profile", order = 1)]
    public class PatternDifficultyProfile : ScriptableObject
    {
        [Header("Obstacle Density")]
        [Tooltip("Base density of obstacles. Higher means more obstacles per tile.")]
        public float BaseDensity = 0.5f;
        [Tooltip("How much density increases per minute of gameplay.")]
        public float DensityGrowthRate = 0.05f;
        [Tooltip("The maximum density value.")]
        public float MaxDensity = 1.5f;

        [Header("Reaction Time")]
        [Tooltip("The initial minimum reaction window the player is given (in seconds).")]
        public float InitialReactionWindow = 0.75f;
        [Tooltip("How much the reaction window shrinks per minute.")]
        public float ReactionWindowShrinkRate = 0.02f;
        [Tooltip("The absolute minimum reaction time allowed. Prevents impossible patterns.")]
        public float MinReactionWindow = 0.25f;

        [Header("Complexity Scaling")]
        [Tooltip("The game time (in seconds) at which more complex obstacle types (e.g., moving obstacles) can appear.")]
        public float TimeToIntroduceComplexObstacles = 120f;
        [Tooltip("The probability (0-1) of a complex obstacle appearing after the introduction time.")]
        public float ComplexObstacleChance = 0.3f;

        [Header("Boss Mode Modifiers")]
        [Tooltip("Multiplier for obstacle density during a boss chase.")]
        public float BossModeDensityMultiplier = 1.5f;
        [Tooltip("Flat reduction in the reaction window during a boss chase.")]
        public float BossModeReactionWindowReduction = 0.1f;

        [Header("Risk Lane Modifiers")]
        [Tooltip("Multiplier for obstacle density in the risk lane.")]
        public float RiskLaneDensityMultiplier = 2.0f;
        [Tooltip("Chance of finding a high-value coin cluster in the risk lane.")]
        public float RiskLaneCoinChance = 0.8f;

        /// <summary>
        /// Calculates the current obstacle density based on the game duration.
        /// </summary>
        public float GetCurrentDensity(float gameTimeInSeconds)
        {
            float minutes = gameTimeInSeconds / 60f;
            float density = BaseDensity + (minutes * DensityGrowthRate);
            return Mathf.Min(density, MaxDensity);
        }

        /// <summary>
        /// Calculates the current minimum reaction window based on game duration.
        /// </summary>
        public float GetCurrentReactionWindow(float gameTimeInSeconds)
        {
            float minutes = gameTimeInSeconds / 60f;
            float window = InitialReactionWindow - (minutes * ReactionWindowShrinkRate);
            return Mathf.Max(window, MinReactionWindow);
        }

        /// <summary>
        /// Determines if complex obstacles should be introduced.
        /// </summary>
        public bool ShouldIntroduceComplexObstacles(float gameTimeInSeconds)
        {
            return gameTimeInSeconds >= TimeToIntroduceComplexObstacles;
        }
    }
}
