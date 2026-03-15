
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

// Mission-related classes and enums
[System.Serializable]
public enum MissionType
{
    RunDistance,
    CollectCoins,
    ScorePoints
}

[System.Serializable]
public class Mission
{
    public string description;
    public MissionType type;
    public float target;
    public float progress;
    public bool isCompleted;

    public Mission(string description, MissionType type, float target)
    {
        this.description = description;
        this.type = type;
        this.target = target;
        this.progress = 0;
        this.isCompleted = false;
    }

    public void UpdateProgress(float amount)
    {
        if (isCompleted) return;

        progress += amount;
        if (progress >= target)
        {
            progress = target;
            isCompleted = true;
            Debug.Log($"MISSION_SYSTEM: Mission completed! '{description}'");
            GameManager.Instance.HandleMissionCompleted(this);
        }
    }
}

[CreateAssetMenu(fileName = "New Mission", menuName = "Endless Runner/Mission Definition")]
public class MissionDefinition : ScriptableObject
{
    public string description;
    public MissionType type;
    public float target;

    public Mission CreateMission()
    {
        return new Mission(description, type, target);
    }
}

// Placeholder classes for merged managers
public class SessionAnalyticsData 
{
    public void StartSession() { }
    public void EndSession(bool wasAbrupt) { }
    public void RecordDeath(string cause, float distance) { }
    public void RecordDodge(bool success) { }
    public void RecordCombo(int peak) { }
    public void RecordRevive() { }
    public void RecordBossEncounter(string bossName, bool survived) { }
}
public class BehaviorTrendAnalyzer 
{
    public void ProcessSession(SessionAnalyticsData data) { }
}
public class FrustrationDetector 
{
    public void Reset() { }
    public void ProcessSession(SessionAnalyticsData data) { }
    public void TrackDeath() { }
    public void TrackRevive() { }
    public float GetFrustrationScore() { return 0.0f; }
}

public enum PowerUpType { SpeedBoost, Shield, CoinMultiplier }

public class PowerUpDefinition
{
    public PowerUpType type;
    private float duration;
    private float timer;

    public PowerUpDefinition(PowerUpType type, float duration)
    {
        this.type = type;
        this.duration = duration;
    }

    public void Activate()
    {
        timer = duration;
    }

    public void Tick(float deltaTime)
    {
        timer -= deltaTime;
    }

    public bool IsActive()
    {
        return timer > 0;
    }
}

public class PowerUpEffectsManager : MonoBehaviour
{
    public void PlayEffect(PowerUpType type) { }
    public void StopEffect(PowerUpType type) { }
}

