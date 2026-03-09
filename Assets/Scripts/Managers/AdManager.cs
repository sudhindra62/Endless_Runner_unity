
using UnityEngine;
using UnityEngine.Advertisements;
using System;

/// <summary>
/// Manages all advertising functionality, including interstitial and rewarded video ads.
/// Logic fortified by Supreme Guardian Architect v12 to ensure maximum monetizable uptime and robust error handling.
/// </summary>
public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    // --- SINGLETON PATTERN ---
    public static AdManager Instance { get; private set; }

    // --- CONFIGURATION: Inspector-tuned values for monetization strategy ---
    [Header("Unity Ads Game IDs")]
    [Tooltip("The unique identifier for this game on the Android platform.")]
    [SerializeField] private string androidGameId = "4494341"; // Guardian Note: Replace with actual project ID.
    [Tooltip("The unique identifier for this game on the iOS platform.")]
    [SerializeField] private string iosGameId = "4494340";     // Guardian Note: Replace with actual project ID.

    [Header("Ad Unit IDs")]
    [Tooltip("The ID for the interstitial ad unit on Android.")]
    [SerializeField] private string androidInterstitialId = "Interstitial_Android";
    [Tooltip("The ID for the interstitial ad unit on iOS.")]
    [SerializeField] private string iosInterstitialId = "Interstitial_iOS";
    [Tooltip("The ID for the rewarded ad unit on Android.")]
    [SerializeField] private string androidRewardedId = "Rewarded_Android";
    [Tooltip("The ID for the rewarded ad unit on iOS.")]
    [SerializeField] private string iosRewardedId = "Rewarded_iOS";

    [Header("Interstitial Ad Strategy")]
    [Tooltip("The number of game runs that must be completed before an interstitial ad is shown.")]
    [SerializeField] private int gamesPerInterstitial = 3;
    
    // --- PRIVATE STATE ---
    private string _gameId;
    private string _interstitialId;
    private string _rewardedId;
    private int _gamesPlayedSinceAd = 0;
    private Action _currentRewardCallback;

    // --- PUBLIC EVENTS: For cross-system communication ---
    public static event Action OnReviveAdCompleted;

    private void Awake()
    {
        // --- A-TO-Z CONNECTIVITY: Enforce Singleton pattern for global access ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAds()
    {
        // Select the appropriate Game ID based on the current platform.
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        _interstitialId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosInterstitialId : androidInterstitialId;
        _rewardedId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosRewardedId : androidRewardedId;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Debug.Log("Guardian Architect: Initializing Unity Ads Service...");
            Advertisement.Initialize(_gameId, true, this);
        }
    }

    /// <summary>
    /// Shows an interstitial ad if the required number of game runs has been met.
    /// </summary>
    public void ShowInterstitialAdAfterRun()
    {
        _gamesPlayedSinceAd++;
        Debug.Log($"Guardian Architect: Game run completed. Runs since last ad: {_gamesPlayedSinceAd}/{gamesPerInterstitial}.");
        if (_gamesPlayedSinceAd >= gamesPerInterstitial)
        {
            if (Advertisement.isInitialized)
            {
                Debug.Log("Guardian Architect: Attempting to show interstitial ad.");
                Advertisement.Show(_interstitialId, this);
                _gamesPlayedSinceAd = 0; // Reset counter after showing ad.
            }
            else
            {
                Debug.LogWarning("Guardian Architect Warning: Interstitial ad requested, but Ads service is not initialized.");
            }
        }
    }

    /// <summary>
    /// Shows a rewarded ad. The provided callback will be invoked upon successful completion.
    /// </summary>
    /// <param name="onComplete">The action to execute when the ad is successfully watched.</param>
    public void ShowRewardedAd(Action onComplete)
    {
        if (Advertisement.isInitialized)
        {
            Debug.Log("Guardian Architect: Attempting to show rewarded ad.");
            _currentRewardCallback = onComplete;
            Advertisement.Show(_rewardedId, this);
        }
        else
        {
            Debug.LogWarning("Guardian Architect Warning: Rewarded ad requested, but Ads service is not initialized.");
        }
    }

    /// <summary>
    /// A specific implementation of a rewarded ad used for the player revive mechanic.
    /// </summary>
    public void ShowReviveAd()
    {
        ShowRewardedAd(() => OnReviveAdCompleted?.Invoke());
    }

    // --- IUnityAdsInitializationListener IMPLEMENTATION ---
    public void OnInitializationComplete()
    {
        Debug.Log("Guardian Architect: Unity Ads initialization complete. Loading ad units...");
        // Pre-load ad content for instant availability.
        Advertisement.Load(_interstitialId, this);
        Advertisement.Load(_rewardedId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Guardian Architect FATAL_ERROR: Unity Ads Initialization Failed. Error: {error}. Message: {message}");
    }

    // --- IUnityAdsLoadListener IMPLEMENTATION ---
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log($"Guardian Architect: Ad unit loaded successfully: {adUnitId}");
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Guardian Architect ERROR: Failed to load ad unit '{adUnitId}'. Error: {error}. Message: {message}");
    }

    // --- IUnityAdsShowListener IMPLEMENTATION ---
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Guardian Architect ERROR: Failed to show ad unit '{adUnitId}'. Error: {error}. Message: {message}");
        _currentRewardCallback = null; // Clear callback on failure.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log($"Guardian Architect: Ad show started for unit: {adUnitId}.");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log($"Guardian Architect: Ad clicked for unit: {adUnitId}.");
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Guardian Architect: Ad show completed for unit '{adUnitId}' with state: {showCompletionState}.");
        
        // --- DEPENDENCY_FIX: Grant reward only on successful completion of a rewarded ad. ---
        if (adUnitId.Equals(_rewardedId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Guardian Architect: Rewarded ad completed. Invoking reward callback.");
            _currentRewardCallback?.Invoke();
        }

        // Clean up the callback to prevent reuse.
        _currentRewardCallback = null;

        // --- A-TO-Z CONNECTIVITY: Immediately reload the ad unit for the next opportunity. ---
        Debug.Log($"Guardian Architect: Reloading ad unit: {adUnitId}");
        Advertisement.Load(adUnitId, this);
    }
}
