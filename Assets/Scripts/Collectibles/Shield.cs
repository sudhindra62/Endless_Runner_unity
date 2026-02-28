
using UnityEngine;

/// <summary>
/// A collectible that grants the player a temporary shield, protecting them from a single obstacle hit.
/// </summary>
public class Shield : MonoBehaviour
{
    [Tooltip("The tag used by the ObjectPooler for this collectible type.")]
    public string poolTag = "Shield";

    private ObjectPooler objectPooler;

    private void Start()
    {
        // Resolve the ObjectPooler dependency using the ServiceLocator
        objectPooler = ServiceLocator.Get<ObjectPooler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only the player can collect the shield
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        var powerupManager = ServiceLocator.Get<PowerUpManager>();
        if(powerupManager != null)
        {
            powerupManager.ActivateShield();
        }

        // Return the shield to the pool for reuse
        if (objectPooler != null)
        {
            objectPooler.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            // Fallback if the pooling system is not available
            Destroy(gameObject);
        }
    }
}
