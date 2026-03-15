
using System;

namespace EndlessRunner.Data
{
    [Serializable]
    public class AchievementData
    {
        public string id;
        public int currentProgress;
        public bool isUnlocked;

        public AchievementData(string achievementId)
        {
            id = achievementId;
            currentProgress = 0;
            isUnlocked = false;
        }
    }
}
