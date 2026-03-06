
using UnityEngine;
using System;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public static event Action<SaveData> OnBeforeSave;
    public static event Action<SaveData> OnLoad;
    
    public SaveData GameData { get; private set; }

    private const string GameSaveFileName = "gameSave.json";

    private string SavePath => Application.persistentDataPath;

    private void Awake()
    {
        // Ensure an IntegrityManager exists to handle save/load operations
        if (IntegrityManager.Instance == null)
        {
            gameObject.AddComponent<IntegrityManager>();
        }
        GameData = new SaveData();
    }

    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        // 1. Let other systems populate the SaveData object before saving.
        OnBeforeSave?.Invoke(GameData);

        // 2. Create a backup of the *current* valid game data before attempting to write.
        IntegrityManager.Instance.saveIntegrityGuard.CreateBackup(GameData);

        try
        {
            string json = JsonUtility.ToJson(GameData);
            File.WriteAllText(Path.Combine(SavePath, GameSaveFileName), json);
            Debug.Log("[SaveManager] Game saved successfully.");
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

        if (!File.Exists(saveFile))
        {
            Debug.Log("[SaveManager] No save file found. Creating a new one.");
            GameData = new SaveData(); // Start with fresh data
            SaveGame(); // Create the initial save file
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFile);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // 3. Validate the loaded data *before* accepting it.
            if (data != null && IntegrityManager.Instance.saveIntegrityGuard.ValidateSaveData(data))
            {
                GameData = data;
                OnLoad?.Invoke(GameData);
                Debug.Log("[SaveManager] Game loaded and validated successfully.");
            }
            else
            {
                // 4. If validation fails or data is null, restore from backup.
                IntegrityManager.Instance.ReportError("Save data failed validation. Attempting to restore backup.");
                GameData = IntegrityManager.Instance.saveIntegrityGuard.RestoreBackup();
                OnLoad?.Invoke(GameData);
            }
        }
        catch (Exception e)
        {
            // 5. If there's a file read error, restore from backup as a failsafe.
            IntegrityManager.Instance.ReportError($"Load I/O Failure: {e.Message}. Attempting to restore backup.");
            GameData = IntegrityManager.Instance.saveIntegrityGuard.RestoreBackup();
            OnLoad?.Invoke(GameData);
        }
    }
}
