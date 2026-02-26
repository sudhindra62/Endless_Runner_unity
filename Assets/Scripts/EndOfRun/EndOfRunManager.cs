using UnityEngine;

public class EndOfRunManager : MonoBehaviour
{
    public static EndOfRunManager Instance { get; private set; }

    private RunSummaryData runSummaryData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndRun()
    {
        runSummaryData = new RunSummaryData
        {
            score = ScoreManager.Instance.GetScore(),
            coinsCollected = RunStatsManager.Instance.CoinsCollectedThisRun,
            distanceTraveled = RunStatsManager.Instance.DistanceTraveledThisRun
        };

        // The GameFlowController will handle pausing the game and showing the revive UI.
        if (GameFlowController.Instance != null)
        {
            GameFlowController.Instance.PauseForDeath();
        } 
        else
        {
            // Fallback for when GameFlowController is not in the scene
            ShowRunSummary();
        }
    }

    // This method will be called by the UI when the player declines to revive.
    public void ShowRunSummary()
    {
        // TODO: Implement UI for run summary
        Debug.Log("Run Summary:");
        Debug.Log("Score: " + runSummaryData.score);
        Debug.Log("Coins Collected: " + runSummaryData.coinsCollected);
        Debug.Log("Distance Traveled: " + runSummaryData.distanceTraveled.ToString("F2"));
    }
}
