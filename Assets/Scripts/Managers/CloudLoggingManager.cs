
using UnityEngine;

public class CloudLoggingManager : MonoBehaviour
{
    public static CloudLoggingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += HandleLog;
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
            Application.logMessageReceived -= HandleLog;
        }
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            // In a real-world scenario, you would send this to a cloud service (e.g., Firebase, Sentry)
            Debug.Log($"CLOUD LOGGING: Error: {logString}\nStackTrace: {stackTrace}");
        }
    }

    public void LogCustomEvent(string eventName, string eventData)
    {
        // Log custom game events for analytics
        Debug.Log($"CLOUD LOGGING: Custom Event: {eventName} - Data: {eventData}");
    }
}
