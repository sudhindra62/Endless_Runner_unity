
using System.Collections.Generic;

public static class AchievementUnlockValidator
{
    // In a real implementation, this would be loaded from the save system
    private static HashSet<AchievementID> unlockedAchievements = new HashSet<AchievementID>();

    public static HashSet<AchievementID> UnlockedAchievements
    {
        get { return unlockedAchievements; }
    }

    public static bool CanUnlock(AchievementID id)
    {
        // 1. Prevent duplicate unlocks
        if (unlockedAchievements.Contains(id))
        {
            return false;
        }

        // 2. Prevent exploit triggers (placeholder for more complex logic)
        // This could involve checking timestamps, game state, etc.
        if (IsPotentialExploit(id))
        {
            return false;
        }

        return true;
    }

    public static void MarkAsUnlocked(AchievementID id)
    {
        if (!unlockedAchievements.Contains(id))
        {
            unlockedAchievements.Add(id);
            // In a real implementation, this would trigger a save.
        }
    }

    private static bool IsPotentialExploit(AchievementID id)
    {
        // Placeholder for exploit detection logic.
        // For example, you might check if multiple achievements are unlocked in an impossibly short time.
        return false;
    }

    public static void LoadUnlockedAchievements(HashSet<AchievementID> unlocked)
    {
        unlockedAchievements = unlocked ?? new HashSet<AchievementID>();
    }
}
