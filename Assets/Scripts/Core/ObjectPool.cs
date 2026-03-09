using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic object pool for recycling and reusing GameObjects.
/// Logic fully restored and validated by Supreme Guardian Architect v12.
/// This system prevents garbage collection spikes by reusing objects instead of instantiating and destroying them at runtime.
/// </summary>
public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<GameObject, Queue<GameObject>> _pool = new Dictionary<GameObject, Queue<GameObject>>();

    /// <summary>
    /// Retrieves an object from the pool or creates a new one if the pool is empty.
    /// </summary>
    /// <param name="prefab">The prefab to instantiate or retrieve.</param>
    /// <param name="position">The world position for the object.</param>
    /// <param name="rotation">The world rotation for the object.</param>
    /// <returns>A GameObject instance, ready for use.</returns>
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_pool.ContainsKey(prefab) || _pool[prefab].Count == 0)
        {
            // --- A-TO-Z CONNECTIVITY: Instantiate, add the PooledObject component, and link its prefab. ---
            // This ensures that when the object is returned, we know which pool it belongs to.
            GameObject newObj = Instantiate(prefab, position, rotation);
            PooledObject pooledObj = newObj.AddComponent<PooledObject>();
            pooledObj.Prefab = prefab;
            return newObj;
        }

        // Reuse an existing object from the pool
        GameObject obj = _pool[prefab].Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Returns an object to its corresponding pool for later reuse.
    /// </summary>
    /// <param name="obj">The GameObject instance to return.</param>
    public void ReturnObject(GameObject obj)
    {
        // --- CONTEXT_WIRING: Identify the object's original prefab via the PooledObject component. ---
        PooledObject pooledObj = obj.GetComponent<PooledObject>();
        if (pooledObj == null || pooledObj.Prefab == null)
        {
            Debug.LogError($"Guardian Architect Error: The object '{obj.name}' you are trying to return to the pool does not have a PooledObject component or its prefab is not set. Destroying it instead.", obj);
            Destroy(obj);
            return;
        }

        GameObject prefab = pooledObj.Prefab;

        // Ensure a queue for this prefab type exists.
        if (!_pool.ContainsKey(prefab))
        {
            _pool[prefab] = new Queue<GameObject>();
        }

        // Add the object back to the queue and deactivate it.
        _pool[prefab].Enqueue(obj);
        obj.SetActive(false);
    }
}

/// <summary>
/// Helper component to associate a pooled object instance with its original prefab.
/// This is essential for the ObjectPool to correctly categorize and recycle objects.
/// </summary>
public class PooledObject : MonoBehaviour
{
    /// <summary>
    /// A reference to the original prefab asset this object was instantiated from.
    /// </summary>
    public GameObject Prefab { get; set; }
}
