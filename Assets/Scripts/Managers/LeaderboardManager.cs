using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stub for Social/Global Leaderboards.
/// </summary>
public class LeaderboardManager : Singleton<LeaderboardManager>
{
    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.Register(this);
    }

    public void SubmitScore(int score)
    {
        Debug.Log($"Guardian Architect: Submitting score {score} to leaderboard.");
        // Integration with PlayFab / GameServices here
    }

    public void FetchTopScores(int count, System.Action<List<string>> callback)
    {
        // Placeholder for async leaderboard fetch
        callback?.Invoke(new List<string> { "Player1: 1000", "Player2: 800" });
    }

    public void UpdatePlayerRank(int bestScore, int distance, int level, int coins, string characterId)
    {
        Debug.Log($"Guardian Architect: Leaderboard sync -> Score {bestScore}, Distance {distance}, Level {level}, Coins {coins}, Character {characterId}");
    }
}
