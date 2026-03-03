using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float laneWidth = 2.5f;

    public int currentLane { get; private set; } = 0; // Center lane by default
    private int minLane = -1;    // Default min lane
    private int maxLane = 1;     // Default max lane

    private void OnEnable()
    {
        // Subscribe to DecisionPathManager events to update lane constraints
        DecisionPathManager.OnPathSplit += HandlePathSplit;
        DecisionPathManager.OnPathMerge += HandlePathMerge;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        DecisionPathManager.OnPathSplit -= HandlePathSplit;
        DecisionPathManager.OnPathMerge -= HandlePathMerge;
    }

    /// <summary>
    /// Dynamically restricts player movement to the available lanes during a path split.
    /// </summary>
    private void HandlePathSplit(int min, int max)
    {
        minLane = min;
        maxLane = max;

        // --- SAFETY: Clamp player to the new lane constraints immediately ---
        // This prevents the player from being in an invalid lane when the split occurs.
        currentLane = Mathf.Clamp(currentLane, minLane, maxLane);
        UpdatePosition();
    }

    /// <summary>
    /// Restores the default lane constraints after the paths merge.
    /// </summary>
    private void HandlePathMerge()
    {
        // --- AUTO-RESTORE: Restore default lane constraints ---
        minLane = -1;
        maxLane = 1;
    }

    /// <summary>
    /// --- PRESERVED & ADAPTED: Original input logic now respects dynamic lane constraints ---
    /// This method would be called by an Input Manager.
    /// </summary>
    public void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;

        // Only allow lane change if it's within the current (dynamic or default) bounds
        if (targetLane >= minLane && targetLane <= maxLane)
        {
            currentLane = targetLane;
            UpdatePosition();
        }
    }

    /// <summary>
    /// --- PRESERVED: Original position update logic remains the same ---
    /// </summary>
    private void UpdatePosition()
    {
        // This logic correctly positions the player based on their current lane index.
        transform.position = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
    }
}
