using UnityEngine;

/// <summary>
/// Defines the possible states of a boss encounter.
/// </summary>
public enum BossState
{
    Inactive,
    Spawning,
    Active,
    Defeated
}

/// <summary>
/// Fortified singleton for managing boss encounters.
/// This system is now stateful, uses object pooling, and is event-driven.
/// </summary>
public class BossManager : Singleton<BossManager>
{
    [Header("Boss Configuration")]
    [Tooltip("The prefab for the boss. Must have a Boss component.")]
    [SerializeField] private Boss _bossPrefab;

    // --- State ---
    private Boss _currentBoss;
    private BossState _bossState;

    // --- Events ---
    public static event System.Action<BossState> OnBossStateChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // In a full implementation, this manager would listen to a higher-level
        // game flow or encounter manager to trigger the boss spawn.
        // e.g., EncounterManager.OnBossEncounterStart += TriggerBossEncounter;
    }

    /// <summary>
    /// Public method to initiate the boss encounter. Should be called by a game flow controller.
    /// </summary>
    public void TriggerBossEncounter()
    {
        if (_bossState != BossState.Inactive)
        {
            Debug.LogWarning("[BossManager] Tried to spawn a boss when one is already active or spawning.");
            return;
        }
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        if (_bossPrefab == null)
        {
            Debug.LogError("[BossManager] Boss Prefab is not set!");
            return;
        }

        SetState(BossState.Spawning);
        
        // Use the PoolManager for efficient object instantiation.
        GameObject bossObject = PoolManager.Instance.GetObject(_bossPrefab.gameObject, Vector3.zero, Quaternion.identity);
        _currentBoss = bossObject.GetComponent<Boss>();

        if (_currentBoss != null)
        {
            // The Boss script should be responsible for notifying the manager of its state changes.
            _currentBoss.OnReady += HandleBossReady;
            _currentBoss.OnDefeated += HandleBossDefeated;
            Debug.Log("<color=orange>[BossManager]</color> Boss has been spawned via PoolManager.");
        }
        else
        {
            Debug.LogError("[BossManager] Spawned object is missing a Boss component!");
            SetState(BossState.Inactive);
        }
    }

    private void DespawnBoss()
    {
        if (_currentBoss == null) return;

        // Unsubscribe from events to prevent memory leaks.
        _currentBoss.OnReady -= HandleBossReady;
        _currentBoss.OnDefeated -= HandleBossDefeated;

        // Return the object to the pool instead of destroying it.
        PoolManager.Instance.ReturnObject(_currentBoss.gameObject);
        _currentBoss = null;

        Debug.Log("<color=orange>[BossManager]</color> Boss has been despawned and returned to the pool.");
        SetState(BossState.Inactive);
    }

    // --- Event Handlers from the Boss instance ---

    private void HandleBossReady()
    {
        SetState(BossState.Active);
    }

    private void HandleBossDefeated()
    {
        SetState(BossState.Defeated);
        // In a real game, you might trigger a cinematic or delay before despawning.
        DespawnBoss();
    }

    private void SetState(BossState newState)
    {
        if (_bossState == newState) return;
        _bossState = newState;
        OnBossStateChanged?.Invoke(_bossState);
    }

    /// <summary>
    /// Returns the currently active boss instance.
    /// </summary>
    public Boss GetCurrentBoss()
    {
        return _currentBoss;
    }
}
