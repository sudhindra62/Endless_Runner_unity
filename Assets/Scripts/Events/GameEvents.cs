
using EndlessRunner.Core;
using System;

namespace EndlessRunner.Events
{
    /// <summary>
    /// Provides a central hub for game-wide events that do not belong to a specific manager.
    /// While many events are handled by their respective managers (e.g., GameManager.OnGameStateChanged),
    /// this class can be used for more generic or cross-cutting concerns.
    /// </summary>
    public class GameEvents : Singleton<GameEvents>
    {
        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
        }

        // --- Gameplay Events ---
        public event Action OnGameStart;
        public void TriggerGameStart() => OnGameStart?.Invoke();

        public event Action OnGameOver;
        public void TriggerGameOver() => OnGameOver?.Invoke();


        // --- UI Events ---
        public event Action OnShowGameOverPanel;
        public void TriggerShowGameOverPanel() => OnShowGameOverPanel?.Invoke();
    }
}
