
using UnityEngine;


    /// <summary>
    /// Returns an object to the object pool when it is behind the player.
    /// </summary>
    public class ReturnToPool : MonoBehaviour
    {
        [Header("Pooling Configuration")]
        [Tooltip("The tag used to return the object to the object pool.")]
        [SerializeField] private string poolTag;

        [Tooltip("The distance behind the player at which the object is despawned.")]
        [SerializeField] private float despawnDistance = -10f;

        private Transform _player;

        private void Start()
        {
            // A more robust system would use a direct reference or a manager.
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                _player = playerObject.transform;
            }
        }

        private void Update()
        {
            if (_player != null && transform.position.z < _player.position.z + despawnDistance)
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
        }
    }

