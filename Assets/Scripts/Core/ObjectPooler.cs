
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Core
{
    /// <summary>
    /// A robust, generic object pooler. Improves performance by reusing GameObjects
    /// instead of instantiating and destroying them at runtime.
    /// </summary>
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [Header("Pools")]
        public List<Pool> pools;

        private Dictionary<string, Queue<GameObject>> poolDictionary;

        protected override void Awake()
        {
            base.Awake();
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectQueue = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectQueue.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectQueue);
            }
        }

        /// <summary>
        /// Spawns an object from the pool.
        /// </summary>
        /// <param name="tag">The tag of the pool to spawn from.</param>
        /// <param name="position">The world position to spawn the object at.</param>
        /// <param name="rotation">The world rotation of the object.</param>
        /// <returns>The spawned GameObject, or null if the tag is not found.</returns>
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"ObjectPooler: Pool with tag 'not found. Is it defined in the inspector?");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            // Call a custom interface method if it exists, for re-initialization
            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
            if (pooledObj != null)
            {
                pooledObj.OnObjectSpawn();
            }

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
    }

    /// <summary>
    /// Interface for objects that can be pooled. Implement this to receive a callback
    /// when the object is spawned from the pool.
    /// </summary>
    public interface IPooledObject
    {
        void OnObjectSpawn();
    }
}
