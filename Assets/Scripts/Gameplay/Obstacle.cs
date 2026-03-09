
using UnityEngine;
using Core;
using Data;

namespace Gameplay
{
    /// <summary>
    /// Defines the behavior of an obstacle.
    /// </summary>
    public class Obstacle : MonoBehaviour
    {
        [Header("Obstacle Configuration")]
        [Tooltip("The type of the obstacle.")]
        [SerializeField] private ObstacleType obstacleType = ObstacleType.Static;

        [Tooltip("The tag used to return the obstacle to the object pool.")]
        [SerializeField] private string poolTag = "Obstacle";

        public ObstacleType Type => obstacleType;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Handle collision with the player, e.g., deal damage.
                Debug.Log("Player collided with an obstacle!");

                // Return the obstacle to the pool.
                if (ObjectPool.Instance != null)
                {
                    ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
