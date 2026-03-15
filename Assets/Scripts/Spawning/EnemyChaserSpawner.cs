
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
        ThemeConfig currentTheme = ThemeManager.Instance.CurrentTheme;
        if (currentTheme == null || currentTheme.enemyChaserPrefab == null)
        {
            Debug.LogWarning("Current theme does not have an enemy chaser prefab defined.");
            return;
        }

        GameObject enemyChaser = Instantiate(currentTheme.enemyChaserPrefab, transform.position, Quaternion.identity);
        enemyChaser.GetComponent<EnemyChaser>().playerTransform = FindObjectOfType<PlayerController>().transform;
    }
}
