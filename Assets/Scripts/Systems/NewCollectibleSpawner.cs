
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the procedural spawning of collectible items in groups ahead of the player.
/// Uses an object pooler to efficiently reuse collectible GameObjects.
/// </summary>
public class NewCollectibleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CollectibleGroup
    {
        public string name;
        [Tooltip("The tag used by the ObjectPooler to retrieve this type of collectible.")]
        public string poolerTag;
        [Tooltip("The minimum number of collectibles to spawn in a single group.")]
        public int minGroupSize = 3;
        [Tooltip("The maximum number of collectibles to spawn in a single group.")]
        public int maxGroupSize = 8;
        [Tooltip("The spacing between individual collectibles within a group along the Z-axis.")]
        public float groupSpacing = 2f;
        [Tooltip("The probability (0.0 to 1.0) that this group will attempt to spawn in a spawn zone.")]
        public float spawnChance = 0.5f;
    }

    [Header("Core Configuration")]
    [Tooltip("The groups of collectibles that can be spawned.")]
    [SerializeField] private List<CollectibleGroup> collectibleGroups;
    [Tooltip("The player's transform, used to determine where to spawn collectibles.")]
    [SerializeField] private Transform player;

    [Header("Spawning Parameters")]
    [Tooltip("How far ahead of the player the spawner should place collectibles.")]
    [SerializeField] private float spawnDistanceAhead = 50f;
    [Tooltip("The distance between the center of the track and the side lanes.")]
    [SerializeField] private float laneWidth = 3f;
    [Tooltip("The vertical position (Y-axis) where collectibles will be spawned.")]
    [SerializeField] private float collectibleYPosition = 1f;
    [Tooltip("The length of the zone where a group might spawn. Spawning is attempted at each interval.")]
    [SerializeField] private float spawnZoneInterval = 20f;
    [Tooltip("The minimum gap to maintain from obstacles to avoid unfair placements.")]
    [SerializeField] private float obstacleSafeGap = 5f;

    private float lastSpawnZ;
    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ServiceLocator.Get<ObjectPooler>();
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler not found. Collectible spawning will not work.");
        }
    }

    private void Update()
    {
        if (player.position.z > lastSpawnZ - spawnDistanceAhead)
        {
            SpawnCollectibleGroups();
        }
    }

    private void SpawnCollectibleGroups()
    {
        if (objectPooler == null) return;

        lastSpawnZ += spawnZoneInterval;

        foreach (var group in collectibleGroups)
        {
            if (Random.value < group.spawnChance)
            {
                int lane = Random.Range(0, 3);
                int groupSize = Random.Range(group.minGroupSize, group.maxGroupSize + 1);

                for (int i = 0; i < groupSize; i++)
                {
                    float spawnX = (lane - 1) * laneWidth;
                    float spawnZ = lastSpawnZ + i * group.groupSpacing;
                    Vector3 spawnPosition = new Vector3(spawnX, collectibleYPosition, spawnZ);

                    if (IsPositionSafe(spawnPosition))
                    {
                        GameObject collectible = objectPooler.GetPooledObject(group.poolerTag);
                        if (collectible != null)
                        {
                            collectible.transform.position = spawnPosition;
                            collectible.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    private bool IsPositionSafe(Vector3 position)
    {
        return !Physics.CheckBox(position, new Vector3(1f, 1f, obstacleSafeGap / 2f), Quaternion.identity, LayerMask.GetMask("Obstacle"));
    }

    public void ResetSpawner()
    {
        lastSpawnZ = 0;
    }
}
