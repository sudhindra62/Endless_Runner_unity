
using UnityEngine;

/// <summary>
/// Manages the behavior of a collectible item, such as a coin or gem.
/// </summary>
public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Coin, Gem }

    [Header("Configuration")]
    [SerializeField] private CollectibleType type = CollectibleType.Coin;
    [SerializeField] private int value = 1;

    [Header("Effects")]
    [SerializeField] private GameObject collectionEffect;
    [SerializeField] private AudioClip collectionSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        switch (type)
        {
            case CollectibleType.Coin:
                CurrencyManager.Instance.AddCoins(value);
                break;
            case CollectibleType.Gem:
                CurrencyManager.Instance.AddGems(value);
                break;
        }

        // --- Visual & Audio Feedback ---
        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }
        if (collectionSound != null)
        {
            // You would need a sound manager to play this properly
            // AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        }

        // Destroy the collectible so it can't be picked up again
        Destroy(gameObject);
    }
}
