
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Manages the saving, loading, and selection of replays.
/// Provides access to recent runs, best runs, and friend replays.
/// </summary>
public class ReplayManager : Singleton<ReplayManager>
{
    private const string REPLAY_FOLDER = "/Replays/";
    private const int MAX_RECENT_REPLAYS = 10;

    public ReplayPlaybackController playbackController; // To be assigned in Inspector

    public void SaveReplay(ReplayData replay)
    {
        string directory = Application.persistentDataPath + REPLAY_FOLDER;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonUtility.ToJson(replay);
        string path = directory + replay.replayId + ".json";
        File.WriteAllText(path, json);

        // Also save as best run if criteria met
        // if (replay.score > GetBestRunReplay()?.score)
        // {
        //     File.WriteAllText(directory + "best_run.json", json);
        // }

        PruneOldReplays();
    }

    public ReplayData LoadReplay(string replayId)
    {
        string path = Application.persistentDataPath + REPLAY_FOLDER + replayId + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ReplayData>(json);
        }
        return null;
    }

    public List<ReplayData> GetAllRecentReplays()
    {
        string directory = Application.persistentDataPath + REPLAY_FOLDER;
        if (!Directory.Exists(directory)) return new List<ReplayData>();

        return Directory.GetFiles(directory, "*.json")
            .Where(f => !f.EndsWith("best_run.json")) // Exclude special replays
            .Select(f => LoadReplay(Path.GetFileNameWithoutExtension(f)))
            .Where(r => r != null)
            .OrderByDescending(r => r.dateRecorded)
            .ToList();
    }

    private void PruneOldReplays()
    {
        List<ReplayData> replays = GetAllRecentReplays();
        if (replays.Count > MAX_RECENT_REPLAYS)
        {
            for(int i = MAX_RECENT_REPLAYS; i < replays.Count; i++)
            {
                string path = Application.persistentDataPath + REPLAY_FOLDER + replays[i].replayId + ".json";
                if (File.Exists(path)) File.Delete(path);
            }
        }
    }

    public void PlayReplay(string replayId)
    {
        ReplayData data = LoadReplay(replayId);
        if (data != null)
        {
            // This would likely involve loading a specific "replay" scene first
            playbackController.StartPlayback(data);
        }
    }
}
