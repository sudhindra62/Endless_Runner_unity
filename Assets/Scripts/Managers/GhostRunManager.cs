
using UnityEngine;
using System.Collections.Generic;

public class GhostRunManager : Singleton<GhostRunManager>
{
    [Header("Dependencies")]
    [SerializeField] public GhostRunRecorder recorder;
    [SerializeField] public GhostRunPlayback playback;

    private List<GhostDataPoint> bestRunData;

    private void Start()
    {
        bestRunData = SaveManager.Instance.LoadBestGhostRun();

        if (bestRunData != null)
        {
            playback.LoadGhostRun(bestRunData);
        }
    }

    public void SaveNewBestRun(List<GhostDataPoint> newRunData)
    {
        // For simplicity, we're not comparing scores here. We'll just save the latest run.
        // In a full implementation, you'd compare the score of the new run with the saved run.
        SaveManager.Instance.SaveBestGhostRun(newRunData);
        bestRunData = newRunData;
        playback.LoadGhostRun(bestRunData);
    }
}
