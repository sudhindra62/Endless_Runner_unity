using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Boss Prefab")]
    [SerializeField] private GameObject bossPrefab;

    private Boss currentBoss;

    public void SpawnBoss()
    {
        if (bossPrefab != null)
        {
            GameObject bossObject = Instantiate(bossPrefab, Vector3.zero, Quaternion.identity);
            currentBoss = bossObject.GetComponent<Boss>();
            Debug.Log("Boss has been spawned!");
        }
    }

    public void DespawnBoss()
    {
        if (currentBoss != null)
        { 
            Destroy(currentBoss.gameObject);
            currentBoss = null;
        }
    }

    // Add methods to trigger boss attacks, check its status, etc.
}
