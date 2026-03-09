using UnityEngine;

namespace Core
{
    /// <summary>
    /// Defines the dynamic difficulty parameters for pattern generation.
    /// This ScriptableObject is the authoritative source for all difficulty calculations, 
    /// providing context-aware values for different gameplay scenarios.
    /// Fortified by Supreme Guardian Architect v12 to ensure logical encapsulation.
    /// </summary>
    [CreateAssetMenu(fileName = "PatternDifficultyProfile", menuName = "EndlessRunner/Pattern Difficulty Profile", order = 1)]
    public class PatternDifficultyProfile : ScriptableObject
    {
        /// <summary>
        /// Describes the gameplay context to correctly apply difficulty modifiers.
        /// </summary>
        public enum PatternContext
        {
            Normal,
            BossMode,
            RiskLane
        }

        [Header("Base Progression Curve")]
        [Tooltip("Base density of obstacles. Higher means more obstacles per tile.")]
        [SerializeField] private float _baseDensity = 0.5f;
        [Tooltip("How much density increases per minute of gameplay.")]
        [SerializeField] private float _densityGrowthRate = 0.05f;
        [Tooltip("The maximum base density value.")]
        [SerializeField] private float _maxDensity = 1.5f;

        [Space(10)]
        [Tooltip("The initial minimum reaction window the player is given (in seconds).")]
        [SerializeField] private float _initialReactionWindow = 0.75f;
        [Tooltip("How much the reaction window shrinks per minute.")]
        [SerializeField] private float _reactionWindowShrinkRate = 0.02f;
        [Tooltip("The absolute minimum reaction time allowed. Prevents impossible patterns.")]
        [SerializeField] private float _minReactionWindow = 0.25f;

        [Header("Complexity Progression")]
        [Tooltip("The game time (in seconds) at which more complex obstacle types (e.g., moving obstacles) can appear.")]
        [SerializeField] private float _timeToIntroduceComplexObstacles = 120f;
        [Tooltip("The probability (0-1) of a complex obstacle appearing after the introduction time.")]
        [SerializeField] [Range(0f, 1f)] private float _complexObstacleChance = 0.3f;

        [Header("Contextual Modifiers")]
        [Tooltip("Multiplier for obstacle density during a boss chase.")]
        [SerializeField] private float _bossModeDensityMultiplier = 1.5f;
        [Tooltip("Flat reduction in the reaction window during a boss chase.")]
        [SerializeField] private float _bossModeReactionWindowReduction = 0.1f;
        [Tooltip("Multiplier for obstacle density in the risk lane.")]
        [SerializeField] private float _riskLaneDensityMultiplier = 2.0f;
        [Tooltip("Chance of finding a high-value coin cluster in the risk lane.")]
        [SerializeField] [Range(0f, 1f)] private float _riskLaneCoinChance = 0.8f;


        /// <summary>
        /// Calculates the context-aware obstacle density based on the game duration.
        /// </summary>
        public float GetCurrentDensity(float gameTimeInSeconds, PatternContext context = PatternContext.Normal)
        {
            float minutes = gameTimeInSeconds / 60f;
            float density = _baseDensity + (minutes * _densityGrowthRate);
            density = Mathf.Min(density, _maxDensity);

            switch (context)
            {
                case PatternContext.BossMode:
                    density *= _bossModeDensityMultiplier;
                    break;
                case PatternContext.RiskLane:
                    density *= _riskLaneDensityMultiplier;
                    break;
            }
            return density;
        }

        /// <summary>
        /// Calculates the context-aware minimum reaction window based on game duration.
        /// </summary>
        public float GetCurrentReactionWindow(float gameTimeInSeconds, PatternContext context = PatternContext.Normal)
        {
            float minutes = gameTimeInSeconds / 60f;
            float window = _initialReactionWindow - (minutes * _reactionWindowShrinkRate);
            window = Mathf.Max(window, _minReactionWindow);

            if (context == PatternContext.BossMode)
            {
                window -= _bossModeReactionWindowReduction;
                // Ensure the reduction doesn't push the window below the absolute minimum.
                window = Mathf.Max(window, _minReactionWindow);
            }
            return window;
        }

        /// <summary>
        /// Determines if complex obstacles should be introduced based on game time.
        /// </summary>
        public bool ShouldIntroduceComplexObstacles(float gameTimeInSeconds)
        {
            return gameTimeInSeconds >= _timeToIntroduceComplexObstacles;
        }
        
        /// <summary>
        /// Gets the probability of spawning a complex obstacle.
        /// </summary>
        public float GetComplexObstacleChance()
        {
            return _complexObstacleChance;
        }

        /// <summary>
        /// Gets the chance of finding a coin cluster in the risk lane.
        /// </summary>
        public float GetRiskLaneCoinChance()
        {
            return _riskLaneCoinChance;
        }
    }
}
