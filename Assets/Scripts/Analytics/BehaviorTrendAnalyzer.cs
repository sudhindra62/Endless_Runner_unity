
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Analyzes trends across multiple gameplay sessions. This class maintains a record
/// of past sessions and provides insights into long-term player behavior, such as
/// identifying a "rage quit" pattern.
/// </summary>
[System.Serializable]
public class BehaviorTrendAnalyzer
{
    private const int SESSION_HISTORY_LIMIT = 20; // Keep the last 20 sessions for trend analysis
    private const float SHORT_SESSION_THRESHOLD = 120f; // 2 minutes

    [SerializeField] private List<SessionAnalyticsData> sessionHistory = new List<SessionAnalyticsData>();

    // --- Trend Metrics ---
    [SerializeField] private float overallDodgeSuccessRate;
    [SerializeField] private int totalRageQuits;
    [SerializeField] private int recentShortSessionCount;

    public void FinalizeSession(SessionAnalyticsData newSession)
    {
        // Add the new session to the history, maintaining the limit.
        if (sessionHistory.Count >= SESSION_HISTORY_LIMIT)
        {
            sessionHistory.RemoveAt(0);
        }
        sessionHistory.Add(newSession);

        // Recalculate trends based on the updated history.
        RecalculateTrends();
    }

    private void RecalculateTrends()
    {
        if (sessionHistory.Count == 0) return;

        // --- Calculate Overall Dodge Success Rate ---
        int totalDodges = 0;
        int successfulDodges = 0;
        foreach (var session in sessionHistory)
        {
            totalDodges += session.TotalDodges;
            successfulDodges += session.SuccessfulDodges;
        }
        overallDodgeSuccessRate = (totalDodges > 0) ? (float)successfulDodges / totalDodges : 0;

        // --- Count Rage Quits ---
        totalRageQuits = 0;
        foreach (var session in sessionHistory)
        {
            if (session.WasAbruptlyEnded)
            {
                totalRageQuits++;
            }
        }

        // --- Count Recent Short Sessions ---
        // This is a simple way to detect if a player is repeatedly starting and stopping.
        recentShortSessionCount = 0;
        int checkRange = Mathf.Min(sessionHistory.Count, 3); // Check the last 3 sessions
        for (int i = sessionHistory.Count - 1; i >= sessionHistory.Count - checkRange; i--)
        {
            if (sessionHistory[i].SessionDuration < SHORT_SESSION_THRESHOLD)
            {
                recentShortSessionCount++;
            }
        }
    }

    public PlayerTrends GetTrends()
    {
        return new PlayerTrends
        {
            OverallDodgeSuccessRate = overallDodgeSuccessRate,
            TotalRageQuits = totalRageQuits,
            IsShowingRageQuitPattern = recentShortSessionCount >= 3
        };
    }
}

/// <summary>
/// A simple data structure to hold the calculated trend information.
/// </summary>
[System.Serializable]
public struct PlayerTrends
{
    public float OverallDodgeSuccessRate;
    public int TotalRageQuits;
    public bool IsShowingRageQuitPattern;
}
