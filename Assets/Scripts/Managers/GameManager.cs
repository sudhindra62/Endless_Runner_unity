using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public RunSessionData runSessionData;
    public PlayerAnalyticsManager playerAnalyticsManager;
    public TimeWarpManager timeWarpManager;
    public MultiplayerGhostManager multiplayerGhostManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerAnalyticsManager = FindObjectOfType<PlayerAnalyticsManager>();
        if (playerAnalyticsManager == null)
        {
            GameObject analyticsObject = new GameObject("PlayerAnalyticsManager");
            playerAnalyticsManager = analyticsObject.AddComponent<PlayerAnalyticsManager>();
        }

        timeWarpManager = FindObjectOfType<TimeWarpManager>();
        if (timeWarpManager == null)
        {
            GameObject timeWarpObject = new GameObject("TimeWarpManager");
            timeWarpManager = timeWarpObject.AddComponent<TimeWarpManager>();
        }

        multiplayerGhostManager = FindObjectOfType<MultiplayerGhostManager>();
        if (multiplayerGhostManager == null)
        {
            GameObject ghostManagerObject = new GameObject("MultiplayerGhostManager");
            multiplayerGhostManager = ghostManagerObject.AddComponent<MultiplayerGhostManager>();
        }
    }

    public void StartNewRun()
    {
        // Reset session data for the new run
        if (runSessionData != null)
        {
            runSessionData.Reset();
        }

        // Reset the time warp ability
        if (timeWarpManager != null)
        {
            timeWarpManager.ResetWarp();
        }

        // Start analytics session
        if (IntegrityManager.Instance.IsAnalyticsEnabled())
        {
            playerAnalyticsManager.StartNewSession();
        }
    }

    public void StartGhostRace(GhostRunData ghostData)
    {
        if (multiplayerGhostManager != null)
        {
            multiplayerGhostManager.StartGhostRace(ghostData);
        }
    }

    private void OnApplicationQuit()
    {
        if (IntegrityManager.Instance.IsAnalyticsEnabled())
        {
            playerAnalyticsManager.EndSession();
        }
    }

    public void PlayerDied()
    {
        if(IntegrityManager.Instance.IsAnalyticsEnabled())
        {
            FrustrationDetector.Instance.ReportPlayerDeath();
        }
    }
}
