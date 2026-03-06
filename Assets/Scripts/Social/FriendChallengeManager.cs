
using UnityEngine;
using System.Collections.Generic;
using System;

public enum ChallengeType
{
    Score,
    Distance,
    Combo
}

[Serializable]
public class Challenge
{
    public string challengeID;
    public string challengerID;
    public string opponentID;
    public ChallengeType type;
    public float valueToBeat;
    public bool isCompleted;
}

public class FriendChallengeManager : Singleton<FriendChallengeManager>
{
    public static event Action<List<Challenge>> OnChallengesUpdated;
    private List<Challenge> activeChallenges = new List<Challenge>();

    public List<Challenge> GetChallenges() {
        return activeChallenges;
    }

    public void CreateChallenge(string opponentID, ChallengeType type, float value)
    {
        var newChallenge = new Challenge
        {
            challengeID = Guid.NewGuid().ToString(),
            challengerID = "PLAYER_ID", // This would be the actual player's ID
            opponentID = opponentID,
            type = type,
            valueToBeat = value,
            isCompleted = false
        };

        activeChallenges.Add(newChallenge);
        OnChallengesUpdated?.Invoke(activeChallenges);
        // In a real game, this would send a notification to the opponent
        Debug.Log($"Challenge of type {type} issued to {opponentID} with value {value}");
    }

    public void CompleteChallenge(string challengeID, float playerPerformance)
    {
        Challenge challenge = activeChallenges.Find(c => c.challengeID == challengeID);
        if (challenge != null && !challenge.isCompleted)
        {
            // Use IntegrityManager to validate the run results before awarding
            if (IntegrityManager.Instance != null)
            {
                // This is a simplified validation. A real one would be more robust.
                if (!IntegrityManager.Instance.sessionValidator.IsTimeScaleValid()) // Example check
                {
                    IntegrityManager.Instance.ReportError("Challenge completion failed validation.");
                    return;
                }
            }

            if (playerPerformance > challenge.valueToBeat)
            {
                Debug.Log("Challenge WON!");
                RewardManager.Instance.AwardChallengeReward();
            }
            else
            {
                Debug.Log("Challenge LOST.");
            }
            challenge.isCompleted = true;
            OnChallengesUpdated?.Invoke(activeChallenges);
        }
    }
}
