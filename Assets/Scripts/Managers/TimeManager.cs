
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.Managers
{
    /// <summary>
    /// Manages the game's concept of time, including pausing, resuming, and time scale effects.
    /// </summary>
    public class TimeManager : Singleton<TimeManager>
    {
        private float timeScaleBeforePause = 1f;

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Pauses the game by setting the time scale to 0.
        /// </summary>
        public void Pause()
        {
            if (Time.timeScale > 0)
            {
                timeScaleBeforePause = Time.timeScale;
                Time.timeScale = 0;
                Debug.Log("TimeManager: Game paused.");
            }
        }

        /// <summary>
        /// Resumes the game by restoring the time scale to its previous value.
        /// </summary>
        public void Resume()
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = timeScaleBeforePause;
                Debug.Log($"TimeManager: Game resumed with time scale {timeScaleBeforePause}.");
            }
        }

        /// <summary>
        /// Sets a custom time scale for slow-motion or fast-forward effects.
        /// </summary>
        /// <param name="newTimeScale">The new time scale to set.</param>
        public void SetTimeScale(float newTimeScale)
        {
            timeScaleBeforePause = newTimeScale;
            Time.timeScale = newTimeScale;
            Debug.Log($"TimeManager: Time scale set to {newTimeScale}.");
        }
    }
}
