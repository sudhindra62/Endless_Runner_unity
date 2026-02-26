
using UnityEngine;
using System;

/// <summary>
/// Manages limited-time events, applying passive bonuses.
/// This is a persistent singleton that tracks the active event's state via PlayerPrefs.
/// 
/// --- Inspector Setup ---
/// 1. Attach this to a persistent GameObject in your starting scene.
/// 2. Define the current event in the 'CurrentEvent' field.
/// --- Integration Note ---
/// Other managers must be modified to read the public multiplier properties from this singleton.
/// For example, CurrencyManager would multiply collected coins by 'CoinMultiplier'.
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("Event Configuration")]
    [SerializeField] private EventData currentEvent;

    // --- Public Multipliers for other scripts to read ---
    public float CoinMultiplier { get; private set; } = 1f;
    public float XPMultiplier { get; private set; } = 1f;

    // --- PlayerPrefs Keys ---
    private const string EventIDKey = "ActiveEvent_ID";
    private const string EventEndTimeKey = "ActiveEvent_EndTime";

    private DateTime eventEndTime;
    private bool isEventActive = false;

    public static event Action<EventData, bool> OnEventStateChanged;

    #region Unity Lifecycle & Initialization

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CheckEventState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private void CheckEventState()
    {
        if (currentEvent == null || string.IsNullOrEmpty(currentEvent.EventID)) 
        {
            isEventActive = false;
            ResetMultipliers();
            OnEventStateChanged?.Invoke(null, false);
            return;
        }

        string savedEventID = PlayerPrefs.GetString(EventIDKey, "");
        string endTimeStr = PlayerPrefs.GetString(EventEndTimeKey, "");

        // Check if a new event has been configured in the inspector
        if (savedEventID != currentEvent.EventID)
        {
            StartNewEvent();
        }
        else
        {
            // Continue with the existing event, checking its end time
            if (DateTime.TryParse(endTimeStr, out eventEndTime))
            {
                if (DateTime.UtcNow < eventEndTime)
                {
                    isEventActive = true;
                    ApplyEventBonus();
                    Debug.Log($"Event '{currentEvent.EventName}' is active. Ends at {eventEndTime.ToLocalTime()}");
                }
                else
                {
                    EndEvent();
                }
            }
            else // Could not parse time, end the event to be safe
            {
                EndEvent();
            }
        }
        OnEventStateChanged?.Invoke(currentEvent, isEventActive);
    }

    private void StartNewEvent()
    {
        eventEndTime = DateTime.UtcNow.AddHours(currentEvent.DurationHours);

        PlayerPrefs.SetString(EventIDKey, currentEvent.EventID);
        PlayerPrefs.SetString(EventEndTimeKey, eventEndTime.ToString("o")); // ISO 8601 format
        PlayerPrefs.Save();

        isEventActive = true;
        ApplyEventBonus();
        Debug.Log($"New event '{currentEvent.EventName}' started. Ends at {eventEndTime.ToLocalTime()}");
    }

    private void EndEvent()
    {
        isEventActive = false;
        ResetMultipliers();
        // Clearing the ID prevents the same event from re-activating on next launch
        PlayerPrefs.DeleteKey(EventIDKey);
        PlayerPrefs.DeleteKey(EventEndTimeKey);
        PlayerPrefs.Save();
        Debug.Log($"Event '{currentEvent.EventName}' has ended.");
    }

    private void ApplyEventBonus()
    {
        ResetMultipliers(); // Reset first to handle all cases
        if (!isEventActive) return;

        switch (currentEvent.BonusType)
        {
            case EventBonusType.CoinMultiplier:
                CoinMultiplier = currentEvent.BonusMultiplier;
                break;
            case EventBonusType.XPMultiplier:
                XPMultiplier = currentEvent.BonusMultiplier;
                break;
        }
    }

    private void ResetMultipliers()
    {
        CoinMultiplier = 1f;
        XPMultiplier = 1f;
    }

    #region Public Accessors
    public bool IsEventActive() => isEventActive;
    public EventData GetActiveEventData() => isEventActive ? currentEvent : null;
    public TimeSpan GetRemainingTime() => isEventActive ? eventEndTime - DateTime.UtcNow : TimeSpan.Zero;
    #endregion
}
