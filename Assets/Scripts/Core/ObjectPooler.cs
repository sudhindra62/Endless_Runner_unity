using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for objects that can be pooled.
/// </summary>
public interface IPooledObject
{
    void OnObjectSpawn();
}

/// <summary>
/// Component to track the tag of a pooled object.
/// </summary>
public class PooledObjectInfo : MonoBehaviour
{
    public string poolTag;
}

/// <summary>
/// Robust object pooling system.
/// Global scope.
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

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    protected override void Awake()
    {
        base.Awake();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        if (poolDictionary[tag].Count == 0)
        {
            Pool pool = pools.Find(p => p.tag == tag);
            if (pool != null)
            {
                GameObject obj = Instantiate(pool.prefab);
                return SetupObject(obj, position, rotation);
            }
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        var info = objectToSpawn.GetComponent<PooledObjectInfo>();
        if (info == null) info = objectToSpawn.AddComponent<PooledObjectInfo>();
        info.poolTag = tag;

        return SetupObject(objectToSpawn, position, rotation);
    }

    private GameObject SetupObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        IPooledObject pooled = obj.GetComponent<IPooledObject>();
        if (pooled != null) pooled.OnObjectSpawn();

        return obj;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Destroy(objectToReturn);
            return;
        }
        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }

    // --- API COMPATIBILITY BRIDGE ---
    public void CreatePool(string tag, GameObject prefab, int size = 10)
    {
        if (poolDictionary != null && !poolDictionary.ContainsKey(tag))
        {
            var pool = new Pool { tag = tag, prefab = prefab, size = size };
            pools.Add(pool);
            var q = new Queue<GameObject>();
            for (int i = 0; i < size; i++) { var o = Instantiate(prefab); o.SetActive(false); q.Enqueue(o); }
            poolDictionary[tag] = q;
        }
    }

    public GameObject GetObject(string tag) => SpawnFromPool(tag, Vector3.zero, Quaternion.identity);
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;

        if (!poolDictionary.ContainsKey(prefab.name))
        {
            CreatePool(prefab.name, prefab, 1);
        }

        return SpawnFromPool(prefab.name, position, rotation);
    }
    public GameObject GetPooledObject(string tag) => GetObject(tag);
    public GameObject GetPooledObject(GameObject prefab) => prefab != null ? GetObject(prefab.name) : null;
    public GameObject GetPooledObject(string tag, Vector3 position) => SpawnFromPool(tag, position, Quaternion.identity);
    public GameObject GetPooledObject(string tag, Vector3 position, Quaternion rotation) => SpawnFromPool(tag, position, rotation);
    public void ReturnObject(string tag, GameObject obj) => ReturnToPool(tag, obj);
    public void ReturnObject(GameObject obj)
    {
        var info = obj.GetComponent<PooledObjectInfo>();
        if (info != null) ReturnToPool(info.poolTag, obj);
        else Destroy(obj);
    }
}
