using UnityEngine;
using Missions;

/// <summary>
/// Fortified manager for detecting "almost win" scenarios to improve player retention.
/// This system is now event-driven and decoupled from direct manager dependencies for its trigger.
/// </summary>
public class AlmostWinManager : Singleton<AlmostWinManager>
{
    [Header("Thresholds")]
    [Tooltip("The percentage difference from the best score to be considered an 'almost win'.")]
    [SerializeField] private float _bestScoreThresholdPercentage = 0.05f;
    [Tooltip("The point difference from league promotion to be considered an 'almost win'.")]
    [SerializeField] private int _leaguePromotionThreshold = 100;
    [Tooltip("The progress difference from mission completion to be considered an 'almost win'.")]
    [SerializeField] private int _nearMissThreshold = 1;

    // --- Events ---
    public static event System.Action<string> OnAlmostWinDetected;

    protected override void Awake()
    {
        base.Awake();
        // The Singleton pattern is already robustly handled by the base class.
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    /// <summary>
    /// Listens for the end of the game to check for almost-win conditions.
    /// </summary>
    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.GameOver)
        {
            CheckAlmostWinConditions();
        }
    }

    /// <summary>
    /// Checks all defined almost-win conditions.
    /// </summary>
    private void CheckAlmostWinConditions()
    {
        CheckBestScore();
        CheckLeaguePromotion();
        CheckMissionCompletion();
        // Note: Fever mode check remains a placeholder pending FlowComboManager refactor.
    }

    private void CheckBestScore()
    {
        int currentScore = ScoreManager.Instance.GetCurrentScore();
        int bestScore = ScoreManager.Instance.GetBestScore();

        if (bestScore > 0 && currentScore < bestScore)
        {
            float difference = bestScore - currentScore;
            if (difference / bestScore <= _bestScoreThresholdPercentage)
            {
                DisplayMessage($"You were only {difference} points away from your best score!");
            }
        }
    }

    private void CheckLeaguePromotion()
    {
        if (LeagueManager.Instance == null) return; // Guard against missing dependency

        LeagueTier currentLeague = LeagueManager.Instance.GetCurrentPlayerLeague();
        int currentScore = ScoreManager.Instance.GetCurrentScore();
        int nextLeagueScore = LeagueManager.Instance.GetAdjustedThreshold(currentLeague.LeagueName) + 1;

        if (nextLeagueScore > currentScore)
        {
            int difference = nextLeagueScore - currentScore;
            if (difference <= _leaguePromotionThreshold)
            {
                DisplayMessage($"You are only {difference} points away from promotion!");
            }
        }
    }

    private void CheckMissionCompletion()
    {
        if (MissionManager.Instance == null) return; // Guard against missing dependency

        Mission closestMission = MissionManager.Instance.GetClosestMission();
        if (closestMission != null)
        {
            float difference = closestMission.GetDifference();
            if (difference > 0 && difference <= _nearMissThreshold)
            {
                DisplayMessage($"Just {(int)difference} more {closestMission.Type.ToString().ToLower()} to beat record!");
            }
        }
    }

    private void DisplayMessage(string message)
    {
        // Fire an event that a UI system can listen to.
        // This decouples the logic from the specific UI implementation.
        OnAlmostWinDetected?.Invoke(message);
        Debug.Log($"<color=cyan>[AlmostWinManager]</color> Almost Win Detected: {message}");
    }
}
