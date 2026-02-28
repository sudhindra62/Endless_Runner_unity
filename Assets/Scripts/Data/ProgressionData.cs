using UnityEngine;

/// <summary>
/// Configures player progression, including the XP curve for leveling up.
/// </summary>
[CreateAssetMenu(fileName = "ProgressionData", menuName = "RPG/Progression Data")]
public class ProgressionData : ScriptableObject
{
    public int maxLevel = 100;
    public AnimationCurve xpCurve;

    public int GetXPForLevel(int level)
    {
        if (level <= 0) return 0;
        return (int)xpCurve.Evaluate((float)level / maxLevel);
    }
}
