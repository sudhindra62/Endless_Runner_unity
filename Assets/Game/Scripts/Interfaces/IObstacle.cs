
using Utils;

namespace Obstacles
{
    public interface IObstacle
    {
        void Spawn();
        void Despawn();
        void SetPool(GameObjectPool pool);
    }
}
