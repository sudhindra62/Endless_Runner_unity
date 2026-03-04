
using UnityEngine;

/// <summary>
/// The central singleton for managing the AAA Player Analytics Engine.
/// It handles session data, aggregates metrics, and provides a single point of access for all analytics tracking.
/// This script is designed to be lightweight and avoid any gameplay interference.
/// </summary>
[RequireComponent(typeof(FrustrationDetector))]
public class PlayerAnalyticsManager : Singleton<PlayerAnalyticsManager>
{
    public SessionAnalyticsData currentSession;
    private FrustrationDetector frustrationDetector;
    private BehaviorTrendAnalyzer trendAnalyzer;

    protected override void Awake()
    {
        base.Awake();
        frustrationDetector = GetComponent<FrustrationDetector>();
        trendAnalyzer = new BehaviorTrendAnalyzer();
    }

    /// <summary>
    /// Begins a new analytics session, resetting all data trackers.
    /// </summary>
    public void StartSession()
    {
        currentSession = new SessionAnalyticsData();
        currentSession.BeginSession();
        frustrationDetector.ResetFrustrationState();
        Debug.Log("New Analytics Session Started.");
    }

    /// <summary>
    /// Ends the current analytics session and logs the final data.
    /// </summary>
    /// <param name="wasAbrupt">Indicates if the session ended unexpectedly (e.g., rage quit).</param>
    public void EndSession(bool wasAbrupt)
    {
        if (currentSession == null) return;

        currentSession.EndSession(wasAbrupt);
        trendAnalyzer.FinalizeSession(currentSession);
        frustrationDetector.OnSessionEnd(currentSession);

        // In a real project, this data would be sent to a backend service.
        Debug.Log($"Analytics Session Ended. Duration: {currentSession.SessionDuration:F2}s. Rage Quit: {wasAbrupt}");
        Debug.Log($"Player Frustrated: {frustrationDetector.IsPlayerFrustrated}");
    }

    // --- DATA COLLECTION METHODS ---

    public void TrackDeath(string cause, float timestamp)
    {
        if (currentSession == null) return;
        currentSession.RecordDeath(cause, timestamp);
        frustrationDetector.OnPlayerDeath(timestamp);
    }

    public void TrackDodge(bool success)
    {
        if (currentSession == null) return;
        currentSession.RecordDodge(success);
    }



    public void TrackReactionTime(float time)
    {
        if (currentSession == null) return;
        currentSession.RecordReactionTime(time);
    }

    public void LogComboPeak(int peak)
    {
        if (currentSession == null) return;
        currentSession.UpdateComboPeak(peak);
    }

    public void LogRevive()
    {
        if (currentSession == null) return;
        currentSession.RecordRevive();
    }

    public void TrackBossEncounter(string bossName, bool survived)
    {
        if (currentSession == null) return;
        currentSession.RecordBossEncounter(bossName, survived);
    }

    public SessionAnalyticsData GetCurrentSessionData()
    {
        return currentSession;
    }

    public PlayerTrends GetPlayerTrends()
    {
        return trendAnalyzer.GetTrends();
    }
}
