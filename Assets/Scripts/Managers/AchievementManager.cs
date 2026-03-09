using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Achievements
{
    /// <summary>
    /// Fortified manager for all achievement-related logic.
    /// This system is now robust, decoupled, and data-driven.
    /// </summary>
    public class AchievementManager : MonoBehaviour
    {
        // --- Singleton Pattern ---
        public static AchievementManager Instance { get; private set; }

        [Header("Configuration")]
        [Tooltip("The database containing all achievements for the game.")]
        [SerializeField] private AchievementDatabase _achievementDatabase;

        // --- State ---
        private Dictionary<AchievementID, AchievementProgressData> _achievementProgress;

        // --- Events ---
        public static event System.Action<Achievement> OnAchievementCompleted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAchievements();
        }

        private void OnEnable()
        {
            AchievementProgressData.OnAchievementUnlocked += HandleAchievementUnlocked;
        }

        private void OnDisable()
        {
            AchievementProgressData.OnAchievementUnlocked -= HandleAchievementUnlocked;
        }

        /// <summary>
        /// Initializes the achievement progress from the database.
        /// </summary>
        private void InitializeAchievements()
        {
            _achievementProgress = new Dictionary<AchievementID, AchievementProgressData>();
            if (_achievementDatabase == null) 
            {
                Debug.LogError("AchievementDatabase is not set in AchievementManager.");
                return;
            }

            foreach (var achievement in _achievementDatabase.achievements)
            {
                _achievementProgress[achievement.ID] = new AchievementProgressData(achievement);
            }
        }

        /// <summary>
        /// Updates the progress of a specific achievement.
        /// </summary>
        public void UpdateAchievementProgress(AchievementID id, int amount)
        {
            if (_achievementProgress.TryGetValue(id, out var progressData))
            {
                progressData.AddProgress(amount);
            }
        }

        /// <summary>
        /// Handles the event when an achievement is unlocked.
        /// </summary>
        private void HandleAchievementUnlocked(Achievement achievement)
        {
            // The achievement is unlocked, now let other systems know about it.
            OnAchievementCompleted?.Invoke(achievement);

            // The RewardManager will listen for this event and grant the rewards.
        }

        /// <summary>
        /// Gets the progress of a specific achievement.
        /// </summary>
        public AchievementProgressData GetAchievementProgress(AchievementID id)
        {
            _achievementProgress.TryGetValue(id, out var progressData);
            return progressData;
        }

        /// <summary>
        /// Gets all achievement progress data.
        /// </summary>
        public IEnumerable<AchievementProgressData> GetAllAchievementProgress()
        {
            return _achievementProgress.Values;
        }
    }
}
