using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

/// <summary>
/// Manages binary and JSON persistence for game data.
/// Global scope Singleton for project-wide accessibility.
/// Includes checksum verification and backup systems.
/// </summary>
public class SaveManager : Singleton<SaveManager>
{
    private const string SaveFileName = "/gameData.dat";
    private const string BackupSaveFileName = "/gameData.bak";
    private const string ChecksumKey = "ANTIGRAVITY_SIG_KEY_2026"; 

    public static event System.Action OnLoad;
    public static event System.Action OnBeforeSave;
    public GameData Data { get; private set; }
    public GameData GameData => Data;

    protected override void Awake()
    {
        base.Awake();
        LoadGame();
    }

    public void SaveGame()
    {
        OnBeforeSave?.Invoke();
        string savePath = Application.persistentDataPath + SaveFileName;
        string backupPath = Application.persistentDataPath + BackupSaveFileName;

        if (File.Exists(savePath))
        {
            File.Copy(savePath, backupPath, true);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        Data.checksum = GenerateChecksum(GetStableDataString(Data));
        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public void LoadGame()
    {
        string savePath = Application.persistentDataPath + SaveFileName;
        if (File.Exists(savePath))
        {
            try {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(savePath, FileMode.Open);
                GameData loadedData = (GameData)formatter.Deserialize(stream);
                stream.Close();

                if (loadedData.checksum == GenerateChecksum(GetStableDataString(loadedData)))
                {
                    Data = loadedData;
                    OnLoad?.Invoke();
                }
                else
                {
                    Debug.LogWarning("Save file checksum mismatch. Loading backup.");
                    LoadBackup();
                }
            } catch {
                LoadBackup();
            }
        }
        else
        {
            Data = new GameData();
        }
    }

    private void LoadBackup()
    {
        string backupPath = Application.persistentDataPath + BackupSaveFileName;
        if (File.Exists(backupPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(backupPath, FileMode.Open);
            GameData loadedData = (GameData)formatter.Deserialize(stream);
            stream.Close();
            Data = loadedData;
            OnLoad?.Invoke();
        }
        else
        {
            Data = new GameData();
        }
    }

    public void DeleteSaveFile()
    {
        string savePath = Application.persistentDataPath + SaveFileName;
        if (File.Exists(savePath)) File.Delete(savePath);
        Data = new GameData();
        Debug.Log("[SaveManager] Save file deleted. Game reset to default.");
    }

    public bool SaveFileExists()
    {
        return File.Exists(Application.persistentDataPath + SaveFileName);
    }

    public void SavePlayerProgress(PlayerStats stats)
    {
        if (Data == null) Data = new GameData();
        Data.totalCoins = stats.coins;
        Data.gems = stats.gems;
        SaveGame();
    }

    public PlayerStats LoadPlayerProgress()
    {
        return new PlayerStats 
        { 
            coins = Data?.totalCoins ?? 0,
            gems = Data?.gems ?? 0
        };
    }

    public void SaveThemeProgress(ThemeProgress data)
    {
        if (Data == null) Data = new GameData();
        SaveGame();
    }

    public ThemeProgress LoadThemeProgress()
    {
        return new ThemeProgress();
    }

    private string GetStableDataString(GameData data)
    {
        // Use a combination of core primitive fields to create a stable signature.
        // Dictionaries are ignored as JsonUtility doesn't support them.
        return $"{data.highScore}|{data.totalCoins}|{data.gems}|{data.playerLevel}|{data.currentXP}|{data.pityCounter}";
    }

    private string GenerateChecksum(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data + ChecksumKey));
            return System.Convert.ToBase64String(hashedBytes);
        }
    }

    // --- Persistence Methods for Architectural Sync (Folder 7) ---
    public long LoadLastSpinTime() => Data.homeScreenLastRewardTimestamp;
    public void SaveLastSpinTime(long ticks)
    {
        Data.homeScreenLastRewardTimestamp = ticks;
        SaveGame();
    }

    public int LoadAdSpinsUsed() => Data != null ? Data.adSpinsUsedToday : 0;
    public void SaveAdSpinsUsed(int count)
    {
        if (Data == null) Data = new GameData();
        Data.adSpinsUsedToday = count;
        SaveGame();
    }

    public Dictionary<string, int> GetShardInventory() => Data.shardInventory;
    public void SetShardInventory(Dictionary<string, int> inventory)
    {
        Data.shardInventory = inventory;
        SaveGame();
    }
}
