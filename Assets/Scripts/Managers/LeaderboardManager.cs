
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles submitting scores to a backend service and fetching leaderboard data.
/// Created by OMNI_LOGIC_COMPLETION_v2.
/// </summary>
public class LeaderboardManager : Singleton<LeaderboardManager>
{
    public static event Action<List<LeaderboardEntry>> OnLeaderboardDataReceived;

    // This would be your backend API endpoint
    private const string LEADERBOARD_API_URL = "https://your-leaderboard-api.com/";

    public void SubmitScore(int score, string playerName)
    {
        Debug.Log($"Submitting score {score} for player {playerName}.");
        // In a real project, you'd make a web request here.
        // StartCoroutine(SubmitScoreRequest(score, playerName));
    }

    public void FetchLeaderboard()
    {
        Debug.Log("Fetching leaderboard data.");
        // In a real project, you'd make a web request here.
        // StartCoroutine(FetchLeaderboardRequest());
        
        // For now, simulate receiving data
        List<LeaderboardEntry> dummyData = new List<LeaderboardEntry>
        {
            new LeaderboardEntry { rank = 1, playerName = "PlayerOne", score = 100000 },
            new LeaderboardEntry { rank = 2, playerName = "PlayerTwo", score = 95000 },
            new LeaderboardEntry { rank = 3, playerName = "PlayerThree", score = 90000 },
        };
        OnLeaderboardDataReceived?.Invoke(dummyData);
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public int rank;
    public string playerName;
    public int score;
}
