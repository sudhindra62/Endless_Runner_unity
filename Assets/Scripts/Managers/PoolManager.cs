using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A fortified, high-performance object pooling system.
/// This manager provides a centralized and efficient way to reuse GameObjects,
/// reducing garbage collection and improving performance.
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, GameObject> prefabDictionary;

    protected override void Awake()
    {
        base.Awake();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabDictionary = new Dictionary<string, GameObject>();
    }

    /// <summary>
    /// Spawns an object from the pool.
    /// </summary>
    /// <param name="prefab">The prefab to spawn.</param>
    /// <param name="position">The position to spawn the object at.</param>
    /// <param name="rotation">The rotation to spawn the object with.</param>
    /// <returns>The spawned GameObject.</returns>
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string tag = prefab.name;
        if (!poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag] = new Queue<GameObject>();
            prefabDictionary[tag] = prefab;
        }

        GameObject objectToSpawn;
        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }
        else
        {
            objectToSpawn = Instantiate(prefabDictionary[tag]);
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    /// <summary>
    /// Returns an object to its pool.
    /// </summary>
    /// <param name="objectToReturn">The GameObject to return.</param>
    public void ReturnObject(GameObject objectToReturn)
    {
        string tag = objectToReturn.name.Replace("(Clone)", ""); // Get the original prefab name
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist. Object will be destroyed.");
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}
