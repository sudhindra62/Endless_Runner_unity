
using UnityEngine;

/// <summary>
/// Manages the boss encounter, including spawning, state transitions, and player interaction.
/// Orchestrated and fortified by Supreme Guardian Architect v12.
/// </summary>
public class BossManager : Singleton<BossManager>
{
    [Header("Boss Configuration")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private float distanceToTriggerBoss = 500f;

    private Boss currentBoss;
    private bool isBossActive = false;
    private bool bossDefeated = false;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isBossActive || bossDefeated || playerTransform == null) return;

        // --- A-TO-Z CONNECTIVITY: Trigger the boss chase when the player reaches a certain distance. ---
        if (playerTransform.position.z >= distanceToTriggerBoss)
        {
            StartBossChase();
        }
    }

    /// <summary>
    /// Initiates the boss chase sequence.
    /// </summary>
    public void StartBossChase()
    {
        if (bossPrefab == null || bossSpawnPoint == null)
        {
            Debug.LogError("Guardian Architect Error: Boss Prefab or Spawn Point not assigned in BossManager!");
            return;
        }

        if (isBossActive) return;

        Debug.Log("Guardian Architect Log: Boss Chase initiated!");
        isBossActive = true;

        // Spawn the boss
        GameObject bossObject = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        currentBoss = bossObject.GetComponent<Boss>();

        // --- DEPENDENCY_FIX: Notify the GameManager or other systems that the boss is active. ---
        // Example: GameManager.Instance.SetState(GameManager.GameState.Boss); 
    }

    /// <summary>
    /// Ends the boss chase, either by victory or defeat.
    /// </summary>
    /// <param name="playerWon">True if the player defeated the boss.</param>
    public void EndChase(bool playerWon)
    {
        if (!isBossActive) return;

        isBossActive = false;
        if (playerWon)
        {
            bossDefeated = true;
            Debug.Log("Guardian Architect Log: Player has won the boss battle!");
            // Grant rewards, progress the game, etc.
            // Example: RewardManager.Instance.GrantBossDefeatReward();
        } 
        else
        {
            Debug.Log("Guardian Architect Log: Player has been defeated by the boss!");
            // Handle player death during the boss fight
            // Example: GameManager.Instance.PlayerDied();
        }
    }

    public bool IsBossActive() => isBossActive;
}
