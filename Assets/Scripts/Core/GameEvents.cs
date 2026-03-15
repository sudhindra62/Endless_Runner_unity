
using System;
using EndlessRunner.Achievements;
using EndlessRunner.Missions;
using EndlessRunner.PowerUps;

namespace EndlessRunner.Core
{
    /// <summary>
    /// A centralized static class for managing game-wide events.
    /// This decouples systems by allowing them to subscribe to and trigger events 
    /// without direct references to each other.
    /// </summary>
    public static class GameEvents
    {
        // Gameplay Events
        public static event Action OnGameStart;
        public static void TriggerGameStart() => OnGameStart?.Invoke();

        public static event Action OnPlayerDeath;
        public static void TriggerPlayerDeath() => OnPlayerDeath?.Invoke();

        public static event Action OnPlayerJump;
        public static void TriggerPlayerJump() => OnPlayerJump?.Invoke();

        // Economy & Scoring Events
        public static event Action<int> OnScoreGained;
        public static void TriggerScoreGained(int amount) => OnScoreGained?.Invoke(amount);

        public static event Action<int> OnCoinsGained;
        public static void TriggerCoinsGained(int amount) => OnCoinsGained?.Invoke(amount);

        // Achievement Events
        public static event Action<Achievement> OnAchievementUnlocked;
        public static void TriggerAchievementUnlocked(Achievement achievement) => OnAchievementUnlocked?.Invoke(achievement);
        
        // PowerUp Events
        public static event Action<PowerUpDefinition> OnPowerUpActivated;
        public static void TriggerPowerUpActivated(PowerUpDefinition powerUp) => OnPowerUpActivated?.Invoke(powerUp);
        
        public static event Action<PowerUpDefinition> OnPowerUpDeactivated;
        public static void TriggerPowerUpDeactivated(PowerUpDefinition powerUp) => OnPowerUpDeactivated?.Invoke(powerUp);
        
        // Mission Events
        public static event Action<Mission> OnMissionCompleted;
        public static void TriggerMissionCompleted(Mission mission) => OnMissionCompleted?.Invoke(mission);

        // UI Events
        public static event Action OnShowGameOverPanel;
        public static void TriggerShowGameOverPanel() => OnShowGameOverPanel?.Invoke();
    }
}
