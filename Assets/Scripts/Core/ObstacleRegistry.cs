using UnityEngine;
using System.Collections.Generic;

public static class ObstacleRegistry
{
    public static object Instance => null;

    private static readonly List<GameObject> activeObstacles = new List<GameObject>();

    public static void Register(GameObject obstacle)
    {
        if (obstacle != null && !activeObstacles.Contains(obstacle))
            activeObstacles.Add(obstacle);
    }

    public static void Unregister(GameObject obstacle)
    {
        if (obstacle != null)
            activeObstacles.Remove(obstacle);
    }

    public static List<GameObject> GetAllActiveObstacles()
    {
        return new List<GameObject>(activeObstacles);
    }

    // ✅ ADDITIVE — REQUIRED BY RestartController
    public static void ClearAll()
    {
        activeObstacles.Clear();
    }
}
