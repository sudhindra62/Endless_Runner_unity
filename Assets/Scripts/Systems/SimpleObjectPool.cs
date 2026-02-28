
using UnityEngine;
using System.Collections.Generic;

public class SimpleObjectPool : MonoBehaviour
{
    [Header("Pool Configuration")]
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private bool allowPoolToGrow = true;

    private readonly Stack<GameObject> pooledObjects = new Stack<GameObject>();

    void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewObjectForPool();
        }
    }

    public GameObject GetObject()
    {
        if (pooledObjects.Count > 0)
        {
            GameObject obj = pooledObjects.Pop();
            obj.SetActive(true);
            return obj;
        }
        else if (allowPoolToGrow)
        {
            return CreateNewObject(true);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] Pool is empty and not allowed to grow. Consider increasing initial size or enabling growth.");
            return null;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
            pooledObjects.Push(obj);
        }
    }

    private void CreateNewObjectForPool()
    {
        GameObject newObj = CreateNewObject(false);
        pooledObjects.Push(newObj);
    }

    private GameObject CreateNewObject(bool activate)
    {
        if (objectToPool == null)
        {
            Debug.LogError($"[{gameObject.name}] Object to pool is not assigned!");
            return null;
        }

        GameObject newObj = Instantiate(objectToPool, transform);
        newObj.SetActive(activate);
        return newObj;
    }
}
