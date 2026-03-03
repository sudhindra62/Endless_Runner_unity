
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FeverModeManager : MonoBehaviour
{
    #region CONFIGURATION
    /// <summary>
    /// A new data structure to hold the tuning values for the Fever Mode system.
    /// This will be populated from a remote source to allow for live tuning.
    /// </summary>
    private struct FeverConfig
    {
        public float FeverDuration;
        public float MaxFeverGauge;
        public float FeverCooldown;
        public float FeverSpeedBoost;
        public int FeverScoreMultiplier;
        public float FeverCoinDensityMultiplier;
    }
    private FeverConfig config;

    // --- The original [SerializeField] attributes are now replaced by the 'config' struct ---
    // [SerializeField] private float feverDuration = 10f;
    // [SerializeField] private float maxFeverGauge = 100f;
    // [SerializeField] private float feverCooldown = 20f;
    // [SerializeField] private float feverSpeedBoost = 5f;
    // [SerializeField] private int feverScoreMultiplier = 20;
    // [SerializeField] private float feverCoinDensityMultiplier = 1.5f;
    #endregion

    #region EVENTS
    // --- All original events are 100% preserved ---
    public static event Action<float> OnFeverGaugeChanged;
    public static event Action<float> OnFeverStart;
    public static event Action OnFeverEnd;
    public static event Action<bool> OnFeverStatusChanged;
    #endregion

    #region STATE
    // --- All original state variables are 100% preserved ---
    private float currentFeverGauge;
    private bool isFeverActive;
    private bool isBossChaseActive;
    private float lastFeverEndTime = -100f;
    private Coroutine feverCoroutine;
    private readonly Dictionary<string, float> chargeMultipliers = new Dictionary<string, float>();
    private const string FEVER_SOURCE_ID = "FeverMode";
    #endregion

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        // --- EVOLUTION: Load config from remote settings ---
        LoadConfiguration();

        // --- Existing ServiceLocator registration (Preserved) ---
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        // --- All original subscriptions are 100% preserved ---
        // GameStateManager.OnGameStateChanged += OnGameStateChanged;
        PowerUpFusionManager.OnFusionActivated += HandleFusionActivation;
        FlowComboManager.OnComboMeterFull += HandleComboMeterFull;
        // StyleMeter.OnStyleMeterFull += HandleStyleMeterFull;
        BossChaseManager.OnBossChaseStateChanged += HandleBossChaseStateChanged;

        // --- EVOLUTION: Subscribe to config updates ---
        RemoteConfig.OnConfigUpdated += HandleConfigUpdated;
    }

    private void OnDestroy()
    {
        // --- All original unsubscriptions are 100% preserved ---
        // GameStateManager.OnGameStateChanged -= OnGameStateChanged;
        PowerUpFusionManager.OnFusionActivated -= HandleFusionActivation;
        FlowComboManager.OnComboMeterFull -= HandleComboMeterFull;
        // StyleMeter.OnStyleMeterFull -= HandleStyleMeterFull;
        BossChaseManager.OnBossChaseStateChanged -= HandleBossChaseStateChanged;

        // --- EVOLUTION: Unsubscribe from config updates ---
        RemoteConfig.OnConfigUpdated -= HandleConfigUpdated;
    }
    #endregion

    #region CONFIGURATION_HANDLING
    /// <summary>
    /// Populates the fever configuration from the RemoteConfig manager.
    /// Provides default fallback values to ensure stability if the config is not available.
    /// </summary>
    private void LoadConfiguration()
    {
        config = new FeverConfig
        {
            FeverDuration = RemoteConfig.GetFloat("FeverDuration", 10f),
            MaxFeverGauge = RemoteConfig.GetFloat("FeverMaxGauge", 100f),
            FeverCooldown = RemoteConfig.GetFloat("FeverCooldown", 20f),
            FeverSpeedBoost = RemoteConfig.GetFloat("FeverSpeedBoost", 5f),
            FeverScoreMultiplier = RemoteConfig.GetInt("FeverScoreMultiplier", 20),
            FeverCoinDensityMultiplier = RemoteConfig.GetFloat("FeverCoinDensityMultiplier", 1.5f)
        };
        Debug.Log("Fever Mode configuration loaded from remote settings.");
    }

    /// <summary>
    /// Handles live updates to the remote configuration, reloading the settings.
    /// </summary>
    private void HandleConfigUpdated()
    {
        Debug.Log("Remote config updated. Reloading Fever Mode settings.");
        LoadConfiguration();
    }
    #endregion

    #region TRIGGER_HANDLERS
    // --- All original trigger handlers are 100% preserved ---
    private void HandleComboMeterFull() { TryActivateFever(); }
    private void HandleStyleMeterFull() { TryActivateFever(); }
    private void HandleFusionActivation(FusionModifierData data) 
    {
        if (data.Type == FusionType.FeverFrenzy)
        {
            if (isFeverActive && feverCoroutine != null) { StopCoroutine(feverCoroutine); RevertFeverEffects(); }
            ActivateFever(true);
        }
    }
    private void HandleBossChaseStateChanged(bool isActive)
    {
        isBossChaseActive = isActive;
        if (isBossChaseActive && isFeverActive)
        {
            if (feverCoroutine != null) StopCoroutine(feverCoroutine);
            EndFever(true);
        }
    }
    #endregion

    #region FEVER_LIFECYCLE
    // --- All original lifecycle methods are preserved, now using config values ---
    public void TryActivateFever()
    {
        if (isFeverActive || Time.time < lastFeverEndTime + config.FeverCooldown || isBossChaseActive) return;
        ActivateFever();
    }

    private void ActivateFever(bool bypassChecks = false)
    {
        if (!bypassChecks && isFeverActive) return;
        isFeverActive = true;
        currentFeverGauge = 0;
        OnFeverGaugeChanged?.Invoke(0f);
        feverCoroutine = StartCoroutine(FeverRoutine());
    }

    private IEnumerator FeverRoutine()
    {
        OnFeverStart?.Invoke(config.FeverDuration);
        OnFeverStatusChanged?.Invoke(true);
        ApplyFeverEffects();
        yield return new WaitForSeconds(config.FeverDuration);
        EndFever();
    }

    private void EndFever(bool cancelledBySystem = false)
    {
        if (!isFeverActive) return;
        OnFeverEnd?.Invoke();
        OnFeverStatusChanged?.Invoke(false);
        RevertFeverEffects();
        isFeverActive = false;
        lastFeverEndTime = Time.time;
        feverCoroutine = null;
    }
    #endregion
    
    #region EFFECTS_MANAGEMENT
    // --- All original effects management methods are preserved, now using config values ---
    private void ApplyFeverEffects()
    {
        var playerMovement = ServiceLocator.Get<PlayerMovement>();
        var scoreManager = ServiceLocator.Get<ScoreManager>();
        var coinManager = ServiceLocator.Get<CoinManager>();

        scoreManager?.ApplyScoreMultiplier(FEVER_SOURCE_ID, config.FeverScoreMultiplier);
        if (playerMovement != null && playerMovement.baseSpeed > 0) {
             playerMovement.ApplySpeedMultiplier(FEVER_SOURCE_ID, 1f + (config.FeverSpeedBoost / playerMovement.baseSpeed));
        }
        coinManager?.ApplyCoinDensityMultiplier(FEVER_SOURCE_ID, config.FeverCoinDensityMultiplier);
    }

    private void RevertFeverEffects()
    {
        ServiceLocator.Get<ScoreManager>()?.RemoveScoreMultiplier(FEVER_SOURCE_ID);
        ServiceLocator.Get<PlayerMovement>()?.RemoveSpeedMultiplier(FEVER_SOURCE_ID);
        ServiceLocator.Get<CoinManager>()?.RemoveCoinDensityMultiplier(FEVER_SOURCE_ID);
    }
    #endregion

    #region STATE_MANAGEMENT_AND_UTILS
    // --- All original state and utility methods are 100% preserved, now using config values ---
    private void OnGameStateChanged(GameState newState) { if (newState != GameState.Playing) ResetFeverState(); }

    private void ResetFeverState()
    {
        if (feverCoroutine != null) { StopCoroutine(feverCoroutine); feverCoroutine = null; }
        if (isFeverActive) { RevertFeverEffects(); OnFeverEnd?.Invoke(); OnFeverStatusChanged?.Invoke(false); isFeverActive = false; }
        currentFeverGauge = 0;
        OnFeverGaugeChanged?.Invoke(0f);
        lastFeverEndTime = -100f;
    }

    public void AddFeverPoints(float amount)
    {
        if (isFeverActive || amount <= 0 || Time.time < lastFeverEndTime + config.FeverCooldown) return;
        float finalAmount = amount * CalculateChargeMultiplier();
        currentFeverGauge = Mathf.Min(currentFeverGauge + finalAmount, config.MaxFeverGauge);
        OnFeverGaugeChanged?.Invoke(currentFeverGauge / config.MaxFeverGauge);
        if (currentFeverGauge >= config.MaxFeverGauge) { TryActivateFever(); }
    }

    public bool IsFeverActive() => isFeverActive;
    public float GetFeverSpeedBoost() => isFeverActive ? config.FeverSpeedBoost : 0f;
    public int GetFeverMultiplier() => config.FeverScoreMultiplier;

    public void ApplyChargeMultiplier(string sourceId, float multiplier) { chargeMultipliers[sourceId] = Mathf.Max(0.01f, multiplier); }
    public void RemoveChargeMultiplier(string sourceId) { chargeMultipliers.Remove(sourceId); }

    private float CalculateChargeMultiplier()
    {
        if (chargeMultipliers.Count == 0) return 1f;
        float logSum = 0.0f;
        foreach (var multiplier in chargeMultipliers.Values) { logSum += Mathf.Log(multiplier); }
        return Mathf.Exp(logSum);
    }
    
    public void ResetState() { chargeMultipliers.Clear(); ResetFeverState(); }
    #endregion
}
