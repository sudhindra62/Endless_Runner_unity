using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A simple, generic object pool for reusing GameObjects.
/// This is a foundational performance pattern to avoid costly Instantiate/Destroy calls.
/// </summary>
public class SimpleObjectPool : MonoBehaviour
{
    [Tooltip("The prefab to be pooled.")]
    [SerializeField] private GameObject prefabToPool;

    [Tooltip("The initial number of objects to create in the pool.")]
    [SerializeField] private int initialPoolSize = 10;

    private readonly Queue<GameObject> availableObjects = new Queue<GameObject>();

    void Awake()
    {
        // Populate the pool with the initial set of objects.
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateAndPoolObject();
        }
    }

    /// <summary>
    /// Retrieves an object from the pool.
    /// </summary>
    /// <returns>An active GameObject from the pool.</returns>
    public GameObject GetObject()
    {
        // If the pool is empty, create a new object.
        if (availableObjects.Count == 0)
        {
            CreateAndPoolObject();
        }

        GameObject obj = availableObjects.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Returns an object to the pool, deactivating it.
    /// </summary>
    /// <param name="obj">The object to return.</param>
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }

    private GameObject CreateAndPoolObject()
    {
        GameObject newObj = Instantiate(prefabToPool, transform);
        newObj.SetActive(false);
        availableObjects.Enqueue(newObj);
        return newObj;
    }
}
