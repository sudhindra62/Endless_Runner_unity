
using UnityEngine;
using Managers; // Required to communicate with the PowerUpManager

/// <summary>
/// Represents a collectible power-up item within the game world.
/// When collected by the player, it activates its associated power-up via the PowerUpManager.
/// Created by Supreme Guardian Architect v13 to bridge the gap between data and gameplay.
/// </summary>
[RequireComponent(typeof(Collider))] // Must be able to collide with the player
public class PowerUpCollectible : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The ScriptableObject that defines the behavior of this power-up.")]
    [SerializeField] private PowerUpDefinition powerUpDefinition;

    [Header("Visual Effects")]
    [Tooltip("Particle effect to play upon collection.")]
    [SerializeField] private GameObject collectionEffect;

    [Tooltip("The part of the object that rotates, for visual appeal.")]
    [SerializeField] private Transform rotatingVisual;

    [Tooltip("The speed at which the visual model rotates.")]
    [SerializeField] private float rotationSpeed = 100f;

    private void Awake()
    {
        // Ensure the collider is set to be a trigger so it doesn't physically block the player.
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        // Add a simple rotation to make the collectible more visually appealing.
        if (rotatingVisual != null)
        {
            rotatingVisual.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Called by Unity when another collider enters this object's trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // We only care about collisions with the player.
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    /// <summary>
    /// Handles the collection logic.
    /// </summary>
    private void Collect()
    {
        if (powerUpDefinition == null)
        {
            Debug.LogError("Guardian Architect CRITICAL ERROR: PowerUpCollectible collected but has no PowerUpDefinition assigned!");
            return;
        }

        // A-TO-Z CONNECTIVITY: Communicate with the central manager to activate the power-up.
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.ActivatePowerUp(powerUpDefinition);
        }

        // Trigger visual/audio feedback.
        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }
        
        // The SoundManager would be called here if it exists and is set up.
        // SoundManager.Instance.PlaySound(powerUpDefinition.collectionSound);

        // Deactivate the collectible. It will be returned to the pool by a managing system.
        gameObject.SetActive(false);
    }
}
