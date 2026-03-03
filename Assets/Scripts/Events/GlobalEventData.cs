using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject to define a Weekly Global Event.
/// Contains all data needed for the event, including the community challenge, milestones, and rewards.
/// </summary>
[CreateAssetMenu(fileName = "GlobalEventData", menuName = "Events/Global Event Data")]
public class GlobalEventData : ScriptableObject
{
    [Header("Event Details")]
    public string eventName;
    public string eventDescription;
    public double globalDistanceChallengeTarget; // e.g., 1,000,000,000 meters

    [Header("Rewards")]
    public string eventExclusiveCosmeticID; // ID for the cosmetic item
    public List<Milestone> communityMilestones;

    [System.Serializable]
    public struct Milestone
    {
        public string milestoneName;
        public double distanceThreshold;
        public string milestoneRewardID; // e.g., a chest or currency pack
    }
}
