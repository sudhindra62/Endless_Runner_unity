using UnityEngine;

[CreateAssetMenu(fileName = "SkillNodeData", menuName = "Endless Runner/Skill Node Data")]
public class SkillNodeData : ScriptableObject
{
    public string nodeId;
    public string skillID => nodeId;
    public string displayName;
    public int maxLevel;
    
    [HideInInspector]
    public int currentLevel;

    public float baseModifierValue;
    public float incrementPerLevel;
    public ModifierType modifierType;

    public float GetCurrentModifierValue()
    {
        if (currentLevel == 0) return 0; // No bonus if not upgraded
        return baseModifierValue + (incrementPerLevel * (currentLevel - 1));
    }
}

