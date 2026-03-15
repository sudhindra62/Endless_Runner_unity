using UnityEngine;
using System.Collections.Generic;

public class EnvironmentVariationManager : MonoBehaviour
{
    public static EnvironmentVariationManager Instance { get; private set; }

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float spawnDistance = 100f;
    [SerializeField]
    private float despawnDistance = 50f;
    [SerializeField]
    private int initialModules = 3;

    private List<GameObject> activeModules = new List<GameObject>();
    private Vector3 nextSpawnPosition = Vector3.zero;
    private float moduleLength = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (ThemeManager.Instance.GetCurrentTheme() != null)
        {
            InitializeEnvironment();
        }
    }

    private void Update()
    {
        if (playerTransform.position.z > nextSpawnPosition.z - spawnDistance)
        {
            SpawnModule();
        }

        if (activeModules.Count > 0 && playerTransform.position.z > activeModules[0].transform.position.z + moduleLength + despawnDistance)
        {
            DespawnModule();
        }
    }

    private void InitializeEnvironment()
    {
        ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
        if (currentTheme.environmentPrefabs.Length == 0) return;

        // Get the length of the module from the first prefab
        var firstModule = currentTheme.environmentPrefabs[0];
        var renderer = firstModule.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            moduleLength = renderer.bounds.size.z;
        }


        for (int i = 0; i < initialModules; i++)
        {
            SpawnModule(true);
        }
    }

    private void SpawnModule(bool isInitial = false)
    {
        ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
        if (currentTheme.environmentPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, currentTheme.environmentPrefabs.Length);
        GameObject modulePrefab = currentTheme.environmentPrefabs[randomIndex];

        GameObject module = Instantiate(modulePrefab, nextSpawnPosition, Quaternion.identity, transform);
        activeModules.Add(module);

        if (isInitial && moduleLength == 0f)
        {
            var renderer = module.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                moduleLength = renderer.bounds.size.z;
            }
        }

        nextSpawnPosition.z += moduleLength;
    }

    private void DespawnModule()
    {
        GameObject moduleToDespawn = activeModules[0];
        activeModules.RemoveAt(0);
        Destroy(moduleToDespawn);
    }
}
