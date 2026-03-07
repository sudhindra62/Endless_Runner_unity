using UnityEngine;

// This class is now the single, authoritative source for all player analytics.
// It now feeds frustration data into the AdaptiveDifficultyManager.
public class PlayerAnalyticsManager : MonoBehaviour
{
    public static PlayerAnalyticsManager Instance { get; private set; }

    private SessionAnalyticsData currentSession;
    private BehaviorTrendAnalyzer trendAnalyzer;
    private FrustrationDetector frustrationDetector;
    private IntegrityManager integrityManager;
    private AdaptiveDifficultyManager adaptiveDifficultyManager; // ◈ ARCHITECT_OMEGA INTEGRATION

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

    private void OnEnable()
    {
        // Event subscriptions would go here
    }

    private void OnDisable()
    {
        // Event unsubscriptions would go here
    }

    public void StartSession()
    {
        currentSession = new SessionAnalyticsData();
        currentSession.StartSession();
        frustrationDetector.Reset();
        Debug.Log("New Analytics Session Started.");
    }

    public void EndSession(bool wasAbrupt)
    {
        if (currentSession == null) return;

        currentSession.EndSession(wasAbrupt);
        trendAnalyzer.ProcessSession(currentSession);
        frustrationDetector.ProcessSession(currentSession);

        // ◈ ARCHITECT_OMEGA INTEGRATION: Feed data to difficulty system.
        if (adaptiveDifficultyManager != null)
        {
            float frustrationScore = frustrationDetector.GetFrustrationScore();
            if (frustrationScore > 0.75f) // High frustration threshold
            {
                adaptiveDifficultyManager.ApplyFrustrationPenalty(frustrationScore);
            }
        }

        Debug.Log($"Session Ended. Abrupt: {wasAbrupt}. Data:\n{JsonUtility.ToJson(currentSession, true)}");
    }

    public void TrackDeath(string cause, float distance = 0f)
    {
        if (currentSession == null) return;
        currentSession.RecordDeath(cause, distance);
        frustrationDetector.TrackDeath();
    }

    public void TrackDodge(bool success)
    {
        if (currentSession == null) return;
        currentSession.RecordDodge(success);
    }

    public void TrackCombo(int peak)
    {
        if (currentSession == null) return;
        currentSession.RecordCombo(peak);
    }

    public void TrackRevive()
    {
        if (currentSession == null) return;
        currentSession.RecordRevive();
        frustrationDetector.TrackRevive();
    }
    
    public void TrackBossEncounter(string bossName, bool survived)
    {
        if (currentSession == null) return;
        currentSession.RecordBossEncounter(bossName, survived);
    }

    // --- Dependency Injection ---

    public void SetIntegrityManager(IntegrityManager manager)
    {
        this.integrityManager = manager;
    }

    // ◈ ARCHITECT_OMEGA INTEGRATION: For connecting to the difficulty system.
    public void SetAdaptiveDifficultyManager(AdaptiveDifficultyManager manager)
    {
        this.adaptiveDifficultyManager = manager;
    }
}
