
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Gameplay
{
    public class ObstacleSpawner : MonoBehaviour
    {
        // This is a placeholder for the actual obstacle spawning logic.
        // You would typically have a more complex system for spawning obstacles.

        void Start()
        {
            SpawnObstacle();
        }

        void SpawnObstacle()
        {
            ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
            if (currentTheme != null && currentTheme.obstaclePrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, currentTheme.obstaclePrefabs.Length);
                GameObject obstaclePrefab = currentTheme.obstaclePrefabs[randomIndex];
                Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
