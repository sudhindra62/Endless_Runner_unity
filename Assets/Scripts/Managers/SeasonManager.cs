using UnityEngine;
using System;

    public class SeasonManager : MonoBehaviour
    {
        public static SeasonManager Instance { get; private set; }

        public event Action OnWeeklyReset;
        public event Action OnSeasonReset;

        private DateTime nextWeeklyReset;
        private DateTime nextSeasonReset;

        private const int WEEKS_IN_SEASON = 4;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            // In a real game, this would be loaded from a persistent save or server time
            CalculateNextResetTimes(DateTime.UtcNow);
            InvokeRepeating(nameof(CheckForResets), 1f, 60f); // Check every minute
        }

        void CheckForResets()
        {
            DateTime now = DateTime.UtcNow;
            if (now >= nextWeeklyReset)
            {
                OnWeeklyReset?.Invoke();
                if (now >= nextSeasonReset)
                {
                    OnSeasonReset?.Invoke();
                }
                CalculateNextResetTimes(now);
            }
        }

        void CalculateNextResetTimes(DateTime fromTime)
        {
            // Find the next Sunday for weekly reset
            nextWeeklyReset = fromTime.AddDays(7 - (int)fromTime.DayOfWeek).Date;

            // Logic for season reset
            // This is a simplified example
            nextSeasonReset = nextWeeklyReset.AddDays(WEEKS_IN_SEASON * 7);

            Debug.Log($"Next Weekly Reset: {nextWeeklyReset}");
            Debug.Log($"Next Season Reset: {nextSeasonReset}");
        }
    }

