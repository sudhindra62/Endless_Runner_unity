using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Displays a countdown timer for the active Live Event.
/// Subscribes to LiveEventManager and updates efficiently without polling per frame.
/// </summary>
public class EventCountdownTimerUI : MonoBehaviour
{
    [SerializeField] private Text countdownText;
    private LiveEvent _activeEvent;
    private bool _isEventActive = false;

    private void OnEnable()
    {
        LiveEventManager.OnEventStarted += StartTimer;
        LiveEventManager.OnEventEnded += StopTimer;
    }

    private void OnDisable()
    {
        LiveEventManager.OnEventStarted -= StartTimer;
        LiveEventManager.OnEventEnded -= StopTimer;
    }

    private void StartTimer(LiveEvent eventData)
    {
        _activeEvent = eventData;
        _isEventActive = true;
        // Using a coroutine for efficient, periodic updates instead of Update()
        StartCoroutine(UpdateTimer());
    }

    private void StopTimer(LiveEvent eventData)
    {
        _isEventActive = false;
        countdownText.text = "Event Ended";
    }

    private System.Collections.IEnumerator UpdateTimer()
    {
        while (_isEventActive)
        {
            DateTime endTime;
            endTime = _activeEvent.endTime;
            TimeSpan remaining = endTime - DateTime.UtcNow;
            if (remaining.TotalSeconds > 0)
            {
                countdownText.text = $"{remaining.Days}d {remaining.Hours}h {remaining.Minutes}m {remaining.Seconds}s";
            }
            else
            {
                countdownText.text = "Event Ending...";
                _isEventActive = false;
            }
            yield return new WaitForSeconds(1f); // Update every second
        }
    }
}
