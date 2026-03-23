
using UnityEngine;
using UnityEngine.UI;
using System;

// This class demonstrates how the UI layer subscribes to and reacts to world events.
public class WorldEventUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject eventBanner; // A parent object for all event UI
    [SerializeField] private Text eventNameText;
    [SerializeField] private Text eventTimerText;
    [SerializeField] private GameObject eventHudIcon; // An icon to show during gameplay

    private WorldEventData currentEventData;
    private bool isEventCurrentlyActive = false;

    private void OnEnable()
    {
        // Subscribe to events. This is the core of the event-driven UI.
        WorldEventManager.OnEventActivated += HandleEventActivation;
        WorldEventManager.OnEventDeactivated += HandleEventDeactivation;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks.
        WorldEventManager.OnEventActivated -= HandleEventActivation;
        WorldEventManager.OnEventDeactivated -= HandleEventDeactivation;
    }

    private void Start()
    {
        // Initially, all UI is hidden.
        eventBanner.SetActive(false);
        eventHudIcon.SetActive(false);
    }

    private void Update()
    {
        // Only update the timer if an event is active. Avoids unnecessary polling.
        if (isEventCurrentlyActive && currentEventData != null)
        {
            UpdateTimer();
        }
    }

    private void HandleEventActivation(WorldEventData eventData)
    {
        Debug.Log("UI received event activation. Displaying banner.");
        currentEventData = eventData;
        isEventCurrentlyActive = true;

        // Activate UI and populate with event data
        eventNameText.text = eventData.eventName;
        eventBanner.SetActive(true);
        eventHudIcon.SetActive(true);
    }

    private void HandleEventDeactivation(WorldEventData eventData)
    {
        Debug.Log("UI received event deactivation. Hiding banner.");
        isEventCurrentlyActive = false;
        currentEventData = null;

        // Hide all UI
        eventBanner.SetActive(false);
        eventHudIcon.SetActive(false);
    }

    private void UpdateTimer()
    {
        TimeSpan remainingTime = currentEventData.endTime - DateTime.UtcNow;

        if (remainingTime.TotalSeconds > 0)
        {
            // Format as Days:Hours:Minutes:Seconds or similar
            eventTimerText.text = string.Format("{0:d2}:{1:d2}:{2:d2}:{3:d2}", 
                remainingTime.Days, remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
        }
        else
        {
            eventTimerText.text = "Event Over";
            // The WorldEventManager will handle the actual deactivation.
            // The UI just reflects the state.
        }
    }
}
