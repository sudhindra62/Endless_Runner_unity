
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class TimeManager : Singleton<TimeManager>
    {
        public bool IsPaused { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
        }

        public void Pause()
        {
            if (IsPaused) return;
            IsPaused = true;
            Time.timeScale = 0f;
            Logger.Log("TIME_MANAGER", "Game paused");
        }

        public void Resume()
        {
            if (!IsPaused) return;
            IsPaused = false;
            Time.timeScale = 1f;
            Logger.Log("TIME_MANAGER", "Game resumed");
        }
    }
}
