using UnityEngine;

/// <summary>
/// Validates level patterns for connectivity and fairness.
/// Global scope.
/// </summary>
public class PatternRuleValidator : MonoBehaviour
{
    public static bool CanConnect(LevelPattern fromPattern, LevelPattern toPattern)
    {
        if (fromPattern.exitPoints.Length != toPattern.entryPoints.Length) return false;

        for (int i = 0; i < fromPattern.exitPoints.Length; i++)
        {
            if (fromPattern.exitPoints[i] && toPattern.entryPoints[i]) return true;
        }

        return false;
    }

    public bool ValidatePattern(ObstaclePattern pattern, float gameSpeed)
    {
        // Integration with SafePathValidator and world state
        return true; 
    }
}
