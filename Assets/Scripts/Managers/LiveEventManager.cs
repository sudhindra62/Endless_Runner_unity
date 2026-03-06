using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class LiveEventManager : MonoBehaviour
{
    public static LiveEventManager Instance { get; private set; }

    [Header("Dependencies")]
    [SerializeField] private EventProgressTracker progressTracker;

    private List<LiveEventData> activeEvents = new List<LiveEventData>();
    private List<LiveEventData> allEvents = new List<LiveEventData>();

    public static event Action OnActiveEventsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // In a real implementation, this would be populated from a remote source
        LoadEventsFromRemoteConfig();
        InvokeRepeating(nameof(CheckForEventChanges), 0f, 60f); // Check every minute
    }

    private void LoadEventsFromRemoteConfig()
    {
        // This is where you would deserialize your event data from Firebase Remote Config or a similar service.
        // For now, we will create some mock data.
        allEvents = CreateMockEventData();
        CheckForEventChanges();
    }

    private void CheckForEventChanges()
    {
        ulong now = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        List<LiveEventData> previouslyActiveEvents = new List<LiveEventData>(activeEvents);

        activeEvents = allEvents.Where(e => now >= e.StartTimeUTC && now <= e.EndTimeUTC).ToList();

        if (!activeEvents.SequenceEqual(previouslyActiveEvents))
        {
            OnActiveEventsChanged?.Invoke();
            ApplyEventModifiers();
        }
    }

    private void ApplyEventModifiers()
    {
        // Reset all modifiers to default values first
        ResetAllModifiers();

        foreach (var liveEvent in activeEvents)
        {
            foreach (var modifier in liveEvent.GameplayModifiers)
            {
                // Route modifiers to the correct manager
                RouteModifier(modifier.TargetManager, modifier.ModifierField, modifier.Value);
            }
        }
    }

    private void ResetAllModifiers()
    {
        // Reset modifiers in all relevant managers
        // Example:
        // DifficultyManager.Instance.ResetModifiers();
        // ScoreManager.Instance.ResetModifiers();
    }

    private void RouteModifier(string manager, string field, float value)
    {
        // This is a simplified routing mechanism.
        // A more robust solution might use reflection or a command pattern.
        switch (manager)
        {
            case "DifficultyManager":
                // DifficultyManager.Instance.ApplyModifier(field, value);
                break;
            case "ScoreManager":
                // ScoreManager.Instance.ApplyModifier(field, value);
                break;
            // Add cases for other managers
        }
    }

    public List<LiveEventData> GetActiveEvents() => activeEvents;

    public void TrackMilestoneProgress(string milestoneID, float amount) => progressTracker.TrackProgress(milestoneID, amount);

    private List<LiveEventData> CreateMockEventData()
    {
        return new List<LiveEventData>
        {
            new LiveEventData
            {
                EventID = "double-coins-1",
                EventName = "Double Coin Festival",
                StartTimeUTC = (ulong)DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)).Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                EndTimeUTC = (ulong)DateTime.UtcNow.Add(TimeSpan.FromDays(1)).Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                Type = EventType.DoubleCoinFestival,
                GameplayModifiers = new List<GameplayModifier>
                {
                    new GameplayModifier { TargetManager = "ScoreManager", ModifierField = "CoinMultiplier", Value = 2f }
                }
            }
        };
    }
}
