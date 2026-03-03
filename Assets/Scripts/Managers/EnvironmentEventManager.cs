
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#region SUPPORTING_DATA_STRUCTURES
public enum EnvironmentEventType
{
    MovingObstacleWave,
    LaneCollapse,
    FogMode,
    SpeedTunnel,
    SplitTrack,
    TrainCrossing,
    EnvironmentalTrap
}

[Serializable]
public class EnvironmentEventData
{
    public string eventName;
    public EnvironmentEventType eventType;
    [Tooltip("How long the event lasts in seconds.")]
    public float duration = 15f;
    [Tooltip("Minimum time that must pass before this event can be triggered again.")]
    public float minTimeBetweenEvents = 60f;
    [Tooltip("Maximum time after the minimum time that this event might be triggered.")]
    public float maxTimeBetweenEvents = 120f;
    [Tooltip("Is this event tied to a specific number of upcoming tiles?")]
    public bool isTileScoped = false;
    [Tooltip("If tile-scoped, how many tiles does this event affect?")]
    public int tileScope = 10;
    [HideInInspector]
    public float lastEventTime = -1000f; // Initialize to a large negative to allow early triggering
}

/// <summary>
/// A runtime class to track the state of an event that is currently in progress.
/// </summary>
public class ActiveEvent
{
    public EnvironmentEventData data;
    public Coroutine activeCoroutine;
    public float endTime;

    public ActiveEvent(EnvironmentEventData data, Coroutine coroutine, float endTime)
    {
        this.data = data;
        this.activeCoroutine = coroutine;
        this.endTime = endTime;
    }
}
#endregion

public class EnvironmentEventManager : MonoBehaviour
{
    public static EnvironmentEventManager Instance { get; private set; }

    #region EVENTS
    public static event Action<EnvironmentEventData> OnEnvironmentEventStart;
    public static event Action<EnvironmentEventData> OnEnvironmentEventEnd;
    #endregion

    [Header("Event Configuration")]
    [SerializeField]
    private List<EnvironmentEventData> environmentEvents;

