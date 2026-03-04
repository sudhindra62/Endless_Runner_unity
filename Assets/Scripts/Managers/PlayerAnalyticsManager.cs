
using UnityEngine;
using System.Collections.Generic;

public class PlayerAnalyticsManager : MonoBehaviour
{
    public static PlayerAnalyticsManager Instance { get; private set; }

    private SessionAnalyticsData currentSession;
    private BehaviorTrendAnalyzer trendAnalyzer;
    private FrustrationDetector frustrationDetector;
    private IntegrityManager integrityManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        trendAnalyzer = new BehaviorTrendAnalyzer();
        frustrationDetector = new FrustrationDetector();
    }

    public void StartNewSession()
    {
        currentSession = new SessionAnalyticsData();
        currentSession.StartSession();
    }

    public void EndSession()
    {
        if (currentSession != null)
        {
            currentSession.EndSession();
            trendAnalyzer.ProcessSession(currentSession);
            frustrationDetector.ProcessSession(currentSession);
            // In a real implementation, you would send this data to your analytics backend
            // For example: BackendAnalyticsService.Instance.Send(currentSession.ToJson());
        }
    }

    public void TrackPlayerDeath(string cause, float distance)
    {
        if (currentSession != null)
        {
            currentSession.LogDeath(cause, distance);
            frustrationDetector.TrackDeath();
        }
    }

    public void TrackDodge(bool success)
    {
        if (currentSession != null)
        {
            currentSession.LogDodge(success);
        }
    }

    public void TrackComboPeak(int peak)
    {
        if (currentSession != null)
        {
            currentSession.LogComboPeak(peak);
        }
    }

    public void TrackRevive()
    {
        if (currentSession != null)
        {
            currentSession.LogRevive();
        }
    }

    public void TrackBossEncounter(bool survived)
    {
        if (currentSession != null)
        {
            currentSession.LogBossSurvival(survived);
        }
    }
    
    public void SetIntegrityManager(IntegrityManager manager)
    {
        integrityManager = manager;
    }
}
