
using UnityEngine;

/// <summary>
/// Handles the behavior of a "Bonus Coin" collectible.
/// When collected, it adds value to a special currency type and then deactivates.
/// </summary>
[RequireComponent(typeof(Collider))]
public class BonusCoin : MonoBehaviour
{
    /// <summary>
    /// The type of special currency this coin represents.
    /// </summary>
    public SpecialCurrencyType currencyType = SpecialCurrencyType.BonusCoin;

    /// <summary>
    /// The amount of currency to award upon collection.
    /// </summary>
    public int value = 1;

    /// <summary>
    /// The rotation speed of the coin for visual effect.
    /// </summary>
    public float rotationSpeed = 100f;

    // --- Private Fields ---
    private bool collected = false;

    void Start()
    {
        // Ensure the collider is a trigger to avoid physics collisions
        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        // Add a simple rotation animation for visual appeal
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Called when another collider enters this object's trigger.
    /// </summary>
    /// <param name="other">The Collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the coin has already been collected and if the collider is the player
        if (!collected && other.CompareTag("Player"))
        {
            Collect();
        }
    }

    /// <summary>
    /// Handles the collection logic for the coin.
    /// </summary>
    private void Collect()
    {
        collected = true;

        // Add currency via the SpecialCurrencyManager
        if (SpecialCurrencyManager.Instance != null)
        {
            SpecialCurrencyManager.Instance.AddCurrency(currencyType, value);
        }
        else
        {
            Debug.LogWarning("SpecialCurrencyManager.Instance is not found in the scene.");
        }

        // Play a particle effect or sound here if desired
        // TODO: Add particle effect instantiation

        // Deactivate the coin object
        gameObject.SetActive(false);

        // Optional: Can be returned to an object pool instead of deactivating
    }
}
