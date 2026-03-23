using UnityEngine;


    public class CloudLoggingManager : Singleton<CloudLoggingManager>
    {
        public void LogEvent(string eventName, string data)
    {
            Debug.Log($"[CloudLogging] {eventName}: {data}");
        }

        public void LogError(string error)
    {
            Debug.LogError($"[CloudLogging] ERROR: {error}");
        }
    }

