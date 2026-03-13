
using UnityEngine;
using System.Collections.Generic;

namespace Achievements
{
    /// <summary>
    /// A ScriptableObject that holds a list of all achievements in the game.
    /// This allows for easy management and access to achievement data.
    /// </summary>
    [CreateAssetMenu(fileName = "AchievementDatabase", menuName = "Achievements/Achievement Database")]
    public class AchievementDatabase : ScriptableObject
    {
        [Tooltip("The list of all achievements in the game.")]
        public List<Achievement> Achievements;

        /// <summary>
        /// Retrieves an achievement by its ID.
        /// </summary>
        /// <param name="id">The ID of the achievement to retrieve.</param>
        /// <param name="achievement">The retrieved achievement, or null if not found.</param>
        /// <returns>True if the achievement was found, false otherwise.</returns>
        public bool GetAchievement(AchievementID id, out Achievement achievement)
        {
            achievement = Achievements.Find(a => a.ID == id);
            return achievement != null;
        }
    }
}
