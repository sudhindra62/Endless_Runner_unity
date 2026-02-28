
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Tooltip("The tag used by the ObjectPooler for this coin type.")]
    public string poolTag = "Coin";

    [Tooltip("The number of coins this collectible represents.")]
    public int value = 1;

    [Tooltip("The rotation speed of the coin for visual effect.")]
    public float rotationSpeed = 100f;

    private bool hasBeenCollected = false;
    private bool isAttracted = false;
    
    // Dependencies to be resolved via ServiceLocator
    private CurrencyManager currencyManager;
    private ObjectPooler objectPooler;

    private void Awake()
    {
        // A more robust solution would be to ensure these services are ready before use.
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        objectPooler = ServiceLocator.Get<ObjectPooler>();
    }

    private void OnEnable()
    {
        hasBeenCollected = false;
        isAttracted = false;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenCollected)
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;
        
        // Use the resolved CurrencyManager instance
        currencyManager?.AddCoins(value);

        // Use the resolved ObjectPooler instance
        objectPooler?.ReturnToPool(poolTag, gameObject);
    }

    public bool IsAttracted()
    {
        return isAttracted;
    }

    public void SetAttracted(bool attracted)
    {
        isAttracted = attracted;
    }
}
