
using UnityEngine;
using System.IO;
using Core;
using Data;

namespace Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
        private PlayerData _playerData;
        private string _savePath;

        protected override void Awake()
        {
            base.Awake();
            _savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            LoadPlayerData();
        }

        public PlayerData GetPlayerData()
        {
            return _playerData;
        }

        public void SavePlayerData(PlayerData data)
        {
            _playerData = data;
            string json = JsonUtility.ToJson(_playerData, true);
            File.WriteAllText(_savePath, json);
        }

        public void LoadPlayerData()
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                _playerData = JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                _playerData = new PlayerData();
            }
        }
    }
}
