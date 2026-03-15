
using System.Collections.Generic;
using EndlessRunner.Missions;

namespace EndlessRunner.Data
{
    [System.Serializable]
    public class GameData
    {
        // Player Stats
        public int highScore;
        public int totalCoins;

        // Settings
        public bool hasCompletedTutorial;

        // Achievements
        public Dictionary<string, AchievementData> achievementData;
        
        // Missions
        public Mission currentMission;

        public GameData()
        {
            highScore = 0;
            totalCoins = 0;
            hasCompletedTutorial = false;
            achievementData = new Dictionary<string, AchievementData>();
            currentMission = null;
        }
    }
}
