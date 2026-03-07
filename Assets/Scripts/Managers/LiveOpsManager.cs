
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

    // Public properties that other managers read from.
    public float PowerUpDurationMultiplier { get; private set; } = 1.0f;
    public float DifficultyMultiplier { get; private set; } = 1.0f;
    public float DropRateMultiplier { get; private set; } = 1.0f;
    public float RiskLaneRewardMultiplier { get; private set; } = 1.0f;
    public int ReviveGemCost { get; private set; } = 10;
    public float AdFrequencyMultiplier { get; private set; } = 1.0f; // ◈ ARCHITECT_OMEGA INTEGRATION
    public bool IsEventActive { get; private set; } = false;

    public static event System.Action OnLiveOpsConfigUpdated;

    protected override void Awake()
    {
        base.Awake();
        ApplyConfigProfile(_safeDefaultProfile);
    }

    private async void Start()
    {
        await FetchAndApplyRemoteConfig();
    }

    /// <summary>
    /// The primary entry point for fetching and applying a new LiveOps configuration.
    /// </summary>
    public async Task FetchAndApplyRemoteConfig()
    {
        LiveOpsConfigProfile fetchedProfile = null;

        if (RemoteConfigBridge.Instance != null)
        {
            fetchedProfile = await RemoteConfigBridge.Instance.FetchLatestConfig();
        }

        if (fetchedProfile != null)
        {
            _activeProfile = LiveOpsSafetyValidator.ValidateAndClamp(fetchedProfile, _safeDefaultProfile);
            _cachedProfile = _active_profile;
            Debug.Log("<color=green>Applied fresh LiveOps config from remote source.</color>");
        }
        else if (_cachedProfile != null)
        {
            _activeProfile = _cachedProfile;
            Debug.LogWarning("LiveOps: Failed to fetch remote config. Applying last known good config from cache.");
        }
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

        DifficultyMultiplier = profile.difficultyMultiplier;
        PowerUpDurationMultiplier = profile.powerUpDurationMultiplier;
        DropRateMultiplier = profile.dropRateMultiplier;
        RiskLaneRewardMultiplier = profile.riskLaneRewardMultiplier;
        ReviveGemCost = profile.reviveGemCost;
        AdFrequencyMultiplier = profile.adFrequencyMultiplier; // ◈ ARCHITECT_OMEGA INTEGRATION
        IsEventActive = profile.isEventActive;

        OnLiveOpsConfigUpdated?.Invoke();
    }
}
