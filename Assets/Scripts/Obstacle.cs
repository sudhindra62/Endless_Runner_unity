using UnityEngine;

/// <summary>
/// Unified Obstacle
/// Preserves:
/// - Shield handling
/// - Player death handling
/// - ObstacleRegistry integration
/// - Trigger enforcement
/// - Rigidbody safety setup
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    private void Awake()
    {
        // Ensure trigger
        GetComponent<Collider>().isTrigger = true;

        // Ensure physics safety
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        // Register for optimized lookup systems
        ObstacleRegistry.Register(gameObject);
    }

    private void OnDisable()
    {
        ObstacleRegistry.Unregister(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPowerUp power = other.GetComponent<PlayerPowerUp>();
        PlayerController player = other.GetComponent<PlayerController>();

        if (power != null && power.HasShield())
        {
            power.BreakShield();
            Destroy(gameObject);
            return;
        }

        if (player != null)
        {
            player.Die();
        }
    }
}
