
using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Core;
using EndlessRunner.Data;

namespace EndlessRunner.Missions
{
    /// <summary>
    /// Manages the player's missions, tracking progress and assigning new ones.
    /// </summary>
    public class MissionManager : Singleton<MissionManager>
    {
        [Header("Mission Pool")]
        [SerializeField] private List<MissionDefinition> allMissions;

        private Mission currentMission;
        private int lastPlayerXPosition = 0; // For distance tracking

        #region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();
            LoadMission();
        }

        private void OnEnable()
        {
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnScoreGained += HandleScoreGained;
            GameEvents.OnCoinsGained += HandleCoinsGained;
            GameEvents.OnMissionCompleted += HandleMissionCompleted;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnScoreGained -= HandleScoreGained;
            GameEvents.OnCoinsGained -= HandleCoinsGained;
            GameEvents.OnMissionCompleted -= HandleMissionCompleted;
            SaveMission();
        }

        private void Update()
        {
            if (currentMission != null && !currentMission.isCompleted && currentMission.type == MissionType.RunDistance && Player.PlayerController.Instance != null)
            {
                int currentPlayerX = (int)Player.PlayerController.Instance.transform.position.x;
                if(currentPlayerX > lastPlayerXPosition)
                {
                    currentMission.UpdateProgress(currentPlayerX - lastPlayerXPosition);
                    lastPlayerXPosition = currentPlayerX;
                }
            }
        }
        #endregion

        #region Public Accessors
        public Mission GetCurrentMission()
        {
            return currentMission;
        }
        #endregion

        #region Event Handlers
        private void HandleGameStart()
        {
            lastPlayerXPosition = 0;
        }

        private void HandleScoreGained(int amount)
        {
            if (currentMission != null && !currentMission.isCompleted && currentMission.type == MissionType.ScorePoints)
            {
                currentMission.UpdateProgress(amount);
            }
        }

        private void HandleCoinsGained(int amount)
        {
            if (currentMission != null && !currentMission.isCompleted && currentMission.type == MissionType.CollectCoins)
            {
                currentMission.UpdateProgress(amount);
            }
        }
        
        private void HandleMissionCompleted(Mission mission)
        {
            AssignNewMission();
        }
        #endregion

        #region Data Management
        private void LoadMission()
        {
            if (DataManager.Instance != null)
            {
                currentMission = DataManager.Instance.GameData.currentMission;
                if (currentMission == null || currentMission.isCompleted)
                {
                    AssignNewMission();
                }
            }
            else
            {
                AssignNewMission();
            }
        }

        private void SaveMission()
        {
            if (DataManager.Instance != null && currentMission != null)
            {
                DataManager.Instance.GameData.currentMission = currentMission;
                DataManager.Instance.SaveData();
            }
        }

        private void AssignNewMission()
        {
            // Simple logic: pick a random mission. Could be more complex.
            if (allMissions != null && allMissions.Count > 0)
            {
                MissionDefinition newMissionDef;
                do
                {
                    newMissionDef = allMissions[Random.Range(0, allMissions.Count)];
                } while (currentMission != null && newMissionDef.description == currentMission.description); // Avoid assigning the same mission twice

                currentMission = newMissionDef.CreateMission();
                lastPlayerXPosition = 0; // Reset for distance missions
                Debug.Log("MISSION_SYSTEM: New mission assigned: " + currentMission.description);
            }
            else
            { 
                Debug.LogWarning("MISSION_SYSTEM: No missions defined in the MissionManager!");
            }
        }
        #endregion
    }
}
