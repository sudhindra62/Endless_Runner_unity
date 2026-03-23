using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Processes validated near-miss events and broadcasts them to other game systems.
/// This manager is the central authority for what constitutes a successful near-miss.
/// It does NOT perform detection itself; it listens to NearMissDetector components.
/// </summary>
public class NearMissManager : MonoBehaviour
{
    public static NearMissManager Instance { get; private set; }

    // --- Events ---
    public event Action<NearMissData> OnNearMissProcessed;
    public static event Action<NearMissData> OnNearMiss;

    [Header("Gameplay Configuration")]
    [SerializeField] private float scoreBonus = 100f;
    [SerializeField] private float comboBonus = 0.5f;
    [SerializeField] private float slowMoDuration = 0.15f;
    [SerializeField] private float slowMoFactor = 0.5f;
    [SerializeField] private float cooldownSeconds = 0.2f; // Cooldown to prevent spam from a single obstacle group

    // --- State ---
    private float lastMissTime = -1f;
    // Tracks obstacle instance IDs that have already triggered a miss to prevent duplicates.
    private HashSet<int> processedObstacles = new HashSet<int>();

    #region UNITY_LIFECYCLE
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        NearMissDetector.OnNearMissCandidate += HandleNearMissCandidate;
        // GameState.OnRunEnded += ClearProcessedObstacles; // Example of hooking into a game state manager
    }

    private void OnDisable()
    {
        NearMissDetector.OnNearMissCandidate -= HandleNearMissCandidate;
        // GameState.OnRunEnded -= ClearProcessedObstacles;
    }
    #endregion

    #region EVENT_HANDLERS
    /// <summary>
    /// Primary logic. Receives a potential near-miss event from a detector.
    /// </summary>
    private void HandleNearMissCandidate(int obstacleInstanceID, Vector3 obstaclePosition, float proximity)
    {
        // --- SAFETY CHECKS ---
        // 1. Global Cooldown: Is the system ready for another near miss?
        if (Time.time < lastMissTime + cooldownSeconds) return;

        // 2. Duplicate Check: Has this specific obstacle already been processed?
        if (processedObstacles.Contains(obstacleInstanceID)) return;

        // --- VALIDATION SUCCESS ---
        lastMissTime = Time.time;
        processedObstacles.Add(obstacleInstanceID);

        // Create rich data object for subscribers
        var nearMissData = new NearMissData(obstaclePosition, proximity);

        // --- BROADCAST and APPLY BONUSES ---
        Debug.Log($"[NearMissManager] Near miss PROCESSED for obstacle {obstacleInstanceID}");
        OnNearMissProcessed?.Invoke(nearMissData);
        OnNearMiss?.Invoke(nearMissData);

        // Route bonuses to the authoritative managers
        ApplyBonuses();
    }

    private void ApplyBonuses()
    {
        // 1. Route to ScoreManager
        // ScoreManager.Instance.AddStyleBonus(scoreBonus, "Near Miss");

        // 2. Route to FlowComboManager
        // FlowComboManager.Instance.IncreaseCombo(comboBonus);

        // 3. Trigger safe slow-motion via TimeControlManager
        // TimeControlManager.Instance.RequestSlowMotion(slowMoDuration, slowMoFactor);

        // 4. Trigger Camera Shake
        // CameraController.Instance.TriggerShake(CameraShakeType.NearMiss);
    }
    #endregion

    #region STATE_MANAGEMENT
    /// <summary>
    /// Called on player revive or new run to ensure near-misses can trigger again.
    /// </summary>
    public void ResetManager()
    {
        lastMissTime = -1f;
        ClearProcessedObstacles();
    }

    private void ClearProcessedObstacles()
    {
        processedObstacles.Clear();
    }
    #endregion
}
