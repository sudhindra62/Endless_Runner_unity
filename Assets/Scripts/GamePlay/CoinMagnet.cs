using UnityEngine;

/// <summary>
/// Handles the coin attraction effect when the magnet power-up is active.
/// This script is placed on the player and uses an efficient, centralized approach
/// to find and attract nearby coins, avoiding per-coin Update() calls.
/// </summary>
public class CoinMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float attractionSpeed = 20f;
    [SerializeField] private float attractionRadius = 5f; // Base radius, can be upgraded.
    [SerializeField] private LayerMask coinLayer; // Assign the 'Coin' layer in the inspector for efficient lookup.

    [Header("Performance")]
    [Tooltip("How often (in seconds) the magnet checks for nearby coins.")]
    [SerializeField] private float checkInterval = 0.1f; 

    private Transform playerTransform;
    private float checkTimer;

    // Cached singleton reference for performance
    private PowerUpEffects powerUpEffects;

    private void Start()
    {
        playerTransform = transform;
        powerUpEffects = PowerUpEffects.Instance;

        // Set the coin layer if it's not assigned, as a fallback.
        if (coinLayer == 0) // LayerMask is 0 if unassigned.
        {
            coinLayer = LayerMask.GetMask("Coin");
        }
    }

    private void Update()
    {
        // Early exit if the magnet is not active or the singleton is missing.
        if (powerUpEffects == null || !powerUpEffects.IsMagnetActive) return;

        // Use a timer to throttle the physics check for performance.
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            checkTimer = checkInterval;
            AttractNearbyCoins();
        }
    }

    /// <summary>
    /// Finds all coins within the attraction radius and pulls them towards the player.
    /// This is far more performant than having each coin check its distance to the player.
    /// </summary>
    private void AttractNearbyCoins()
    {
        // Use OverlapSphere for a highly efficient spatial query.
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, attractionRadius, coinLayer);

        foreach (var hitCollider in hitColliders)
        {
            // Move the coin's transform directly towards the player.
            hitCollider.transform.position = Vector3.MoveTowards(
                hitCollider.transform.position, 
                playerTransform.position, 
                attractionSpeed * Time.deltaTime
            );
        }
    }
}
