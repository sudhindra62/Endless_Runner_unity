
using UnityEngine;

/// <summary>
/// A collectible that grants the player a temporary speed boost.
/// It adds a SpeedBoostPowerUp to the PowerUpManager when collected.
/// </summary>
[RequireComponent(typeof(Collider))]
public class SpeedBoost : MonoBehaviour
{
    [Header("Power-Up Configuration")]
    [SerializeField] private float duration = 10f;
    [SerializeField] private float speedMultiplier = 1.5f;

    [Header("VFX & SFX")]
    [SerializeField] private GameObject collectionEffect;
    [SerializeField] private AudioClip collectionSound;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // Use the ServiceLocator to get the PowerUpManager instance.
        PowerUpManager powerUpManager = ServiceLocator.Get<PowerUpManager>();
        if (powerUpManager == null)
        {
            Debug.LogError("PowerUpManager not found in the scene!");
            return;
        }

        // Create and add the speed boost effect.
        powerUpManager.AddPowerUp(new SpeedBoostPowerUp(duration, speedMultiplier));

        // --- Visual and Audio Feedback ---

        // Play sound at the pickup's position.
        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        }

        // Instantiate a visual effect for collection.
        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }

        // For simplicity, we disable the object. In a real project, this would be returned to an object pool.
        gameObject.SetActive(false);
    }
}
