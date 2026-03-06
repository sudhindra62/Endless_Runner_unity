
using UnityEngine;

/// <summary>
/// Manages the behavior of a power-up item.
/// </summary>
public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Invincibility, ScoreMultiplier, Magnet }

    [Header("Configuration")]
    [SerializeField] private PowerUpType type;
    [SerializeField] private float duration = 10f;

    [Header("Effects")]
    [SerializeField] private GameObject collectionEffect;
    [SerializeField] private AudioClip collectionSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        // Activate the power-up on the player
        PowerUpManager.Instance.ActivatePowerUp(type, duration);

        // --- Visual & Audio Feedback ---
        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }
        if (collectionSound != null)
        {
            // AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        }

        Destroy(gameObject);
    }
}
