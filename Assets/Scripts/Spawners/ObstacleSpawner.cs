
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages obstacle spawning. It now listens to the RiskLaneManager to increase
/// obstacle density in high-risk lanes, creating a greater challenge for greater reward.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    #region CONFIGURATION
    [Header("Obstacle Prefabs")]
    [SerializeField] private List<GameObject> obstaclePrefabs;

    [Header("Spawn Parameters")]
    [Tooltip("Base probability of an obstacle spawning in a lane.")]
    [Range(0f, 1f)]
    [SerializeField] private float baseSpawnChance = 0.2f;
    #endregion

    #region STATE
    private float globalDifficultyMultiplier = 1f;

    // --- EVOLUTION: State for Risk Lane System ---
    private int currentRiskLane = -99; // -99 indicates no active risk lane
    private float riskLaneObstacleMultiplier = 1f;
    #endregion

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        // --- EVOLUTION: Subscribe to the RiskLaneManager event ---
        RiskLaneManager.OnRiskLaneChanged += HandleRiskLaneChange;
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<ObstacleSpawner>();
        // --- EVOLUTION: Unsubscribe to prevent memory leaks ---
        RiskLaneManager.OnRiskLaneChanged -= HandleRiskLaneChange;
    }
    #endregion

    #region EVENT_HANDLERS
    /// <summary>
    /// NEW: Handles the event from the RiskLaneManager to update internal state.
    /// </summary>
    private void HandleRiskLaneChange(int laneIndex, bool isRiskLane, float coinMultiplier, float obstacleMultiplier)
    {
        if (isRiskLane)
        {
            currentRiskLane = laneIndex;
            riskLaneObstacleMultiplier = obstacleMultiplier;
        }
        else
        {
            // If the reverted lane is the one we are tracking, reset our state.
            if (currentRiskLane == laneIndex)
            {
                currentRiskLane = -99;
                riskLaneObstacleMultiplier = 1f;
            }
        }
    }
    #endregion

    #region PUBLIC_API
    /// <summary>
    /// The primary method called by a TileSpawner.
    /// --- EVOLUTION: Now checks if a lane is a risk lane before spawning. ---
    /// </summary>
    public void SpawnObstaclesForTile(Tile tile)
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Count == 0)
            return;

        // PRESERVED: Get the most current global difficulty multiplier.
        this.globalDifficultyMultiplier = ServiceLocator.Get<GameDifficultyManager>()?.GetDifficultyMultiplier() ?? 1f;

        // Process each lane on the tile.
        for (int laneIndex = -1; laneIndex <= 1; laneIndex++) // Assuming 3 lanes: -1, 0, 1
        {
            ProcessLane(tile, laneIndex);
        }
    }
    #endregion

    #region INTERNAL_LOGIC
    /// <summary>
    /// Processes a single lane, deciding whether to spawn an obstacle there.
    /// --- EVOLUTION: Now incorporates the risk lane multiplier. ---
    /// </summary>
    private void ProcessLane(Tile tile, int laneIndex)
    {
        // Start with the base spawn chance.
        float finalSpawnChance = baseSpawnChance;
        
        // PRESERVED: Apply the global difficulty multiplier.
        finalSpawnChance *= globalDifficultyMultiplier;

        // EVOLUTION: If this is the risk lane, apply its specific multiplier.
        if (laneIndex == currentRiskLane)
        {
            finalSpawnChance *= riskLaneObstacleMultiplier;
        }

        // Final check to decide if we spawn.
        if (Random.Range(0f, 1f) > finalSpawnChance)
        {
            return;
        }

        // If we decide to spawn, select a random obstacle and place it.
        var spawnPoint = tile.GetObstacleSpawnPointForLane(laneIndex);
        if (spawnPoint != null)
        {
            GameObject prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        }
    }
    #endregion
}
