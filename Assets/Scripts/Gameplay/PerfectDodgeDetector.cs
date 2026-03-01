
using UnityEngine;

/// <summary>
/// Detects when the player performs a "perfect dodge" by narrowly avoiding an obstacle.
/// When a successful dodge is detected, it notifies the FlowComboManager.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PerfectDodgeDetector : MonoBehaviour
{
    [Tooltip("The layer that contains the obstacles to be dodged.")]
    [SerializeField] private LayerMask obstacleLayer;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with is on the obstacle layer.
        if ((obstacleLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // Notify the FlowComboManager that a perfect dodge occurred.
            FlowComboManager.Instance.AddToCombo();
            Debug.Log("Perfect Dodge! Combo increased.");
        }
    }
}
