
using UnityEngine;

namespace EndlessRunner.Core
{
    public static class Logger
    {
        public static void Log(string tag, string message)
        {
#if UNITY_EDITOR
            Debug.Log($"[{tag}]: {message}");
#endif
        }

        public static void LogWarning(string tag, string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[{tag}]: {message}");
#endif
        }
        
        public static void LogError(string tag, string message)
        {
#if UNITY_EDITOR
            Debug.LogError($"[{tag}]: {message}");
#endif
        }
    }
}
