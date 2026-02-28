
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolItem
{
    [Tooltip("A unique tag to identify the pool.")]
    public string tag;
    [Tooltip("The prefab to be pooled.")]
    public GameObject objectToPool;
    [Tooltip("The initial number of objects to create in the pool.")]
    public int amountToPool;
    [Tooltip("If true, the pool will create new objects if it runs out.")]
    public bool shouldExpand = true;
}

/// <summary>
/// A robust, persistent singleton-based object pooling system that manages reusable GameObjects.
/// It helps to reduce instantiation overhead by pre-instantiating objects and recycling them.
/// This singleton persists across scene loads.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// The static singleton instance of the ObjectPooler.
    /// </summary>
    public static ObjectPooler Instance { get; private set; }

    // Internal class to manage the data for a single object pool.
    private class Pool
    {
        public readonly Queue<GameObject> ObjectQueue;
        public readonly bool ShouldExpand;
        public readonly GameObject Prefab;
        public readonly Transform ParentTransform;

        public Pool(GameObject prefab, bool shouldExpand, Transform parentTransform)
        {
            Prefab = prefab;
            ShouldExpand = shouldExpand;
            ParentTransform = parentTransform;
            ObjectQueue = new Queue<GameObject>();
        }
    }

    [Tooltip("The list of object pools to be created on startup.")]
    [SerializeField] private List<ObjectPoolItem> itemsToPool;
    private Dictionary<string, Pool> poolDictionary;

    private void Awake()
    {
        // Standard persistent singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Ensure the pooler persists across scenes

        poolDictionary = new Dictionary<string, Pool>();

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool == null)
            {
                Debug.LogWarning($"[ObjectPooler] Skipping pool with tag '{item.tag}' because its prefab is not set.");
                continue;
            }

            // Create a new pool and add it to the dictionary
            Pool newPool = new Pool(item.objectToPool, item.shouldExpand, transform);
            poolDictionary.Add(item.tag, newPool);

            // Pre-populate the pool with the specified number of objects
            for (int i = 0; i < item.amountToPool; i++)
            {
                CreateAndEnqueueObject(newPool);
            }
        }
    }

    /// <summary>
    /// Retrieves a disabled object from the specified pool.
    /// </summary>
    /// <param name="tag">The tag of the pool to retrieve an object from.</param>
    /// <returns>A GameObject from the pool, or null if the pool doesn't exist or cannot expand.</returns>
    public GameObject GetPooledObject(string tag)
    {
        if (!poolDictionary.TryGetValue(tag, out Pool pool))
        {
            Debug.LogWarning($"[ObjectPooler] Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        // If the pool is empty, try to expand it
        if (pool.ObjectQueue.Count == 0)
        {
            if (pool.ShouldExpand)
            {
                Debug.Log($"[ObjectPooler] Expanding pool with tag '{tag}'.");
                CreateAndEnqueueObject(pool);
            }
            else
            {
                Debug.LogWarning($"[ObjectPooler] Pool with tag '{tag}' is empty and cannot expand.");
                return null;
            }
        }

        GameObject objectFromPool = pool.ObjectQueue.Dequeue();
        objectFromPool.transform.SetParent(null); // Un-parent for use in the scene
        return objectFromPool;
    }

    /// <summary>
    /// Returns an object to its pool, deactivating it and parenting it back to the pooler.
    /// </summary>
    /// <param name="tag">The tag of the pool the object belongs to.</param>
    /// <param name="objectToReturn">The GameObject to return to the pool.</param>
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.TryGetValue(tag, out Pool pool))
        {
            Debug.LogWarning($"[ObjectPooler] Pool with tag '{tag}' doesn't exist. Object '{objectToReturn.name}' will be destroyed.");
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(pool.ParentTransform); // Re-parent to the pooler
        pool.ObjectQueue.Enqueue(objectToReturn);
    }

    /// <summary>
    /// Instantiates a new object, deactivates it, and enqueues it in the specified pool.
    /// </summary>
    private void CreateAndEnqueueObject(Pool pool)
    {
        GameObject obj = Instantiate(pool.Prefab, pool.ParentTransform);
        obj.SetActive(false); // Ensure it's inactive when pooled
        pool.ObjectQueue.Enqueue(obj);
    }
}
