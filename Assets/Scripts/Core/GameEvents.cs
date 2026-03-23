using System;
using UnityEngine;

/// <summary>
/// A centralized static class for managing game-wide events.
/// This decouples systems by allowing them to subscribe to and trigger events 
/// without direct references to each other.
/// Global scope for maximum project-wide accessibility.
/// </summary>
public static class GameEvents
{
    // --- Core Gameplay Events ---
    public static event Action OnGameStart;
    public static void TriggerGameStart()
    {
        OnGameStart?.Invoke();
        if (GameManager.Instance != null) GameManager.Instance.StartGame();
    }

    public static event Action OnGameOver;
    public static void TriggerGameOver()
    {
        OnGameOver?.Invoke();
        if (GameManager.Instance != null) GameManager.Instance.EndGame();
    }

    public static event Action OnPlayerDeath;
    public static void TriggerPlayerDeath() => OnPlayerDeath?.Invoke();

    public static event Action OnPlayerJump;
    public static void TriggerPlayerJump() => OnPlayerJump?.Invoke();

    public static event Action OnRunStarted;
    public static void TriggerRunStarted() => OnRunStarted?.Invoke();

    public static event Action<bool> OnRunEnded;
    public static void TriggerRunEnded(bool success) => OnRunEnded?.Invoke(success);

    // --- Gameplay Mechanics (Folder 1) ---
    public static event Action OnNearMiss;
    public static void TriggerNearMiss() => OnNearMiss?.Invoke();

    public static event Action<string> OnWorldEvent;
    public static void TriggerWorldEvent(string eventType) => OnWorldEvent?.Invoke(eventType);

    public static event Action<int> OnComboChanged;
    public static void TriggerComboChanged(int combo) => OnComboChanged?.Invoke(combo);

    // --- Scoring & Economy ---
    public static event Action<int> OnScoreGained;
    public static void TriggerScoreGained(int amount) => OnScoreGained?.Invoke(amount);

    public static event Action<int> OnCoinsGained;
    public static void TriggerCoinsGained(int amount) => OnCoinsGained?.Invoke(amount);

    public static event Action<int> OnCoinCollected;
    public static void TriggerCoinCollected(int amount) => OnCoinCollected?.Invoke(amount);

    // --- PowerUps ---
    public static event Action<string> OnPowerUpActivated;
    public static void TriggerPowerUpActivated(string type) => OnPowerUpActivated?.Invoke(type);

    public static event Action<string> OnPowerUpDeactivated;
    public static void TriggerPowerUpDeactivated(string type) => OnPowerUpDeactivated?.Invoke(type);

    public static event Action<string, string> OnPowerUpFused;
    public static void TriggerPowerUpFused(string basePowerUp, string ingredientPowerUp) => OnPowerUpFused?.Invoke(basePowerUp, ingredientPowerUp);

    // --- Achievements & Missions ---
    public static event Action<string> OnAchievementUnlocked;
    public static void TriggerAchievementUnlocked(string id) => OnAchievementUnlocked?.Invoke(id);
    public static void TriggerAchievementUnlocked(Achievement achievement) => TriggerAchievementUnlocked(achievement != null ? achievement.id : string.Empty);

    public static event Action<string> OnMissionCompleted;
    public static void TriggerMissionCompleted(string id) => OnMissionCompleted?.Invoke(id);
    public static void TriggerMissionCompleted(Mission mission) => TriggerMissionCompleted(mission != null ? mission.missionId : string.Empty);

    // --- UI & System ---
    public static event Action OnShowGameOverPanel;
    public static void TriggerShowGameOverPanel() => OnShowGameOverPanel?.Invoke();

    public static event Action OnSettingsChanged;
    public static void TriggerSettingsChanged() => OnSettingsChanged?.Invoke();

    public static event Action<string> OnThemeChanged;
    public static void TriggerThemeChanged(string themeId) => OnThemeChanged?.Invoke(themeId);

    public static event Action OnRevive;
    public static void TriggerRevive() => OnRevive?.Invoke();

    // Singleton compatibility wrapper (for legacy calls)
    public static class Instance
    {
        public static void TriggerGameStart() => GameEvents.TriggerGameStart();
        public static void TriggerGameOver() => GameEvents.TriggerGameOver();
    }
}
