using UnityEngine;
using System;
using System.Collections.Generic;

public class LiveEventManager : Singleton<LiveEventManager> 
{
    public static event Action<LiveEvent> OnEventStarted;
    public static event Action<LiveEvent> OnEventEnded;

    private HashSet<string> activeEvents = new HashSet<string>();

    public bool IsEventActive(string eventId)
    {
        return activeEvents.Contains(eventId);
    }

    public bool IsAnyEventActive()
    {
        return activeEvents.Count > 0;
    }
}
