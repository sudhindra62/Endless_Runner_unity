
using UnityEngine;

    [CreateAssetMenu(fileName = "PatternDifficultyProfile", menuName = "EndlessRunner/Pattern Difficulty Profile", order = 1)]
    public class PatternDifficultyProfile : ScriptableObject
    {
        public enum PatternContext
        {
            Normal,
            BossMode,
            RiskLane
        }

        [Header("Progression Curves")]
        [Tooltip("The time in seconds over which the difficulty curves are evaluated. After this time, difficulty remains at its maximum.")]
        [SerializeField] private float maxProgressionTime = 300f; // 5 minutes to reach max difficulty

        [Tooltip("Determines obstacle density over time. Time (X-axis) is normalized from 0 to 1, corresponding to 0 to maxProgressionTime. Value (Y-axis) is the density.")]
        [SerializeField] private AnimationCurve densityCurve = AnimationCurve.Linear(0, 0.5f, 1, 1.5f);

        [Tooltip("Determines the player's reaction window over time. Time (X-axis) is normalized. Value (Y-axis) is the reaction time in seconds.")]
        [SerializeField] private AnimationCurve reactionWindowCurve = AnimationCurve.Linear(0, 0.75f, 1, 0.25f);

        [Tooltip("Chance of a complex obstacle appearing over time. Time (X-axis) is normalized. Value (Y-axis) is the probability (0-1).")]
        [SerializeField] private AnimationCurve complexObstacleChanceCurve = AnimationCurve.Linear(0, 0, 1, 0.3f);

        [Header("Contextual Modifiers")]
        [Tooltip("Multiplier for obstacle density during a boss chase.")]
        [SerializeField] private float bossModeDensityMultiplier = 1.5f;
        [Tooltip("Flat reduction in the reaction window during a boss chase.")]
        [SerializeField] private float bossModeReactionWindowReduction = 0.1f;
        [Tooltip("Multiplier for obstacle density in the risk lane.")]
        [SerializeField] private float riskLaneDensityMultiplier = 2.0f;
        [Tooltip("Chance of finding a high-value coin cluster in the risk lane.")]
        [SerializeField] [Range(0f, 1f)] private float riskLaneCoinChance = 0.8f;

        /// <summary>
        /// Calculates the context-aware obstacle density based on the game duration.
        /// </summary>
        public float GetCurrentDensity(float gameTimeInSeconds, PatternContext context = PatternContext.Normal)
        {
            float normalizedTime = Mathf.Clamp01(gameTimeInSeconds / maxProgressionTime);
            float density = densityCurve.Evaluate(normalizedTime);

            switch (context)
            {
                case PatternContext.BossMode:
                    density *= bossModeDensityMultiplier;
                    break;
                case PatternContext.RiskLane:
                    density *= riskLaneDensityMultiplier;
                    break;
            }
            return density;
        }

        /// <summary>
        /// Calculates the context-aware minimum reaction window based on game duration.
        /// </summary>
        public float GetCurrentReactionWindow(float gameTimeInSeconds, PatternContext context = PatternContext.Normal)
        {
            float normalizedTime = Mathf.Clamp01(gameTimeInSeconds / maxProgressionTime);
            float window = reactionWindowCurve.Evaluate(normalizedTime);

            if (context == PatternContext.BossMode)
            {
                window -= bossModeReactionWindowReduction;
                // Ensure the reduction doesn't push the window below the curve's minimum value.
                float minPossibleWindow = reactionWindowCurve.keys.Length > 0 ? reactionWindowCurve.keys[reactionWindowCurve.length - 1].value : 0.1f;
                window = Mathf.Max(window, minPossibleWindow);
            }
            return window;
        }

        /// <summary>
        /// Gets the probability of spawning a complex obstacle based on game time.
        /// </summary>
        public float GetComplexObstacleChance(float gameTimeInSeconds)
        {
            float normalizedTime = Mathf.Clamp01(gameTimeInSeconds / maxProgressionTime);
            return complexObstacleChanceCurve.Evaluate(normalizedTime);
        }

        /// <summary>
        /// Gets the chance of finding a coin cluster in the risk lane.
        /// </summary>
        public float GetRiskLaneCoinChance()
        {
            return riskLaneCoinChance;
        }
    }

