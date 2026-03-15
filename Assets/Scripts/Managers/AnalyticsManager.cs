
using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Themes;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TrackEvent(string eventName, Dictionary<string, object> eventData)
    {
        // In a real implementation, you would integrate with an analytics service like Unity Analytics, Firebase, or a custom backend.
        Debug.Log($"Tracking event: {eventName} with data: {Newtonsoft.Json.JsonConvert.SerializeObject(eventData)}");
    }

    public void TrackThemeSelectedEvent(ThemeSO theme)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "theme_name", theme.name }
        };
        TrackEvent("theme_selected", eventData);
    }

    public void TrackObstacleHitEvent(string obstacleType, float playerSpeed)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "obstacle_type", obstacleType },
            { "player_speed", playerSpeed }
        };
        TrackEvent("obstacle_hit", eventData);
    }
}
