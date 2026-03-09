
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
            public bool prewarm = false;
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        private Transform prewarmedObjectsContainer;

        private void Awake()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            prewarmedObjectsContainer = new GameObject("PrewarmedObjects").transform;
            prewarmedObjectsContainer.SetParent(transform);

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectQueue = new Queue<GameObject>();
                if (pool.prewarm)
                {
                    for (int i = 0; i < pool.size; i++)
                    {
                        GameObject obj = Instantiate(pool.prefab, prewarmedObjectsContainer);
                        obj.SetActive(false);
                        objectQueue.Enqueue(obj);
                    }
                }
                poolDictionary.Add(pool.tag, objectQueue);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Count > 0 ? poolDictionary[tag].Dequeue() : Instantiate(GetPoolByTag(tag).prefab);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.SetPositionAndRotation(position, rotation);

            return objectToSpawn;
        }

        public void ReturnToPool(string tag, GameObject objectToReturn)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                Destroy(objectToReturn);
                return;
            }

            objectToReturn.SetActive(false);
            poolDictionary[tag].Enqueue(objectToReturn);
        }
        
        private Pool GetPoolByTag(string tag)
        {
            return pools.Find(p => p.tag == tag);
        }
    }
}
