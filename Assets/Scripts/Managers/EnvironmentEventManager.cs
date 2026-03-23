
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// The SUPREME manager for all in-game events. It orchestrates dynamic environmental events, handles scheduled live events, and manages world theme changes.
/// This script has absorbed all functionality from the redundant WorldEventManager.
/// </summary>
public class EnvironmentEventManager : Singleton<EnvironmentEventManager>
{
    // Event Actions
    public event Action<EnvironmentEventData> OnEventWillStart;
    public event Action<EnvironmentEventData> OnEventDidStart;
    public event Action<EnvironmentEventType> OnEventDidEnd;
    public static event Action<ThemeProfileData> OnThemeChanged; // Merged from WorldEventManager
    public static event Action<string> OnWeatherEffectTriggered; // Merged from the other EnvironmentEventManager
    public static event Action<WorldEventData> OnWorldEvent;

    [Header("Dynamic Event Configuration")]
    [SerializeField] private List<EnvironmentEventData> availableDynamicEvents;
    [SerializeField] private float minTimeBetweenEvents = 30f;
    [SerializeField] private float maxTimeBetweenEvents = 90f;

    private EnvironmentEventData activeEvent = null;
    private float eventTimer;
    private float nextEventCooldown;
    private bool isEventActive = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        WorldThemeManager.OnThemeApplied += HandleThemeApplied; // Merged from WorldEventManager
    }

    private void OnDisable()
    {
        WorldThemeManager.OnThemeApplied -= HandleThemeApplied; // Merged from WorldEventManager
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
        float currentRunDistance = 0; // Replace with actual distance tracking.
        var possibleEvents = availableDynamicEvents.Where(e => currentRunDistance >= e.MinTriggerDistance).ToList();

        if (possibleEvents.Count > 0)
        {
            activeEvent = possibleEvents[UnityEngine.Random.Range(0, possibleEvents.Count)];
            StartEvent(activeEvent);
        }
        else
        {
            ScheduleNextEvent();
        }
    }

    public void StartEvent(EnvironmentEventData eventData)
    {
        isEventActive = true;
        eventTimer = eventData.DurationSeconds;
        activeEvent = eventData;

        Debug.Log($"[EnvironmentEventManager] Event Starting: {eventData.EventType}");

        OnEventWillStart?.Invoke(eventData);
        OnEventDidStart?.Invoke(eventData);
    }

    private void EndActiveEvent()
    {
        if (activeEvent == null) return;

        Debug.Log($"[EnvironmentEventManager] Event Ended: {activeEvent.EventType}");

        var endedEventType = activeEvent.EventType;
        isEventActive = false;
        activeEvent = null;

        OnEventDidEnd?.Invoke(endedEventType);

        ScheduleNextEvent();
    }

    public bool IsEventActive(EnvironmentEventType type)
    {
        return isEventActive && activeEvent != null && activeEvent.EventType == type;
    }

    public void ResetManager()
    {
        if(isEventActive) EndActiveEvent();
        ScheduleNextEvent();
    }

    #region Merged Live Event Logic

    public void InitializeLiveEvent(LiveEventData eventData)
    {
        EnvironmentEventData envEventData = ConvertLiveEventToEnvironmentEvent(eventData);
        if (envEventData != null)
        {
            StartEvent(envEventData);
        }
    }

    private EnvironmentEventData ConvertLiveEventToEnvironmentEvent(LiveEventData liveEventData)
    {
        if (liveEventData.EventID == "TestEvent01")
        {
            return availableDynamicEvents.Find(e => e.EventType == EnvironmentEventType.MeteorShower); 
        }
        return null;
    }

    #endregion

    #region Merged Theme Management Logic

    private void HandleThemeApplied()
    {
        HandleThemeApplied(null); // Satisfies the Action delegate from WorldThemeManager
    }

    private void HandleThemeApplied(ThemeProfileData newTheme)
    {
        // When the WorldThemeManager applies a theme, this event manager
        // broadcasts it to the rest of the game.
        OnThemeChanged?.Invoke(newTheme);
    }

    #endregion

    public void TriggerWeatherEffect(string effectName)
    {
        OnWeatherEffectTriggered?.Invoke(effectName);
    }

    public void BroadcastWorldEvent(WorldEventData eventData)
    {
        OnWorldEvent?.Invoke(eventData);
    }
}
