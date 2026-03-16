
using UnityEngine;

namespace Obstacles
{
    public class BaseObstacle : Obstacle
    {
        public override void Spawn()
        {
            gameObject.SetActive(true);
        }

        public override void Despawn()
        {
            Pool.Release(gameObject);
        }
    }
}
