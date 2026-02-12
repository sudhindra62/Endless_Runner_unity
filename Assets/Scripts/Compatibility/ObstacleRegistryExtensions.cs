/// <summary>
/// Compatibility extension to satisfy legacy call sites:
/// ObstacleRegistry.Instance.ClearAll();
/// 
/// DOES NOT change behavior.
/// Routes call to static ObstacleRegistry safely.
/// </summary>
public static class ObstacleRegistryExtensions
{
    public static void ClearAll(this object _)
    {
        ObstacleRegistry_Clear();
    }

    private static void ObstacleRegistry_Clear()
    {
        // Clear via static access
        var obstacles = ObstacleRegistry.GetAllActiveObstacles();
        foreach (var obj in obstacles)
        {
            if (obj != null)
                ObstacleRegistry.Unregister(obj);
        }
    }
}
