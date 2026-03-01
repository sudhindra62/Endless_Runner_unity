
using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
    }

    public void AddPool(string tag, GameObject prefab, int size)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            poolDictionary.Add(tag, new Queue<GameObject>());

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                poolDictionary[tag].Enqueue(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            GameObject obj = poolDictionary[tag].Dequeue();
            obj.SetActive(true);
            poolDictionary[tag].Enqueue(obj);
            return obj;
        }
        else
        {
            return null;
        }
    }
}
