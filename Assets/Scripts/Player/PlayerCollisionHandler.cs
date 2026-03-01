
using UnityEngine;
using System;

/// <summary>
/// Handles the player's collision with obstacles and other game elements.
/// Notifies other systems when significant collision events occur.
/// </summary>
public class PlayerCollisionHandler : MonoBehaviour
{
    public static event Action OnPlayerHit;

    [Tooltip("The layer that contains the obstacles that can hurt the player.")]
    [SerializeField] private LayerMask obstacleLayer;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the object we collided with is on the obstacle layer.
        if ((obstacleLayer.value & (1 << hit.gameObject.layer)) > 0)
        {
            // Notify listeners that the player has been hit.
            OnPlayerHit?.Invoke();

            // Break the flow combo.
            FlowComboManager.Instance.BreakCombo();

            // Reset the coin streak.
            DataManager.Instance.ResetCoinStreak();

            Debug.Log("Player hit an obstacle! Combo broken and coin streak reset.");

            // --- Placeholder for actual death/damage logic ---
            // For example, you might call a method on a HealthManager:
            // HealthManager.Instance.TakeDamage(1);
        }
    }
}
