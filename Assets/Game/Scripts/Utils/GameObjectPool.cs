
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class GameObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Queue<GameObject> _pool = new Queue<GameObject>();

        public GameObjectPool(GameObject prefab)
        {
            _prefab = prefab;
        }

        public GameObject Get()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }

            return Object.Instantiate(_prefab);
        }

        public void Release(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
