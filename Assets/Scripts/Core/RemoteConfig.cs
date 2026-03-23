public static class RemoteConfig
{
    public static event System.Action OnConfigUpdated;
    public static bool IsEnabled(string key) => false;
    public static float GetFloat(string key, float defaultValue = 0f) => defaultValue;
    public static int GetInt(string key, int defaultValue = 0) => defaultValue;
    public static bool GetBool(string key, bool defaultValue = false) => defaultValue;
    public static string GetString(string key, string defaultValue = "") => defaultValue;
    public static void Refresh() => OnConfigUpdated?.Invoke();
}
