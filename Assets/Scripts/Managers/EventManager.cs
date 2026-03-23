
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private Dictionary<Type, Delegate> events = new Dictionary<Type, Delegate>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Subscribe<T>(Action<T> listener)
    {
        if (!events.ContainsKey(typeof(T)))
        {
            events[typeof(T)] = null;
        }
        events[typeof(T)] = (Action<T>)events[typeof(T)] + listener;
    }

    public void Unsubscribe<T>(Action<T> listener)
    {
        if (events.ContainsKey(typeof(T)))
        {
            events[typeof(T)] = (Action<T>)events[typeof(T)] - listener;
        }
    }

    public void Publish<T>(T eventArgs)
    {
        if (events.ContainsKey(typeof(T)))
        {
            (events[typeof(T)] as Action<T>)?.Invoke(eventArgs);
        }
    }

    public bool IsSpecialEventActive(string eventName)
    {
        // Delegate to LiveEventManager for actual implementation
        if (LiveEventManager.Instance != null)
            return LiveEventManager.Instance.IsEventActive(eventName);
        return false;
    }

    public bool IsSpecialEventActive()
    {
        // Check if any special event is active
        if (LiveEventManager.Instance != null)
            return LiveEventManager.Instance.IsAnyEventActive();
        return false;
    }

    public void TriggerEnvironmentEvent(EnvironmentEventType type)
    {
        if (EnvironmentEventManager.Instance != null)
        {
            EnvironmentEventManager.Instance.StartEvent(new EnvironmentEventData(type, 30f, 10, 0f));
        }
    }
}
