
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Managers;

namespace EndlessRunner.AI
{
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
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += HandleScoreChanged;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
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
}
