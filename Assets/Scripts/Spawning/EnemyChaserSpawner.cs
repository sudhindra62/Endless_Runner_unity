
using UnityEngine;

public class EnemyChaserSpawner : MonoBehaviour
{
    public float spawnDelay = 30f;

    private float timeSinceLastSpawn = 0f;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnDelay)
        {
            SpawnEnemyChaser();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnEnemyChaser()
    {
        ThemeSO currentTheme = ThemeManager.Instance != null ? ThemeManager.Instance.CurrentTheme : null;
        GameObject enemyChaserPrefab = currentTheme != null
            ? (currentTheme.enemyChaserPrefab != null ? currentTheme.enemyChaserPrefab : currentTheme.config != null ? currentTheme.config.enemyChaserPrefab : null)
            : null;

        if (enemyChaserPrefab == null)
        {
            Debug.LogWarning("Current theme does not have an enemy chaser prefab defined.");
            return;
        }

        GameObject enemyChaser = ObjectPool.Instance.GetObject(enemyChaserPrefab, transform.position, Quaternion.identity);
        if (enemyChaser != null)
            enemyChaser.GetComponent<EnemyChaser>().playerTransform = PlayerController.Instance.transform;
    }
}
