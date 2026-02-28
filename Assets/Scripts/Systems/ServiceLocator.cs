
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// A simple service locator for managing and providing access to global systems.
/// </summary>
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    /// <summary>
    /// Registers a service with the service locator.
    /// </summary>
    public static void Register<T>(T service)
    {
        if (services.ContainsKey(typeof(T)))
        {
            Debug.LogWarning($"[ServiceLocator] Service of type '{typeof(T)}' is already registered. Overwriting.");
            services[typeof(T)] = service;
        }
        else
        {
            services.Add(typeof(T), service);
        }
    }

    /// <summary>
    /// Unregisters a service from the service locator.
    /// </summary>
    public static void Unregister<T>()
    {
        if (services.ContainsKey(typeof(T)))
        {
            services.Remove(typeof(T));
        }
    }

    /// <summary>
    /// Gets a registered service.
    /// </summary>
    public static T Get<T>()
    {
        if (services.TryGetValue(typeof(T), out object service))
        {
            return (T)service;
        }

        Debug.LogError($"[ServiceLocator] Service of type '{typeof(T)}' not found.");
        return default;
    }
}
