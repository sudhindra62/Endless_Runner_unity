
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the procedural spawning of coins. It now listens to the RiskLaneManager
/// to dynamically increase coin density in designated high-risk lanes.
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    #region CONFIGURATION
    [Header("Coin Prefab")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Spawn Parameters")]
    [Tooltip("Base probability that a coin pattern will spawn on any given tile segment.")]
    [Range(0f, 1f)]
    [SerializeField] private float baseSpawnChance = 0.5f;
    [Tooltip("The default number of coins to spawn in a line pattern.")]
    [SerializeField] private int defaultLineLength = 5;
    #endregion

    #region STATE
    // This will hold density multipliers from various sources (Fever, Risk Lane, etc.)
    private readonly Dictionary<string, float> densityMultipliers = new Dictionary<string, float>();

    // --- EVOLUTION: State for Risk Lane System ---
    private int currentRiskLane = -99; // -99 indicates no active risk lane
    private float riskLaneCoinMultiplier = 1f;
    private const string RISK_LANE_SOURCE_ID = "RiskLane";
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
        ServiceLocator.Unregister<CoinSpawner>();
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
            riskLaneCoinMultiplier = coinMultiplier;
        }
        else
        {
            // If the reverted lane is the one we are tracking, reset our state.
            if (currentRiskLane == laneIndex)
            {
                currentRiskLane = -99;
                riskLaneCoinMultiplier = 1f;
            }
        }
    }
    #endregion

    #region PUBLIC_API
    /// <summary>
    /// The primary method called by a TileSpawner.
    /// --- EVOLUTION: Now checks if the tile belongs to a risk lane. ---
    /// </summary>
    /// <param name="tile">The tile script requesting coins, which contains lane info.</param>
    public void SpawnCoinsForTile(Tile tile)
    {
        if (coinPrefab == null || tile.GetSpawnPoints().Length == 0)
            return;

        // The core logic now resides in a lane-specific method.
        for (int laneIndex = -1; laneIndex <= 1; laneIndex++) // Assuming 3 lanes: -1, 0, 1
        {
            ProcessLane(tile, laneIndex);
        }
    }

    /// <summary>
    /// Applies a density multiplier from a specific source (e.g., "FeverMode").
    /// This function is PRESERVED and works alongside the new risk lane logic.
    /// </summary>
    public void ApplyDensityMultiplier(string sourceId, float multiplier)
    {
        densityMultipliers[sourceId] = multiplier;
    }

    /// <summary>
    /// Removes a density multiplier from a specific source.
    /// This function is PRESERVED.
    /// </summary>
    public void RemoveDensityMultiplier(string sourceId)
    {
        densityMultipliers.Remove(sourceId);
    }
    #endregion

    #region INTERNAL_LOGIC
    /// <summary>
    /// NEW: Processes a single lane on a tile, deciding whether to spawn coins.
    /// </summary>
    private void ProcessLane(Tile tile, int laneIndex)
    {
        float spawnChance = baseSpawnChance;
        float globalMultiplier = GetTotalDensityMultiplier();

        // Check if the current lane is the designated risk lane.
        if (laneIndex == currentRiskLane)
        {
            // In a risk lane, the chance is higher, and we use the specific risk multiplier.
            spawnChance = 1f; // Guarantee a spawn attempt in the risk lane
            globalMultiplier *= riskLaneCoinMultiplier;
        }

        // Combine the base chance with global multipliers (from Fever Mode, etc.)
        if (Random.Range(0f, 1f) > spawnChance * globalMultiplier)
        {
            return;
        }

        // If we decide to spawn, place a line of coins.
        var spawnPointsInLane = tile.GetSpawnPointsForLane(laneIndex);
        SpawnLinePattern(spawnPointsInLane, defaultLineLength);
    }

    /// <summary>
    /// Spawns a straight line of coins using the provided spawn points.
    /// This function is PRESERVED and now takes a more specific list of points.
    /// </summary>
    private void SpawnLinePattern(Transform[] spawnPoints, int length)
    {
        for (int i = 0; i < length && i < spawnPoints.Length; i++)
        {
            Instantiate(coinPrefab, spawnPoints[i].position, Quaternion.identity, spawnPoints[i]);
        }
    }

    /// <summary>
    /// Calculates the final density multiplier from all non-risk-lane sources.
    /// This function is PRESERVED.
    /// </summary>
    private float GetTotalDensityMultiplier()
    {
        if (densityMultipliers.Count == 0) return 1f;

        float totalMultiplier = 1f;
        foreach (var multiplier in densityMultipliers.Values)
        {
            totalMultiplier *= multiplier;
        }
        return totalMultiplier;
    }
    #endregion
}
