
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance { get; private set; }

    [Header("Unity Ads Game IDs")]
    [SerializeField] private string androidGameId = "4494341"; // Example ID
    [SerializeField] private string iosGameId = "4494340"; // Example ID

    [Header("Ad Unit IDs")]
    [SerializeField] private string androidInterstitialId = "Interstitial_Android";
    [SerializeField] private string iosInterstitialId = "Interstitial_iOS";
    [SerializeField] private string androidRewardedId = "Rewarded_Android";
    [SerializeField] private string iosRewardedId = "Rewarded_iOS";

    private string gameId;
    private string interstitialId;
    private string rewardedId;
    private int gamesPlayedSinceAd = 0;
    [SerializeField] private int gamesPerInterstitial = 3;

    private Action currentRewardCallback;

    public static event Action OnReviveAdCompleted;

    private void Awake()
    {
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
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iosGameId
            : androidGameId;

        interstitialId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iosInterstitialId
            : androidInterstitialId;

        rewardedId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iosRewardedId
            : androidRewardedId;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, true, this);
        }
    }

    public void ShowInterstitialAdAfterRun()
    {
        gamesPlayedSinceAd++;
        if (gamesPlayedSinceAd >= gamesPerInterstitial)
        {
            if (Advertisement.isInitialized)
            {
                Advertisement.Show(interstitialId, this);
                gamesPlayedSinceAd = 0;
            }
        }
    }

    public void ShowRewardedAd(Action onComplete)
    {
        if (Advertisement.isInitialized)
        {
            currentRewardCallback = onComplete;
            Advertisement.Show(rewardedId, this);
        }
    }

    public void ShowReviveAd()
    {
        ShowRewardedAd(() => OnReviveAdCompleted?.Invoke());
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        Advertisement.Load(interstitialId, this);
        Advertisement.Load(rewardedId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId) {}

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) {}

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) {}

    public void OnUnityAdsShowStart(string adUnitId) {}

    public void OnUnityAdsShowClick(string adUnitId) {}

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(rewardedId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            currentRewardCallback?.Invoke();
        }
        currentRewardCallback = null;

        // Reload the ad after it has been shown
        Advertisement.Load(adUnitId, this);
    }
}
