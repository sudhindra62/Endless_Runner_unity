
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Core
{
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int initialSize;
            public bool allowGrowth = true; // Allow the pool to grow if needed
        }

        [Tooltip("The list of object pools to be created at startup.")]
        public List<Pool> pools;

        // The master dictionary holding all pools, keyed by their tag.
        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        // A parent transform for pooled objects to keep the hierarchy clean.
        private Transform _poolParent;

        protected override void Awake()
        {
            base.Awake();
            _poolParent = new GameObject("ObjectPool").transform;
            InitializePools();
        }

        private void InitializePools()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.initialSize; i++)
                {
                    GameObject obj = CreateNewObject(pool.prefab);
                    objectPool.Enqueue(obj);
                }
                _poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.TryGetValue(tag, out Queue<GameObject> objectPool))
            {
                Debug.LogWarning($"Guardian Architect Warning: Pool with tag '{tag}' does not exist.");
                return null;
            }

            GameObject objectToSpawn = null;

            // If the pool has an available object, use it.
            if (objectPool.Count > 0)
            {
                objectToSpawn = objectPool.Dequeue();
            }
            else
            {
                // If the pool is empty, check if it's allowed to grow.
                Pool poolConfig = pools.Find(p => p.tag == tag);
                if (poolConfig != null && poolConfig.allowGrowth)
                {
                    objectToSpawn = CreateNewObject(poolConfig.prefab, false); // Create a new one, but don't add to queue yet
                }
                else
                {
                    Debug.LogWarning($"Guardian Architect Warning: Pool with tag '{tag}' is empty and growth is disallowed.");
                    return null;
                }
            }

            // Prepare the object for use.
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);

            // An IPoolable interface allows objects to reset their state upon spawn.
            IPoolable pooledObj = objectToSpawn.GetComponent<IPoolable>();
            pooledObj?.OnObjectSpawn();

            return objectToSpawn;
        }

        public void ReturnToPool(string tag, GameObject objectToReturn)
        {
            if (!_poolDictionary.TryGetValue(tag, out Queue<GameObject> objectPool))
            {
                Debug.LogWarning($"Guardian Architect Warning: Cannot return object. Pool with tag '{tag}' does not exist.");
                Destroy(objectToReturn); // Destroy object if its pool doesn't exist
                return;
            }

            objectToReturn.SetActive(false);
            objectPool.Enqueue(objectToReturn);
        }

        private GameObject CreateNewObject(GameObject prefab, bool addToQueue = true)
        {
            GameObject obj = Instantiate(prefab, _poolParent);
            obj.SetActive(false); // Start disabled
            return obj;
        }
    }

    /// <summary>
    /// Interface for objects that can be pooled. Provides a hook to reset state when spawned.
    /// </summary>
    public interface IPoolable
    {
        void OnObjectSpawn();
    }
}
