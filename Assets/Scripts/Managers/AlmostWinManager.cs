using UnityEngine;

public class AlmostWinManager : MonoBehaviour
{
    public static AlmostWinManager Instance { get; private set; }

    [Header("Thresholds")]
    [SerializeField] private float bestScoreThresholdPercentage = 0.05f;
    [SerializeField] private int leaguePromotionThreshold = 100;
    [SerializeField] private int nearMissThreshold = 1;
    [SerializeField] private float feverTimeThreshold = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CheckAlmostWinConditions()
    {
        CheckBestScore();
        CheckLeaguePromotion();
        CheckMissionCompletion();
        CheckFever();
    }

    private void CheckBestScore()
    {
        int currentScore = ScoreManager.Instance.GetCurrentScore();
        int bestScore = ScoreManager.Instance.GetBestScore();

        if (bestScore > 0 && currentScore < bestScore)
        {
            float difference = bestScore - currentScore;
            if (difference / bestScore <= bestScoreThresholdPercentage)
            {
                DisplayMessage($"You were only {difference} points away from your best score!");
            }
        }
    }

    private void CheckLeaguePromotion()
    {
        LeagueTier currentLeague = LeagueManager.Instance.GetCurrentPlayerLeague();
        int currentScore = ScoreManager.Instance.GetCurrentScore();
        int nextLeagueScore = LeagueManager.Instance.GetAdjustedThreshold(currentLeague.LeagueName) + 1; 

        if (nextLeagueScore > currentScore)
        {
            int difference = nextLeagueScore - currentScore;
            if (difference <= leaguePromotionThreshold)
            {
                DisplayMessage($"You are only {difference} points away from promotion!");
            }
        }
    }

    private void CheckMissionCompletion()
    {
        Mission closestMission = MissionManager.Instance.GetClosestMission();
        if (closestMission != null)
        {
            float difference = closestMission.GetDifference();
            if (difference > 0 && difference <= nearMissThreshold)
            {
                DisplayMessage($"Just {(int)difference} more {closestMission.Type.ToString().ToLower()} to beat record!");
            }
        }
    }

    private void CheckFever()
    {
        // Placeholder for FlowComboManager integration
        // As FlowComboManager does not expose time to fever, we will skip this for now.
    }

    private void DisplayMessage(string message)
    {
        // In a full implementation, this would trigger a UI element.
        Debug.Log(message);
    }
}
