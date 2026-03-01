
using UnityEngine;

/// <summary>
/// Detects when the player performs a "near miss" by passing close to an obstacle.
/// When a near miss is detected, it notifies the FlowComboManager.
/// </summary>
[RequireComponent(typeof(Collider))]
public class NearMissDetector : MonoBehaviour
{
    [Tooltip("The layer that contains the obstacles for near misses.")]
    [SerializeField] private LayerMask obstacleLayer;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with is on the obstacle layer.
        if ((obstacleLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // Notify the FlowComboManager that a near miss occurred.
            FlowComboManager.Instance.AddToCombo();
            Debug.Log("Near Miss! Combo increased.");
        }
    }
}
