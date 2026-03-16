
using UnityEngine;
using Utils;

namespace Obstacles
{
    public abstract class Obstacle : MonoBehaviour, IObstacle
    {
        protected GameObjectPool Pool;

        public abstract void Spawn();
        public abstract void Despawn();

        public void SetPool(GameObjectPool pool)
        {
            Pool = pool;
        }
    }
}
