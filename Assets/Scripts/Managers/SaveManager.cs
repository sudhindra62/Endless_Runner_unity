
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using EndlessRunner.Core;
using EndlessRunner.Data;

namespace EndlessRunner.Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
        private readonly string _saveFileName = "player.dat";

        public SaveData LoadData()
        {
            string path = Path.Combine(Application.persistentDataPath, _saveFileName);
            if (File.Exists(path))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        return formatter.Deserialize(stream) as SaveData;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Guardian Architect Save System Error: Failed to load data from {path}. Reason: {e.Message}");
                    return new SaveData(); // Return a fresh instance on error
                }
            }
            return new SaveData(); // No save file found, return a fresh instance
        }

        public void SaveData(SaveData data)
        {
            string path = Path.Combine(Application.persistentDataPath, _saveFileName);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    formatter.Serialize(stream, data);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Guardian Architect Save System Error: Failed to save data to {path}. Reason: {e.Message}");
            }
        }
    }
}
