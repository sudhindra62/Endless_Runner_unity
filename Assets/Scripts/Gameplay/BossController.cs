
using System.Collections;
using UnityEngine;


    /// <summary>
    /// Manages the behavior of a boss character, including chasing the player and performing attacks.
    /// </summary>
    public class BossController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [SerializeField] private float chaseSpeed = 15f;
        [SerializeField] private float followDistance = 15f;
        [SerializeField] private float laneWidth = 3f;

        [Header("Attack Parameters")]
        [SerializeField] private float attackCooldown = 5f;
        [SerializeField] private Transform attackSpawnPoint;
        [SerializeField] private string projectileTag = "BossProjectile";
        [SerializeField] private string barrierTag = "Barrier";

        private Transform _playerTransform;
        private Coroutine _chaseCoroutine;
        private Coroutine _attackCoroutine;

        private void OnEnable()
        {
            // Attempt to find the player in the scene.
            // A more robust solution would be to use a service locator or manager to get the player reference.
            var player = PlayerController.Instance;
            if (player != null)
            {
                _playerTransform = player.transform;
                StartCoroutines();
            }
            else
            {
                Debug.LogWarning("BossController could not find a PlayerController in the scene.");
            }
        }

        private void OnDisable()
        {
            StopCoroutines();
        }

        private void StartCoroutines()
        {
            _chaseCoroutine = StartCoroutine(ChasePlayer());
            _attackCoroutine = StartCoroutine(PerformAttacks());
        }

        private void StopCoroutines()
        {
            if (_chaseCoroutine != null)
            {
                StopCoroutine(_chaseCoroutine);
                _chaseCoroutine = null;
            }
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        private IEnumerator ChasePlayer()
        {
            while (true)
            {
                if (_playerTransform != null)
                {
                    Vector3 targetPosition = _playerTransform.position - _playerTransform.forward * followDistance;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * chaseSpeed);
                    transform.LookAt(_playerTransform);
                }
                yield return null;
            }
        }

        private IEnumerator PerformAttacks()
        {
            yield return new WaitForSeconds(attackCooldown * 0.5f); // Initial delay

            while (true)
            {
                if (Random.value > 0.5f)
                {
                    PerformProjectileAttack();
                }
                else
                {
                    PerformLaneBlockAttack();
                }

                yield return new WaitForSeconds(attackCooldown);
            }
        }

        private void PerformProjectileAttack()
        {
            if (_playerTransform == null || ObjectPool.Instance == null) return;

            Debug.Log("Boss is performing a projectile attack!");
            Quaternion attackRotation = Quaternion.LookRotation(_playerTransform.position - attackSpawnPoint.position);
            ObjectPool.Instance.SpawnFromPool(projectileTag, attackSpawnPoint.position, attackRotation);
        }

        private void PerformLaneBlockAttack()
        {
            if (_playerTransform == null || ObjectPool.Instance == null) return;

            Debug.Log("Boss is performing a lane block attack!");
            int randomLane = Random.Range(-1, 2); // -1 for left, 0 for middle, 1 for right
            float laneOffset = randomLane * laneWidth;

            Vector3 barrierPosition = _playerTransform.position + _playerTransform.forward * 20f + new Vector3(laneOffset, 0.5f, 0);
            ObjectPool.Instance.SpawnFromPool(barrierTag, barrierPosition, _playerTransform.rotation);
        }

        /// <summary>
        /// Calculates the distance from the boss to the player.
        /// </summary>
        public float GetDistanceFromPlayer()
        {
            if (_playerTransform == null) return float.MaxValue;
            return Vector3.Distance(transform.position, _playerTransform.position);
        }
    }

