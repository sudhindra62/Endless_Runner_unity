/// <summary>
/// COMPATIBILITY EXTENSION for legacy instance-style calls.
/// Does NOT change ObstacleRegistry behavior.
/// </summary>
public static class ObstacleRegistryCompatibility
{
    // 🔹 Allows: ObstacleRegistry.Instance.ClearAll()
    public static void ClearAll(this object _)
    {
        // Forward safely to the real static registry
        ClearAllStatic();
    }

    // 🔹 Internal static forwarder (kept explicit & safe)
    private static void ClearAllStatic()
    {
        // Remove all registered obstacles
        var obstacles = ObstacleRegistry.GetAllActiveObstacles();
        foreach (var obstacle in obstacles)
        {
            if (obstacle != null)
            {
                ObstacleRegistry.Unregister(obstacle);
            }
        }
    }
}
