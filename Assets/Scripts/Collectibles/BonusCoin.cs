
using UnityEngine;

/// <summary>
/// A collectible that awards the player with a bonus number of coins.
/// </summary>
public class BonusCoin : MonoBehaviour
{
    [Tooltip("The number of bonus coins to award the player.")]
    [SerializeField] private int coinValue = 10;

    [Tooltip("The tag used by the ObjectPooler for this collectible type.")]
    public string poolTag = "BonusCoin";

    private ObjectPooler objectPooler;

    private void Start()
    {
        // Resolve the ObjectPooler dependency using the ServiceLocator
        objectPooler = ServiceLocator.Get<ObjectPooler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only the player can collect the bonus coin
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // Use the CurrencyManager to add the coins
        CurrencyManager currencyManager = ServiceLocator.Get<CurrencyManager>();
        if (currencyManager != null)
        {
            currencyManager.AddCoins(coinValue);
        }

        // Return the bonus coin to the pool for reuse
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
