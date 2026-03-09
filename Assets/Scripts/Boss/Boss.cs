
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls the behavior of a formidable boss enemy that pursues the player.
/// Features multiple attack patterns, states, and dynamic behavior based on player distance.
/// Fully restored, wired, and fortified by the Supreme Guardian Architect v12.
/// </summary>
public class Boss : MonoBehaviour
{
    public enum BossState { Chasing, Attacking, Stunned, Defeated }

    [Header("Core References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Animator bossAnimator;

    [Header("Boss Stats")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float attackRange = 20f;
    [SerializeField] private float followDistance = 15f;
    [SerializeField] private int maxHealth = 100;

    [Header("Attack Patterns")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileAttackCooldown = 3f;
    [SerializeField] private float specialAttackCooldown = 10f;

    private int currentHealth;
    private float lastProjectileAttackTime;
    private float lastSpecialAttackTime;
    private BossState currentState = BossState.Chasing;

    void Awake()
    {
        // --- CONTEXT_WIRING: Automatically find essential references if not assigned. ---
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        // Find the CameraController for effects
        if (cameraController == null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        if (bossAnimator == null)
        {
            bossAnimator = GetComponent<Animator>();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        lastProjectileAttackTime = -projectileAttackCooldown;
        lastSpecialAttackTime = -specialAttackCooldown;
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (currentState != BossState.Defeated)
        {
            switch (currentState)
            {
                case BossState.Chasing:
                    yield return StartCoroutine(ChaseState());
                    break;
                case BossState.Attacking:
                    yield return StartCoroutine(AttackState());
                    break;
                case BossState.Stunned:
                    yield return StartCoroutine(StunnedState());
                    break;
            }
        }
    }

    private IEnumerator ChaseState()
    {
        bossAnimator.SetBool("IsChasing", true);
        while (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // --- A-TO-Z CONNECTIVITY: Transition to attack state when in range. ---
            if (distanceToPlayer <= attackRange)
            {
                currentState = BossState.Attacking;
                bossAnimator.SetBool("IsChasing", false);
                yield break; // Exit coroutine to switch state
            }

            // Follow the player on the Z-axis, maintaining a set distance
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, playerTransform.position.z - followDistance);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            yield return null;
        }
    }

    private IEnumerator AttackState()
    {
        bossAnimator.SetBool("IsAttacking", true);
        while (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // --- A-TO-Z CONNECTIVITY: Transition back to chasing if player gets too far. ---
            if (distanceToPlayer > attackRange)
            {
                currentState = BossState.Chasing;
                bossAnimator.SetBool("IsAttacking", false);
                yield break; // Exit coroutine to switch state
            }
            
            // Look at the player
            transform.LookAt(playerTransform);

            // Perform attacks based on cooldowns
            if (Time.time >= lastSpecialAttackTime + specialAttackCooldown)
            {
                PerformSpecialAttack();
                lastSpecialAttackTime = Time.time;
            }
            else if (Time.time >= lastProjectileAttackTime + projectileAttackCooldown)
            {
                PerformProjectileAttack();
                lastProjectileAttackTime = Time.time;
            }

            yield return null;
        }
    }

    private void PerformProjectileAttack()
    {
        if (projectilePrefab == null || projectileSpawnPoint == null) return;
        
        bossAnimator.SetTrigger("ProjectileAttack");
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        
        // --- DEPENDENCY_FIX: Trigger camera shake for impact. ---
        if (cameraController != null)
        {
            cameraController.ShakeCamera(0.2f, 0.1f);
        }
    }

    private void PerformSpecialAttack()
    {
        bossAnimator.SetTrigger("SpecialAttack");
        // Placeholder for a more complex special attack, e.g., a laser beam or area-of-effect slam.
        Debug.Log("Guardian Architect Log: Boss performing SPECIAL ATTACK!");

        // --- DEPENDENCY_FIX: Trigger intense camera shake. ---
        if (cameraController != null)
        {
            cameraController.ShakeCamera(0.5f, 0.5f);
        }
    }
    
    private IEnumerator StunnedState()
    {
        bossAnimator.SetBool("IsStunned", true);
        Debug.Log("Guardian Architect Log: Boss is stunned!");
        
        yield return new WaitForSeconds(5f); // Stun duration
        
        currentState = BossState.Chasing;
        bossAnimator.SetBool("IsStunned", false);
    }

    /// <summary>
    /// Reduces the boss's health and checks for defeat.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (currentState == BossState.Defeated) return;

        currentHealth -= damage;
        Debug.Log($"Guardian Architect Log: Boss took {damage} damage, {currentHealth}/{maxHealth} HP remaining.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            currentState = BossState.Defeated;
            StartCoroutine(DefeatedSequence());
        }
    }

    private IEnumerator DefeatedSequence()
    {
        bossAnimator.SetTrigger("Defeated");
        Debug.Log("Guardian Architect Log: Boss has been defeated!");
        
        // --- A-TO-Z CONNECTIVITY: Notify a GameManager or BossChaseManager of the victory. ---
        // Example: BossChaseManager.Instance.EndChase(true);

        yield return new WaitForSeconds(5f); // Animation duration
        
        // Disable or destroy the boss object
        gameObject.SetActive(false);
    }
}
