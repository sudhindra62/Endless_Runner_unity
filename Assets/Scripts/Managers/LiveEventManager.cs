using UnityEngine;

public class LiveEventManager : MonoBehaviour
{
    [SerializeField] private EventProgressTracker eventProgressTracker;

    private void Start()
    {
        // Example of initializing with a dummy event
        LiveEventData dummyEvent = new LiveEventData { EventID = "DUMMY_EVENT" };
        InitializeEvent(dummyEvent);
    }

    public void InitializeEvent(LiveEventData eventData)
    {
        if (eventProgressTracker != null)
        {
            eventProgressTracker.Initialize(eventData);
        }
    }
}
