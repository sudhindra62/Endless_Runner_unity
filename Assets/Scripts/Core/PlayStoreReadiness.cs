using UnityEngine;

/// <summary>
/// Provides a single point of reference for checks and configurations required for a production-ready build.
/// This script is intended to be used by other managers to query the readiness status.
/// </summary>
public partial class PlayStoreReadiness : MonoBehaviour
{
    public static PlayStoreReadiness Instance { get; private set; }

    [Header("Production Readiness Flags")]
    [Tooltip("Set to true for release builds. Disables debug cheats and verbose logging.")]
    [SerializeField] private bool isProductionBuild = false;

    [Tooltip("Master switch for enabling or disabling all IAP functionality.")]
 [SerializeField] private bool _iapEnabled = true;

     [Tooltip("Master switch for enabling or disabling all ad functionality.")]

[SerializeField] private bool _adsEnabled = true;

    // 🔹 EXISTING PROPERTIES (UNCHANGED)
    public bool IsProductionBuild => isProductionBuild;
    public bool IsIapEnabled => _iapEnabled;
    public bool IsAdsEnabled => _adsEnabled;

    // 🔹 ADDITIVE FIELD ALIASES — REQUIRED FOR LEGACY ACCESS
    // No logic change, no duplication of state
    public bool iapEnabled_Public =>iapEnabled;
    public bool adsEnabled_Public => adsEnabled;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LogReadinessStatus();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LogReadinessStatus()
    {
        Debug.Log("--- Play Store Readiness Status ---");
        Debug.LogFormat("[PlayStoreReadiness] Production Build: {0}", isProductionBuild);
        Debug.LogFormat("[PlayStoreReadiness] IAP Enabled: {0}", iapEnabled);
        Debug.LogFormat("[PlayStoreReadiness] Ads Enabled: {0}", adsEnabled);

        if (!isProductionBuild)
        {
            Debug.LogWarning(
                "[PlayStoreReadiness] Game is currently in a DEBUG configuration. " +
                "Ensure 'isProductionBuild' is checked before publishing."
            );
        }
    }
}
