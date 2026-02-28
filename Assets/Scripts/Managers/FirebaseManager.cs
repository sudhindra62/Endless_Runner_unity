using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static event Action OnRemoteConfigFetchedAndActivated;

    private bool isInitialized = false;
    private BuildSettings buildSettings;

    void Awake()
    {
        ServiceLocator.Register<FirebaseManager>(this);
    }

    void Start()
    {
        buildSettings = ServiceLocator.Get<BuildSettings>();
        if (buildSettings == null)
        {
            Debug.LogError("BuildSettings not found in ServiceLocator. FirebaseManager requires it for initialization.");
            return;
        }

        Initialize();
    }

    void OnDestroy()
    {
        ServiceLocator.Unregister<FirebaseManager>();
    }

    private void Initialize()
    {
        if (isInitialized) return;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                InitializeRemoteConfig();

                if (!buildSettings.IsProductionBuild)
                    Debug.Log("Firebase initialized successfully.");
                isInitialized = true;
            }
            else
            {
                if (!buildSettings.IsProductionBuild)
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    private void InitializeRemoteConfig()
    {
        var defaults = new System.Collections.Generic.Dictionary<string, object>();
        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                if (!buildSettings.IsProductionBuild)
                    Debug.Log("Remote Config defaults set.");
                FetchRemoteConfig();
            });
    }

    public Task FetchRemoteConfig()
    {
        if (!buildSettings.IsProductionBuild)
            Debug.Log("Fetching Remote Config data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (!buildSettings.IsProductionBuild)
                    Debug.Log("Fetch completed.");
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activationTask =>
                {
                    if (activationTask.IsCompleted)
                    {
                        if (!buildSettings.IsProductionBuild)
                            Debug.Log("Remote Config activated.");
                        OnRemoteConfigFetchedAndActivated?.Invoke();
                    }
                });
            }
        });
    }
}
