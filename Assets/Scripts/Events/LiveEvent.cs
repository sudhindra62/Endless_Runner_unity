using System;

/// <summary>
/// Data model representing a live event in the game.
/// Used by LiveEventManager for event lifecycle tracking.
/// </summary>
[System.Serializable]
public class LiveEvent
{
    public string eventId { get; set; }
    public string eventName { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string description { get; set; }
    
    public LiveEvent() {}
    
    public LiveEvent(string id, string name, DateTime start, DateTime end)
    {
        eventId = id;
        eventName = name;
        startTime = start;
        endTime = end;
    }
    
    public bool IsActive() => DateTime.UtcNow >= startTime && DateTime.UtcNow <= endTime;
}
