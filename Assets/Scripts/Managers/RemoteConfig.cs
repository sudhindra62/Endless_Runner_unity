
using System.Collections.Generic;

public static class RemoteConfig
{
    private static Dictionary<string, string> config = new Dictionary<string, string>();

    public static void SetConfig(string key, string value)
    {
        config[key] = value;
    }

    public static string GetString(string key, string defaultValue = "")
    {
        return config.ContainsKey(key) ? config[key] : defaultValue;
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        if (config.ContainsKey(key) && int.TryParse(config[key], out int result))
        {
            return result;
        }
        return defaultValue;
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        if (config.ContainsKey(key) && float.TryParse(config[key], out float result))
        {
            return result;
        }
        return defaultValue;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (config.ContainsKey(key) && bool.TryParse(config[key], out bool result))
        {
            return result;
        }
        return defaultValue;
    }
}
