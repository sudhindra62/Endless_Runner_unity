
using UnityEngine;

[CreateAssetMenu(fileName = "ProgressionData", menuName = "Gameplay/Progression Data")]
public class ProgressionData : ScriptableObject
{
    public int[] XpPerLevel;

    public int GetXPForLevel(int level)
    {
        if (level > 0 && level <= XpPerLevel.Length)
        {
            return XpPerLevel[level - 1];
        }
        return 0; // Or a default value for levels beyond the defined curve
    }
}
