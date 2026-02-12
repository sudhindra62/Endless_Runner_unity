
using UnityEngine;

/// <summary>
/// A singleton manager that orchestrates the flow of data between a single run
/// and the persistent long-term player meta-data.
/// It listens for game state changes (e.g., from a GameManager).
/// 
/// --- Integration ---
/// Call OnRunStart() when a new run begins.
/// Call OnRunEnd() when the player fails a run.
/// </summary>
public class MetaProgressionManager : MonoBehaviour
{
    public static MetaProgressionManager Instance { get; private set; }

    [Header("Data Sources")]
    [SerializeField] private RunSessionData runSessionData; // Assign in inspector
    [SerializeField] private PlayerMetaData playerMetaData; // Assign in inspector

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

    /// <summary>
    /// Called by the GameManager when a new run begins.
    /// </summary>
    public void OnRunStart()
    {
        if (runSessionData == null) {
            Debug.LogError("RunSessionData is not assigned!");
            return;
        }
        runSessionData.StartNewRun();
    }

    /// <summary>
    /// Called by the GameManager when a run ends.
    /// Finalizes the run data and commits it to the persistent meta-data.
    /// </summary>
    public void OnRunEnd()
    {
        if (runSessionData == null || playerMetaData == null) {
            Debug.LogError("One or more data sources are not assigned!");
            return;
        }

        runSessionData.EndRun();

        // Commit session data to persistent meta-data
        playerMetaData.AddCoins(runSessionData.CoinsCollectedThisRun);
        playerMetaData.AddDistance(runSessionData.DistanceThisRun);
        playerMetaData.AddPlayTime(runSessionData.TimeThisRun);
        playerMetaData.IncrementRuns();

        Debug.Log("MetaProgressionManager: Run data has been committed to PlayerMetaData.");
    }
}
