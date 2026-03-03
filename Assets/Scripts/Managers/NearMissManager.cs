
using UnityEngine;
using System;

public class NearMissManager : MonoBehaviour
{
    #region EVENTS
    public static event Action OnNearMiss;
    #endregion

    #region CONFIGURATION
    [Header("Detection Settings")]
    [SerializeField] private float nearMissThreshold = 1.5f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float cooldownSeconds = 0.5f; // Prevent multiple triggers for one obstacle group
    #endregion

    #region STATE
    private Collider[] overlapResults = new Collider[10];
    private float lastNearMissTime = -1f;
    private int lastNearMissInstanceId = -1;
    #endregion

    private void OnEnable()
    {
        // Subscribe to player death to disable functionality if needed
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void Update()
    {
        // Performance: Only run detection if not on cooldown
        if (Time.time < lastNearMissTime + cooldownSeconds) return;

        DetectNearMiss();
    }

    private void DetectNearMiss()
    {
        int numFound = Physics.OverlapSphereNonAlloc(transform.position, nearMissThreshold, overlapResults, obstacleLayer);

        if (numFound > 0)
        {
            for (int i = 0; i < numFound; i++)
            {
                Collider obstacleCollider = overlapResults[i];
                int instanceId = obstacleCollider.GetInstanceID();

                // SAFETY RULE: Not trigger twice for same obstacle
                if (instanceId != lastNearMissInstanceId)
                {
                    // Validate player did NOT collide (This is implicitly handled by the fact that the player is not dead)
                    // Obstacle passed fully (This is a simplified check; for a more robust solution, we'd track obstacle positions)
                    
                    // Check if the player is actually moving past the obstacle, not just standing near it.
                    // This is a simplified check using the obstacle's bounds.
                    Vector3 closestPoint = obstacleCollider.bounds.ClosestPoint(transform.position);
                    float distance = Vector3.Distance(transform.position, closestPoint);

                    if (distance > 0.1f) // Ensure we are not currently colliding
                    {
                        lastNearMissTime = Time.time;
                        lastNearMissInstanceId = instanceId;

                        // Emit near-miss event
                        OnNearMiss?.Invoke();

                        // Stop checking after the first valid near miss in a frame to prevent event stacking
                        return; 
                    }
                }
            }
        }
        else
        {
            // Reset instance ID when no obstacles are nearby
            lastNearMissInstanceId = -1;
        }
    }

    private void HandlePlayerDeath()
    {
        // Disable near miss detection on death
        this.enabled = false;
    }
    
    // Called via animation event or GameManager on revive
    public void OnPlayerRevive()
    {
        this.enabled = true;
        lastNearMissTime = Time.time; // Add a brief cooldown on revive
        lastNearMissInstanceId = -1;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f); // Yellow, semi-transparent
        Gizmos.DrawSphere(transform.position, nearMissThreshold);
    }
#endif
}
