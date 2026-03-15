
using System.Collections.Generic;
using EndlessRunner.Generation.Patterns;

namespace EndlessRunner.Generation.Rules
{
    /// <summary>
    /// A static utility class to validate the internal integrity of a LevelPattern.
    /// It ensures that there is at least one traversable path from an entry point to an exit point.
    /// </summary>
    public static class SafePathValidator
    {
        /// <summary>
        /// Analyzes a LevelPattern to determine if a safe, traversable path exists within it.
        /// </summary>
        /// <param name="pattern">The pattern to validate.</param>
        /// <returns>True if a path from an entry lane to an exit lane is found.</returns>
        public static bool IsPathSafe(LevelPattern pattern)
        {
            var queue = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();

            // 1. Add all open entry points to the queue
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

            // 2. Perform a Breadth-First Search (BFS) to find a path
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                // 3. Check if we have reached a valid exit
                if (currentNode.y == pattern.patternLength - 1) // Is it in the last row?
                {
                    if (pattern.exitPoints[currentNode.x]) // Is the corresponding exit open?
                    {
                        return true; // Path found!
                    }
                }

                // 4. Explore neighbors (left, right, forward)
                Vector2Int[] neighbors = {
                    new Vector2Int(currentNode.x, currentNode.y + 1), // Forward
                    new Vector2Int(currentNode.x - 1, currentNode.y), // Left
                    new Vector2Int(currentNode.x + 1, currentNode.y)  // Right
                };

                foreach (var neighbor in neighbors)
                {
                    // Check bounds and if already visited
                    if (neighbor.x >= 0 && neighbor.x < pattern.patternWidth &&
                        neighbor.y >= 0 && neighbor.y < pattern.patternLength &&
                        !visited.Contains(neighbor))
                    {
                        // Check if the cell is traversable
                        if (pattern.GetGridItem(neighbor.x, neighbor.y) != PatternItemType.Obstacle)
                        {
                            visited.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            // 5. If the queue is exhausted and no exit was reached, the path is unsafe.
            UnityEngine.Debug.LogWarning($"SafePathValidator: Pattern '{pattern.patternID}' has no safe path from entry to exit!");
            return false;
        }
    }
}
