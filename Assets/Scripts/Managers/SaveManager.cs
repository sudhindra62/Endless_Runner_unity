
using UnityEngine;
using System;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public static event Action<GameData> OnBeforeSave;
    public static event Action<GameData> OnLoad;

    private const string GameSaveFileName = "gameSave.json";
    private const string ChecksumFileName = "gameSave.checksum";

    private string SavePath => Application.persistentDataPath;

    public void SaveGame()
    {
        GameData data = new GameData();
        OnBeforeSave?.Invoke(data);

        string json = JsonUtility.ToJson(data);
        string checksum = Checksum.Calculate(json);

        try
        {
            File.WriteAllText(Path.Combine(SavePath, GameSaveFileName), json);
            File.WriteAllText(Path.Combine(SavePath, ChecksumFileName), checksum);
            Debug.Log("Game saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public void LoadGame()
    {
        string saveFile = Path.Combine(SavePath, GameSaveFileName);
        string checksumFile = Path.Combine(SavePath, ChecksumFileName);

        if (!File.Exists(saveFile) || !File.Exists(checksumFile))
        {
            Debug.Log("No game save file found.");
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFile);
            string savedChecksum = File.ReadAllText(checksumFile);

            if (Checksum.Calculate(json) != savedChecksum)
            {
                Debug.LogError("Save file has been tampered with or is corrupt!");
                return;
            }

            GameData data = JsonUtility.FromJson<GameData>(json);
            if (data != null)
            {
                OnLoad?.Invoke(data);
                Debug.Log("Game loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to deserialize save data.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
        }
    }
}
