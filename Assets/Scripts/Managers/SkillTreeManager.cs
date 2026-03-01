
using UnityEngine;
using System;
using System.Collections.Generic;

public class SkillTreeManager : Singleton<SkillTreeManager>
{
    public static event Action OnSkillTreeChanged;

    public int SkillPoints { get; private set; }
    public Dictionary<string, int> SkillNodeLevels { get; } = new Dictionary<string, int>();

    private const int MAX_SKILL_LEVEL = 5;

    public void AddSkillPoints(int amount)
    {
        if (amount < 0) return;
        SkillPoints += amount;
        OnSkillTreeChanged?.Invoke();
    }

    public bool UnlockSkillNode(string skillId)
    {
        if (SkillPoints <= 0)
        {
            Debug.LogWarning("Cannot unlock skill: Not enough skill points.");
            return false;
        }

        if (!SkillNodeLevels.ContainsKey(skillId))
        {
            SkillNodeLevels[skillId] = 0;
        }

        if (SkillNodeLevels[skillId] >= MAX_SKILL_LEVEL)
        {
            Debug.LogWarning($"Cannot unlock skill: {skillId} is already at max level.");
            return false;
        }

        // In a real game, you would check for incompatible nodes here.

        SkillPoints--;
        SkillNodeLevels[skillId]++;
        OnSkillTreeChanged?.Invoke();
        
        Debug.Log($"Unlocked skill {skillId}. New level: {SkillNodeLevels[skillId]}");
        return true;
    }

    // This would be called when a player levels up.
    public void GrantPointsForLevel(int level)
    {
        // Example: 1 point per level
        AddSkillPoints(1);
    }
}
