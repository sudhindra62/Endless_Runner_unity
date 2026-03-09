
using System.Collections;
using UnityEngine;
using Core;

namespace Gameplay
{
    /// <summary>
    /// Manages the behavior of a collectible coin, including its attraction to the player and collection.
    /// </summary>
    public class Coin : MonoBehaviour
    {
        [Header("Coin Parameters")]
        [Tooltip("The tag used to identify the coin's pool in the ObjectPool.")]
        [SerializeField] private string poolTag = "Coin";
        [SerializeField] private int coinValue = 1;

        [Header("Attraction")]
        [SerializeField] private float attractionSpeed = 15f;
        [SerializeField] private float collectionDistance = 1f;

        private bool _isCollected = false;

        /// <summary>
        /// Initiates the attraction of the coin towards a target transform (usually the player).
        /// </summary>
        public void AttractTo(Transform target)
        {
            if (!_isCollected)
            {
                StartCoroutine(AttractAndCollect(target));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isCollected)
            {
                Collect();
            }
        }

        private IEnumerator AttractAndCollect(Transform target)
        {
            _isCollected = true; // Mark as collected to prevent multiple triggers

            while (target != null && Vector3.Distance(transform.position, target.position) > collectionDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, attractionSpeed * Time.deltaTime);
                yield return null;
            }
            
            Collect();
        }

        private void Collect()
        {
            if (_isCollected) return; // Ensure collection logic runs only once
            _isCollected = true;

            GameEvents.OnCoinCollected?.Invoke(coinValue);
            
            // Reset state for when it's reused from the pool
            _isCollected = false; 

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
