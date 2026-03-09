using System;

/// <summary>
/// Defines the type of a challenge.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public enum ChallengeType
{
    Distance,
    Score,
    CoinsCollected
}

/// <summary>
/// Data structure representing a single challenge.
/// Created by Supreme Guardian Architect v12 to establish a robust, data-driven challenge system.
/// </summary>
[Serializable]
public class Challenge
{
    public string challengeID;
    public string challengerID;
    public ChallengeType type;
    public float valueToBeat;
    public bool isCompleted;
    public bool isAccepted;

    public Challenge(string id, string challenger, ChallengeType challengeType, float value)
    {
        challengeID = id;
        challengerID = challenger;
        type = challengeType;
        valueToBeat = value;
        isCompleted = false;
        isAccepted = false;
    }
}
