using UnityEngine;

public class GhostRunManager : Singleton<GhostRunManager>
{
    [Header("Dependencies")]
    [SerializeField] public GhostRunRecorder recorder;
    [SerializeField] public GhostRunPlayback playback;

    private byte[] bestRunData;

    private void Start()
    {
        SaveManager.OnLoad += LoadBestGhostRun;
    }

    private void OnDestroy()
    {
        SaveManager.OnLoad -= LoadBestGhostRun;
    }

    private void LoadBestGhostRun(SaveData data)
    {
        bestRunData = data.bestGhostRunData;
        if (bestRunData != null && playback != null)
        {
            playback.StartPlayback(bestRunData);
        }
    }

    public void SaveNewBestRun(byte[] newRunData)
    {
        // In a full implementation, you'd compare the score of the new run with the saved run.
        bestRunData = newRunData;
        SaveManager.Instance.GameData.bestGhostRunData = newRunData;
        SaveManager.Instance.SaveGame();
    }
}
