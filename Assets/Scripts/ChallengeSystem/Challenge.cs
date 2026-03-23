using System;

/// <summary>
/// Defines the type of a challenge.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public enum ChallengeType
{
    Distance,
    Score,
    CoinsCollected,
    Combo
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
    public string opponentID;
    public ChallengeType type;
    public float valueToBeat;
    public bool isCompleted;
    public bool isAccepted;
    public RewardItem reward;

    public Challenge(string id, string challenger, string opponent, ChallengeType challengeType, float value)
    {
        challengeID = id;
        challengerID = challenger;
        opponentID = opponent;
        type = challengeType;
        valueToBeat = value;
        reward = new RewardItem { type = RewardType.Coins, amount = 100 }; // Default reward
        isCompleted = false;
        isAccepted = false;
    }

    public Challenge(string id, ChallengeType challengeType, float value)
    {
        challengeID = id;
        type = challengeType;
        valueToBeat = value;
        isCompleted = false;
        isAccepted = true;
    }

    public Challenge() { }
}
