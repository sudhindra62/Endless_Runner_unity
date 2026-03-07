
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Provides a centralized service for logging events, warnings, and errors to a simulated cloud service.
/// Fulfills the ERROR_HANDLING_POLICY.md mandate.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v2.
/// </summary>
public class CloudLoggingManager : Singleton<CloudLoggingManager>
{
    // In a real scenario, this would be a REST API endpoint.
    private const string CLOUD_ENDPOINT_URL = "https://my-game-analytics-backend.com/logs";

    private bool isInitialized = false;
    private string sessionId;
    private string deviceId;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public void Initialize()
    {
        if (isInitialized) return;

        // Generate a unique session ID for this game launch.
        sessionId = System.Guid.NewGuid().ToString();
        deviceId = SystemInfo.deviceUniqueIdentifier;
        isInitialized = true;
        
        LogEvent("GameSession_Start", new Dictionary<string, object> 
        {
            { "platform", Application.platform.ToString() },
            { "gameVersion", Application.version }
        });

        Debug.Log($"Cloud Logging Initialized. Session ID: {sessionId}");
    }

    /// <summary>
    /// Logs a standard gameplay or system event.
    /// </summary>
    /// <param name="eventName">The name of the event (e.g., 'Level_Start', 'Ad_Watched').</param>
    /// <param name="parameters">A dictionary of associated data (e.g., { "levelName", "Level1" }).</param>
    public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
    {
        if (!isInitialized) return;
        
        var logData = new Dictionary<string, object>
        {
            { "eventName", eventName },
            { "sessionId", sessionId },
            { "deviceId", deviceId },
            { "timestamp", System.DateTime.UtcNow.ToString("o") },
            { "logType", "Event" },
            { "data", parameters }
        };
        
        SendToCloud(logData);
    }

    /// <summary>
    /// Logs a non-critical warning that does not disrupt gameplay.
    /// </summary>
    /// <param name="warningMessage">The warning message (e.g., "Failed to load optional asset").</param>
    /// <param name="context">Optional context, like the method or system where the warning occurred.</param>
    public void LogWarning(string warningMessage, string context = null)
    {
        if (!isInitialized) return;

        var logData = new Dictionary<string, object>
        {
            { "message", warningMessage },
            { "context", context },
            { "sessionId", sessionId },
            { "deviceId", deviceId },
            { "timestamp", System.DateTime.UtcNow.ToString("o") },
            { "logType", "Warning" },
        };

        SendToCloud(logData);
        Debug.LogWarning($"[Cloud Warning | Context: {context ?? "N/A"}] {warningMessage}");
    }

    /// <summary>
    /// Logs a critical, game-disrupting error, such as a caught exception.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The stack trace from the exception object.</param>
    public void LogError(string errorMessage, string stackTrace = null)
    {
        if (!isInitialized) return;

        var logData = new Dictionary<string, object>
        {
            { "message", errorMessage },
            { "stackTrace", stackTrace },
            { "sessionId", sessionId },
            { "deviceId", deviceId },
            { "timestamp", System.DateTime.UtcNow.ToString("o") },
            { "logType", "Error" },
        };

        SendToCloud(logData);
        Debug.LogError($"[Cloud Error] {errorMessage}\nStack Trace:\n{stackTrace}");
    }

    private void SendToCloud(Dictionary<string, object> logData)
    {
        // In a real implementation, you would use UnityWebRequest to POST this data.
        // For this reconstruction, we simulate the log being sent.
        // A real implementation would also require a robust JSON serialization library.
        string jsonPayload = "{ \"log_data\": \"...serialized_payload...\" }"; // Placeholder for serialized logData
        Debug.Log($"<color=cyan>[Cloud Log Sent]</color> to endpoint {CLOUD_ENDPOINT_URL}. Payload: {jsonPayload}");

        // Example of a real Coroutine-based request:
        // StartCoroutine(PostRequest(CLOUD_ENDPOINT_URL, jsonPayload));
    }
}
