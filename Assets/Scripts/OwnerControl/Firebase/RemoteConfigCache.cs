using UnityEngine;

namespace EndlessRunner.OwnerControl.Firebase
{
    /// <summary>
    /// Local cache for owner-controlled Remote Config values.
    /// Uses PlayerPrefs to persist last known good configuration.
    /// </summary>
    internal static class RemoteConfigCache
    {
        private const string Prefix = "OWNER_CFG_";

        public static void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(Prefix + key, value);
        }

        public static void SaveBool(string key, bool value)
        {
            PlayerPrefs.SetInt(Prefix + key, value ? 1 : 0);
        }

        public static int LoadInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(Prefix + key, defaultValue);
        }

        public static bool LoadBool(string key, bool defaultValue)
        {
            int def = defaultValue ? 1 : 0;
            return PlayerPrefs.GetInt(Prefix + key, def) == 1;
        }
    }
}
