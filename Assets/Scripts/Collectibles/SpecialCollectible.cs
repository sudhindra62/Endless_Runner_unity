
using UnityEngine;

/// <summary>
/// Represents a special collectible item that triggers an immediate in-game effect.
/// In this case, it converts all active obstacles into coins.
/// </summary>
public class SpecialCollectible : MonoBehaviour
{
    [Tooltip("The tag used by the ObjectPooler for this collectible type.")]
    public string poolTag = "SpecialCollectible";

    private ObjectPooler objectPooler;
    private EffectsManager effectsManager;

    private void Start()
    {
        // Resolve dependencies using the ServiceLocator
        objectPooler = ServiceLocator.Get<ObjectPooler>();
        effectsManager = ServiceLocator.Get<EffectsManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (effectsManager != null)
        {
            effectsManager.ConvertAllObstaclesToCoins();
        }
        else
        {
            Debug.LogWarning("EffectsManager not found. Cannot convert obstacles.");
        }

        // Return the collectible to the pool
        if (objectPooler != null)
        {
            objectPooler.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            Debug.LogWarning("ObjectPooler not found. Cannot return collectible to the pool.");
            // Fallback to destroying the object if the pool is not available
            Destroy(gameObject);
        }
    }
}
