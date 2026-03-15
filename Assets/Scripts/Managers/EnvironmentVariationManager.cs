
using UnityEngine;
using System.Collections.Generic;

public class EnvironmentVariationManager : MonoBehaviour
{
    public Transform playerTransform;
    public float spawnDistance = 100f;
    public float despawnDistance = 50f;

    private List<GameObject> spawnedModules = new List<GameObject>();
    private float lastSpawnZ = 0f;

    private void Start()
    {
        // Initialize with a few modules
        for (int i = 0; i < 3; i++)
        {
            SpawnRandomModule();
        }
    }

    private void Update()
    {
        if (playerTransform.position.z > lastSpawnZ - spawnDistance)
        {
            SpawnRandomModule();
            DespawnOldModules();
        }
    }

    private void SpawnRandomModule()
    {
        ThemeConfig currentTheme = ThemeManager.Instance.CurrentTheme;
        if (currentTheme == null || currentTheme.environmentModules == null || currentTheme.environmentModules.Length == 0)
        {
            Debug.LogWarning("Current theme does not have any environment modules defined.");
            return;
        }

        int randomIndex = Random.Range(0, currentTheme.environmentModules.Length);
        GameObject modulePrefab = currentTheme.environmentModules[randomIndex];

        GameObject newModule = Instantiate(modulePrefab, new Vector3(0, 0, lastSpawnZ), Quaternion.identity, transform);
        spawnedModules.Add(newModule);

        // Assuming modules have a consistent length, which you'd define.
        // For now, let's assume a placeholder length of 100 units.
        lastSpawnZ += 100f; 
    }

    private void DespawnOldModules()
    {
        List<GameObject> modulesToDespawn = new List<GameObject>();
        foreach (var module in spawnedModules)
        {
            if (playerTransform.position.z - module.transform.position.z > despawnDistance)
            {
                modulesToDespawn.Add(module);
            }
        }

        foreach (var module in modulesToDespawn)
        {
            spawnedModules.Remove(module);
            Destroy(module);
        }
    }
}
