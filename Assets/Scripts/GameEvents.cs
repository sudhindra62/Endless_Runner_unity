
using System;

namespace EndlessRunner.Events
{
    /// <summary>
    /// Static class for managing all game-wide events.
    /// Fully integrated by the Supreme Guardian Architect v13 to ensure total system connectivity.
    /// This is the central event bus for the entire application.
    /// </summary>
    public static class GameEvents
    {
        // Core Gameplay
        public static event Action OnRunStart;
        public static void RunStart() => OnRunStart?.Invoke();

        public static event Action<bool> OnRunComplete;
        public static void RunComplete(bool noRevive) => OnRunComplete?.Invoke(noRevive);
        
        public static event Action OnPlayerDeath;
        public static void PlayerDeath() => OnPlayerDeath?.Invoke();

        // Scoring & Currency
        public static event Action<int> OnScoreIncreased;
        public static void ScoreIncreased(int amount) => OnScoreIncreased?.Invoke(amount);

        public static event Action<int> OnCoinCollected;
        public static void CoinCollected(int amount) => OnCoinCollected?.Invoke(amount);

        // PowerUps
        public static event Action<string> OnPowerUpActivated; // Include type
        public static void PowerUpActivated(string powerUpType) => OnPowerUpActivated?.Invoke(powerUpType);
        
        public static event Action<string, string> OnPowerUpFused; // Fusion of two types
        public static void PowerUpFused(string basePowerUp, string ingredientPowerUp) => OnPowerUpFused?.Invoke(basePowerUp, ingredientPowerUp);


        // Player Actions & Skills
        public static event Action OnNearMiss;
        public static void NearMiss() => OnNearMiss?.Invoke();

        public static event Action OnReviveUsed;
        public static void ReviveUsed() => OnReviveUsed?.Invoke();

        // Progression & Unlocks
        public static event Action<string> OnMilestoneUnlocked;
        public static void MilestoneUnlocked(string milestoneId) => OnMilestoneUnlocked?.Invoke(milestoneId);
        
        public static event Action<string> OnCharacterUnlocked;
        public static void CharacterUnlocked(string characterId) => OnCharacterUnlocked?.Invoke(characterId);

        public static event Action<string> OnCosmeticUnlocked;
        public static void CosmeticUnlocked(string cosmeticId) => OnCosmeticUnlocked?.Invoke(cosmeticId);

        // Boss & Enemies
        public static event Action OnBossEncounterStart;
        public static void BossEncounterStart() => OnBossEncounterStart?.Invoke();
        
        public static event Action OnBossDefeated;
        public static void BossDefeated() => OnBossDefeated?.Invoke();

        // System & Session
        public static event Action OnLogin;
        public static void Login() => OnLogin?.Invoke();
        
        public static event Action OnSettingsChanged;
        public static void SettingsChanged() => OnSettingsChanged?.Invoke();
    }
}
