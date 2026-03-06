
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// The sole authority for providing runtime-adjusted configuration values.
/// It fetches, validates, and caches configs, providing safe access to other managers.
/// Adheres to IRONCLAD rules: NO direct mutation, ONLY applies reversible modifiers.
/// </summary>
public class LiveOpsManager : Singleton<LiveOpsManager>
{
    [Header("Configuration Profiles")]
    [SerializeField]
    [Tooltip("A baked-in, guaranteed-safe config profile to use as a last resort.")]
    private LiveOpsConfigProfile _safeDefaultProfile;

    private LiveOpsConfigProfile _activeProfile;
    private LiveOpsConfigProfile _cachedProfile;

    // The conceptual modifier pipeline is managed here
    // In a more complex system, this would be its own class instance.
    public float PowerUpDurationMultiplier { get; private set; } = 1.0f;
    public float DifficultyMultiplier { get; private set; } = 1.0f;
    public float DropRateMultiplier { get; private set; } = 1.0f;
    public float RiskLaneRewardMultiplier { get; private set; } = 1.0f;
    public int ReviveGemCost { get; private set; } = 10;
    public bool IsEventActive { get; private set; } = false;

    public static event System.Action OnLiveOpsConfigUpdated;

    protected override void Awake()
    {
        base.Awake();

        // Initialize with safe defaults to prevent null issues on startup
        ApplyConfigProfile(_safeDefaultProfile);
    }

    private async void Start()
    {
        // This mimics a RemoteConfigBridge call
        await FetchAndApplyRemoteConfig();
    }

    /// <summary>
    /// The primary entry point for fetching and applying a new LiveOps configuration.
    /// Implements the full Failsafe System (Remote -> Cached -> Default).
    /// </summary>
    public async Task FetchAndApplyRemoteConfig()
    {
        LiveOpsConfigProfile fetchedProfile = null;

        // STEP 1: Attempt to fetch from remote source
        if (RemoteConfigBridge.Instance != null)
        {
             // In a real scenario, this would involve JSON deserialization.
             // For this simulation, we'll assume the bridge returns a ScriptableObject.
            fetchedProfile = await RemoteConfigBridge.Instance.FetchLatestConfig();
        }

        // STEP 2: Validate and Apply
        if (fetchedProfile != null)
        {
            _activeProfile = LiveOpsSafetyValidator.ValidateAndClamp(fetchedProfile, _safeDefaultProfile);
            _cachedProfile = _activeProfile; // Cache the newly validated config
            Debug.Log("<color=green>Applied fresh LiveOps config from remote source.</color>");
        }
        // STEP 3: Fallback to Cached
        else if (_cachedProfile != null)
        {
            _activeProfile = _cachedProfile;
            Debug.LogWarning("LiveOps: Failed to fetch remote config. Applying last known good config from cache.");
        }
        // STEP 4: Fallback to Safe Default
        else
        {
            _activeProfile = _safeDefaultProfile;
            Debug.LogError("LiveOps CRITICAL: Failed to fetch and no cache available. Applying internal safe defaults.");
        }

        ApplyConfigProfile(_activeProfile);
    }

    /// <summary>
    /// Applies the values from a given profile to the manager's public properties.
    /// </summary>
    private void ApplyConfigProfile(LiveOpsConfigProfile profile)
    { 
        if (profile == null) return;

        // This is where the "modifier pipeline" is conceptually applied.
        // The properties are updated, and other systems will read these new values.
        DifficultyMultiplier = profile.difficultyMultiplier;
        PowerUpDurationMultiplier = profile.powerUpDurationMultiplier;
        DropRateMultiplier = profile.dropRateMultiplier;
        RiskLaneRewardMultiplier = profile.riskLaneRewardMultiplier;
        ReviveGemCost = profile.reviveGemCost;
        IsEventActive = profile.isEventActive;
        // ... and so on for all other properties

        // Broadcast that the configuration has been updated
        OnLiveOpsConfigUpdated?.Invoke();
    }

    // Other managers will call these public properties. They do not get to set them.
    // Example: float finalDuration = baseDuration * LiveOpsManager.Instance.PowerUpDurationMultiplier;
}
