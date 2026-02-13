using UnityEngine;

/// <summary>
/// A marker component for obstacles in the game.
/// This script has been optimized to have no active logic, reducing overhead.
/// All collision handling is now managed by the PlayerController for better performance.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour
{
    private void Awake()
    {
        // Ensure the collider is set to be a trigger.
        // This is a one-time setup cost and is more reliable than manual setup.
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnEnable()
    {
        // Register this obstacle with the ObstacleRegistry when it becomes active
        ObstacleRegistry.Register(gameObject);
    }

    private void OnDisable()
    {
        // Unregister this obstacle when it is no longer active
        ObstacleRegistry.Unregister(gameObject);
    }
}
