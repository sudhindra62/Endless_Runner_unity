
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EndlessRunner.Data;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class LeaderboardManager : Singleton<LeaderboardManager>
    {
        private const int MaxLeaderboardEntries = 10;
        private const string LeaderboardDataKey = "LeaderboardData";

        public List<LeaderboardEntry> LeaderboardEntries { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LoadLeaderboard();
        }

        private void OnEnable()
        {
            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnGameOver -= HandleGameOver;
        }

        public void AddEntry(string playerName, int score)
        {
            LeaderboardEntries.Add(new LeaderboardEntry { playerName = playerName, score = score });
            LeaderboardEntries = LeaderboardEntries.OrderByDescending(e => e.score).Take(MaxLeaderboardEntries).ToList();
            SaveLeaderboard();
        }

        private void SaveLeaderboard()
        {
            LeaderboardSaveData saveData = new LeaderboardSaveData { Entries = LeaderboardEntries };
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(LeaderboardDataKey, json);
            PlayerPrefs.Save();
        }

        private void LoadLeaderboard()
        {
            if (PlayerPrefs.HasKey(LeaderboardDataKey))
            {
                string json = PlayerPrefs.GetString(LeaderboardDataKey);
                LeaderboardSaveData saveData = JsonUtility.FromJson<LeaderboardSaveData>(json);
                LeaderboardEntries = saveData.Entries;
            }
            else
            {
                LeaderboardEntries = new List<LeaderboardEntry>();
            }
        }

        private void HandleGameOver()
        {
            // Example of adding an entry. In a real game, you would get the player's name.
            AddEntry("Player", ScoreManager.Instance.Score);
        }
    }

    [System.Serializable]
    public class LeaderboardSaveData
    {
        public List<LeaderboardEntry> Entries;
    }
}
