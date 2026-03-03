using UnityEngine;

public class AlmostWinManager : Singleton<AlmostWinManager>
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private LeagueManager leagueManager;
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private FlowComboManager flowComboManager;

    public string GetAlmostWinMessage()
    {
        // Check for best score
        int currentScore = scoreManager.Score;
        int bestScore = scoreManager.GetBestScore(); // This method needs to be created in ScoreManager
        if (bestScore > 0 && currentScore > bestScore * 0.95f)
        {
            return $"You were only {bestScore - currentScore} points away from your best score!";
        }

        // Check for league promotion
        // This requires a method in LeagueManager to get the points needed for promotion
        int pointsToPromotion = leagueManager.GetPointsToPromotion();
        if (pointsToPromotion > 0 && pointsToPromotion < 100) // Example threshold
        {
            return $"Only {pointsToPromotion} points away from promotion!";
        }

        // Check for mission completion
        // This requires a method in MissionManager to get the closest mission
        string closestMission = missionManager.GetClosestMission();
        if (!string.IsNullOrEmpty(closestMission))
        {
            return closestMission;
        }

        // Check for rare drop threshold
        // This requires a method in RareDropManager to get the progress

        // Check for fever mode
        // This requires a method in FeverModeManager

        return string.Empty;
    }
}