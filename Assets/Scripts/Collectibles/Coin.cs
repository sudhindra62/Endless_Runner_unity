
using UnityEngine;

public class Coin : Collectible
{
    [Header("Coin Settings")]
    [Tooltip("The number of coins this collectible represents.")]
    public int value = 1;

    [Tooltip("The rotation speed of the coin for visual effect.")]
    public float rotationSpeed = 100f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem collectionParticles;
    [SerializeField] private AudioClip collectionSound;

    private bool hasBeenCollected = false;
    private bool isAttracted = false;
    private CurrencyManager currencyManager;

    protected override void Start()
    {
        base.Start();
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

        if (currencyManager != null)
        {
            currencyManager.AddCoins(value);
        }

        if (collectionParticles != null)
        {
            Instantiate(collectionParticles, transform.position, Quaternion.identity);
        }

        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        }
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
