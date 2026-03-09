
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct ObstaclePlacementData
{
    public int laneIndex; // e.g., 0 for left, 1 for middle, 2 for right
    public GameObject obstaclePrefab;

    public ObstaclePlacementData(int lane, GameObject prefab)
    {
        laneIndex = lane;
        obstaclePrefab = prefab;
    }
}

[System.Serializable]
public class ObstaclePattern
{
    public List<ObstaclePlacementData> obstacles;
    public ObstaclePattern()
    {
        obstacles = new List<ObstaclePlacementData>();
    }
}

public class ProceduralPatternGenerator : Singleton<ProceduralPatternGenerator>
{
    [Header("Obstacle Prefabs")]
    [SerializeField] private GameObject[] easyObstacles;
    [SerializeField] private GameObject[] mediumObstacles;
    [SerializeField] private GameObject[] hardObstacles;

    private enum PatternType { Wall, Alternating, Single }

    public ObstaclePattern GeneratePattern(float difficulty)
    {
        ObstaclePattern pattern = null;
        for (int i = 0; i < 5; i++) // Try a few times to generate a fair pattern
        {
            pattern = CreatePattern(difficulty);
            if (IsPatternFair(pattern))
            {
                return pattern;
            }
        }
        Debug.LogWarning("Could not generate a fair pattern. Returning last attempt.");
        return pattern;
    }

    private ObstaclePattern CreatePattern(float difficulty)
    {
        var pattern = new ObstaclePattern();
        PatternType type = (PatternType)Random.Range(0, System.Enum.GetValues(typeof(PatternType)).Length);
        
        GameObject[] selectedPrefabs = easyObstacles;
        if (difficulty > 0.75f) selectedPrefabs = hardObstacles;
        else if (difficulty > 0.4f) selectedPrefabs = mediumObstacles;

        if (selectedPrefabs.Length == 0) return pattern;

        switch (type)
        {
            case PatternType.Wall:
                int safeLane = Random.Range(0, 3); // 3 lanes
                for (int i = 0; i < 3; i++)
                {
                    if (i != safeLane)
                    {
                        pattern.obstacles.Add(new ObstaclePlacementData(i, selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                    }
                }
                break;

            case PatternType.Alternating:
                int startLane = Random.Range(0, 2); // 0 or 1
                pattern.obstacles.Add(new ObstaclePlacementData(startLane, selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                pattern.obstacles.Add(new ObstaclePlacementData(startLane + 1, selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                break;
                
            case PatternType.Single:
                pattern.obstacles.Add(new ObstaclePlacementData(Random.Range(0, 3), selectedPrefabs[Random.Range(0, selectedPrefabs.Length)]));
                break;
        }
        return pattern;
    }

    private bool IsPatternFair(ObstaclePattern pattern)
    {
        if (pattern == null || pattern.obstacles == null) return false;

        var occupiedLanes = new bool[3]; // Assuming 3 lanes
        foreach (var obstacle in pattern.obstacles)
        {
            if (obstacle.laneIndex >= 0 && obstacle.laneIndex < 3)
            {
                occupiedLanes[obstacle.laneIndex] = true;
            }
        }

        // Return true if at least one lane is not occupied.
        return occupiedLanes.Contains(false);
    }
}
