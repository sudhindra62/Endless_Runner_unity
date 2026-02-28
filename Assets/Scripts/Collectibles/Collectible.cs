
using UnityEngine;

/// <summary>
/// A base class for all collectible items in the game.
/// </summary>
public abstract class Collectible : MonoBehaviour
{
    [Tooltip("The tag used by the ObjectPooler for this collectible type.")]
    [SerializeField] protected string poolTag;

    protected ObjectPooler objectPooler;

    protected virtual void Start()
    {
        objectPooler = ServiceLocator.Get<ObjectPooler>();
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
        OnCollect();

        if (objectPooler != null)
        {
            objectPooler.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This method is called when the collectible is collected by the player.
    /// Subclasses should override this method to implement their specific collection behavior.
    /// </summary>
    protected abstract void OnCollect();
}
