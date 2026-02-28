
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A static global registry for tracking all active obstacle GameObjects in the scene.
/// This provides a centralized place to access all obstacles without needing to use expensive find operations.
/// </summary>
public static class ObstacleRegistry
{
    // A private list to hold references to all registered and currently active obstacles.
    private static readonly List<GameObject> activeObstacles = new List<GameObject>();

    /// <summary>
    /// Adds an obstacle to the registry. This should be called when an obstacle is enabled or spawned.
    /// </summary>
    /// <param name="obstacle">The obstacle GameObject to register. It will not be added if it is null or already in the registry.</param>
    public static void Register(GameObject obstacle)
    {
        if (obstacle != null && !activeObstacles.Contains(obstacle))
        {
            activeObstacles.Add(obstacle);
        }
    }

    /// <summary>
    /// Removes an obstacle from the registry. This should be called when an obstacle is disabled or destroyed.
    /// </summary>
    /// <param name="obstacle">The obstacle GameObject to unregister.</param>
    public static void Unregister(GameObject obstacle)
    {
        if (obstacle != null)
        {
            activeObstacles.Remove(obstacle);
        }
    }

    /// <summary>
    /// Retrieves a new list containing all currently active obstacles.
    /// </summary>
    /// <returns>A copy of the list of active obstacles. Modifying this list will not affect the registry itself.</returns>
    public static List<GameObject> GetAllActiveObstacles()
    {
        // Return a copy to prevent external modification of the internal list.
        return new List<GameObject>(activeObstacles);
    }

    /// <summary>
    /// Clears the entire registry of all obstacles. This is useful for resetting the game state, 
    /// for instance, when restarting a run.
    /// </summary>
    public static void ClearAll()
    {
        activeObstacles.Clear();
    }
}
