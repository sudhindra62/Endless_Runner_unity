
using UnityEngine;

public class NearMissDetector : MonoBehaviour
{
    [Tooltip("The layer containing obstacles that should trigger a near miss.")]
    [SerializeField] private LayerMask obstacleLayer;

    [Tooltip("The cooldown in seconds before another near miss can be registered.")]
    [SerializeField] private float nearMissCooldown = 0.5f;

    private float lastNearMissTime;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is on the obstacle layer.
        if ((obstacleLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // Ensure we are not in a cooldown period.
            if (Time.time > lastNearMissTime + nearMissCooldown)
            {
                // Update the time of the last near miss.
                lastNearMissTime = Time.time;

                // Notify the FlowComboManager to register the style point.
                FlowComboManager.Instance.AddToCombo(1); // Award 1 style point for a near miss.
            }
        }
    }
}
