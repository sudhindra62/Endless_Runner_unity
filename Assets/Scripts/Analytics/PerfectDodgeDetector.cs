using UnityEngine;

/// <summary>
/// This script is dedicated to detecting and analyzing "perfect" dodges.
/// It works closely with the PlayerMovement script to provide more granular data
/// on player performance, which is then fed into the analytics system.
/// </summary>
public class PerfectDodgeDetector : MonoBehaviour
{
    [Header("Dodge Detection Settings")]
    [Tooltip("The time window (in seconds) for a dodge to be considered 'perfect'.")]
    [SerializeField] private float perfectDodgeTimeWindow = 0.2f;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// This method would be called by the trigger system when a near-miss occurs.
    /// It then determines if the dodge was "perfect" and reports it to the analytics manager.
    /// </summary>
    public void OnNearMiss(float timeSinceDodgeInput)
    {
        if (playerMovement == null) return;

        if (timeSinceDodgeInput <= perfectDodgeTimeWindow)
        {
            playerMovement.SuccessfulDodge(timeSinceDodgeInput);
        }
        else
        {
            playerMovement.FailedDodge();
        }
    }
}
