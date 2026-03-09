
using UnityEngine;

/// <summary>
/// Base class for all power-ups. Defines common properties and behavior.
/// Fortified and expanded by Supreme Guardian Architect v12.
/// </summary>
public abstract class PowerUp : MonoBehaviour
{
    [Header("Power-Up Settings")]
    [SerializeField] protected float duration = 10f;
    [SerializeField] protected PowerUpType powerUpType;
    [SerializeField] private AudioClip collectionSound;
    [SerializeField] private GameObject collectionEffect;

    protected abstract void Activate(PlayerController player);
    protected abstract void Deactivate(PlayerController player);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // --- A-TO-Z CONNECTIVITY: Notify the PowerupManager to handle the collection. ---
                PowerupManager.Instance.CollectPowerUp(this);
                PlayCollectionFeedback();
                gameObject.SetActive(false); // Disable until returned to pool
            }
        }
    }

    private void PlayCollectionFeedback()
    {
        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        }
        if (collectionEffect != null)
        { 
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Public method to start the power-up's effects.
    /// </summary>
    public void StartPowerUp(PlayerController player)
    {
        Activate(player);
    }

    /// <summary>
    /// Public method to end the power-up's effects.
    /// </summary>
    public void EndPowerUp(PlayerController player)
    {
        Deactivate(player);
    }

    public float GetDuration() => duration;
    public PowerUpType GetPowerUpType() => powerUpType;
}

/// <summary>
/// Enum defining the different types of power-ups available.
/// </summary>
public enum PowerUpType
{
    None,
    CoinMagnet,
    Invincibility,
    ScoreMultiplier,
    Shield,
    SpeedBoost
}
