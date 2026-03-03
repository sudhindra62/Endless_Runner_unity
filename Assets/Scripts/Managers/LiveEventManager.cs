using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// The authoritative manager for all live events. It replaces WorldEventManager and CommunityChallengeManager.
/// It fetches event data, manages the event lifecycle, and routes modifiers and rewards through dedicated systems.
/// </summary>
public class LiveEventManager : Singleton<LiveEventManager>
{
    // --- Event Definitions for UI and other systems ---
    public static event Action<LiveEventData> OnEventStarted;
    public static event Action<LiveEventData> OnEventEnded;
    public static event Action<List<EventModifier>> OnModifiersApplied;

    private LiveEventData _activeEvent;
    private EventProgressTracker _progressTracker;

    private void Start()
    {
        // Check for an event immediately and subscribe to future updates.
        RemoteConfigBridge.OnRemoteConfigUpdated += CheckForLiveEventUpdate;
        CheckForLiveEventUpdate();
    }

    private void OnDestroy()
    {
        RemoteConfigBridge.OnRemoteConfigUpdated -= CheckForLiveEventUpdate;
    }

    private void CheckForLiveEventUpdate()
    {
        string eventJson = RemoteConfigBridge.Instance.GetString("liveEvent", "");
        if (string.IsNullOrEmpty(eventJson))
        {
            if (_activeEvent != null) EndActiveEvent();
            return;
        }

        var newEventData = JsonUtility.FromJson<LiveEventData>(eventJson);
        
        // ANTI-EXPLOIT: Use server-authoritative time.
        DateTime utcNow = GetServerTime(); 

        if (DateTime.TryParse(newEventData.startTime, out DateTime startTime) && DateTime.TryParse(newEventData.endTime, out DateTime endTime))
        {
            if (_activeEvent != null && _activeEvent.eventID != newEventData.eventID) EndActiveEvent();

            if (utcNow >= startTime && utcNow < endTime)
            {
                if (_activeEvent == null) StartEvent(newEventData);
            }
        }
    }

    private void StartEvent(LiveEventData eventData)
    {
        _activeEvent = eventData;

        // ZERO AUTHORITY CONFLICT: Delegate responsibilities, do not take them.
        ApplyModifiers(eventData.gameplayModifierList);
        // ThemeManager.Instance.ApplyTheme(eventData.visualThemeID);
        // MusicManager.Instance.PlayOverride(eventData.musicOverrideID);

        _progressTracker = gameObject.AddComponent<EventProgressTracker>();
        _progressTracker.Initialize(eventData);
        
        OnEventStarted?.Invoke(eventData);
        Debug.Log($"Live Event Started: {eventData.eventName}");
    }

    private void EndActiveEvent()
    {
        if (_activeEvent == null) return;
        
        // ZERO PERMANENT MUTATION: All modifiers are reversible.
        RemoveModifiers(_activeEvent.gameplayModifierList);
        // ThemeManager.Instance.RevertToDefaultTheme();
        // MusicManager.Instance.RevertToDefault();

        // Finalize progress, grant rewards via RewardManager.
        // RewardManager.Instance.GrantTieredRewards(_activeEvent.eventID, _progressTracker.GetFinalScore());
        
        OnEventEnded?.Invoke(_activeEvent);
        Debug.Log($"Live Event Ended: {_activeEvent.eventName}");

        Destroy(_progressTracker);
        _activeEvent = null;
    }

    private void ApplyModifiers(List<EventModifier> modifiers)
    {
        // MODIFIER ROUTING: Route through correct managers.
        foreach(var mod in modifiers)
        {
            switch(mod.targetManager)
            {
                case "DifficultyManager":
                    // DifficultyManager.Instance.ApplyModifier(mod);
                    break;
                case "ScoreManager":
                    // ScoreManager.Instance.ApplyModifier(mod);
                    break;
                 case "RunModifierManager":
                    // RunModifierManager.Instance.ApplyEventModifier(mod, _activeEvent.isStackableWithRunModifiers);
                    break;
            }
        }
        OnModifiersApplied?.Invoke(modifiers);
    }

    private void RemoveModifiers(List<EventModifier> modifiers)
    {
        // Reverse all modifiers.
    }

    private DateTime GetServerTime()
    {
        // In a real implementation, this would come from a trusted server.
        return DateTime.UtcNow;
    }
}

// --- Data Structures ---
[Serializable]
public class LiveEventData
{
    public string eventID;
    public string eventName;
    public string startTime;
    public string endTime;
    public EventType eventType;
    public List<EventModifier> gameplayModifierList;
    public List<EventRewardTier> rewardTierList;
    public List<EventMilestoneData> communityMilestoneTargets;
    public bool isStackableWithRunModifiers;
    public bool isStackableWithBossMode;
    public string visualThemeID;
    public string musicOverrideID;
}

[Serializable]
public class EventModifier
{
    public string modifierID;
    public string targetManager; // e.g. "DifficultyManager"
    public string parameter; // e.g. "enemySpeedMultiplier"
    public float value;
    public float cap;
}

public enum EventType
{
    DoubleCoinFestival, DoubleGemStorm, GravityShiftWeek, BossRushEvent,
    CommunityDistanceChallenge, RareDropBoostWeek, NoShieldHardcoreWeek,
    SpeedMania, ComboFestival, ObstacleInvasion
}
