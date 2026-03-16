using UnityEngine;

public class EnvironmentVariationManager : MonoBehaviour
{
    public static EnvironmentVariationManager Instance { get; private set; }

    private ThemeSO currentTheme;
    private GameObject[] environmentModules;
    private int lastSpawnedModuleIndex = -1;

    // The Z-position where the next environment module should be spawned.
    private float nextSpawnZ = 0f;

    // The length of each environment module.
    public float moduleLength = 100f;

    // The number of modules to keep active in the scene at any given time.
    private int activeModules = 3;

    // A list of the currently active environment modules.
    private System.Collections.Generic.List<GameObject> activeModuleObjects = new System.Collections.Generic.List<GameObject>();

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
        currentTheme = ThemeManager.Instance.GetCurrentTheme();

        if (currentTheme != null)
        {
            environmentModules = currentTheme.environmentPrefabs;

            // Initially spawn the necessary number of modules.
            for (int i = 0; i < activeModules; i++)
            {
                SpawnModule();
            }
        }
    }

    private void Update()
    {
        // If the player has moved far enough, spawn a new module and remove the oldest one.
        if (transform.position.z > nextSpawnZ - (activeModules * moduleLength))
        {
            SpawnModule();
            if (activeModuleObjects.Count > activeModules)
            {
                Destroy(activeModuleObjects[0]);
                activeModuleObjects.RemoveAt(0);
            }
        }
    }

    private void SpawnModule()
    {
        if (environmentModules == null || environmentModules.Length == 0)
        {
            return;
        }

        // Select a random module, ensuring it's not the same as the last one.
        int randomIndex = lastSpawnedModuleIndex;
        while (randomIndex == lastSpawnedModuleIndex)
        {
            randomIndex = Random.Range(0, environmentModules.Length);
        }
        lastSpawnedModuleIndex = randomIndex;

        GameObject modulePrefab = environmentModules[randomIndex];

        // Instantiate the module at the correct position.
        GameObject newModule = Instantiate(modulePrefab, new Vector3(0, 0, nextSpawnZ), Quaternion.identity, transform);
        activeModuleObjects.Add(newModule);

        // Update the next spawn position.
        nextSpawnZ += moduleLength;
    }
}
