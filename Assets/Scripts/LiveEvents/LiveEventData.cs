
using System;
using System.Collections.Generic;

[Serializable]
public class LiveEventData
{
    public string EventID;
    public string EventName;
    public ulong StartTimeUTC;
    public ulong EndTimeUTC;
    public string eventName => EventName;
    public ulong endTime => EndTimeUTC;
    public EventType Type;
    public List<GameplayModifier> GameplayModifiers;
    public List<EventRewardTier> RewardTiers;
    public List<EventMilestoneData> CommunityMilestones;
    public bool IsStackableWithRunModifiers;
    public bool IsStackableWithBossMode;
    public string VisualThemeID;
    public string MusicOverrideID;
}


