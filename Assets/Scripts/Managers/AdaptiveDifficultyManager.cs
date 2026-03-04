using UnityEngine;

/// <summary>
/// The core of the adaptive difficulty system. This manager periodically analyzes
/// player frustration levels and can issue a "soft difficulty ease" to other game systems.
/// It acts as a mediator between the FrustrationDetector and the rest of the game.
/// </summary>
public class AdaptiveDifficultyManager : Singleton<AdaptiveDifficultyManager>
{
    [Header("Analysis Settings")]
    [Tooltip("How often (in seconds) the manager should analyze the player's frustration level.")]
    [SerializeField] private float analysisInterval = 15f;

    private FrustrationDetector frustrationDetector;
    private PlayerAnalyticsManager analyticsManager;

    private float analysisTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        frustrationDetector = GetComponent<FrustrationDetector>();
        analyticsManager = PlayerAnalyticsManager.Instance;
    }

    private void Update()
    {
        // We only want to run the analysis while the game is actively being played.
        if (GameFlowController.Instance.CurrentState != GameState.Playing) return;

        analysisTimer += Time.deltaTime;

        if (analysisTimer >= analysisInterval)
        {
            analysisTimer = 0f;
            AnalyzePlayerState();
        }
    }

    private void AnalyzePlayerState()
    {
        if (frustrationDetector == null || analyticsManager == null) return;

        // The FrustrationDetector does the heavy lifting.
        frustrationDetector.AnalyzeSession(analyticsManager.GetCurrentSessionData());

        if (frustrationDetector.IsPlayerFrustrated)
        {
            // --- ADAPTIVE EASE: This is where the magic happens ---
            // In a real project, this would trigger events or call methods
            // on other systems to make the game slightly easier.
            Debug.LogWarning("ADAPTIVE EASE TRIGGERED: Reducing game difficulty slightly.");
            // Example: ObstacleManager.Instance.ReduceObstacleDensity(0.1f);
            // Example: PowerUpManager.Instance.IncreaseSpawnRate(0.15f);
        }
    }
    
    /// <summary>
    /// Called by the GameFlowController at the end of a run.
    /// </summary>
    public void OnSessionEnd()
    {
        if (frustrationDetector != null && analyticsManager != null)
        {
            frustrationDetector.OnSessionEnd(analyticsManager.GetCurrentSessionData());
        }
    }
}
