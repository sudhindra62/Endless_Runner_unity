
using UnityEngine;

    /// <summary>
    /// Dynamically adjusts the game's difficulty based on player performance (score).
    /// This influences the pattern selection in the procedural engine.
    /// </summary>
    public class AdaptiveAIDifficultyManager : Singleton<AdaptiveAIDifficultyManager>
    {
        [Header("Difficulty Settings")]
        [Tooltip("The score required to advance to the next difficulty level.")]
        [SerializeField] private int scoreThresholdPerLevel = 1000;
        [Tooltip("The maximum difficulty level.")]
        [SerializeField] private int maxDifficulty = 10;

        public int CurrentDifficulty { get; private set; }

        private int lastScoreCheck = 0;

        protected override void Awake()
        {
            base.Awake();
            ResetDifficulty();
        }

        private void OnEnable()
        {
            // The provided snippet seems to be a replacement for the OnEnable method.
            // It also introduces `aiController` and `difficultyConfig` which are not defined in this class.
            // Assuming the intent is to change `GameManager.Instance.OnScoreChanged` to `GameManager.OnScoreChanged`
            // and add the new conditions from the snippet, while keeping the method structure.
            // Note: `aiController` and `difficultyConfig` would need to be defined in this class for the code to compile.
            // For now, I'm including them as per the instruction, assuming they will be defined elsewhere or are placeholders.
            // If `GameManager.OnScoreChanged` is a static event, the `GameManager.Instance != null` check might be redundant for the event subscription itself,
            // but it's kept as part of the provided snippet's conditions.
            if (GameManager.Instance != null /*&& aiController != null*/) // aiController is not defined
            {
                // if (difficultyConfig != null && difficultyConfig.scalingFactors.Length > 0) // difficultyConfig is not defined
                // {
                    GameManager.OnScoreChanged += HandleScoreChanged;
                // }
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.OnScoreChanged -= HandleScoreChanged;
            }
        }

        private void HandleScoreChanged(int newScore)
        {
            // Check if the player has crossed the next difficulty threshold
            if (newScore > lastScoreCheck + scoreThresholdPerLevel)
            {
                IncreaseDifficulty();
                lastScoreCheck = newScore;
            }
        }

        private void IncreaseDifficulty()
        {
            if (CurrentDifficulty < maxDifficulty)
            {
                CurrentDifficulty++;
                Debug.Log($"AI_DIFFICULTY: Player has advanced to difficulty level {CurrentDifficulty}");

                // Notify the pattern engine of the new difficulty
                ProceduralPatternEngine.Instance.SetDifficulty(CurrentDifficulty);
            }
        }

        /// <summary>
        /// Resets the difficulty to its starting value for a new game.
        /// </summary>
        public void ResetDifficulty()
        {
            CurrentDifficulty = 1;
            lastScoreCheck = 0;
            // Also notify the engine of the reset difficulty
            if (ProceduralPatternEngine.Instance != null)
            {
                ProceduralPatternEngine.Instance.SetDifficulty(CurrentDifficulty);
            }
        }
    }

