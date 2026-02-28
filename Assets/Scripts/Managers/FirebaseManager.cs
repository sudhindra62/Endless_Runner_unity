using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    private bool isInitialized = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
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

                if (!BuildSettings.Instance.IsProductionBuild)
                    Debug.Log("Firebase initialized successfully.");
                isInitialized = true;
            }
            else
            {
                if (!BuildSettings.Instance.IsProductionBuild)
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
                if (!BuildSettings.Instance.IsProductionBuild)
                    Debug.Log("Remote Config defaults set.");
                FetchRemoteConfig();
            });
    }

    public Task FetchRemoteConfig()
    {
        if (!BuildSettings.Instance.IsProductionBuild)
            Debug.Log("Fetching Remote Config data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (!BuildSettings.Instance.IsProductionBuild)
                    Debug.Log("Fetch completed.");
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            }
        });
    }
}
