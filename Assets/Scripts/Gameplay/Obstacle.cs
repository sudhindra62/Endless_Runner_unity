using UnityEngine;

/// <summary>
/// Base class for all obstacles in the game.
/// Handles collision with player and integration with power-up systems.
/// Global scope for project-wide accessibility.
/// </summary>
public class Obstacle : MonoBehaviour, IPooledObject
{
    [Header("Obstacle Settings")]
    public float damage = 1f;

    public virtual void OnObjectSpawn()
    {
        // Reset state if needed when reused from pool
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision(hit.gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision(collision.gameObject);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerCollision(other.gameObject);
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        // Priority 1: Shield Power-Up
        if (PowerUpManager.Instance != null && PowerUpManager.Instance.IsPowerUpActive(PowerUpType.Shield))
        {
            // Shield destroys the obstacle
            gameObject.SetActive(false);
            return;
        }

        // Priority 2: Fatal Collision
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }
}
