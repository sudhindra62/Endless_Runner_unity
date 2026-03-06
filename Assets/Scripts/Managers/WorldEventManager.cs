using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    [SerializeField] private LiveEventManager liveEventManager;

    private void Start()
    {
        // This would be driven by a server or configuration file in a real game
        LiveEventData testEvent = new LiveEventData
        {
            EventID = "TestEvent01",
            EventName = "Test Event",
            StartTimeUTC = (ulong)System.DateTime.UtcNow.AddSeconds(-10).ToBinary(),
            EndTimeUTC = (ulong)System.DateTime.UtcNow.AddDays(1).ToBinary(),
        };

        StartEvent(testEvent);
    }

    public void StartEvent(LiveEventData eventData)
    {
        if (liveEventManager != null)
        {
            liveEventManager.InitializeEvent(eventData);
            Debug.Log($"Event {eventData.EventName} has started.");
        }
    }

    public void EndEvent(LiveEventData eventData)
    {
        // Logic to finalize the event
        Debug.Log($"Event {eventData.EventName} has ended.");
    }
}