public static class GameEvents
{
    public static void TriggerPowerUpActivated(PowerUpDefinition powerUp) { }
    public static void TriggerPowerUpDeactivated(PowerUpDefinition powerUp) { }
}

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadUnlockedSkins();
        LoadCurrentSkin();
        replayBuffer = new List<Vector3>(replayBufferSize);
        InitializeIAP();
        LoadMission();

        // Analytics Initialization
        trendAnalyzer = new BehaviorTrendAnalyzer();
        frustrationDetector = new FrustrationDetector();

        // PowerUpManager initialization
        effectsManager = gameObject.AddComponent<PowerUpEffectsManager>();
    }

    void Start()
    {
        CheckDailyRewardStatus();
        HandleGameStart();
        CheckForOfflineProgress();
        StartSession();
    }

    void Update()
    {
        if (currentMission != null && !currentMission.isCompleted && currentMission.type == MissionType.RunDistance)
        {
            int currentPlayerX = (int)transform.position.x; // Assuming player position is tracked by GameManager
            if(currentPlayerX > lastPlayerXPosition)
            {
                currentMission.UpdateProgress(currentPlayerX - lastPlayerXPosition);
                lastPlayerXPosition = currentPlayerX;
            }
        }

        // PowerUpManager update logic
        for (int i = activePowerUps.Count - 1; i >= 0; i--)
        {
            PowerUpDefinition powerUp = activePowerUps[i];
            powerUp.Tick(Time.deltaTime);

            if (!powerUp.IsActive())
            {
                DeactivatePowerUp(powerUp);
            }
        }
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    // Placeholder for AchievementManager logic
    public void UnlockAchievement(string achievementId)
    {
        Debug.Log("Achievement unlocked: " + achievementId);
    }

    // Placeholder for AdaptiveDifficultyManager logic
    public void AdjustDifficulty()
    {
        Debug.Log("Difficulty adjusted");
    }

    // Placeholder for AdManager logic
    public void ShowAd()
    {
        Debug.Log("Showing ad");
    }

    // Placeholder for AlmostWinManager logic
    public void CheckForAlmostWin()
    {
        Debug.Log("Checking for almost win");
    }

    // Placeholder for AnalyticsManager logic
    public void LogEvent(string eventName)
    {
        Debug.Log("Event logged: " + eventName);
    }

    // Placeholder for AuthenticationManager logic
    public void AuthenticatePlayer()
    {
        Debug.Log("Player authenticated");
    }

    // Placeholder for CharacterUpgradeManager logic
    public void UpgradeCharacter(string characterId)
    {
        Debug.Log("Character upgraded: " + characterId);
    }

    // Placeholder for CoinManager logic
    public int Coins { get; private set; }
    public void AddCoins(int amount)
    {
        Coins += amount;
        HandleCoinsGained(amount);
        Debug.Log(amount + " coins added. Total coins: " + Coins);
    }

    // Placeholder for LeaderboardManager logic
    public void ShowLeaderboard()
    {
        Debug.Log("Showing leaderboard");
    }

    // CharacterCustomizationManager logic
    public List<Skin> availableSkins = new List<Skin>();
    public int currentSkinIndex = 0;

    public event Action<Skin> OnSkinChanged;

    private const string UnlockedSkinsKey = "UnlockedSkins";
    private const string CurrentSkinKey = "CurrentSkin";

    public void UnlockSkin(int skinIndex)
    {
        if (skinIndex >= 0 && skinIndex < availableSkins.Count)
        {
            availableSkins[skinIndex].isUnlocked = true;
            SaveUnlockedSkins();
        }
    }

    public void SetCurrentSkin(int skinIndex)
    {
        if (availableSkins[skinIndex].isUnlocked)
        {
            currentSkinIndex = skinIndex;
            SaveCurrentSkin();
            OnSkinChanged?.Invoke(availableSkins[currentSkinIndex]);
        }
    }

    private void LoadUnlockedSkins()
    {
        string unlockedSkinsString = PlayerPrefs.GetString(UnlockedSkinsKey);
        if (!string.IsNullOrEmpty(unlockedSkinsString))
        {
            string[] unlockedIndices = unlockedSkinsString.Split(',');
            foreach (string indexString in unlockedIndices)
            {
                if (int.TryParse(indexString, out int index))
                {
                    if (index >= 0 && index < availableSkins.Count)
                    {
                        availableSkins[index].isUnlocked = true;
                    }
                }
            }
        }
    }

    private void SaveUnlockedSkins()
    {
        List<string> unlockedIndices = new List<string>();
        for (int i = 0; i < availableSkins.Count; i++)
        {
            if (availableSkins[i].isUnlocked)
            {
                unlockedIndices.Add(i.ToString());
            }
        }
        PlayerPrefs.SetString(UnlockedSkinsKey, string.Join(",", unlockedIndices));
        PlayerPrefs.Save();
    }

    private void LoadCurrentSkin()
    {
        currentSkinIndex = PlayerPrefs.GetInt(CurrentSkinKey, 0);
    }

    private void SaveCurrentSkin()
    {
        PlayerPrefs.SetInt(CurrentSkinKey, currentSkinIndex);
        PlayerPrefs.Save();
    }

    public Skin GetCurrentSkin()
    {
        return availableSkins[currentSkinIndex];
    }

    [System.Serializable]
    public class Skin
    {
        public string skinName;
        public Material skinMaterial;
        public bool isUnlocked;
    }

    // CinematicFinishManager logic
    [Header("Slow Motion Settings")]
    [SerializeField] private float slowMotionTimeScale = 0.2f;
    [SerializeField] private float slowMotionDuration = 0.7f;

    [Header("Replay Settings")]
    [SerializeField] private int replayBufferSize = 180; // 3 seconds at 60fps
    private List<Vector3> replayBuffer;

    public void OnPlayerDeath()
    {
        StartCoroutine(DeathSequence());
    }

    public void RecordPosition(Vector3 position)
    {
        if (replayBuffer.Count >= replayBufferSize)
        {
            replayBuffer.RemoveAt(0);
        }
        replayBuffer.Add(position);
    }

    public void ClearReplayBuffer()
    {
        replayBuffer.Clear();
    }

    private IEnumerator DeathSequence()
    {
        yield return StartCoroutine(SlowMotionEffect());

        CheckForAlmostWin();

        PlayReplay();
    }

    private IEnumerator SlowMotionEffect()
    {
        Time.timeScale = slowMotionTimeScale;
        yield return new WaitForSecondsRealtime(slowMotionDuration);

        // Assuming a revive manager is also merged
        // if (IsReviveActive()) 
        // {
        //     yield break;
        // }

        Time.timeScale = 1f;
    }

    private void PlayReplay()
    {
        // In a full implementation, this would trigger a replay system.
        // For now, we will just log the contents of the buffer.
        Debug.Log("--- REPLAY BUFFER ---");
        foreach (Vector3 position in replayBuffer)
        {
            Debug.Log(position);
        }
        Debug.Log("--- END REPLAY ---");
    }

    // CloudLoggingManager logic
    public void LogError(string message, string stackTrace)
    {
        // In a real implementation, this would send the error to a cloud service
        Debug.LogError($"CLOUD_LOGGING: Error: {message}\nStackTrace: {stackTrace}");
    }

    public void LogEvent(string eventName, System.Collections.Generic.Dictionary<string, object> parameters)
    {
        // In a real implementation, this would send the event to a cloud service
        Debug.Log($"CLOUD_LOGGING: Event: {eventName}");
    }

    // DailyRewardManager logic
    public event Action<bool> OnDailyRewardStatus;

    [SerializeField] private int dailyRewardAmount = 100;
    [SerializeField] private int streakBonusMultiplier = 50;

    public void CheckDailyRewardStatus()
    {
        bool isAvailable = IsRewardAvailable();
        OnDailyRewardStatus?.Invoke(isAvailable);
    }

    public bool IsRewardAvailable()
    {
        DateTime now = DateTime.UtcNow;
        long lastRewardTimestamp = 0; // Placeholder for SaveManager.Instance.Data.lastDailyRewardTimestamp;

        if (lastRewardTimestamp == 0)
        {
            return true; // First time playing
        }

        DateTime lastRewardDate = DateTime.FromBinary(lastRewardTimestamp);
        return (now - lastRewardDate).TotalDays >= 1;
    }

    public void ClaimReward()
    {
        if (!IsRewardAvailable()) return;

        DateTime now = DateTime.UtcNow;
        long lastRewardTimestamp = 0; // Placeholder for SaveManager.Instance.Data.lastDailyRewardTimestamp;
        DateTime lastRewardDate = DateTime.FromBinary(lastRewardTimestamp);

        int dailyRewardStreak = 1; // Placeholder for SaveManager.Instance.Data.dailyRewardStreak;
        if ((now - lastRewardDate).TotalDays < 2) // Less than 48 hours passed
        {
            dailyRewardStreak++;
        }
        else
        {
            dailyRewardStreak = 1; // Reset streak
        }

        int rewardAmount = dailyRewardAmount + (dailyRewardStreak * streakBonusMultiplier);

        AddCoins(rewardAmount);

        // Placeholder for saving game data
        // SaveManager.Instance.Data.lastDailyRewardTimestamp = now.ToBinary();
        // SaveManager.Instance.SaveGame();

        OnDailyRewardStatus?.Invoke(false);
        Debug.Log($"DAILY_REWARD: Player claimed {rewardAmount} coins. Current streak: {dailyRewardStreak}");
    }

    // IAPManager logic
    public event Action<string> OnPurchaseSuccess;
    public event Action<string> OnPurchaseFailure;

    public const string ProductIdCoinsTier1 = "com.omni_guardian.endlessrunner.coins1";
    public const string ProductIdCoinsTier2 = "com.omni_guardian.endlessrunner.coins2";
    public const string ProductIdRemoveAds = "com.omni_guardian.endlessrunner.removeads";

    public void InitializeIAP()
    {
        Debug.Log("IAP_MANAGER: Initializing In-App Purchase systems...");
    }

    public void PurchaseProduct(string productId)
    {
        Debug.Log($"IAP_MANAGER: Attempting to purchase product: {productId}");
        SimulatePurchaseSuccess(productId);
    }

    private void SimulatePurchaseSuccess(string productId)
    {
        Debug.Log($"IAP_MANAGER: Purchase successful for product: {productId}");
        OnPurchaseSuccess?.Invoke(productId);

        int coinsToAdd = 0;
        switch (productId)
        {
            case ProductIdCoinsTier1:
                coinsToAdd = 1000;
                break;
            case ProductIdCoinsTier2:
                coinsToAdd = 5000;
                break;
            case ProductIdRemoveAds:
                Debug.Log("IAP_MANAGER: Ads have been permanently removed.");
                break;
        }

        if (coinsToAdd > 0)
        {
            AddCoins(coinsToAdd);
        }
    }

    // MissionManager logic
    [Header("Mission Pool")]
    [SerializeField] private List<MissionDefinition> allMissions;

    private Mission currentMission;
    private int lastPlayerXPosition = 0; // For distance tracking

    public Mission GetCurrentMission()
    {
        return currentMission;
    }

    private void HandleGameStart()
    {
        lastPlayerXPosition = 0;
    }

    private void HandleScoreGained(int amount)
    {
        if (currentMission != null && !currentMission.isCompleted && currentMission.type == MissionType.ScorePoints)
        {
            currentMission.UpdateProgress(amount);
        }
    }

    private void HandleCoinsGained(int amount)
    {
        if (currentMission != null && !currentMission.isCompleted && currentMission.type == MissionType.CollectCoins)
        {
            currentMission.UpdateProgress(amount);
        }
    }
    
    public void HandleMissionCompleted(Mission mission)
    {
        AssignNewMission();
    }

    private void LoadMission()
    {
        // Placeholder for loading mission data
        AssignNewMission();
    }

    private void SaveMission()
    {
        // Placeholder for saving mission data
    }

    private void AssignNewMission()
    {
        if (allMissions != null && allMissions.Count > 0)
        {
            MissionDefinition newMissionDef;
            do
            {
                newMissionDef = allMissions[UnityEngine.Random.Range(0, allMissions.Count)];
            } while (currentMission != null && newMissionDef.description == currentMission.description);

            currentMission = newMissionDef.CreateMission();
            lastPlayerXPosition = 0;
            Debug.Log("MISSION_SYSTEM: New mission assigned: " + currentMission.description);
        }
        else
        { 
            Debug.LogWarning("MISSION_SYSTEM: No missions defined in the MissionManager!");
        }
    }

    // OfflineProgressManager logic
    [Header("Offline Progress Configuration")]
    [SerializeField] private int coinsPerMinute = 1;
    [SerializeField] private int maxOfflineMinutes = 240; // Max time to accumulate rewards (e.g., 4 hours)

    private const string QUIT_TIME_KEY = "AppQuitTime";

    private void CheckForOfflineProgress()
    {
        if (!PlayerPrefs.HasKey(QUIT_TIME_KEY))
        {
            return; // No stored quit time, so can't calculate progress.
        }

        long temp = Convert.ToInt64(PlayerPrefs.GetString(QUIT_TIME_KEY));
        DateTime quitTime = DateTime.FromBinary(temp);
        DateTime now = DateTime.UtcNow;

        TimeSpan offlineDuration = now - quitTime;

        if (offlineDuration.TotalMinutes > 1) // Only grant if offline for more than a minute
        {
            int minutesOffline = (int)Math.Min(offlineDuration.TotalMinutes, maxOfflineMinutes);
            
            int coinsToGrant = minutesOffline * coinsPerMinute;

            if (coinsToGrant > 0)
            {
                AddCoins(coinsToGrant);
                Debug.Log($"Player was offline for {minutesOffline} minutes. Granted {coinsToGrant} coins.");
            }
        }

        PlayerPrefs.DeleteKey(QUIT_TIME_KEY);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(QUIT_TIME_KEY, DateTime.UtcNow.ToBinary().ToString());
        PlayerPrefs.Save();
        Debug.Log("Saving quit time for offline progress calculation.");
        EndSession(false);
    }

    // ParticleEffectManager logic
    private float globalParticleMultiplier = 1.0f;

    public void SetGlobalParticleMultiplier(float multiplier)
    {
        globalParticleMultiplier = Mathf.Clamp01(multiplier);
        Debug.Log($"Global particle effect multiplier set to {globalParticleMultiplier}");
    }

    public float GetGlobalParticleMultiplier()
    {
        return globalParticleMultiplier;
    }

    // PersistentDataManager logic
    public void SaveData(string key, object data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    public T LoadData<T>(string key)
    {
        string jsonData = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<T>(jsonData);
    }

    // PlayerAnalyticsManager Logic
    private SessionAnalyticsData currentSession;
    private BehaviorTrendAnalyzer trendAnalyzer;
    private FrustrationDetector frustrationDetector;

    public void StartSession()
    {
        currentSession = new SessionAnalyticsData();
        currentSession.StartSession();
        frustrationDetector.Reset();
        Debug.Log("New Analytics Session Started.");
    }

    public void EndSession(bool wasAbrupt)
    {
        if (currentSession == null) return;

        currentSession.EndSession(wasAbrupt);
        trendAnalyzer.ProcessSession(currentSession);
        frustrationDetector.ProcessSession(currentSession);

        float frustrationScore = frustrationDetector.GetFrustrationScore();
        if (frustrationScore >.75f) // High frustration threshold
        {
            AdjustDifficulty(); // Placeholder for adaptiveDifficultyManager.ApplyFrustrationPenalty(frustrationScore);
        }

        Debug.Log($"Session Ended. Abrupt: {wasAbrupt}. Data:\n{JsonUtility.ToJson(currentSession, true)}");
    }

    public void TrackDeath(string cause, float distance = 0f)
    {
        if (currentSession == null) return;
        currentSession.RecordDeath(cause, distance);
        frustrationDetector.TrackDeath();
    }

    public void TrackDodge(bool success)
    {
        if (currentSession == null) return;
        currentSession.RecordDodge(success);
    }

    public void TrackCombo(int peak)
    {
        if (currentSession == null) return;
        currentSession.RecordCombo(peak);
    }

    public void TrackRevive()
    {
        if (currentSession == null) return;
        currentSession.RecordRevive();
        frustrationDetector.TrackRevive();
    }
    
    public void TrackBossEncounter(string bossName, bool survived)
    {
        if (currentSession == null) return;
        currentSession.RecordBossEncounter(bossName, survived);
    }

    // PowerUpManager Logic
    private List<PowerUpDefinition> activePowerUps = new List<PowerUpDefinition>();
    private PowerUpEffectsManager effectsManager;

    public void ActivatePowerUp(PowerUpDefinition powerUp)
    {
        powerUp.Activate();
        if (!activePowerUps.Contains(powerUp))
        {
            activePowerUps.Add(powerUp);
        }
        
        GameEvents.TriggerPowerUpActivated(powerUp);
        if (effectsManager != null)
        {
            effectsManager.PlayEffect(powerUp.type);
        }
        Debug.Log($"POWERUP_MANAGER: {powerUp.type} activated!");
    }

    private void DeactivatePowerUp(PowerUpDefinition powerUp)
    {
        activePowerUps.Remove(powerUp);
        GameEvents.TriggerPowerUpDeactivated(powerUp);
        if (effectsManager != null)
        {
            effectsManager.StopEffect(powerUp.type);
        }
        Debug.Log($"POWERUP_MANAGER: {powerUp.type} deactivated!");
    }
}
