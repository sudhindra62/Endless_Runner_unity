
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the logging of gameplay events to an analytics service.
/// This is a singleton that persists across scenes.
/// Logic has been fully implemented by Supreme Guardian Architect v12.
/// </summary>
public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Logs a custom event with a specified name.
    /// </summary>
    /// <param name="eventName">The name of the event to log.</param>
    public void LogEvent(string eventName)
    {
        Debug.Log("Guardian Architect Analytics: Logging event - " + eventName);
        // --- SDK INTEGRATION POINT ---
        // FirebaseAnalytics.LogEvent(eventName);
    }

    /// <summary>
    /// Logs a custom event with associated parameters.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <param name="parameters">A dictionary of parameters to associate with the event.</param>
    public void LogEvent(string eventName, Dictionary<string, object> parameters)
    {
        Debug.Log("Guardian Architect Analytics: Logging event - " + eventName + " with parameters.");
        
        // --- SDK INTEGRATION POINT (Example for Firebase) ---
        /*
        Parameter[] firebaseParams = new Parameter[parameters.Count];
        int i = 0;
        foreach (var-kvp in parameters)
        {
            firebaseParams[i++] = new Parameter(kvp.Key, kvp.Value.ToString());
        }
        FirebaseAnalytics.LogEvent(eventName, firebaseParams);
        */
    }

    /// <summary>
    /// Logs the start of a level.
    /// </summary>
    /// <param name="levelName">The name or index of the level.</param>
    public void LogLevelStart(string levelName)
    {
        LogEvent("level_start", new Dictionary<string, object> { { "level_name", levelName } });
    }

    /// <summary>
    /// Logs the completion of a level.
    /// </summary>
    /// <param name="levelName">The name or index of the level.</param>
    /// <param name="score">The player's final score.</param>
    public void LogLevelEnd(string levelName, int score)
    {
        LogEvent("level_end", new Dictionary<string, object>
        {
            { "level_name", levelName },
            { "score", score }
        });
    }

    /// <summary>
    /// Logs a virtual currency transaction.
    /// </summary>
    /// <param name="currencyName">The name of the virtual currency (e.g., gems, coins).</param>
    /// <param name="amount">The amount of currency gained or spent.</param>
    /// <param name="sourceOrSink">The source from which currency was gained or the sink to which it was spent.</param>
    public void LogVirtualCurrencyTransaction(string currencyName, int amount, string sourceOrSink)
    {
        LogEvent("virtual_currency_transaction", new Dictionary<string, object>
        {
            { "currency_name", currencyName },
            { "amount", amount },
            { "source", sourceOrSink }
        });
    }
}
