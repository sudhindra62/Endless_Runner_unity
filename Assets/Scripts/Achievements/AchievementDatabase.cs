using UnityEngine;
using System.Collections.Generic;

namespace Achievements
{
    /// <summary>
    /// A ScriptableObject that serves as a database for all achievements in the game.
    /// This allows for easy editing of achievement data without modifying code.
    /// </summary>
    [CreateAssetMenu(fileName = "AchievementDatabase", menuName = "EndlessRunner/Achievement Database", order = 1)]
    public class AchievementDatabase : ScriptableObject
    {
        public List<Achievement> achievements;
    }
}
