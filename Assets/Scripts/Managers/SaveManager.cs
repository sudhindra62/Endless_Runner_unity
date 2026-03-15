
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace EndlessRunner.Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
        private const string SaveFileName = "/gameData.dat";
        private const string BackupSaveFileName = "/gameData.bak";
        private const string ChecksumKey = "YOUR_CHECKSUM_KEY"; // Replace with a unique key

        public GameData Data { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LoadGame();
        }

        public void SaveGame()
        {
            string savePath = Application.persistentDataPath + SaveFileName;
            string backupPath = Application.persistentDataPath + BackupSaveFileName;

            // Create a backup of the previous save file
            if (File.Exists(savePath))
            {
                File.Copy(savePath, backupPath, true);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Create);

            // Generate and save a checksum for data integrity
            Data.checksum = GenerateChecksum(JsonUtility.ToJson(Data));
            formatter.Serialize(stream, Data);
            stream.Close();
        }

        public void LoadGame()
        {
            string savePath = Application.persistentDataPath + SaveFileName;
            if (File.Exists(savePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(savePath, FileMode.Open);

                GameData loadedData = (GameData)formatter.Deserialize(stream);
                stream.Close();

                // Verify checksum to detect tampering
                if (loadedData.checksum == GenerateChecksum(JsonUtility.ToJson(loadedData)))
                {
                    Data = loadedData;
                }
                else
                {
                    Debug.LogWarning("Save file may be corrupted. Loading from backup.");
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
            }
            else
            {
                Debug.LogError("Both save and backup files are corrupted or missing!");
                Data = new GameData();
            }
        }

        private string GenerateChecksum(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data + ChecksumKey));
                return System.Convert.ToBase64String(hashedBytes);
            }
        }
    }

    [System.Serializable]
    public class GameData
    {
        public int coins;
        public int gems;
        public bool hasCompletedTutorial;
        public long lastDailyRewardTimestamp;
        public int dailyRewardStreak;
        public string checksum;
    }
}
