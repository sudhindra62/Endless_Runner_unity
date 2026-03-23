using UnityEngine;

public class GhostRunManager : Singleton<GhostRunManager>
{
    [Header("Dependencies")]
    [SerializeField] public GhostRunRecorder recorder;
    [SerializeField] public GhostRunPlayback playback;

    private GhostRunData bestRunData;

    private void Start()
    {
        SaveManager.OnLoad += LoadBestGhostRun;
    }

    private void OnDestroy()
    {
        SaveManager.OnLoad -= LoadBestGhostRun;
    }

    private void LoadBestGhostRun()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.GameData != null)
        {
            bestRunData = SaveManager.Instance.GameData.bestGhostRunData;
            if (bestRunData != null && playback != null)
            {
                playback.StartPlayback(bestRunData.ToBytes());
            }
        }
    }

    public void SaveNewBestRun(GhostRunData newRunData)
    {
        if (newRunData != null)
        {
            SaveNewBestRun(newRunData.ToBytes());
        }
    }

    public void SaveNewBestRun(byte[] newRunData)
    {
        // In a full implementation, you'd compare the score of the new run with the saved run.
        bestRunData = GhostRunData.FromBytes(newRunData);
        SaveManager.Instance.GameData.bestGhostRunData = bestRunData;
        SaveManager.Instance.SaveGame();
    }
}
