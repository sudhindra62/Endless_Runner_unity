using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A centralized Service Locator for registering and retrieving global services.
/// Global scope for maximum project-wide accessibility.
/// </summary>
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"[SERVICE_LOCATOR] Service of type {type.Name} is already registered.");
            return;
        }
        _services[type] = service;
    }

    public static void Unregister<T>()
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            _services.Remove(type);
        }
    }

    public static T Get<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
        {
            Debug.LogError($"[SERVICE_LOCATOR] Service of type {type.Name} is not registered.");
            return default;
        }
        return (T)service;
    }
}
