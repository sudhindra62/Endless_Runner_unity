using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The central authority for generating fair, procedural challenge patterns.
/// Global scope.
/// </summary>
public class ProceduralPatternGenerator : Singleton<ProceduralPatternGenerator>
{
    [Header("Obstacle Prefabs")]
    [SerializeField] private GameObject[] easyObstacles;
    [SerializeField] private GameObject[] mediumObstacles;
    [SerializeField] private GameObject[] hardObstacles;

    private enum PatternType { Wall, Alternating, Single }

    public ObstaclePattern GeneratePattern(float difficulty)
    {
        for (int i = 0; i < 5; i++)
        {
            var pattern = CreatePattern(difficulty);
            if (SafePathValidator.IsPatternFair(pattern)) return pattern;
        }
        return CreatePattern(difficulty); // Fallback
    }

    private ObstaclePattern CreatePattern(float difficulty)
    {
        var pattern = new ObstaclePattern();
        PatternType type = (PatternType)Random.Range(0, 3);
        
        GameObject[] selectedPrefabs = easyObstacles;
        if (difficulty > 0.75f) selectedPrefabs = hardObstacles;
        else if (difficulty > 0.4f) selectedPrefabs = mediumObstacles;

        if (selectedPrefabs == null || selectedPrefabs.Length == 0) return pattern;

        switch (type)
        {
            case PatternType.Wall:
                int safeLane = Random.Range(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    if (i != safeLane)
                    {
                        pattern.obstacles.Add(new ObstaclePlacementData(i, selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                    }
                }
                break;

            case PatternType.Alternating:
                int startLane = Random.Range(0, 2);
                pattern.obstacles.Add(new ObstaclePlacementData(startLane, selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                pattern.obstacles.Add(new ObstaclePlacementData(startLane + 1, selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                break;
                
            case PatternType.Single:
                pattern.obstacles.Add(new ObstaclePlacementData(Random.Range(0, 3), selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                break;
        }
        return pattern;
    }
}
