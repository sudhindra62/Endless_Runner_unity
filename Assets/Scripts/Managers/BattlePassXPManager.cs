
using UnityEngine;

/// <summary>
/// Responsible for awarding Battle Pass XP from various in-game sources.
/// This decouples the Battle Pass progression from the systems that grant XP.
/// </summary>
public class BattlePassXPManager : Singleton<BattlePassXPManager>
{
    [Header("XP Sources")]
    [SerializeField] private int xpPerMission = 100;
    [SerializeField] private int xpPerDailyChallenge = 250;
    [SerializeField] private int xpPerMeterRan = 1; // Example: 1 XP per 10 meters
    [SerializeField] private int distanceDivisor = 10;

    private BattlePassManager battlePassManager;

    private void Start()
    {
        battlePassManager = BattlePassManager.Instance;

        // Subscribe to relevant events
        // MissionManager.OnMissionCompleted += GrantMissionXP;
        // DailyChallengeManager.OnChallengeCompleted += GrantDailyChallengeXP;
        // GameStateManager.OnGameEnd += GrantRunPerformanceXP;
    }

    private void GrantMissionXP(/* MissionData mission */)
    {
        Debug.Log($"Granting {xpPerMission} Battle Pass XP for completing a mission.");
        battlePassManager.AddXP(xpPerMission);
    }

    private void GrantDailyChallengeXP(/* ChallengeData challenge */)
    {
        Debug.Log($"Granting {xpPerDailyChallenge} Battle Pass XP for completing a daily challenge.");
        battlePassManager.AddXP(xpPerDailyChallenge);
    }

    private void GrantRunPerformanceXP(/* RunStats stats */)
    {
        // Example: Grant XP based on distance ran
        // int distance = stats.distance;
        int distance = 500; // Placeholder for example
        int xpFromDistance = (distance / distanceDivisor) * xpPerMeterRan;

        if (xpFromDistance > 0)
        {
            Debug.Log($"Granting {xpFromDistance} Battle Pass XP for run performance.");
            battlePassManager.AddXP(xpFromDistance);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
    }
}
