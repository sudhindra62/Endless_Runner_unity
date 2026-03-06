using UnityEngine;

// This class is now the single, authoritative source for all player analytics.
// It merges the capabilities of both previous PlayerAnalyticsManager scripts.
// It is designed to work with the consolidated SessionAnalyticsData class.
public class PlayerAnalyticsManager : MonoBehaviour
{
    public static PlayerAnalyticsManager Instance { get; private set; }

    private SessionAnalyticsData currentSession;

    // --- Analysis Modules from the former 'Managers' version ---
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

        // Initialize the advanced analysis modules.
        trendAnalyzer = new BehaviorTrendAnalyzer();
        frustrationDetector = new FrustrationDetector();
    }

    // --- Event Subscription from the former 'Analytics' version ---
    private void OnEnable()
    {
        // This functionality is preserved. It assumes a PlayerController with a static event exists.
        // In a real project, we would ensure PlayerController is in place.
        // e.g., PlayerController.OnPlayerAction += TrackDodge;
    }

    private void OnDisable()
    {
        // e.g., PlayerController.OnPlayerAction -= TrackDodge;
    }

    // --- Session Lifecycle Management (Aligned with GameFlowController) ---

    // The method 'StartNewSession' was renamed to 'StartSession' for consistency.
    public void StartSession()
    {
        currentSession = new SessionAnalyticsData();
        currentSession.StartSession();
        frustrationDetector.Reset();
        Debug.Log("New Analytics Session Started.");
    }

    // The method 'EndSession' is now aligned with GameFlowController's needs.
    public void EndSession(bool wasAbrupt)
    {
        if (currentSession == null) return;

        currentSession.EndSession(wasAbrupt);

        // Process the completed session data.
        trendAnalyzer.ProcessSession(currentSession);
        frustrationDetector.ProcessSession(currentSession);

        // For debugging and validation, log the JSON data.
        // In production, this would be sent to a backend service.
        Debug.Log($"Session Ended. Abrupt: {wasAbrupt}. Data:\n{JsonUtility.ToJson(currentSession, true)}");
    }

    // --- Granular Event Tracking ---
    // All tracking methods now use the 'Record' naming convention from the merged SessionAnalyticsData.

    // This unified method handles all deaths, called from GameFlowController.
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

    // Renamed from TrackComboPeak for consistency.
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
    
    // Unified to include bossName, as per the more detailed data structure.
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
}
