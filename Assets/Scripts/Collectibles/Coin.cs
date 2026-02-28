
using UnityEngine;

public class Coin : Collectible
{
    [Tooltip("The number of coins this collectible represents.")]
    public int value = 1;

    [Tooltip("The rotation speed of the coin for visual effect.")]
    public float rotationSpeed = 100f;

    private bool hasBeenCollected = false;
    private bool isAttracted = false;
    
    // Dependencies to be resolved via ServiceLocator
    private CurrencyManager currencyManager;

    private void Awake()
    {
        // A more robust solution would be to ensure these services are ready before use.
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        poolTag = "Coin";
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
    
    protected override void OnCollect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;
        
        // Use the resolved CurrencyManager instance
        currencyManager?.AddCoins(value);
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
