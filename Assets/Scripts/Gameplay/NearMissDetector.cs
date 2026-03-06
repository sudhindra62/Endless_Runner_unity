using UnityEngine;
using System;

/// <summary>
/// A lightweight component attached to obstacles to detect potential near-misses.
/// It uses a trigger collider and fires an event when the player enters its zone.
/// This is a "fire and forget" component; all complex logic is handled by the NearMissManager.
/// </summary>
[RequireComponent(typeof(Collider))]
public class NearMissDetector : MonoBehaviour
{
    // --- Static Event ---
    // Annouces that a player has entered a near-miss zone. The manager will validate it.
    // Parameters: Obstacle Instance ID, Obstacle Position, Proximity (distance)
    public static event Action<int, Vector3, float> OnNearMissCandidate;

    [Header("Detection Setup")]
    [Tooltip("The layer the player is on. This must be set correctly.")]
    [SerializeField] private LayerMask playerLayer;

    // --- State ---
    private bool hasBeenTriggered = false;
    private Collider obstacleCollider; // Reference to the main collider of this obstacle

    private void Awake()
    {
        // We need the main collider to get the instance ID and position.
        // This assumes the detector is on a child object or the same object as the main collider.
        obstacleCollider = GetComponentInParent<Collider>();
        if (obstacleCollider == null)
        {
            Debug.LogError("[NearMissDetector] Could not find a parent obstacle collider!", this);
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // --- SAFETY CHECKS ---
        // 1. Avoid re-triggering for the same obstacle pass.
        if (hasBeenTriggered) return;

        // 2. Layer Check: Did the correct object enter the trigger?
        // This is a more performant check than comparing tags.
        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;

        // --- VALIDATION & EVENT ---
        hasBeenTriggered = true;

        float proximity = Vector3.Distance(transform.position, other.transform.position);

        // Fire the event with all necessary context for the manager to process.
        OnNearMissCandidate?.Invoke(obstacleCollider.GetInstanceID(), obstacleCollider.transform.position, proximity);

        // Optional: Disable the trigger collider after firing to save a tiny bit of performance.
        // GetComponent<Collider>().enabled = false;
    }
}
