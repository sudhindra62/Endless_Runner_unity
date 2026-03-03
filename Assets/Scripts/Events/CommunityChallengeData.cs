using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject to define a Global Community Challenge.
/// This contains all the static data for a challenge, including the
/// overall goal, milestone thresholds, and the associated reward IDs.
/// </summary>
[CreateAssetMenu(fileName = "CommunityChallengeData", menuName = "Events/Community Challenge Data")]
public class CommunityChallengeData : ScriptableObject
{
    [Header("Challenge Details")]
    public string challengeName;
    [TextArea] public string challengeDescription;
    public double globalDistanceTarget;
    public string finalRewardCosmeticID;

    [Header("Milestones")]
    public List<Milestone> milestones;

    [System.Serializable]
    public struct Milestone
    {
        public double distanceThreshold;
        public string milestoneRewardID; // ID for a currency pack, chest, etc.
    }
}
