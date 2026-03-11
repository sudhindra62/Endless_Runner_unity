using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Managers
{
    public class AchievementManager : MonoBehaviour
    {
        public List<Achievement> achievements = new List<Achievement>();

        public void UnlockAchievement(Achievement achievement)
        {
            if (!achievement.isUnlocked)
            {
                achievement.Unlock();
            }
        }
    }

    [System.Serializable]
    public class Achievement
    {
        public string achievementName;
        public string description;
        public bool isUnlocked;

        public void Unlock()
        {
            isUnlocked = true;
        }
    }
}
