using UnityEngine;
using System;

/// <summary>
/// Defines the type of bonus applied during a limited-time event.
/// </summary>
[Serializable]
public enum EventBonusType
{
    CoinMultiplier,
    XPMultiplier
}

/// <summary>
/// A serializable data container that defines a single limited-time event.
/// </summary>
[Serializable]
public class EventData
{
    public string EventID;
    public string EventName;
    [Tooltip("Duration of the event in hours from its start time.")]
    public int DurationHours;

    [Header("Bonus")]
    public EventBonusType BonusType;
    [Tooltip("The multiplier to apply. E.g., 2.0 for a 2x bonus.")]
    public float BonusMultiplier = 1.0f;
}
