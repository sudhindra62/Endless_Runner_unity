
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Gameplay
{
    /// <summary>
    /// Represents a collectible coin that triggers game events upon collection.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Coin : MonoBehaviour
    {
        [Header("Coin Settings")]
        [SerializeField] private int scoreValue = 10;
        [SerializeField] private int coinValue = 1;

        [Header("Effects")]
        [SerializeField] private GameObject collectionEffectPrefab;

        private bool isCollected = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isCollected) return;

            if (other.CompareTag("Player"))
            {
                isCollected = true;
                HandleCollection();
            }
        }

        private void HandleCollection()
        {
            // 1. Trigger Game Events
            GameEvents.TriggerScoreGained(scoreValue);
            GameEvents.TriggerCoinsGained(coinValue);

            // 2. Play visual effects
            if (collectionEffectPrefab != null)
            {
                Instantiate(collectionEffectPrefab, transform.position, Quaternion.identity);
            }

            // 3. Deactivate the coin
            gameObject.SetActive(false);
        }
    }
}
