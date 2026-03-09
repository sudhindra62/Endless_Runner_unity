
using System.Collections;
using UnityEngine;
using Core;

namespace Gameplay
{
    /// <summary>
    /// Manages the lifecycle of a barrier obstacle.
    /// It automatically returns itself to an object pool after a specified duration.
    /// </summary>
    public class Barrier : MonoBehaviour
    {
        [Tooltip("The tag used to identify the barrier's pool in the ObjectPool.")]
        [SerializeField] private string poolTag = "Barrier";

        [Tooltip("The duration in seconds before the barrier is returned to the pool.")]
        [SerializeField] private float lifetime = 2f;

        private void OnEnable()
        {
            StartCoroutine(DeactivateAfterTime());
        }

        /// <summary>
        /// A coroutine that waits for the barrier's lifetime to expire,
        /// then returns it to the object pool.
        /// </summary>
        private IEnumerator DeactivateAfterTime()
        {
            yield return new WaitForSeconds(lifetime);

            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
            }
            else
            {
                // Fallback if the pool is no longer available (e.g., during scene changes)
                Destroy(gameObject);
            }
        }
    }
}
