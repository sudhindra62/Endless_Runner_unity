
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private const string SAVE_FILE_NAME = "/player.dat";

    public static void SaveData(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + SAVE_FILE_NAME;
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + SAVE_FILE_NAME;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                SaveData data = formatter.Deserialize(stream) as SaveData;
                return data;
            }
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path + ". Creating a new one.");
            return new SaveData();
        }
    }
}
