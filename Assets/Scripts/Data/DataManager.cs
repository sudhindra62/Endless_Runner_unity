
using UnityEngine;
using System.IO;

namespace EndlessRunner.Data
{
    /// <summary>
    /// Manages the saving and loading of all persistent game data.
    /// </summary>
    public class DataManager : Core.Singleton<DataManager>
    {
        public GameData GameData { get; private set; }

        private const string SAVE_FILE_NAME = "game_data.json";
        private string saveFilePath;

        protected override void Awake()
        {
            base.Awake();
            saveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            LoadData();
        }

        public void LoadData()
        {
            if (File.Exists(saveFilePath))
            {
                try
                {
                    string json = File.ReadAllText(saveFilePath);
                    GameData = JsonUtility.FromJson<GameData>(json);

                    // If new data is added to GameData and the save file is old, it might be null.
                    if(GameData == null)
                    {
                        CreateNewGameData();
                    }
                    Debug.Log("DATA_MANAGER: Game data loaded successfully.");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"DATA_MANAGER: Failed to load data. Error: {e.Message}. Creating new data file.");
                    CreateNewGameData();
                }
            }
            else
            {
                Debug.Log("DATA_MANAGER: No save file found. Creating new data file.");
                CreateNewGameData();
            }
        }

        public void SaveData()
        {
            try
            {
                string json = JsonUtility.ToJson(GameData, true); // Using pretty print for readability
                File.WriteAllText(saveFilePath, json);
                Debug.Log("DATA_MANAGER: Game data saved successfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"DATA_MANAGER: Failed to save data. Error: {e.Message}");
            }
        }

        private void CreateNewGameData()
        {
            GameData = new GameData();
            SaveData();
        }
    }
}
