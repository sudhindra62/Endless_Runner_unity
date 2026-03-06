
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerAchievementSaveData
{
    public List<AchievementData> achievements;
    public HashSet<AchievementID> unlockedAchievements;

    public PlayerAchievementSaveData(List<AchievementData> achievements, HashSet<AchievementID> unlocked)
    {
        this.achievements = achievements;
        this.unlockedAchievements = unlocked;
    }
}

public static class SaveSystem
{
    public static void SaveGame(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamesave.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/gamesave.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveAchievements(PlayerAchievementSaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/achievements.dat";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
        Debug.Log("Achievements saved to " + path);
    }

    public static PlayerAchievementSaveData LoadAchievements()
    {
        string path = Application.persistentDataPath + "/achievements.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                PlayerAchievementSaveData data = formatter.Deserialize(stream) as PlayerAchievementSaveData;
                return data;
            }
        }
        else
        {
            Debug.LogWarning("Achievement save file not found in " + path);
            return null;
        }
    }
}