    private readonly List<ActiveEvent> activeEvents = new List<ActiveEvent>();
    private bool isSchedulerRunning = false;
    private const string EVENT_SOURCE_ID = "EnvironmentEvent";

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Assuming a central GameManager exists to broadcast game state
        // GameManager.OnGameStart += StartEventScheduler;
        // GameManager.OnGameEnd += StopEventScheduler;
        // ReviveManager.OnPlayerRevive += HandlePlayerRevive;
    }

    private void OnDisable()
    {
        // GameManager.OnGameStart -= StartEventScheduler;
        // GameManager.OnGameEnd -= StopEventScheduler;
        // ReviveManager.OnPlayerRevive -= HandlePlayerRevive;
        StopAllCoroutines();
    }
    
    // Manual start for now until GameManager is confirmed
    private void Start()
    {
        StartEventScheduler();
    }
    #endregion

    #region SCHEDULER
    public void StartEventScheduler()
    {
        if (isSchedulerRunning) return;
        isSchedulerRunning = true;
        Debug.Log("Environment Event Scheduler STARTED.");
        StartCoroutine(EventScheduler());
    }

    public void StopEventScheduler()
    {
        if (!isSchedulerRunning) return;
        isSchedulerRunning = false;
        Debug.Log("Environment Event Scheduler STOPPED.");
        StopAllCoroutines(); 
        RevertAllEvents();
    }

    private IEnumerator EventScheduler()
    {
        yield return new WaitForSeconds(10f); // Initial delay before first event

        while (isSchedulerRunning)
        {
            EnvironmentEventData eventToTrigger = GetRandomReadyEvent();
            if (eventToTrigger != null)
            {
                TriggerEvent(eventToTrigger);
            }
            // Wait for a random time before checking for the next event
            yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 15f));
        }
    }
    #endregion

    #region EVENT_HANDLING
    private void TriggerEvent(EnvironmentEventData eventData)
    {
        // Safety check to prevent incompatible events from overlapping
        if (IsEventIncompatible(eventData))
        {
            Debug.LogWarning($"Could not start event '{eventData.eventName}' due to incompatible active event.");
            return;
        }

        Coroutine eventCoroutine = StartCoroutine(RunEventLifecycle(eventData));
        activeEvents.Add(new ActiveEvent(eventData, eventCoroutine, Time.time + eventData.duration));
        eventData.lastEventTime = Time.time;
    }

    private IEnumerator RunEventLifecycle(EnvironmentEventData eventData)
    {
        Debug.Log($"<color=yellow>EVENT STARTING:</color> {eventData.eventName}");
        OnEnvironmentEventStart?.Invoke(eventData);
        ApplyEventEffects(eventData);
        
        yield return new WaitForSeconds(eventData.duration);
        
        // The event ends here naturally
        Debug.Log($"<color=orange>EVENT ENDING:</color> {eventData.eventName}");
        RevertEvent(eventData);
    }
    
    private void RevertEvent(EnvironmentEventData eventData, bool isImmediate = false)
    {
        ActiveEvent activeEvent = activeEvents.FirstOrDefault(e => e.data.eventType == eventData.eventType);
        if (activeEvent != null)
        {
            if (!isImmediate)
            {
                OnEnvironmentEventEnd?.Invoke(eventData);
            }
            RevertEventEffects(eventData);
            activeEvents.Remove(activeEvent);
            Debug.Log($"Event '{eventData.eventName}' reverted.");
        }
    }

    private void RevertAllEvents()
    {
        // Create a copy to modify the list while iterating
        foreach (var activeEvent in new List<ActiveEvent>(activeEvents))
        {
            if (activeEvent.activeCoroutine != null)
            {
                StopCoroutine(activeEvent.activeCoroutine);
            }
            RevertEvent(activeEvent.data, true);
        }
        activeEvents.Clear();
    }

    private void HandlePlayerRevive()
    {
        Debug.Log("Re-applying persistent environment events after revive.");
        foreach (var activeEvent in new List<ActiveEvent>(activeEvents))
        {
            // Re-apply effects if the event was supposed to still be active
            if (Time.time < activeEvent.endTime)
            {
                ApplyEventEffects(activeEvent.data);
            }
        }
    }
    #endregion

    #region EVENT_LOGIC
    private void ApplyEventEffects(EnvironmentEventData eventData)
    {
        // Using ServiceLocator to get manager instances to ensure they exist
        PlayerMovement playerMovement = ServiceLocator.Get<PlayerMovement>();
        GameDifficultyManager difficultyManager = ServiceLocator.Get<GameDifficultyManager>();

        switch (eventData.eventType)
        {
            case EnvironmentEventType.SpeedTunnel:
                playerMovement?.ApplySpeedMultiplier(EVENT_SOURCE_ID, 1.5f);
                // ScoreManager.Instance.ApplyScoreMultiplier(EVENT_SOURCE_ID, 2f); // Assuming ScoreManager exists
                break;
            case EnvironmentEventType.FogMode:
                // VisionFogManager.Instance.EnableFog(0.8f, Color.gray); // Assuming VisionFogManager exists
                break;
            case EnvironmentEventType.EnvironmentalTrap:
                difficultyManager?.ApplyDifficultyMultiplier(EVENT_SOURCE_ID, 1.2f);
                break;
            // Other event logic will be added here
        }
    }

    private void RevertEventEffects(EnvironmentEventData eventData)
    {
        PlayerMovement playerMovement = ServiceLocator.Get<PlayerMovement>();
        GameDifficultyManager difficultyManager = ServiceLocator.Get<GameDifficultyManager>();

        switch (eventData.eventType)
        {
            case EnvironmentEventType.SpeedTunnel:
                playerMovement?.RemoveSpeedMultiplier(EVENT_SOURCE_ID);
                // ScoreManager.Instance.RemoveScoreMultiplier(EVENT_SOURCE_ID);
                break;
            case EnvironmentEventType.FogMode:
                // VisionFogManager.Instance.DisableFog();
                break;
            case EnvironmentEventType.EnvironmentalTrap:
                difficultyManager?.RemoveDifficultyMultiplier(EVENT_SOURCE_ID);
                break;
        }
    }
    #endregion

    #region UTILITY_AND_CHECKS
    private EnvironmentEventData GetRandomReadyEvent()
    {
        var readyEvents = environmentEvents
            .Where(e => Time.time > e.lastEventTime + e.minTimeBetweenEvents && !activeEvents.Any(a => a.data.eventType == e.eventType))
            .ToList();
            
        if (readyEvents.Count > 0)
        {
            return readyEvents[UnityEngine.Random.Range(0, readyEvents.Count)];
        }
        return null;
    }

    private bool IsEventIncompatible(EnvironmentEventData newEvent)
    {
        if (activeEvents.Any(e => e.data.eventType == EnvironmentEventType.SpeedTunnel) && newEvent.eventType == EnvironmentEventType.SpeedTunnel)
        {
            return true; // Don't stack speed tunnels
        }
        if (activeEvents.Any(e => e.data.eventType == EnvironmentEventType.LaneCollapse) && newEvent.eventType == EnvironmentEventType.SplitTrack)
        {
            return true; // Don't collapse a lane during a track split
        }
        return false;
    }
    #endregion

    #region TILE_INTEGRATION_GUIDE
    /*
    * TILE INTEGRATION GUIDE
    * 
    * To integrate tile-scoped events like LaneCollapse or SplitTrack, the EnvironmentEventManager
    * will "inject" event flags into upcoming tiles. The TileSpawner needs a minor modification
    * to check for these flags.
    *
    * 1. Modify TileData (or equivalent structure in TileSpawner):
    *    - Add: `public EnvironmentEventType activeEvent;`
    * 
    * 2. In EnvironmentEventManager, when a tile-scoped event starts:
    *    - Get a reference to the TileSpawner.
    *    - For the next 'tileScope' number of tiles to be spawned, set the 'activeEvent' flag.
    *      (e.g., `tileSpawner.upcomingTileData[i].activeEvent = eventData.eventType;`)
    *
    * 3. In TileSpawner's tile generation logic:
    *    - When a tile is spawned, check its 'activeEvent' flag.
    *    - (e.g., `if (tileData.activeEvent == EnvironmentEventType.LaneCollapse) { ... }`)
    *    - Call a method on the tile's script to activate the event visuals/logic for that specific tile.
    * 
    * 4. In the Tile script:
    *    - Implement methods like 'CollapseLane(int laneIndex)' or 'ActivateSplitTrack()'.
    *    - The EnvironmentEventManager will be responsible for reverting the general state,
    *      but the tile itself handles its own specific manifestation of the event.
    *
    * This approach keeps the TileSpawner's core logic clean and delegates event-specific behavior,
    * adhering to the "No Core Replacement" and "Not modifying TileSpawner core logic" rules.
    */
    #endregion
}
