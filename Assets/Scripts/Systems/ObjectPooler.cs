
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
/// A robust, persistent object pooling system that manages reusable GameObjects.
/// It helps to reduce instantiation overhead by pre-instantiating objects and recycling them.
/// This system is registered with the ServiceLocator and persists across scene loads.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
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
        // Make this a persistent service, registered with the ServiceLocator.
        var instances = FindObjectsByType<ObjectPooler>(FindObjectsSortMode.None);
        if (instances.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        ServiceLocator.Register<ObjectPooler>(this);

        poolDictionary = new Dictionary<string, Pool>();

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool == null)
            {
                Debug.LogWarning($"[ObjectPooler] Skipping pool with tag '{item.tag}' because its prefab is not set.");
                continue;
            }

            Pool newPool = new Pool(item.objectToPool, item.shouldExpand, transform);
            poolDictionary.Add(item.tag, newPool);

            for (int i = 0; i < item.amountToPool; i++)
            {
                CreateAndEnqueueObject(newPool);
            }
        }
    }

    private void OnDestroy()
    {
        if (ServiceLocator.Get<ObjectPooler>() == this)
        {
            ServiceLocator.Unregister<ObjectPooler>();
        }
    }

    /// <summary>
    /// Retrieves a disabled object from the specified pool.
    /// </summary>
    public GameObject GetPooledObject(string tag)
    {
        if (!poolDictionary.TryGetValue(tag, out Pool pool))
        {
            Debug.LogWarning($"[ObjectPooler] Pool with tag '{tag}' doesn't exist.");
            return null;
        }

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
        objectFromPool.transform.SetParent(null);
        return objectFromPool;
    }

    /// <summary>
    /// Returns an object to its pool, deactivating it and parenting it back to the pooler.
    /// </summary>
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.TryGetValue(tag, out Pool pool))
        {
            Debug.LogWarning($"[ObjectPooler] Pool with tag '{tag}' doesn't exist. Object '{objectToReturn.name}' will be destroyed.");
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(pool.ParentTransform);
        pool.ObjectQueue.Enqueue(objectToReturn);
    }

    private void CreateAndEnqueueObject(Pool pool)
    {
        GameObject obj = Instantiate(pool.Prefab, pool.ParentTransform);
        obj.SetActive(false);
        pool.ObjectQueue.Enqueue(obj);
    }
}
