
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A bridge for fetching and applying remote configurations.
/// </summary>
public class RemoteConfigBridge : MonoBehaviour
{
    private Dictionary<string, object> configCache = new Dictionary<string, object>();

    /// <summary>
    /// Fetches the latest configuration from a remote source.
    /// </summary>
    public void FetchConfig()
    {
        // In a real implementation, this would involve a web request to a service like Firebase Remote Config.
        // For this example, we'll simulate fetching a new configuration.
        configCache["PlayerSpeed"] = 12.5f;
        configCache["CoinValue"] = 2;

        Debug.Log("Remote configuration fetched and updated.");
    }

    /// <summary>
    /// Gets a configuration value by key.
    /// </summary>
    public T GetValue<T>(string key, T defaultValue)
    {
        if (configCache.ContainsKey(key))
        {
            return (T)configCache[key];
        }
        return defaultValue;
    }
}
