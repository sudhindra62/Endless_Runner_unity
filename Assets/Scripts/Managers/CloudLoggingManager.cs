
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class CloudLoggingManager : Singleton<CloudLoggingManager>
    {
        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
        }

        public void LogError(string message, string stackTrace)
        {
            // In a real implementation, this would send the error to a cloud service
            Debug.LogError($"CLOUD_LOGGING: Error: {message}\nStackTrace: {stackTrace}");
        }

        public void LogEvent(string eventName, System.Collections.Generic.Dictionary<string, object> parameters)
        {
            // In a real implementation, this would send the event to a cloud service
            Debug.Log($"CLOUD_LOGGING: Event: {eventName}");
        }
    }
}
