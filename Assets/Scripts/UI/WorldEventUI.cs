
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class WorldEventUI : MonoBehaviour
{
    [SerializeField] private GameObject eventPanel; // Assign a UI panel with Text elements
    [SerializeField] private Text eventNameText;
    [SerializeField] private Text eventTimerText;
    [SerializeField] private Text eventDescriptionText; // Optional

    private Coroutine countdownCoroutine;

    private void Start()
    {
        WorldEventManager.OnEventActivated += HandleEventActivated;
        WorldEventManager.OnEventDeactivated += HandleEventDeactivated;

        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        WorldEventManager.OnEventActivated -= HandleEventActivated;
        WorldEventManager.OnEventDeactivated -= HandleEventDeactivated;
    }

    private void HandleEventActivated(WorldEventData eventData)
    {
        if (eventPanel == null) return;

        eventPanel.SetActive(true);
        if (eventNameText != null) eventNameText.text = eventData.eventName;
        if (eventDescriptionText != null) eventDescriptionText.text = $"Event ends in:"; // Or a more detailed description

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownTimer(eventData.endTime));
    }

    private void HandleEventDeactivated()
    {
        if (eventPanel == null) return;
        
        eventPanel.SetActive(false);
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
    }

    private IEnumerator CountdownTimer(DateTime endTime)
    {
        while (DateTime.UtcNow < endTime)
        {
            TimeSpan remaining = endTime - DateTime.UtcNow;
            if (eventTimerText != null) 
            {
                eventTimerText.text = remaining.ToString(@"hh\:mm\:ss");
            }
            yield return new WaitForSeconds(1f);
        }
        
        // Final update to show 00:00:00 and hide panel
        if (eventTimerText != null) eventTimerText.text = "00:00:00";
        yield return new WaitForSeconds(1f);
        HandleEventDeactivated();
    }
}
