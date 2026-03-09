
using System.Collections;
using UnityEngine;
using Core;

namespace Gameplay
{
    /// <summary>
    /// Defines the behavior of a projectile.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Configuration")]
        [Tooltip("The speed of the projectile.")]
        [SerializeField] private float speed = 20f;

        [Tooltip("The lifetime of the projectile in seconds.")]
        [SerializeField] private float lifetime = 5f;

        [Tooltip("The tag used to return the projectile to the object pool.")]
        [SerializeField] private string poolTag = "Projectile";

        private Coroutine _lifetimeCoroutine;

        private void OnEnable()
        {
            _lifetimeCoroutine = StartCoroutine(LifetimeCoroutine());
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Assuming the player has a method to handle damage
                // other.GetComponent<PlayerHealth>()?.TakeDamage(1);
                ReturnToPool();
            }
        }

        private IEnumerator LifetimeCoroutine()
        {
            yield return new WaitForSeconds(lifetime);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            if (_lifetimeCoroutine != null)
            {
                StopCoroutine(_lifetimeCoroutine);
            }
        }
    }
}
