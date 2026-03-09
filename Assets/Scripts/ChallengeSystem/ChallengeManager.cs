
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages challenges, their state, and player progress towards completing them.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public class ChallengeManager : Singleton<ChallengeManager>
{
    // --- EVENTS ---
    public static event Action<Challenge> OnNewChallengeReceived;
    public static event Action<Challenge> OnChallengeAccepted;
    public static event Action<Challenge> OnChallengeDeclined;
    public static event Action<Challenge> OnChallengeCompleted;

    // --- PRIVATE STATE ---
    private List<Challenge> _activeChallenges = new List<Challenge>();
    private Challenge _currentTrackedChallenge;

    #region Public API

    /// <summary>
    /// Simulates receiving a new challenge from an external source.
    /// </summary>
    public void ReceiveNewChallenge(string challengerID, ChallengeType type, float valueToBeat)
    {
        // In a real system, this would come from a server or another player.
        string newID = Guid.NewGuid().ToString();
        Challenge newChallenge = new Challenge(newID, challengerID, type, valueToBeat);
        _activeChallenges.Add(newChallenge);
        
        Debug.Log($"Guardian Architect: New challenge received from {challengerID}. Type: {type}, Value: {valueToBeat}");
        OnNewChallengeReceived?.Invoke(newChallenge);
    }

    /// <summary>
    /// Accepts a challenge, making it the one to be tracked during gameplay.
    /// </summary>
    public void AcceptChallenge(Challenge challenge)
    {
        if (challenge == null) return;
        
        challenge.isAccepted = true;
        _currentTrackedChallenge = challenge;

        Debug.Log($"<color=cyan>Guardian Architect: Challenge {challenge.challengeID} accepted!</color>");
        OnChallengeAccepted?.Invoke(challenge);
        
        // TODO: This would likely trigger a scene change or a specific gameplay mode.
    }

    /// <summary>
    /// Declines a challenge, removing it from active consideration.
    /// </summary>
    public void DeclineChallenge(Challenge challenge)
    {
        if (challenge == null) return;
        
        _activeChallenges.Remove(challenge);
        
        Debug.Log($"Guardian Architect: Challenge {challenge.challengeID} declined.");
        OnChallengeDeclined?.Invoke(challenge);
    }

    /// <summary>
    /// Call this from your gameplay systems to check if the active challenge is completed.
    /// </summary>
    public void CheckChallengeProgress(ChallengeType type, float currentValue)
    {
        if (_currentTrackedChallenge == null || _currentTrackedChallenge.isCompleted || _currentTrackedChallenge.type != type)
        {
            return;
        }

        if (currentValue > _currentTrackedChallenge.valueToBeat)
        {
            CompleteChallenge(_currentTrackedChallenge);
        }
    }

    #endregion

    #region Private Helpers

    private void CompleteChallenge(Challenge challenge)
    {
        challenge.isCompleted = true;
        _currentTrackedChallenge = null; // Stop tracking

        Debug.Log($"<color=green>Guardian Architect: Challenge {challenge.challengeID} COMPLETED!</color>");
        OnChallengeCompleted?.Invoke(challenge);
        
        // TODO: Grant rewards for completing the challenge.
    }
    
    #endregion

    #region Getters

    /// <summary>
    /// Gets the currently tracked challenge.
    /// </summary>
    public Challenge GetCurrentChallenge() => _currentTrackedChallenge;

    #endregion
}
