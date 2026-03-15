
using System;
using System.Collections.Generic;

namespace EndlessRunner.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Logger.LogWarning("SERVICE_LOCATOR", $"Service of type {type.Name} is already registered.");
                return;
            }
            _services[type] = service;
            Logger.Log("SERVICE_LOCATOR", $"Service registered: {type.Name}");
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (!_services.TryGetValue(type, out var service))
            {
                Logger.LogError("SERVICE_LOCATOR", $"Service of type {type.Name} is not registered.");
                return default;
            }
            return (T)service;
        }
    }
}
