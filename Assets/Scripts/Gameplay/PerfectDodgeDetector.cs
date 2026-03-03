
using UnityEngine;

public class PerfectDodgeDetector : MonoBehaviour
{
    [Tooltip("The layer containing obstacles to check for a perfect dodge.")]
    [SerializeField] private LayerMask obstacleLayer;

    [Tooltip("The cooldown in seconds before another perfect dodge can be registered.")]
    [SerializeField] private float perfectDodgeCooldown = 1f;

    private float lastDodgeTime;
    private bool isObstacleInRange;

    /// <summary>
    /// Called by the PlayerController when a dodge input is received.
    /// Checks if an obstacle is in range to award a perfect dodge.
    /// </summary>
    public void CheckForPerfectDodge()
    {
        if (isObstacleInRange && Time.time > lastDodgeTime + perfectDodgeCooldown)
        {
            lastDodgeTime = Time.time;
            FlowComboManager.Instance.AddToCombo(2); // Award 2 style points for a perfect dodge.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((obstacleLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            isObstacleInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((obstacleLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            isObstacleInRange = false;
        }
    }

    /// <summary>
    /// Resets the detector's state, typically called when the player resets.
    /// </summary>
    public void ResetDetector()
    {
        isObstacleInRange = false;
    }
}
