
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }

    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxZombies = 20;

    private bool isZombieModeActive = false;
    private float spawnTimer;
    private int activeZombies = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isZombieModeActive)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0 && activeZombies < maxZombies)
            {
                SpawnZombie();
                spawnTimer = spawnInterval;
            }
        }
    }

    public void ActivateZombieMode()
    {
        if (isZombieModeActive) return;
        isZombieModeActive = true;
        spawnTimer = spawnInterval;
        Debug.Log("Zombie Mode Activated!");
    }

    public void DeactivateZombieMode()
    {
        if (!isZombieModeActive) return;
        isZombieModeActive = false;
        // Logic to despawn all active zombies
        Debug.Log("Zombie Mode Deactivated!");
    }

    private void SpawnZombie()
    {
        // In a real implementation, we would use an object pooler
        // and spawn zombies in front of the player.
        activeZombies++;
        Debug.Log("A zombie has spawned!");
    }

    public void OnZombieKilled()
    {
        activeZombies--;
    }
}
