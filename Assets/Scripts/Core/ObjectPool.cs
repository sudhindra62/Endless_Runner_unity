
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic object pool for recycling and reusing GameObjects.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(prefab) || pool[prefab].Count == 0)
        {
            return Instantiate(prefab, position, rotation);
        }

        GameObject obj = pool[prefab].Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        GameObject prefab = obj.GetComponent<PooledObject>()?.Prefab;
        if (prefab == null) {
            // Fallback for objects without a PooledObject component
            prefab = obj;
        }

        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        pool[prefab].Enqueue(obj);
        obj.SetActive(false);
    }
}

/// <summary>
/// A helper component to associate a pooled object with its original prefab.
/// </summary>
public class PooledObject : MonoBehaviour
{
    public GameObject Prefab { get; set; }
}
