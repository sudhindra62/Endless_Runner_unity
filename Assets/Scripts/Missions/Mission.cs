
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.Missions
{
    [System.Serializable]
    public enum MissionType
    {
        RunDistance,
        CollectCoins,
        ScorePoints
    }

    [System.Serializable]
    public class Mission
    {
        public string description;
        public MissionType type;
        public float target;
        public float progress;
        public bool isCompleted;

        public Mission(string description, MissionType type, float target)
        {
            this.description = description;
            this.type = type;
            this.target = target;
            this.progress = 0;
            this.isCompleted = false;
        }

        public void UpdateProgress(float amount)
        {
            if (isCompleted) return;

            progress += amount;
            if (progress >= target)
            {
                progress = target;
                isCompleted = true;
                Debug.Log("MISSION_SYSTEM: Mission completed! '" + description + "'");
                GameEvents.TriggerMissionCompleted(this);
            }
        }
    }
}
