
using UnityEngine;

/// <summary>
/// A service that holds all critical build and readiness settings for the game.
/// This provides a centralized and easily accessible point for checking whether the game 
/// is in a production state, and for enabling or disabling major features like IAP and ads.
/// </summary>
public class BuildSettings : MonoBehaviour
{
    public static BuildSettings Instance { get; private set; }

    [Header("Production Readiness Flags")]
    [Tooltip("Set to true for release builds. This typically disables debug cheats, verbose logging, and other development features.")]
    [SerializeField] private bool isProductionBuild = false;

    [Tooltip("A master switch to enable or disable all in-app purchase functionality.")]
    [SerializeField] private bool iapEnabled = true;

    [Tooltip("A master switch to enable or disable all ad functionality.")]
    [SerializeField] private bool adsEnabled = true;

    /// <summary>
    /// True if the game is configured for a release build.
    /// </summary>
    public bool IsProductionBuild => isProductionBuild;

    /// <summary>
    /// True if In-App Purchases are globally enabled.
    /// </summary>
    public bool IapEnabled => iapEnabled;

    /// <summary>
    /// True if Ads are globally enabled.
    /// </summary>
    public bool AdsEnabled => adsEnabled;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<BuildSettings>(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            ServiceLocator.Unregister<BuildSettings>();
            Instance = null;
        }
    }
}
