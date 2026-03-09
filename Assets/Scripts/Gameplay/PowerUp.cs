
using UnityEngine;
using Core;
using Data;

namespace Gameplay
{
    /// <summary>
    /// Manages the behavior of a power-up collectible.
    /// </summary>
    public class PowerUp : MonoBehaviour
    {
        [Header("Power-Up Configuration")]
        [Tooltip("The type of this power-up.")]
        [SerializeField] private PowerUpType powerUpType = PowerUpType.None;

        [Tooltip("The tag used to return the power-up to the object pool.")]
        [SerializeField] private string poolTag = "PowerUp";

        [Tooltip("The visual effect to play when the power-up is collected.")]
        [SerializeField] private GameObject collectionEffect;

        [Tooltip("The duration of the power-up effect.")]
        [SerializeField] private float duration = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collect(other.gameObject);
            }
        }

        private void Collect(GameObject player)
        {
            // Notify the PowerUpEffectsController to apply the effect.
            PowerUpEffectsController effectsController = player.GetComponent<PowerUpEffectsController>();
            if (effectsController != null)
            {
                effectsController.ApplyEffect(powerUpType, duration);
            }

            // Play the collection effect.
            if (collectionEffect != null)
            {
                Instantiate(collectionEffect, transform.position, Quaternion.identity);
            }

            // Return the power-up to the pool.
            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
