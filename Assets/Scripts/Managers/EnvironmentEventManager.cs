using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Orchestrates dynamic environmental events during a run.
/// This manager schedules and triggers events, but delegates the actual
/// implementation of event mechanics to other specialized systems.
/// </summary>
public class EnvironmentEventManager : MonoBehaviour
{
    public static EnvironmentEventManager Instance { get; private set; }

    // --- Events broadcast to other systems ---
    public event Action<EnvironmentEventData> OnEventWillStart; // Sent as a warning before the event begins.
    public event Action<EnvironmentEventData> OnEventDidStart;
    public event Action<EnvironmentEventType> OnEventDidEnd;

    [Header("Configuration")]
    [SerializeField] private List<EnvironmentEventData> availableEvents; // All possible events, configured in the Inspector.
    [SerializeField] private float minTimeBetweenEvents = 30f;
    [SerializeField] private float maxTimeBetweenEvents = 90f;

    // --- State ---
    private EnvironmentEventData activeEvent = null;
    private float eventTimer;
    private float nextEventCooldown;
    private bool isEventActive = false;

    // Assume a global reference to the player's progress exists.
    // private float currentRunDistance => ScoreManager.Instance.Distance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ScheduleNextEvent();
    }

    private void Update()
    {
        if (isEventActive)
        {
            eventTimer -= Time.deltaTime;
            if (eventTimer <= 0)
            {
                EndActiveEvent();
            }
        }
        else
        {
            nextEventCooldown -= Time.deltaTime;
            if (nextEventCooldown <= 0)
            {
                TryTriggerEvent();
            }
        }
    }

    private void ScheduleNextEvent()
    {
        nextEventCooldown = UnityEngine.Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    private void TryTriggerEvent()
    {
        // Filter for events that can be triggered at the current distance.
        float currentRunDistance = 0; // Replace with actual distance tracking.
        var possibleEvents = availableEvents.Where(e => currentRunDistance >= e.MinTriggerDistance).ToList();

        if (possibleEvents.Count > 0)
        {
            activeEvent = possibleEvents[UnityEngine.Random.Range(0, possibleEvents.Count)];
            StartEvent();
        }
        else
        {
            // No valid events can be triggered yet, try again later.
            ScheduleNextEvent();
        }
    }

    private void StartEvent()
    {
        isEventActive = true;
        eventTimer = activeEvent.DurationSeconds;

        Debug.Log($"[EnvironmentEventManager] Event Starting: {activeEvent.EventType}");

        // Broadcast warning and start events.
        OnEventWillStart?.Invoke(activeEvent);
        OnEventDidStart?.Invoke(activeEvent);
    }

    private void EndActiveEvent()
    {
        if (activeEvent == null) return;

        Debug.Log($"[EnvironmentEventManager] Event Ended: {activeEvent.EventType}");

        var endedEventType = activeEvent.EventType;
        isEventActive = false;
        activeEvent = null;

        OnEventDidEnd?.Invoke(endedEventType);

        // Schedule the next one.
        ScheduleNextEvent();
    }

    public bool IsEventActive(EnvironmentEventType type)
    {
        return isEventActive && activeEvent != null && activeEvent.EventType == type;
    }

    // Call this on run end or revive to prevent broken states.
    public void ResetManager()
    {
        if(isEventActive) EndActiveEvent();
        ScheduleNextEvent();
    }
}
