using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Validates obstacle patterns to ensure they are physically navigable.
/// Global scope.
/// </summary>
public class SafePathValidator : MonoBehaviour
{
    public static bool IsPatternFair(ObstaclePattern pattern)
    {
        if (pattern == null || pattern.obstacles == null) return false;

        var occupiedLanes = new bool[3]; 
        foreach (var obstacle in pattern.obstacles)
        {
            if (obstacle.laneIndex >= 0 && obstacle.laneIndex < 3)
            {
                occupiedLanes[obstacle.laneIndex] = true;
            }
        }

        return occupiedLanes.Contains(false);
    }

    /// <summary>
    /// BFS-based validation for grid-based patterns.
    /// </summary>
    public static bool IsPathSafe(LevelPattern pattern)
    {
        var queue = new Queue<Vector2Int>();
        var visited = new HashSet<Vector2Int>();

        for (int x = 0; x < pattern.patternWidth; x++)
        {
            if (pattern.entryPoints[x])
            {
                var startNode = new Vector2Int(x, 0);
                if (pattern.GetGridItem(startNode.x, startNode.y) != PatternItemType.Obstacle)
                {
                    queue.Enqueue(startNode);
                    visited.Add(startNode);
                }
            }
        }

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            if (currentNode.y == pattern.patternLength - 1)
            {
                if (pattern.exitPoints[currentNode.x]) return true;
            }

            Vector2Int[] neighbors = {
                new Vector2Int(currentNode.x, currentNode.y + 1),
                new Vector2Int(currentNode.x - 1, currentNode.y),
                new Vector2Int(currentNode.x + 1, currentNode.y)
            };

            foreach (var neighbor in neighbors)
            {
                if (neighbor.x >= 0 && neighbor.x < pattern.patternWidth &&
                    neighbor.y >= 0 && neighbor.y < pattern.patternLength &&
                    !visited.Contains(neighbor))
                {
                    if (pattern.GetGridItem(neighbor.x, neighbor.y) != PatternItemType.Obstacle)
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return false;
    }
}
