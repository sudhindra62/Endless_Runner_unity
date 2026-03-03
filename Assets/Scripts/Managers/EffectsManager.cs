
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages global visual effects. It is now evolved to listen to the RiskLaneManager
/// and provide visual feedback to the player about which lane is the high-risk lane.
/// </summary>
public class EffectsManager : Singleton<EffectsManager>
{
    #region CONFIGURATION
    [Header("Risk Lane Visuals")]
    [Tooltip("The visual effect prefab to instantiate on the risk lane.")]
    [SerializeField] private GameObject riskLaneIndicatorPrefab;

    [Tooltip("The positional offsets for the indicator in each lane (Left, Middle, Right).")]
    [SerializeField] private Vector3[] laneOffsets = new Vector3[3]
    {
        new Vector3(-2.5f, 0, 0), // Index 0: Left Lane (-1)
        new Vector3(0, 0, 0),      // Index 1: Middle Lane (0)
        new Vector3(2.5f, 0, 0)    // Index 2: Right Lane (1)
    };
    #endregion

    #region STATE
    // --- EVOLUTION: Dictionary to track the active lane effect instance ---
    private readonly Dictionary<int, GameObject> activeLaneEffects = new Dictionary<int, GameObject>();
    #endregion

    #region UNITY_LIFECYCLE_EVOLUTION
    // Using the base class Awake is preserved.
    // We add Start and OnDestroy to manage event subscriptions.

    private void Start()
    {
        // --- EVOLUTION: Subscribe to the RiskLaneManager event ---
        RiskLaneManager.OnRiskLaneChanged += HandleRiskLaneChange;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy(); // Preserve Singleton cleanup
        // --- EVOLUTION: Unsubscribe to prevent memory leaks ---
        RiskLaneManager.OnRiskLaneChanged -= HandleRiskLaneChange;
    }
    #endregion

    #region EVENT_HANDLERS
    /// <summary>
    /// NEW: Handles the event from the RiskLaneManager to show or hide the visual indicator.
    /// </summary>
    private void HandleRiskLaneChange(int laneIndex, bool isRiskLane, float coinMultiplier, float obstacleMultiplier)
    {
        if (riskLaneIndicatorPrefab == null) return;

        if (isRiskLane)
        {
            // If an effect for this lane already exists for some reason, destroy it first.
            if (activeLaneEffects.ContainsKey(laneIndex))
            {
                Destroy(activeLaneEffects[laneIndex]);
            }

            // Convert lane index (-1, 0, 1) to array index (0, 1, 2)
            int offsetIndex = laneIndex + 1;
            if (offsetIndex >= 0 && offsetIndex < laneOffsets.Length)
            {
                Vector3 position = laneOffsets[offsetIndex];
                GameObject indicatorInstance = Instantiate(riskLaneIndicatorPrefab, position, Quaternion.identity, transform);
                activeLaneEffects[laneIndex] = indicatorInstance;
            }
        }
        else
        {
            // If the lane is reverting, and we have an effect for it, destroy it.
            if (activeLaneEffects.TryGetValue(laneIndex, out GameObject indicatorToDestroy))
            {
                Destroy(indicatorToDestroy);
                activeLaneEffects.Remove(laneIndex);
            }
        }
    }
    #endregion

    #region PRESERVED_METHODS
    /// <summary>
    /// This existing public method is 100% preserved and unaffected by the new functionality.
    /// </summary>
    public void ConvertAllObstaclesToCoins()
    {
        Obstacle[] obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        ObjectPooler objectPooler = ServiceLocator.Get<ObjectPooler>();

        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler not found. Cannot convert obstacles.");
            return;
        }

        foreach (Obstacle obstacle in obstacles)
        {
            if (obstacle != null && obstacle.gameObject.activeInHierarchy)
            {
                GameObject coin = objectPooler.GetFromPool("Coin", obstacle.transform.position, obstacle.transform.rotation);
                if (coin != null)
                {
                    coin.SetActive(true);
                }
                obstacle.gameObject.SetActive(false);
            }
        }

        Debug.Log($"Converted {obstacles.Length} obstacles to coins.");
    }
    #endregion
}
