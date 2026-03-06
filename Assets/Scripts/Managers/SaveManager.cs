
using UnityEngine;
using System;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public static event Action<SaveData> OnBeforeSave;
    public static event Action<SaveData> OnLoad;
    
    public SaveData GameData { get; private set; }

    private const string GameSaveFileName = "gameSave.json";
    private const string ChecksumFileName = "gameSave.checksum";

    private string SavePath => Application.persistentDataPath;

    private void Awake()
    {
        GameData = new SaveData();
    }

    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        OnBeforeSave?.Invoke(GameData);

        string json = JsonUtility.ToJson(GameData);
        // INTEGRATION: Use IntegrityManager for checksum generation.
        string checksum = IntegrityManager.Instance.GenerateSaveChecksum(json);

        try
        {
            File.WriteAllText(Path.Combine(SavePath, GameSaveFileName), json);
            File.WriteAllText(Path.Combine(SavePath, ChecksumFileName), checksum);
            Debug.Log("Game saved successfully via Integrity-Guarded SaveManager.");
        }
        catch (Exception e)
        {
            IntegrityManager.Instance.ReportError($"Save I/O Failure: {e.Message}");
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public void LoadGame()
    {
        string saveFile = Path.Combine(SavePath, GameSaveFileName);
        string checksumFile = Path.Combine(SavePath, ChecksumFileName);

        if (!File.Exists(saveFile) || !File.Exists(checksumFile))
        {
            Debug.Log("No game save file found. Creating a new one.");
            SaveGame();
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFile);
            string savedChecksum = File.ReadAllText(checksumFile);

            // INTEGRATION: Use IntegrityManager for checksum validation.
            if (!IntegrityManager.Instance.ValidateSaveChecksum(json, savedChecksum))
            {
                IntegrityManager.Instance.ReportError("Save file checksum mismatch. Data may be corrupt or tampered with.");
                // FAILSAFE: As per requirements, do not load corrupt data. Can optionally restore from a backup.
                Debug.LogError("Save file has been tampered with or is corrupt! Halting load.");
                return;
            }

            SaveData data = JsonUtility.FromJson<SaveData>(json);
            if (data != null)
            { 
                GameData = data;
                OnLoad?.Invoke(GameData);
                Debug.Log("Game loaded successfully via Integrity-Guarded SaveManager.");
            }
            else
            {
                 IntegrityManager.Instance.ReportError("Save file deserialization failed.");
                Debug.LogError("Failed to deserialize save data.");
            }
        }
        catch (Exception e)
        {
            IntegrityManager.Instance.ReportError($"Load I/O Failure: {e.Message}");
            Debug.LogError($"Failed to load game: {e.Message}");
        }
    }
}
